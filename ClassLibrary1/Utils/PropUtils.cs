using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.Utils
{
    class PropUtils
    {
        private static Dictionary<string, PrefabInfo> m_propInfoDict = new Dictionary<string, PrefabInfo>();

        /// <summary>
        /// Attempt to get the prop, if not loaded, then attempt to load it
        /// </summary>
        /// <param name="propName">the name of the prop to be found</param>
        /// <returns>PropInfo of prop if available, null if not available</returns>
        public static PrefabInfo tryGetPropInfo(string propName)
        {
            if( propName == null )
            {
                return null;
            }
            if (m_propInfoDict.ContainsKey(propName))
            {
                return m_propInfoDict[propName];
            }
            List<PrefabInfo> m_allPropInfos = Resources.FindObjectsOfTypeAll<PrefabInfo>().Where(prefabInfo =>
                    prefabInfo.GetType().Equals(typeof(PropInfo)) ||
                    prefabInfo.GetType().Equals(typeof(TreeInfo))).ToList();
            for (int i = 0; i < m_allPropInfos.Count(); ++i)
            {

                if (m_allPropInfos[i].name.Contains(propName))
                {
                    m_propInfoDict[propName] = m_allPropInfos[i];
                    return m_propInfoDict[propName];
                }

            }
            return null;


        }
    }
}
