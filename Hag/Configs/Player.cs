using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Player
    {
        public bool Enable = true;
        public bool Box = true;
        public bool RoundedBox = false;
        public bool FillBox = false;
        public bool Distance = true;
        public bool Name = true;
        public bool KD = true;
        public bool Level = true;
        public bool Value = true;
        public bool HealthBar = true;
        public bool Weapon = true;
        public bool Ammo = true;
        public bool Flag = true;
        public bool Chams = true;
        public bool ChamGear = false;
        public bool ChamTopmost = true;
        public bool ChamRGB = false;
        public int ChamType = 0;

        public int MaxDistance = 2000;
    }
}
