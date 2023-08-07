using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using EFT.InventoryLogic;
namespace Hag.Hooks
{
    class SetSearchedStatus : MonoBehaviour
    {
        private static DumbHook SearchHook;
        void Start()
        {
            SearchHook = new DumbHook(); // the hidden char is \uE003 for updating purposes
            SearchHook.Init(typeof(EFT.UI.DragAndDrop.SearchableView).GetMethod("", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance), typeof(SetSearchedStatus).GetMethod("SetSearchStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
            SearchHook.Hook();
        }
        private void SetSearchStatus(SearchedState searchState)
        {
            if(Globals.Config.LocalPlayerWorld.BypassSearch)
            searchState = SearchedState.FullySearched;
            SearchHook.Unhook();


            object[] parameters = new object[]
               {
                    searchState,
                    
               };
            object result = SearchHook.OriginalMethod.Invoke(this, parameters);

            SearchHook.Hook();
        }
    }
}
