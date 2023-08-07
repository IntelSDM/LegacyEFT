using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using EFT.InventoryLogic;
using EFT;
namespace Hag.Hooks
{
    class SeeSlot : MonoBehaviour
    {
        private static DumbHook SeeSlotHook;
        void Start()
        {
            //https://charbase.com/e9c2-unicode-invalid-character
            SeeSlotHook = new DumbHook(); // the hidden char is \ue9c2 for updating purposes
            SeeSlotHook.Init(typeof(Player).Assembly.GetType("").GetMethod("IsAllowedToSeeSlot"), typeof(SeeSlot).GetMethod("IsAllowedToSeeSlot"));
            SeeSlotHook.Hook();

        }

        public virtual bool IsAllowedToSeeSlot(Slot slot, EquipmentSlot slotName)
        {
            if (Globals.Config.LocalPlayerWorld.BypassSearch)
            {
                return true;
            }
            SeeSlotHook.Unhook();


            object[] parameters = new object[]
               {
                    slot,
                    slotName,
               

               };
            object result = SeeSlotHook.OriginalMethod.Invoke(this, parameters);

            SeeSlotHook.Hook();
            return true;
        }
 
    }
}
