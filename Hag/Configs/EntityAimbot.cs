using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class EntityAimbot
    {
        public bool StaticAimbot = true;
        public bool SilentAimbot = true;
        public int MaxDistance = 300;
        public int HitBone = 1; // 1 = all, 2 = head, 3 = chest, 4 = legs

        public bool RageAimbot = true;
        public int RageAimbotHitchance = 80;
        public bool AutoShoot = false;
        public int AutoShootMaxDistance = 150;

        public bool LegitAimbot = true;
        public int LegitSmoothing = 50;
        public int LegitHitbox = 1;
        public int LegitAimbotMaxDistance = 350;
        public bool LegitVischecks = true;

        public bool SilentMelee = true;
    }
}
