using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class PlayerInformation
    {
        public bool Enable = true;
        public bool ClosestToCrosshair = true;
        public int X = Screen.width - 210;
        public int Y = 250;
        public bool AutoSizeToRadar = true;
        public bool IgnoreTeam = true;
    }
}
