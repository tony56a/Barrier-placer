using BarrierPlacer.Managers;
using BarrierPlacer.Utils;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.UI
{
    class PropNameRowItem : UIPanel, IUIFastListRow
    {
        private UIPanel background;
        private UIButton button;
        private UILabel label;
        private PrefabInfo prop;

        public override void Start()
        {
            base.Start();

            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = parent.width;
            height = 100;

            background = AddUIComponent<UIPanel>();
            background.width = width;
            background.height = 100;
            background.relativePosition = Vector2.zero;
            background.zOrder = 0;

            button = this.AddUIComponent<UIButton>();
            button.width = 80;
            button.height = 70;
            
            button.disabledTextColor = new Color32(7, 7, 7, 255);
            button.hoveredTextColor = new Color32(7, 132, 255, 255);
            button.focusedTextColor = new Color32(255, 255, 255, 255);
            button.pressedTextColor = new Color32(30, 30, 44, 255);
            button.eventClick += button_eventClick;
            button.relativePosition = new Vector3(0, 5);

            label = this.AddUIComponent<UILabel>();
            label.textScale = 1f;
            label.size = new Vector2(width, height);
            label.textColor = new Color32(180, 180, 180, 255);
            label.relativePosition = new Vector2(button.width + 15,0.25f*this.height);
            label.textAlignment = UIHorizontalAlignment.Left;
        }

        private void button_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            LoggerUtils.Log("PropNameRowItem Selected:" + label.text);
            EventBusManager.Instance().Publish("setPropName", label.text);
        }

        public void Display(object data, bool isRowOdd)
        {
            if (data != null)
            {
                prop = data as PrefabInfo;

                if (prop != null && background != null)
                {
                    label.text = prop.GetLocalizedTitle();

                    string thumbnail = prop.m_Thumbnail;
                    if (string.IsNullOrEmpty(prop.m_Thumbnail) || prop.m_Atlas[prop.m_Thumbnail] == (UITextureAtlas.SpriteInfo)null)
                    {
                        thumbnail = "ThumbnailBuildingDefault";
                        UIButton buttonTemplate = UITemplateManager.Get("PlaceableItemTemplate") as UIButton;
                        button.atlas = buttonTemplate.atlas;
                        button.width = 80;
                        button.height = 70;
                    }
                    else
                    {
                        button.atlas = prop.m_Atlas;
                    }
                    
                    button.normalBgSprite = "ButtonMenu";
                    button.disabledBgSprite = "ButtonMenuDisabled";
                    button.hoveredBgSprite = "ButtonMenuHovered";
                    button.focusedBgSprite = "ButtonMenuFocused";
                    button.pressedBgSprite = "ButtonMenuPressed";
                    button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
                    button.normalFgSprite = thumbnail;
                    button.focusedFgSprite = thumbnail + "Focused";
                    button.hoveredFgSprite = thumbnail + "Hovered";
                    button.pressedFgSprite = thumbnail + "Pressed";
                    button.disabledFgSprite = thumbnail + "Disabled";

                    if (isRowOdd)
                    {
                        background.backgroundSprite = "UnlockingItemBackground";
                        background.color = new Color32(0, 0, 0, 128);
                    }
                    else
                    {
                        background.backgroundSprite = null;
                    }
                }
            }
        }

        public void Select(bool isRowOdd)
        {
            if (background != null)
            {
                /*background.backgroundSprite = "ListItemHighlight";
                background.color = new Color32(255, 255, 255, 255);*/
            }
        }

        public void Deselect(bool isRowOdd)
        {
            if (background != null)
            {
                if (isRowOdd)
                {
                    background.backgroundSprite = "UnlockingItemBackground";
                    background.color = new Color32(0, 0, 0, 128);
                }
                else
                {
                    background.backgroundSprite = null;
                }
            }
        }

    }
}
