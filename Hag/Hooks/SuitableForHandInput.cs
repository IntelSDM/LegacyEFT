using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;

namespace Hag.Hooks
{
    class SuitibleForHandInput : MonoBehaviour
    {

        private static DumbHook HandsHook;
        void Start()
        {
            HandsHook = new DumbHook();
            HandsHook.Init(typeof(EFT.Player).GetMethod("get_StateIsSuitableForHandInput", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance), typeof(SuitibleForHandInput).GetMethod("StateIsSuitableForHandInput", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
            HandsHook.Hook();
        }
        public bool StateIsSuitableForHandInput()
        {
            if (Globals.Config.Movement.RunAndShoot)
                return true;
            else
                return Array.IndexOf<EPlayerState>(EFTHardSettings.Instance.UnsuitableStates, Globals.LocalPlayer.CurrentState.Name) < 0;

        }
    }
}
