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
    class SetLocked : MonoBehaviour
    {

        private static DumbHook LockedHook;
        private static DumbHook LockedHook2;
        void Start()
        {
            LockedHook = new DumbHook();
            LockedHook.Init(typeof(EFT.UI.DragAndDrop.ModSlotView).GetMethod("SetLocked",System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance), typeof(SetLocked).GetMethod("SetLockedHook", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
            LockedHook.Hook();

            LockedHook2 = new DumbHook();
            LockedHook2.Init(typeof(EFT.UI.WeaponModding.ModdingScreenSlotView).GetMethod("SetLockedStatus", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance), typeof(SetLocked).GetMethod("SetLockedStatusHook", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
            LockedHook2.Hook();
        }
        public void SetLockedHook(KeyValuePair<EModLockedState, string> lockedState, Item item)
        {
            KeyValuePair<EModLockedState, string> NewLockedState = new KeyValuePair<EModLockedState, string>(lockedState.Key, lockedState.Value);
            if (Globals.Config.LocalPlayerWorld.UnlockMods)
            {
               NewLockedState = new KeyValuePair<EModLockedState, string>(EModLockedState.Unlocked, lockedState.Value);
            }
            
            LockedHook.Unhook();


            object[] parameters = new object[]
               {
                    NewLockedState,
                    item,
               };
            object result = LockedHook.OriginalMethod.Invoke(this, parameters);

            LockedHook.Hook();
        }
        public void SetLockedStatusHook(bool locked)
        {
        
            if (Globals.Config.LocalPlayerWorld.UnlockMods)
            {
                locked = false;
            }

            LockedHook.Unhook();


            object[] parameters = new object[]
               {
                    locked,
                    
               };
            object result = LockedHook2.OriginalMethod.Invoke(this, parameters);

            LockedHook2.Hook();
        }
    }
}
