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
        public List<BarrierStrip> m_barrierList = new List<BarrierStrip>();

        public static BarrierManager Instance()
        {
            if (instance == null)
            {
                instance = new BarrierManager();
            }
            return instance;
        }

        public void setBarrierLine( List<Vector3> positions, string propName=null, float spacing = 0)
        {
            BarrierStrip strip = new BarrierStrip(positions,propName, spacing);
            m_barrierList.Add(strip);
        }

        public void setBarrierLine(List<float> x, List<float> y, List<float> z, string propName = null, float spacing = 0)
        {
            BarrierStrip strip = new BarrierStrip(x,y,z, propName, spacing);
            m_barrierList.Add(strip);
        }

        internal void Load(BarrierStrip[] barriers)
        {
            if (barriers != null)
            {
                foreach (BarrierStrip sign in barriers)
                {
                    setBarrierLine(sign.x,sign.y,sign.z,sign.propName,sign.spacing);
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

        public List<float> x = new List<float>();
        public List<float> y = new List<float>();
        public List<float> z = new List<float>();

        [NonSerialized]
        public List<Vector3> positions = new List<Vector3>();

        [NonSerialized]
        public GameObject m_gameObj;

        public BarrierStrip(List<float>x, List<float>y, List<float>z, string propName = null, float spacing=0)
        {

            for(int i =0; i< x.Count; i++)
            {
                Vector3 position = new Vector3(x[i], y[i], z[i]);
                this.positions.Add(position);
                this.x.Add(x[i]);
                this.y.Add(y[i]);
                this.z.Add(z[i]);
            }
            this.propName = propName;
            this.spacing = spacing;
        }

        public BarrierStrip(List<Vector3> positions,string propName=null, float spacing = 0)
        {
            foreach( Vector3 position in positions)
            {
                this.positions.Add(position);
                this.x.Add(position.x);
                this.y.Add(position.y);
                this.z.Add(position.z);

            }
            this.propName = propName;
            this.spacing = spacing;
        }
    }
}
