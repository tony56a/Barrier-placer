﻿using BarrierPlacer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using UnityEngine;

namespace BarrierPlacer.Manager
{
    public class RenderingManager : SimulationManagerBase<RenderingManager, DistrictProperties>, IRenderableManager, ISimulationManager
    {

        private int m_lastCount = 0;
        private bool textHidden = false;

        public float m_renderHeight = 1000f;
        public bool m_alwaysShowText = false;
        public bool m_registered = false;
        public bool m_routeEnabled = true;
        public PropInfo test;

        protected override void Awake()
        {
            base.Awake();

            LoggerUtils.Log("Initialising RenderingManager");
        }

        protected override void BeginOverlayImpl(RenderManager.CameraInfo cameraInfo)
        {
            NetManager netManager = NetManager.instance;
            DistrictManager districtManager = DistrictManager.instance;

            if (m_lastCount != BarrierManager.Instance().m_barrierList.Count)
            {
                m_lastCount = BarrierManager.Instance().m_barrierList.Count;
                try
                {
                    RenderText();
                }
                catch (Exception ex)
                {
                    LoggerUtils.LogException(ex);
                }
            }

            if (!textHidden && cameraInfo.m_height > m_renderHeight)
            {

                foreach (BarrierStrip sign in BarrierManager.Instance().m_barrierList)
                {
                    sign.m_gameObj.SetActive(false);
                }
                textHidden = true;
            }
            else if (textHidden && cameraInfo.m_height <= m_renderHeight) //This is a mess, and I'll sort it soon :)
            {

                if (m_routeEnabled)
                {
                    foreach (BarrierStrip sign in BarrierManager.Instance().m_barrierList)
                    {
                        sign.m_gameObj.SetActive(true);
                    }
                }
                textHidden = false;
            }
        }

        /// <summary>
        /// Redraw the text to be drawn later with a mesh. Use sparingly, as 
        /// this is an expensive task.
        /// </summary>
        private void RenderText()
        {
            
            foreach (NewBarrierStrip sign in BarrierManager.Instance().m_barrierList)
            {
                string propName = sign.propName ?? "Jersey Barrier";
                PrefabInfo prefab = PropUtils.tryGetPropInfo(propName);

                if (prefab == null)
                {
                    LoggerUtils.Log(String.Format("Prefab {0} cannot be found!", propName));
                    continue;
                }

                PropInfo propInfo = prefab as PropInfo;
                TreeInfo tree = prefab as TreeInfo;

                Mesh mesh = prefab.GetType().Equals(typeof(PropInfo)) ? propInfo.m_mesh : tree.m_mesh;
                Material material = prefab.GetType().Equals(typeof(PropInfo)) ? propInfo.m_material : tree.m_material;
                bool requireHeightMap = prefab.GetType().Equals(typeof(PropInfo)) ? propInfo.m_requireHeightMap : false;
                
                if (sign.positions.Count < 2)
                {
                    LoggerUtils.Log("A line has fewer then 2 points!");
                    continue;
                }

                for (int i = 1; i < sign.positions.Count; i++) {
                    Vector3 startPosition = sign.positions[i-1];
                    Vector3 endPosition = sign.positions[i];
                    Vector3 heading = endPosition - startPosition;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;

                    int numBarriers = (int)(distance / ((mesh.bounds.size.x * sign.sizeOffset) + sign.spacing));
                    float angle = (float)Math.Atan2(direction.z, direction.x) * -1 * Mathf.Rad2Deg + sign.rotationOffset;
                    Vector3 position = startPosition + direction * (mesh.bounds.extents.x * sign.sizeOffset);

#if DEBUG
                LoggerUtils.Log(String.Format("Distance:{0},Direction{1}", distance, direction));
                LoggerUtils.Log(String.Format("number of barriers:{0} Angle:{1}", numBarriers, angle));
#endif
                    if (sign.m_gameObj != null)
                    {
                        continue;
                    }

                    sign.m_gameObj = new GameObject("barrier"+heading);
                    
                    Texture heightMap;
                    Vector4 heightMapping;
                    Vector4 surfaceMapping;

                    PropManager instance = PropManager.instance;
                    MaterialPropertyBlock properties = instance.m_materialBlock;

                    for (int j = 0; j < numBarriers; j++)
                    {
                        GameObject barrier = new GameObject();
                        MeshFilter filter = barrier.AddComponent<MeshFilter>();
                        MeshRenderer renderer = barrier.AddComponent<MeshRenderer>();

                        float terrainHeight = TerrainManager.instance.SampleDetailHeight(position);

                        if (requireHeightMap)
                        {
                            TerrainManager.instance.GetHeightMapping(new Vector3(position.x, terrainHeight, position.z), out heightMap, out heightMapping, out surfaceMapping);
                            properties.Clear();
                            properties.AddTexture(instance.ID_HeightMap, heightMap);
                            properties.AddVector(instance.ID_HeightMapping, heightMapping);
                            properties.AddVector(instance.ID_SurfaceMapping, surfaceMapping);
                            renderer.SetPropertyBlock(properties);

                        }

                        barrier.transform.position = new Vector3(position.x, terrainHeight, position.z);
                        barrier.transform.Rotate(0, angle, 0);
                        barrier.transform.parent = sign.m_gameObj.transform;
                        barrier.transform.localScale = new Vector3(sign.sizeOffset, sign.sizeOffset, sign.sizeOffset);
                        filter.mesh = mesh;
                        renderer.material = material;

                        position += direction * ((mesh.bounds.size.x * sign.sizeOffset) + sign.spacing);

                    }
                }

               
            }

        }


        /// <summary>
        /// Forces rendering to update immediately. Use sparingly, as it
        /// can be quite expensive.
        /// </summary>
        public void ForceUpdate()
        {
            m_lastCount = -1;
        }
    }
}
