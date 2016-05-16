using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrierPlacer
{
    public class BarrierPlacerMod : IUserMod
    {
        public string Name
        {
            get
            {
                return "Barrier Placer";
            }
        }

        public string Description
        {
            get
            {
                return "A mod that lets you draw lines of barriers";
            }
        }
    }
}
