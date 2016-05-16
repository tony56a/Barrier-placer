using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrierPlacer.Utils
{
    class PropUtils
    {
        private static Dictionary<string, PropInfo> m_propInfoDict = new Dictionary<string, PropInfo>();

        /// <summary>
        /// Attempt to get the prop, if not loaded, then attempt to load it
        /// </summary>
        /// <param name="propName">the name of the prop to be found</param>
        /// <returns>PropInfo of prop if available, null if not available</returns>
        public static PropInfo tryGetPropInfo(string propName)
        {
            if( propName == null)
            {
                return null;
            }
            if (m_propInfoDict.ContainsKey(propName))
            {
                return m_propInfoDict[propName];
            }

            for (uint i = 0; i < PrefabCollection<PropInfo>.PrefabCount(); ++i)
            {

                if (PrefabCollection<PropInfo>.GetPrefab(i).name.Contains(propName))
                {
                    m_propInfoDict[propName] = PrefabCollection<PropInfo>.GetLoaded(i);
                    return m_propInfoDict[propName];
                }

            }
            return null;


        }
    }
}
