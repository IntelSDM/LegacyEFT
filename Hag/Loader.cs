using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Hag;
using Hag.Esp;
using System.Threading;
using System.IO;
using Hag.Helpers;

namespace Hag
{
    public static class Loader
    {

        public static void Load()
        {


            DumbHook FileAvaliability = new DumbHook();

            FileAvaliability.Init(
                   typeof(FilesChecker.CheckResultExtension).GetMethod(
                       "Succeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static),
                   typeof(Globals).GetMethod(
                       "Succeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                   );
            FileAvaliability.Hook();
            DumbHook FileFailed = new DumbHook();

            FileFailed.Init(
                   typeof(FilesChecker.CheckResultExtension).GetMethod(
                       "Failed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static),
                   typeof(Globals).GetMethod(
                       "Failed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                   );
            FileFailed.Hook();





            GameObject.DontDestroyOnLoad(HackObject);


            HackObject.AddComponent<Globals>();
            HackObject.AddComponent<Hag.Esp.Caching>();
            HackObject.AddComponent<Hag.Esp.Updating>();
            HackObject.AddComponent<Hag.Menu.RenderMenu>();
            HackObject.AddComponent<Hag.Aimbot.LegitAimbot>();

            HackObject.AddComponent<Hag.Hooks.CreateShot>();
            HackObject.AddComponent<Hag.Hooks.FindInteractable>();
            HackObject.AddComponent<Hag.Hooks.SetLocked>();
            HackObject.AddComponent<Hag.Hooks.NewGridItemView>();
            HackObject.AddComponent<Hag.Hooks.Show>();
            HackObject.AddComponent<Hag.Hooks.SetSearchedStatus>();
            HackObject.AddComponent<Hag.Hooks.SeeSlot>();
            HackObject.AddComponent<Hag.Hooks.Flash>();
            HackObject.AddComponent<Hag.Hooks.SessionID>();
            HackObject.AddComponent<Hag.Hooks.SuitibleForHandInput>();
            HackObject.AddComponent<Hag.Hooks.PhysicalDamage>();
            HackObject.AddComponent<Hag.Hooks.GarbageCollector>();



            GameObject.DontDestroyOnLoad(HackObject);

        }
        private static GameObject HackObject = new GameObject();
    }
}
