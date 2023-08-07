using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class LocalPlayerInfo
    {
        public bool Enable = true;
        public int x = 30;
        public int y = 800;
        public bool Crosshair = true;
        public bool Radar = true;
        public int Radarx = Screen.width - 110;
        public int Radary = 110;
        public int RadarMaxDistance = 300;
        public int RadarSize = 100;
    }
}
