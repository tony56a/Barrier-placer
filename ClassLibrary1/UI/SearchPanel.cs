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
    class SearchPanel : UIPanel, IEventSubscriber
    {
        protected RectOffset m_UIPadding = new RectOffset(5, 5, 5, 5);

        private int titleOffset = 40;
        private TitleBar m_panelTitle;
        public UIFastList propNamesList = null;

        private UITextField m_textField;
        private UILabel m_searchLabel;

        private List<string> m_allPropInfos;

        private Vector2 offset = Vector2.zero;

        public LineDrawingTool mLineDrawingTool;
        public SpacingDialog spacingDialog;

        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 350;
            this.height = 300;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();

            m_allPropInfos = new List<string>();
            for(uint i =0; i< PrefabCollection<PropInfo>.PrefabCount(); i++)
            {
                m_allPropInfos.Add( PrefabCollection<PropInfo>.PrefabName(i));
            }
 
            m_panelTitle = this.AddUIComponent<TitleBar>();
            m_panelTitle.title = "Props";
            m_panelTitle.m_closeActions.Add("closeAll");

            CreatePanelComponents();

            this.relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2) + width, Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            this.backgroundSprite = "MenuPanel2";
        }

        private void CreatePanelComponents()
        {
            // From CWMlolzlz's Building Search mod
            m_searchLabel = this.AddUIComponent<UILabel>();
            m_searchLabel.text = "Search:";
            m_searchLabel.autoSize = false;
            m_searchLabel.size = new Vector2(this.width - m_UIPadding.left - m_UIPadding.right, 50);
            m_searchLabel.padding = m_UIPadding;
            m_searchLabel.relativePosition = new Vector2(m_UIPadding.left, titleOffset + m_UIPadding.top);
            m_searchLabel.textAlignment = UIHorizontalAlignment.Left;
            m_searchLabel.verticalAlignment = UIVerticalAlignment.Middle;

            m_textField = this.AddUIComponent<UITextField>();
            m_textField.builtinKeyNavigation = true;
            m_textField.isInteractive = true;
            m_textField.canFocus = true;
            m_textField.bottomColor = Color.red;

            m_textField.normalBgSprite = "TextFieldPanel";
            m_textField.disabledBgSprite = "TextFieldPanelDisabled";
            m_textField.focusedBgSprite = m_textField.normalBgSprite + "Focused";
            m_textField.hoveredBgSprite = m_textField.normalBgSprite + "Hovered";

            m_textField.size = new Vector2(this.width - m_UIPadding.left - m_UIPadding.right, 35);
            m_textField.relativePosition = new Vector2(m_UIPadding.left, m_searchLabel.relativePosition.y + m_searchLabel.height + 2 * m_UIPadding.top);
            m_textField.horizontalAlignment = UIHorizontalAlignment.Left;
            m_textField.verticalAlignment = UIVerticalAlignment.Middle;
            m_textField.padding = m_UIPadding;
            m_textField.selectOnFocus = true;
            //Debug.Print ("Finsihed UISearch");

            this.eventKeyPress += this.SearchConfirm;
            this.m_textField.eventTextSubmitted += this.SearchConfirm;

            propNamesList = UIFastList.Create<PropNameRowItem>(this);
            propNamesList.backgroundSprite = "UnlockingPanel";
            propNamesList.size = new Vector2(this.width - m_UIPadding.left - m_UIPadding.right, (this.height - titleOffset - m_UIPadding.top - m_UIPadding.bottom));
            propNamesList.canSelect = false;
            propNamesList.relativePosition = new Vector2(m_UIPadding.left, m_textField.relativePosition.y + m_textField.height + 2 * m_UIPadding.top);
            propNamesList.rowHeight = 40f;
            propNamesList.rowsData.Clear();
            propNamesList.selectedIndex = -1;

            this.height = propNamesList.relativePosition.y + propNamesList.height + m_UIPadding.bottom;
            RefreshList("");
        }

        public void RefreshList(string name)
        {

            propNamesList.rowsData.Clear();
            foreach (string prop in m_allPropInfos.Where(prop => prop.ToLower().Contains(name.ToLower()) || String.IsNullOrEmpty(name)))
            {
                propNamesList.rowsData.Add(prop);
            }
            propNamesList.DisplayAt(0);
            propNamesList.selectedIndex = 0;
        }

        private void SearchConfirm(UIComponent component, UIKeyEventParameter eventParam)
        {
            if (eventParam.keycode == KeyCode.Return)
            {
                RefreshList(this.m_textField.text);
                eventParam.Use();
            }
        }

        private void SearchConfirm(UIComponent component, string text)
        {
            RefreshList(text);
        }

        private void setPropName(string message)
        {

            mLineDrawingTool.spacingDialog = spacingDialog;
            mLineDrawingTool.m_propName = message;
            mLineDrawingTool.setProp();
            mLineDrawingTool.spacingDialog.Show();

            ToolsModifierControl.toolController.CurrentTool = mLineDrawingTool;
            ToolsModifierControl.SetTool<LineDrawingTool>();
        }


        public void onReceiveEvent(string eventName, object eventData)
        {
            string message = eventData as string;
            switch (eventName)
            {
                case "setPropName":
                    setPropName(message);
                    Hide();
                    break;
                case "closeUsedNamePanel":
                    Hide();
                    break;
                case "closeAll":
                    Hide();
                    break;
                default:
                    break;
            }
        }


    }
}
