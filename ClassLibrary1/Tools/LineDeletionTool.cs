using BarrierPlacer.Manager;
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
    class LineDeletionTool : DefaultTool
    {
        BarrierStrip stripContainer = null;

        protected override void Awake()
        {
            LoggerUtils.Log("Line Deletion Tool awake");
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            float size = 5;
            if(stripContainer != null)
            {
                ++Singleton<ToolManager>.instance.m_drawCallData.m_overlayCalls;
                Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, this.GetToolColor(false, false), stripContainer.positions[0], size, stripContainer.positions[0].y - 100f, stripContainer.positions[0].y + 100f, false, true);
                Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, this.GetToolColor(false, false), stripContainer.positions[stripContainer.positions.Count-1], size, stripContainer.positions[stripContainer.positions.Count - 1].y - 100f, stripContainer.positions[stripContainer.positions.Count - 1].y + 100f, false, true);
            }
            base.RenderOverlay(cameraInfo);
        }

        protected override void OnToolUpdate()
        {

            if (m_toolController != null && !m_toolController.IsInsideUI && Cursor.visible)
            {

                Ray currentPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (RaycastLine(currentPosition, out stripContainer))
                {
                    if (stripContainer != null)
                    {
                        if (Event.current.type == EventType.MouseDown /*&& Event.current.button == (int)UIMouseButton.Left*/)
                        {
                            //unset tool
                            ShowToolInfo(false, null, new Vector3());
                            GameObject.Destroy(stripContainer.m_gameObj);
                            BarrierManager.Instance().m_barrierList.Remove(stripContainer);
                        }
                        else
                        {
                            ShowToolInfo(true, "Delete this prop line", stripContainer.positions[0]);
                        }

                    }
                }


            }
            else
            {
                ShowToolInfo(false, null, new Vector3());
            }
        }

        bool RaycastLine(Ray currentPosition, out BarrierStrip returnValue)
        {
            Vector3 origin = currentPosition.origin;
            Vector3 normalized = currentPosition.direction.normalized;
            Vector3 _b = currentPosition.origin + normalized * Camera.main.farClipPlane;
            Segment3 ray = new Segment3(origin, _b);

            foreach (BarrierStrip container in BarrierManager.Instance().m_barrierList)
            {
                for( int i=0; i<container.m_gameObj.transform.childCount; i++)
                {
                    if (ray.DistanceSqr(container.m_gameObj.transform.GetChild(i).position) < 300)
                    {
                        returnValue = container;
                        return true;
                    }
                }
                    
            }

            returnValue = null;
            return false;
        }
    }
}
