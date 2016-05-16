using BarrierPlacer.Managers;
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
        private UILabel label;

        public override void Start()
        {
            base.Start();

            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = parent.width;
            height = 40;

            background = AddUIComponent<UIPanel>();
            background.width = width;
            background.height = 40;
            background.relativePosition = Vector2.zero;
            background.zOrder = 0;

            label = this.AddUIComponent<UILabel>();
            label.textScale = 1f;
            label.size = new Vector2(width, height);
            label.textColor = new Color32(180, 180, 180, 255);
            label.relativePosition = new Vector2(0, height * 0.25f);
            label.textAlignment = UIHorizontalAlignment.Left;
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {

            base.OnMouseDown(p);
            EventBusManager.Instance().Publish("setPropName", label.text);
        }

        public void Display(object data, bool isRowOdd)
        {
            if (data != null)
            {
                string propName = data as string;

                if (propName != null && background != null)
                {
                    label.text = propName;

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
