using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Weapons
    {
        public bool NoRecoil = true;
        public float NoRecoilAmount = 0.3f;
        public bool NoSway = true;
        public float NoSwayAmount = 0f;
        public bool NoMalfunction = true;
        public float NoMalfunctionChance = 0f;
        public bool FullAuto = true;
        public bool FastFire = false;
        public int FastFireRate = 1000;
    }
}
