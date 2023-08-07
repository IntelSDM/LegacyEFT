using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hag.Configs
{
    class Container
    {
        public bool Enable = true;
        public int MaxDistance = 800;
        public int MinValue = 60000;
        public int ContentMinValue = 50000;
        public bool Distance = true;
        public bool Tag = true;
        public bool Value = true;
        public KeyCode ContentsKey = KeyCode.Z;
        public bool DrawContents = true;
        public bool OnlyDrawWithSubItems = true;
    }
}
