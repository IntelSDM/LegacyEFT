using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Helpers
{
    class ColourHelper
    {
        #region RGBColours
        public static int Cases = 0;
        public static float R = 1.00f;
        public static float G = 0.00f;
        public static float B = 1.00f;
        #endregion
        public static void AddColours()
        {
            AddColour("Menu Primary Colour", Color.red);
            AddColour("Menu Secondary Colour", Color.white);
            AddColour("Radar Colour", new Color32(10, 10, 10, 180));

            AddColour("Item Colour", Color.white);
            AddColour("Keycard Item Colour", Color.magenta);
            AddColour("Rare Item Colour", Color.blue);
            AddColour("Super Rare Item Colour", Color.cyan);
            AddColour("Whitelisted Item Colour", Color.red);
            AddColour("Quest Item Colour", Color.yellow);

            AddColour("Scav Colour", new Color32(0, 255, 255, 255));
            AddColour("Scav Boss Colour", new Color32(0, 119, 255, 255));
            AddColour("Scav Box Colour", Color.red);
            AddColour("Scav Player Colour", new Color32(255, 0, 22, 255));
            AddColour("Scav Player Box Colour", Color.red);
            AddColour("Player Colour", Color.red);
            AddColour("Player Box Colour", Color.red);
            AddColour("Team Colour", new Color32(0, 255, 0, 255));
            AddColour("Chams Visible Colour", Color.red);
            AddColour("Chams Transparent Colour", Color.red);
            AddColour("Chams Invisible Colour", Color.white);
            AddColour("Hand Chams Colour", new Color32(0, 175, 255, 200));
            AddColour("Filled Box Colour", new Color32(0, 0, 0, 95));



            AddColour("Exfil Colour", Color.green);
            AddColour("Container Colour", new Color32(230, 255, 0, 255));
            AddColour("Corpse Colour", new Color32(128, 0, 255, 255));

            
        }
        public static Color32 GetColour(string identifier)
        {
            if (Globals.Config.Colours.GlobalColors.TryGetValue(identifier, out var toret))
                return toret;
            return Color.magenta;
        }

        public static void AddColour(string id, Color32 c)
        {
            if (!Globals.Config.Colours.GlobalColors.ContainsKey(id))
                Globals.Config.Colours.GlobalColors.Add(id, c);
        }

        public static void SetColour(string id, Color32 c) => Globals.Config.Colours.GlobalColors[id] = c;

        public static string ColourToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
    }
}
