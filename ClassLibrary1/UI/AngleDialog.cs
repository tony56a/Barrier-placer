using BarrierPlacer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.UI
{
    class AngleDialog : SliderDialog
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
                return "Angle";
            }
        }

        public override float intervalVal
        {
            get
            {
                return 5f;
            }
        }

        public override int maxVal
        {
            get
            {
                return 360;
            }
        }

        public override int minVal
        {
            get
            {
                return 0;
            }
        }

        public override Vector3 posOffset
        {
            get
            {
                return new Vector3(0, -120, 0);
            }
        }

        public override void SliderSetValue(float value)
        {
            EventBusManager.Instance().Publish("setAngle", value);
        }
    }
}
