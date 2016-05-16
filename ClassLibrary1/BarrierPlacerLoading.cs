using BarrierPlacer.Manager;
using BarrierPlacer.UI;
using BarrierPlacer.Utils;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrierPlacer
{
    public class BarrierPlacerLoading : LoadingExtensionBase
    {
        public ModUI UI { get; set; }
        public RenderingManager m_renderingManager;

        public override void OnCreated(ILoading loading)
        {
          
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame || mode == LoadMode.NewMap || mode == LoadMode.LoadMap)
            {
                UI = ToolsModifierControl.toolController.gameObject.AddComponent<ModUI>();
                
                m_renderingManager = RenderingManager.instance;
                m_renderingManager.enabled = true;

                if (m_renderingManager != null && !m_renderingManager.m_registered)
                {
                    RenderManager.RegisterRenderableManager(m_renderingManager);
                    m_renderingManager.m_registered = true;
                }
                LoggerUtils.Log("Mod loaded");
            }
        }

        public override void OnLevelUnloading()
        {
            
        }
    }
}
