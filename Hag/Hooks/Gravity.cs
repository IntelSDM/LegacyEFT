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
    class Gravity : MonoBehaviour
    {
        private static DumbHook SearchHook;
        void Start()
        {
            SearchHook = new DumbHook(); // the hidden char is \uE003 for updating purposes
            SearchHook.Init(typeof(Player).Assembly.GetType("").GetMethod("ApplyGravity"), typeof(Gravity).GetMethod("ApplyGravity"));
            SearchHook.Hook();
        }
        public virtual void ApplyGravity(ref Vector3 motion, float deltaTime, bool stickToGround)
        {
            motion = Vector3.zero;
            stickToGround = true;

            SearchHook.Unhook();


            object[] parameters = new object[]
               {
                    motion,
                    deltaTime,
                    stickToGround

               };
            object result = SearchHook.OriginalMethod.Invoke(this, parameters);

            SearchHook.Hook();
        }
    }
}
