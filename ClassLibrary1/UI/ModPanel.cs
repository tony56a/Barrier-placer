using BarrierPlacer.Managers;
using BarrierPlacer.Tools;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.UI
{
    class ModPanel: UIPanel
    {
        private static readonly int WIDTH = 170;
        private static readonly int PADDING = 10;

        private LineDeletionTool mLineDeletionTool;
        private LineDrawingTool mLineDrawingTool;

        private UIButton drawLineBtn;
        private UIButton deleteLineBtn;

        private GameObject m_selectPropPanelObject;
        private SearchPanel m_selectPropPanel;

        private GameObject m_propSpacingDialogObject;
        private SpacingDialog m_propSpacingDialog;

        public ModPanel()
        {
            UIView uiView = UIView.GetAView();
            int yCursor = PADDING;
            this.backgroundSprite = "GenericPanel";
            color = new Color32(75, 75, 135, 255);
            width = WIDTH;

            drawLineBtn = getButton(yCursor, "Draw Line", drawLineBtn_eventClick);
            yCursor += (30 + PADDING);
            deleteLineBtn = getButton(yCursor, "Delete Line", deleteLineBtn_eventClick);
            yCursor += (30 + PADDING);
            
            this.height = deleteLineBtn.relativePosition.y + deleteLineBtn.height + PADDING * 2;

            mLineDeletionTool = ToolsModifierControl.toolController.gameObject.AddComponent<LineDeletionTool>();
            mLineDrawingTool = ToolsModifierControl.toolController.gameObject.AddComponent<LineDrawingTool>();

            m_propSpacingDialogObject = new GameObject("SpacingDialog");
            m_propSpacingDialog = m_propSpacingDialogObject.AddComponent<SpacingDialog>();
            m_propSpacingDialog.transform.parent = uiView.transform;
            m_propSpacingDialog.Hide();

            m_selectPropPanelObject = new GameObject("SearchPanel");
            m_selectPropPanel = m_selectPropPanelObject.AddComponent<SearchPanel>();
            m_selectPropPanel.transform.parent = uiView.transform;
            m_selectPropPanel.mLineDrawingTool = mLineDrawingTool;
            m_selectPropPanel.spacingDialog = m_propSpacingDialog;
            m_selectPropPanel.Hide();
            
            EventBusManager.Instance().Subscribe("closeAll", m_selectPropPanel);
            EventBusManager.Instance().Subscribe("setPropName", m_selectPropPanel);
            EventBusManager.Instance().Subscribe("setSpacing", mLineDrawingTool);
            
            ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
            ToolsModifierControl.SetTool<DefaultTool>();
            
        }

        public override void Start()
        {
            relativePosition = new Vector3(40f, 100f);
        }

        private UIButton getButton(int y, String text, MouseEventHandler handler)
        {
            UIButton button = AddUIComponent(typeof(UIButton)) as UIButton;

            button.text = text;
            button.width = 150;
            button.height = 30;
            button.normalBgSprite = "ButtonMenu";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.focusedBgSprite = "ButtonMenuFocused";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.textColor = new Color32(255, 255, 255, 255);
            button.disabledTextColor = new Color32(7, 7, 7, 255);
            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);
            button.pressedTextColor = new Color32(30, 30, 44, 255);
            button.eventClick += handler;
            button.relativePosition = new Vector3(PADDING, y);
            return button;
        }

        private void drawLineBtn_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (m_selectPropPanel.isVisible)
            {
                m_selectPropPanel.isVisible = false;
                m_selectPropPanel.Hide();

            }
            else
            {
                m_selectPropPanel.isVisible = true;
                m_selectPropPanel.Show();
            }
       
        }

        private void deleteLineBtn_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (ToolsModifierControl.toolController.CurrentTool != mLineDeletionTool)
            {
                ToolsModifierControl.toolController.CurrentTool = mLineDeletionTool;
                ToolsModifierControl.SetTool<LineDeletionTool>();
            }
            else
            {
                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                ToolsModifierControl.SetTool<DefaultTool>();
            }
        }
    }
}
