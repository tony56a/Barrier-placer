using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace BarrierPlacer.Manager
{
    class BarrierManager
    {
        private static BarrierManager instance = null;
        public List<NewBarrierStrip> m_barrierList = new List<NewBarrierStrip>();

        public static BarrierManager Instance()
        {
            if (instance == null)
            {
                instance = new BarrierManager();
            }
            return instance;
        }

        public void setBarrierLine(List<Vector3> positions, string propName = null, float spacing = 0, float angle = 0, float size=1, string extraString=null)
        {
            NewBarrierStrip strip = new NewBarrierStrip(positions, propName, spacing, angle,size, extraString);
            m_barrierList.Add(strip);
        }

        public void setBarrierLine(List<float> x, List<float> y, List<float> z, string propName = null, float spacing = 0, float angle = 0, float size = 1, string extraString = null)
        {
            NewBarrierStrip strip = new NewBarrierStrip(x, y, z, propName, spacing, angle,size,extraString);
            m_barrierList.Add(strip);
        }

        internal void Load(BarrierStrip[] barriers)
        {
            if (barriers != null)
            {
                foreach (BarrierStrip sign in barriers)
                {
                    setBarrierLine(sign.x, sign.y, sign.z, sign.propName, sign.spacing, sign.rotationOffset);
                }
            }
        }

        internal void Load(NewBarrierStrip[] barriers)
        {
            if (barriers != null)
            {
                foreach (NewBarrierStrip barrier in barriers)
                {
                    setBarrierLine(barrier.x, barrier.y, barrier.z, barrier.propName, barrier.spacing, barrier.rotationOffset, barrier.sizeOffset,barrier.extensionString);
                }
            }
        }
    }

    [Serializable]
    public class BarrierStrip
    {
        [XmlElement(IsNullable = true)]
        public string propName = null;

        [XmlElement(IsNullable = true)]
        public float spacing = 0;

        [XmlElement(IsNullable = true)]
        public float rotationOffset = 0;

        public List<float> x = new List<float>();
        public List<float> y = new List<float>();
        public List<float> z = new List<float>();

        [NonSerialized]
        public List<Vector3> positions = new List<Vector3>();

        [NonSerialized]
        public GameObject m_gameObj = null;

        public BarrierStrip(List<float> x, List<float> y, List<float> z, string propName = null, float spacing = 0, float angle = 0)
        {

            for (int i = 0; i < x.Count; i++)
            {
                Vector3 position = new Vector3(x[i], y[i], z[i]);
                this.positions.Add(position);
                this.x.Add(x[i]);
                this.y.Add(y[i]);
                this.z.Add(z[i]);
            }
            this.propName = propName;
            this.spacing = spacing;
            this.rotationOffset = angle;
        }

        public BarrierStrip(List<Vector3> positions, string propName = null, float spacing = 0, float angle = 0)
        {
            foreach (Vector3 position in positions)
            {
                this.positions.Add(position);
                this.x.Add(position.x);
                this.y.Add(position.y);
                this.z.Add(position.z);

            }
            this.propName = propName;
            this.spacing = spacing;
            this.rotationOffset = angle;
        }

        //Just to keep the compiler from complaining
        public BarrierStrip()
        {

        }
    }

    /// <summary>
    /// Workaround for MemoryStream not allowing for schema to be updated
    /// </summary>
    [Serializable]
    public class NewBarrierStrip : BarrierStrip
    {
        [XmlElement(IsNullable = true)]
        public float sizeOffset;

        /// <summary>
        /// Just something to stick new ideas into, requires parsing first
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string extensionString;

        public NewBarrierStrip(List<Vector3> positions, string propName = null, float spacing = 0, float angle = 0,float size=0, string extras=null)
        {
            
            foreach (Vector3 position in positions)
            {
                this.positions.Add(position);
                this.x.Add(position.x);
                this.y.Add(position.y);
                this.z.Add(position.z);

            }
            this.propName = propName;
            this.spacing = spacing;
            this.rotationOffset = angle;

            this.sizeOffset = size;
            this.extensionString = extras;
        }

        public NewBarrierStrip(List<float> x, List<float> y, List<float> z, string propName = null, float spacing = 0, float angle = 0,float size = 0, string extras = null)
        {

            for (int i = 0; i < x.Count; i++)
            {
                Vector3 position = new Vector3(x[i], y[i], z[i]);
                this.positions.Add(position);
                this.x.Add(x[i]);
                this.y.Add(y[i]);
                this.z.Add(z[i]);
            }
            this.propName = propName;
            this.spacing = spacing;
            this.rotationOffset = angle;

            this.sizeOffset = size;
            this.extensionString = extras;
        }
    }
}
