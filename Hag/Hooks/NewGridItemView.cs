using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using JetBrains.Annotations;
namespace Hag.Hooks
{
    class NewGridItemView : MonoBehaviour
    {
        private static DumbHook ItemViewHook;
        void Start()
        {
            ItemViewHook = new DumbHook();
            ItemViewHook.Init(typeof(EFT.UI.DragAndDrop.GridItemView).GetMethod("NewGridItemView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance), typeof(NewGridItemView).GetMethod("NewGridItemViewHook", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance));
            ItemViewHook.Hook();
        }
        // means each item doesn't need to be searched
        object NewGridItemViewHook(Item item, object sourceContext, ItemRotation rotation, object itemController, IItemOwner itemOwner, [CanBeNull] EFT.UI.FilterPanel filterPanel, [CanBeNull] object container, [CanBeNull] EFT.UI.ItemUiContext itemUiContext, object insurance, bool isSearched = true)
        {
            if(Globals.Config.LocalPlayerWorld.FastSearch)
            isSearched = true;

            ItemViewHook.Unhook();


            object[] parameters = new object[]
               {
                    item,
                    sourceContext,
                    rotation,
                    itemController,
                    itemOwner,
                    filterPanel,
                    container,
                    itemUiContext,
                    insurance,
                    isSearched,
               };
            object result = ItemViewHook.OriginalMethod.Invoke(this, parameters);
            
            ItemViewHook.Hook();
            return result;
        }
    }
}
