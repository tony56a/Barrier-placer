using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BarrierPlacer.Utils
{
    public static class VectorUtils
    {
        /// <summary>
        /// Gets the distance from a point to a line segment defined by 2 other vector points, taken from: http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float minimumDistance(Vector3 start, Vector3 end, Vector3 position)
        {
            // Return minimum distance between line segment start->end and point p
            float squaredLength = Vector3.SqrMagnitude(end - start);  // i.e. |end-start|^2 -  avoid a sqrt
            if (Mathf.Approximately(squaredLength, 0.0f))
            {
                return Vector3.Distance(position, start);   // start == end case
            }
            // Consider the line extending the segment, parameterized as start + t (end - start).
            // We find projection of point p onto the line. 
            // It falls where t = [(position-start) . (end-start)] / |end-start|^2
            // We clamp t from [0,1] to handle points outside the segment start->end.
            float t = Mathf.Max(0, Mathf.Min(1, Vector3.Dot(position - start, end - start) / squaredLength));
            Vector3 projection = start + t * (end - start);  // Projection falls on the segment
            return Vector3.Distance(position, projection);
        }
    }
}
