using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EFT;
using EFT.Interactive;
using Hag.Helpers;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.UI;
using System.Windows.Forms;
using System.Reflection;
using Hag.Esp;
using System.Threading;
using System.Reflection.Emit;
using EFT.InventoryLogic;

namespace Hag
{
    class Globals : MonoBehaviour
    {

    
        public static Camera MainCamera;
        public static EFT.InventoryLogic.Weapon LocalPlayerWeapon;
        public static EFT.Player LocalPlayer;
        public static GameWorld GameWorld;
        public static int LocalPlayerValue;


        public static bool EndedFrame = true;
        public static Hag.Configs.Config Config = new Configs.Config();
        public static float FlyHackValue = 0;

        public static Vector3 test123;

        public static List<BasePlayer> PlayerList = new List<BasePlayer>();
        public static List<BaseExfil> ExfilList = new List<BaseExfil>();
        public static List<BaseItem> LootList = new List<BaseItem>();
        public static List<BaseCorpse> CorpseList = new List<BaseCorpse>();
        public static List<BaseContainer> ContainerList = new List<BaseContainer>();
        public static List<BaseGrenade> GrenadeList = new List<BaseGrenade>();
        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return screenPoint.z > 0.01f && screenPoint.x > -5f && screenPoint.y > -5f && screenPoint.x < (float)UnityEngine.Screen.width && screenPoint.y < (float)UnityEngine.Screen.height;
        }

        void Start()
        {
            Esp.Drawing drawing = new Esp.Drawing();
            drawing.Start();
            Helpers.ConfigHelper.CreateEnvironment();
            Helpers.ColourHelper.AddColours();
            ShaderHelper.GetShader();
        }
       

        public static bool Failed(FilesChecker.ICheckResult result)
        {
            return false;
        }
        public static bool Succeed(FilesChecker.ICheckResult result)
        {
            return true;
        }
    }
}
