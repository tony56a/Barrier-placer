using BarrierPlacer.Tools;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.UI
{
    public class ModUI : UICustomControl
    {
        UIButton drawLineBtn;
        ModPanel modPanel;

        private LineDrawingTool mRoadSelectTool;

        private bool m_isUiShowing;

        public ModUI()
        {
            UIView uiView = UIView.GetAView();
            drawLineBtn = (UIButton)uiView.AddUIComponent(typeof(UIButton));

            drawLineBtn.text = "Draw Prop Line";
            drawLineBtn.width = 150;
            drawLineBtn.height = 30;
            drawLineBtn.normalBgSprite = "ButtonMenu";
            drawLineBtn.disabledBgSprite = "ButtonMenuDisabled";
            drawLineBtn.hoveredBgSprite = "ButtonMenuHovered";
            drawLineBtn.focusedBgSprite = "ButtonMenuFocused";
            drawLineBtn.pressedBgSprite = "ButtonMenuPressed";
            drawLineBtn.textColor = new Color32(255, 255, 255, 255);
            drawLineBtn.disabledTextColor = new Color32(7, 7, 7, 255);
            drawLineBtn.hoveredTextColor = new Color32(7, 132, 255, 255);
            drawLineBtn.focusedTextColor = new Color32(255, 255, 255, 255);
            drawLineBtn.pressedTextColor = new Color32(30, 30, 44, 255);
            drawLineBtn.eventClick += drawLineBtn_eventClick;
            drawLineBtn.relativePosition = new Vector3(180, 100f);

            mRoadSelectTool = ToolsModifierControl.toolController.gameObject.AddComponent<LineDrawingTool>();
            ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
            ToolsModifierControl.SetTool<DefaultTool>();
        }

        private void drawLineBtn_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
            ToolsModifierControl.SetTool<DefaultTool>();

            if (m_isUiShowing)
            {
                hideUI();
            }
            else
            {
                showUI();
            }
        }

        private void showUI()
        {
            UIView uiView = UIView.GetAView();
            if (modPanel != null)
            {
                modPanel.Show();
            }
            else
            {
                modPanel = uiView.AddUIComponent(typeof(ModPanel)) as ModPanel;
            }

            m_isUiShowing = true;
        }

        private void hideUI()
        {
            if (modPanel != null)
            {
                modPanel.Hide();
            }
            m_isUiShowing = false;
        }
    }
}
