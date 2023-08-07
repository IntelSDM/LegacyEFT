using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class Aimbot
    {
        public bool TargetTeam = false;
        public bool DrawFov = true;
        public int Fov = 200;
        public KeyCode AimbotKey = KeyCode.Mouse3;
        public bool AutoWall = true;
        public bool InstantHit = false;
        public int AutoWallPasses = 4;
        public bool IgnoreFov = false;
    }
}
