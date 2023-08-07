using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class Movement
    {
        public bool UnlimitedStamina = true;
        public bool NoIntertia = true;
        public bool FlyHack = false;
        public KeyCode FlyHackKey = KeyCode.RightControl;
        public KeyCode FlyUpwardsKey = KeyCode.Space;
        public KeyCode FlyDownWardsKey = KeyCode.LeftControl;
        public bool NoFall = true;
        public bool HighJump = false;
        public float HighJumpAmount = 1f;
        public bool Bhop = false;
        public bool RunAndShoot = false;
        public bool InstantCrouch = true;
        public bool MedAndRun = false;
        public bool FakeProne = false;
    }
}
