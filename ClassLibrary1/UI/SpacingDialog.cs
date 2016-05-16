using BarrierPlacer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrierPlacer.UI
{
    class SpacingDialog : SliderDialog
    {
        public override int defaultValue
        {
            get
            {
                return 0;
            }
        }

        public override string descText
        {
            get
            {
                return "Prop Spacing";
            }
        }

        public override float intervalVal
        {
            get
            {
                return 0.1f;
            }
        }

        public override int maxVal
        {
            get
            {
                return 10;
            }
        }

        public override int minVal
        {
            get
            {
                return 0;
            }
        }

        public override void SliderSetValue(float value)
        {
            EventBusManager.Instance().Publish("setSpacing", value);
        }
    }
}
