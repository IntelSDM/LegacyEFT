using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class Corpse
    {
        public bool Enable = true;
        public int MaxDistance = 200;
        public int MinValue = 40000;
        public int ContentMinValue = 50000;
        public bool Distance = true;
        public bool Tag = true;
        public bool Value = true;
        public KeyCode ContentsKey = KeyCode.X;
        public bool DrawCorpseContents = true;
    }
}
