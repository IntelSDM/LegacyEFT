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
    class SessionID : MonoBehaviour
    {

        private static DumbHook SessionHook;
        void Start()
        {
            SessionHook = new DumbHook();
            SessionHook.Init(typeof(EFT.UI.PreloaderUI).GetMethod("SetSessionId", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance), typeof(SessionID).GetMethod("SetSessionId", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
            SessionHook.Hook();
        }
        public void SetSessionId(string id)
        {
            id = "Hidden";

            SessionHook.Unhook();


            object[] parameters = new object[]
               {
                  id
                    
               };
            object result = SessionHook.OriginalMethod.Invoke(this, parameters);

            SessionHook.Hook();
        }
    }
}
