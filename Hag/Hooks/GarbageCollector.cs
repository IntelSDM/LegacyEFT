using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT;
using Hag.Helpers;
using UnityEngine.Scripting;

namespace Hag.Hooks
{
    class GarbageCollector : MonoBehaviour
    {
        DumbHook Hook = new DumbHook();
        void Start()
        {
            Hook.Init(typeof(Player).Assembly.GetType("").GetMethod("GCEnabled", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static), typeof(GarbageCollector).GetMethod("GCEnabled", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static));
            Hook.Hook();
        }
        public static bool GCEnabled()
        {
            UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
            return true;
        }
    }
}
