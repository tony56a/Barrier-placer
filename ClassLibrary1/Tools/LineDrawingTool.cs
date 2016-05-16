using BarrierPlacer.Manager;
using BarrierPlacer.Managers;
using BarrierPlacer.UI;
using BarrierPlacer.Utils;
using ColossalFramework;
using ColossalFramework.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.Tools
{
    enum BarrierPlacerToolState
    {
        none,
        startPlaced,
    }

    class LineDrawingTool : DefaultTool, IEventSubscriber
    {
        public float m_angle;
        public Texture2D m_brush;
        public CursorInfo m_buildCursor;
        protected PropInfo m_propInfo;
        private bool m_mouseLeftDown;
        private bool m_mouseRightDown;
        private ToolBase.ToolErrors m_placementErrors;
        private Randomizer m_randomizer;

        protected Vector3 m_cachedPosition;
        protected float m_cachedAngle;
        public string m_propName = null;
        private float m_spacing = 0;

        private List<Vector3> m_positions = new List<Vector3>();
        private GameObject[] m_previewObjs;

        public SpacingDialog spacingDialog;

        private BarrierPlacerToolState state;

        protected override void OnToolGUI(UnityEngine.Event e)
        {
            if (!this.m_toolController.IsInsideUI && e.type == UnityEngine.EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    this.m_mouseLeftDown = true;
                    switch (state)
                    {
                        case BarrierPlacerToolState.none:
                            m_positions.Add(this.m_cachedPosition);

                            state = BarrierPlacerToolState.startPlaced;
                            break;
                        case BarrierPlacerToolState.startPlaced:
                            m_positions.Add(this.m_cachedPosition);
                            BarrierManager.Instance().setBarrierLine(m_positions, m_propName,m_spacing);
                            m_positions = new List<Vector3>();
                            state = BarrierPlacerToolState.none;
                            for (int i = 0; i < 150; i++)
                            {
                                Destroy(m_previewObjs[i]);
                                m_previewObjs[i] = null;
                            }

                            RenderingManager.instance.ForceUpdate();
                            ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                            ToolsModifierControl.SetTool<DefaultTool>();

                            break;
                    }

                }
                else
                {
                    if (e.button != 1)
                        return;
                    this.m_mouseRightDown = true;
                }
            }
            else
            {
                if (e.type != UnityEngine.EventType.MouseUp)
                    return;
                if (e.button == 0)
                {
                    this.m_mouseLeftDown = false;
                }
                else
                {
                    if (e.button != 1)
                        return;
                    this.m_mouseRightDown = false;
                }
            }
        }

        public void setProp()
        {
            m_propInfo = PropUtils.tryGetPropInfo(m_propName ?? "Jersey Barrier");
            m_previewObjs = new GameObject[150];
            for (int i = 0; i < 150; i++)
            {
                if (m_previewObjs[i] != null)
                {
                    Destroy(m_previewObjs[i]);
                    m_previewObjs[i] = null;
                }
                m_previewObjs[i] = new GameObject();
            }
            for (int i = 0; i < 150; i++)
            {
                MeshFilter meshFilter = m_previewObjs[i].AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = m_previewObjs[i].AddComponent<MeshRenderer>();
                meshFilter.mesh = m_propInfo.m_mesh;
                meshRenderer.material = Instantiate(m_propInfo.m_material);
                m_previewObjs[i].SetActive(false);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.ToolCursor = this.m_buildCursor;
            this.m_toolController.ClearColliding();
            this.m_placementErrors = ToolBase.ToolErrors.Pending;
            this.m_toolController.SetBrush((Texture2D)null, Vector3.zero, 1f);

            m_positions = new List<Vector3>();
            setProp();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.ToolCursor = (CursorInfo)null;
            this.m_toolController.SetBrush((Texture2D)null, Vector3.zero, 1f);
            this.m_mouseLeftDown = false;
            this.m_mouseRightDown = false;
            this.m_placementErrors = ToolBase.ToolErrors.Pending;
            this.m_mouseRayValid = false;

            m_positions = new List<Vector3>();
            state = BarrierPlacerToolState.none;
            for (int i = 0; i < 150; i++)
            {
                Destroy(m_previewObjs[i]);
                m_previewObjs[i] = null;
            }
            spacingDialog.Hide();
        }


        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            float size = 5;
            ++Singleton<ToolManager>.instance.m_drawCallData.m_overlayCalls;
            Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, this.GetToolColor(false, false), this.m_cachedPosition, size, this.m_cachedPosition.y - 100f, this.m_cachedPosition.y + 100f, false, true);
            base.RenderOverlay(cameraInfo);
        }

        public static void CheckOverlayAlpha(PropInfo info, float scale, ref float alpha)
        {
            if ((UnityEngine.Object)info == (UnityEngine.Object)null)
                return;
            float f = (float)((double)Mathf.Max(info.m_generatedInfo.m_size.x, info.m_generatedInfo.m_size.z) * (double)scale * 0.5);
            alpha = Mathf.Min(alpha, 2f / Mathf.Max(1f, Mathf.Sqrt(f)));
        }

        protected override void OnToolUpdate()
        {
            if (m_positions.Count >= 1)
            {
                if (m_propInfo != null)
                {


                    Vector3 heading = this.m_cachedPosition - m_positions[0];
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;
                    int numBarriers = Math.Min((int)(distance / (m_propInfo.m_mesh.bounds.size.x + m_spacing) ), 150);
                    float angle = (float)Math.Atan2(direction.z, direction.x) * -1 * Mathf.Rad2Deg;

                    if (angle != float.NaN)
                    {
                        Texture heightMap;
                        Vector4 heightMapping;
                        Vector4 surfaceMapping;

                        PropManager instance = PropManager.instance;
                        MaterialPropertyBlock properties = instance.m_materialBlock;

                        Vector3 position = m_positions[0] + direction * m_propInfo.m_mesh.bounds.extents.x;
                        for (int i = 0; i < 150; i++)
                        {
                            m_previewObjs[i].SetActive(false);
                        }
                        for (int j = 0; j < numBarriers; j++)
                        {
                            float terrainHeight = TerrainManager.instance.SampleDetailHeight(position);

                            if (m_propInfo.m_requireHeightMap)
                            {
                                TerrainManager.instance.GetHeightMapping(new Vector3(position.x, terrainHeight, position.z), out heightMap, out heightMapping, out surfaceMapping);
                                properties.Clear();
                                properties.AddTexture(instance.ID_HeightMap, heightMap);
                                properties.AddVector(instance.ID_HeightMapping, heightMapping);
                                properties.AddVector(instance.ID_SurfaceMapping, surfaceMapping);
                                m_previewObjs[j].GetComponent<Renderer>().SetPropertyBlock(properties);

                            }

                            Quaternion rotation = m_previewObjs[j].transform.rotation;
                            m_previewObjs[j].transform.position = new Vector3(position.x, terrainHeight, position.z);
                            rotation.eulerAngles = new Vector3(0, angle, 0);
                            m_previewObjs[j].transform.rotation = rotation;
                            m_previewObjs[j].SetActive(true);
                            position += direction * ( m_propInfo.m_mesh.bounds.size.x + m_spacing );

                        }
                    }

                }
            }

        }

        protected override void OnToolLateUpdate()
        {
            this.m_mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            this.m_mouseRayLength = Camera.main.farClipPlane;
            this.m_mouseRayValid = !this.m_toolController.IsInsideUI && Cursor.visible;
            this.m_toolController.SetBrush((Texture2D)null, Vector3.zero, 1f);
            this.m_cachedPosition = this.m_mousePosition;
            this.m_cachedAngle = this.m_angle * (float)(Math.PI / 180.0);
        }

        public static void DispatchPlacementEffect(Vector3 pos, bool bulldozing)
        {
            EffectInfo effect = !bulldozing ? Singleton<PropManager>.instance.m_properties.m_placementEffect : Singleton<PropManager>.instance.m_properties.m_bulldozeEffect;
            if (effect == null)
                return;
            InstanceID instance = new InstanceID();
            EffectInfo.SpawnArea spawnArea = new EffectInfo.SpawnArea(pos, Vector3.up, 1f);
            Singleton<EffectManager>.instance.DispatchEffect(effect, instance, spawnArea, Vector3.zero, 0.0f, 1f, Singleton<AudioManager>.instance.DefaultGroup);
        }

        public override void SimulationStep()
        {

            ToolBase.RaycastInput input = new ToolBase.RaycastInput(this.m_mouseRay, this.m_mouseRayLength);
            input.m_ignoreSegmentFlags = NetSegment.Flags.None;
            input.m_ignoreNodeFlags = NetNode.Flags.None;
            ulong[] collidingSegments;
            ulong[] collidingBuildings;
            this.m_toolController.BeginColliding(out collidingSegments, out collidingBuildings);
            try
            {
                ToolBase.RaycastOutput output;
                if (this.m_mouseRayValid && ToolBase.RayCast(input, out output))
                {

                    float terrainHeight = TerrainManager.instance.SampleDetailHeight(output.m_hitPos);
                    output.m_hitPos.y = output.m_hitPos.y > terrainHeight ? output.m_hitPos.y : terrainHeight;
                    Randomizer r = this.m_randomizer;
                    ushort id = Singleton<PropManager>.instance.m_props.NextFreeItem(ref r);
                    this.m_mousePosition = output.m_hitPos;
                    this.m_placementErrors = ToolErrors.None;

                }
                else
                    this.m_placementErrors = ToolBase.ToolErrors.RaycastFailed;
            }
            finally
            {
                this.m_toolController.EndColliding();
            }
        }

        public override ToolBase.ToolErrors GetErrors()
        {
            return ToolErrors.None;
        }

        public void onReceiveEvent(string eventName, object eventData)
        {
            switch (eventName)
            {
                case "setSpacing":
                    this.m_spacing = (float)eventData;
                    break;
                default:
                    break;
            }
        }
    }

}
