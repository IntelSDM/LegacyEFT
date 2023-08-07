using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT.InventoryLogic;
using Hag.Helpers;

namespace Hag.Hooks
{
    class Show : MonoBehaviour
    {
        private static DumbHook InteractHook;
        void Start()
        {
            InteractHook = new DumbHook();
            InteractHook.Init(typeof(EFT.UI.DragAndDrop.SearchableView).GetMethod("HideContents", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance), typeof(Show).GetMethod("HideContents", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
            InteractHook.Hook();
        }
        public void HideContents()
        { }
        public void ShowHook(Slot slot, object parentItemContext, object inventoryController, EFT.UI.ItemUiContext itemUiContext, object skills, object insurance)
        { 
        
        }
    }
}
