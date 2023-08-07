using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using EFT.Interactive;
using EFT;
namespace Hag.Hooks
{
    class LimitCheck : MonoBehaviour
    {
        private static DumbHook LimitHook;
        void Start()
        {
            LimitHook = new DumbHook();
            LimitHook.Init(typeof(Player).GetMethod("HasDiscardLimits"), typeof(LimitCheck).GetMethod("AntiCheck"));
            LimitHook.Hook();
        }

        private bool AntiCheck()
        {
            return false;
        }
    }
}
