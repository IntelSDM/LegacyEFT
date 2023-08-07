using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
namespace Hag.Helpers
{
    class ShaderHelper
    {
        public static Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
        public static Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        public static Dictionary<string, Texture2D> SpiritesList = new Dictionary<string, Texture2D>();
        public static List<AssetBundle> GameBundles = new List<AssetBundle>();
        public static Material GalaxyMat;
        public static Shader TransparentShader;
        
        // load all assets textures from game files
        public static void GetShader()
        {
            AssetBundle Bundle = AssetBundle.LoadFromMemory(File.ReadAllBytes("C:\\Battlestate Games\\Shaders\\Shaders.assets"));
            AssetBundle Bundle2 = AssetBundle.LoadFromMemory(File.ReadAllBytes("C:\\Battlestate Games\\Shaders\\Shader2"));
            AssetBundle Bundle3 = AssetBundle.LoadFromMemory(File.ReadAllBytes("C:\\Battlestate Games\\Shaders\\Shader3"));

            GalaxyMat = Bundle2.LoadAsset<Material>("mat12_10sdglksdg949gsgs.mat");
            TransparentShader = Bundle3.LoadAsset<Shader>("Force Field.shader");

            
            foreach (Shader s in Bundle.LoadAllAssets<Shader>())
                Shaders.Add(s.name, s);



        }
       
      

    }
}
