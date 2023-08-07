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
using EFT;
namespace Hag.Hooks
{
    class PhysicalDamage : MonoBehaviour
    {

        private static DumbHook PhysicalCondition;
        private static DumbHook DamageHook;
        void Start()
        {
            PhysicalCondition = new DumbHook(); // ue615
            PhysicalCondition.Init(typeof(Player).GetMethod("ApplyHitDebuff"), typeof(PhysicalDamage).GetMethod("ApplyHitDebuff"));
            PhysicalCondition.Hook();

   //         DamageHook = new DumbHook(); // ue80f
     //       DamageHook.Init(typeof(Player).Assembly.GetType("").GetMethod("ApplyDamage"), typeof(PhysicalDamage).GetMethod("ApplyDamage"));
       //     DamageHook.Hook();
        }
        public virtual void ApplyHitDebuff(float damage, float staminaBurnRate, EBodyPart bodyPartType, EDamageType damageType)
        {
            if (!Globals.Config.LocalPlayerWorld.NoPhysicalDamage)
            {
                PhysicalCondition.Unhook();


                object[] parameters = new object[]
                   {
                damage,
                staminaBurnRate,
                bodyPartType,
                damageType

                   };
                object result = PhysicalCondition.OriginalMethod.Invoke(this, parameters);

                PhysicalCondition.Hook();
            }
        }
        public object ApplyDamage(EBodyPart bodyPart, float damage, object damageInfo)
        {
        /*    if (Globals.Config.LocalPlayerWorld.NoPhysicalDamage)
                return 0;
            else
            {*/
                DamageHook.Unhook();


                object[] parameters = new object[]
                   {
                bodyPart,
                damage,
                damageInfo,


                   };
                object result = DamageHook.OriginalMethod.Invoke(this, parameters);

                DamageHook.Hook();
                return result;
           // }
        }
    }
}
