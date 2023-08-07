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
    class Flash : MonoBehaviour
    {
        private static DumbHook BurnEyesHook;
        private static DumbHook StunHook;
        void Start()
        {
            //https://charbase.com/e973-unicode-invalid-character
            BurnEyesHook = new DumbHook(); // the hidden char is \uE86B for updating purposes
            BurnEyesHook.Init(typeof(Player).Assembly.GetType("").GetMethod("DoBurnEyes"), typeof(Flash).GetMethod("DoBurnEyes"));
            BurnEyesHook.Hook();

            StunHook = new DumbHook(); // the hidden char is \uE86B for updating purposes
            StunHook.Init(typeof(Player).Assembly.GetType("").GetMethod("DoStun"), typeof(Flash).GetMethod("DoStun"));
            StunHook.Hook();
        }
        public void DoBurnEyes(Vector3 position, float distanceStrength, float normalTime)
        {
            if (Globals.Config.Visuals.NoFlash)
            {
                position = Vector3.zero;
                distanceStrength = 0;
            }
            BurnEyesHook.Unhook();


            object[] parameters = new object[]
               {
                    position,
                    distanceStrength,
                    normalTime

               };
            object result = BurnEyesHook.OriginalMethod.Invoke(this, parameters);

            BurnEyesHook.Hook();
        }
        public void DoStun(float maxTime, float normalizedStrength)
        {
            if (Globals.Config.Visuals.NoFlash)
            {
                maxTime = 0;
                normalizedStrength = 0;
            }
            StunHook.Unhook();


            object[] parameters = new object[]
               {
                    maxTime,
                    normalizedStrength,
                    

               };
            object result = StunHook.OriginalMethod.Invoke(this, parameters);

            StunHook.Hook();
        }
    }
}
