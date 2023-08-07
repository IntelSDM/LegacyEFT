using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EFT;
using Hag.Renderer;
using SharpDX;
using SharpDX.Direct2D1;
using UnityEngine;
using EFT.Interactive;
using System.IO;
namespace Hag.Esp
{
    class Drawing
    {
        #region import
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        #endregion
        public static OverlayWindow Overlay;
        private static IntPtr GameWindow;
        Direct2DRenderer Renderer;
        private static int DrawTime;
        public const string WINDOW_NAME = "EscapeFromTarkov";
        Rect windowSize = new Rect();
        public static Direct2DColor CrosshairColor;
        Direct2DBrush whiteSolid;
        Direct2DFont infoFont;
        Direct2DFont Tahoma;
        Direct2DFont Tahoma2;
        Direct2DFont Tahoma3;
        Direct2DFont Tahoma4;
        public void Start()
        {
           
                GameWindow = FindWindow(null, WINDOW_NAME);
          
            GetWindowRect(GameWindow, ref windowSize);
            OverlayCreationOptions overlayOptions = new OverlayCreationOptions()
            {
                BypassTopmost = true,
                Height = windowSize.Bottom - windowSize.Top,
                Width = windowSize.Right - windowSize.Left,
                WindowTitle = HelperMethods.GenerateRandomString(5, 11),
                X = windowSize.Left,
                Y = windowSize.Top
            };

            StickyOverlayWindow overlay = new StickyOverlayWindow(GameWindow, overlayOptions);
            Overlay = overlay;
            var rendererOptions = new Direct2DRendererOptions()
            {
                AntiAliasing = true,
                Hwnd = overlay.WindowHandle,
                MeasureFps = true,
                VSync = false

            };

            Renderer = new Direct2DRenderer(rendererOptions);
            whiteSolid = Renderer.CreateBrush(255, 255, 255, 255);
            //   var RedSolid = Renderer.CreateBrush(255, 0, 0, 200);
            infoFont = Renderer.CreateFont("Consolas", 11);
            Tahoma = Renderer.CreateFont("Tahoma", 10);
            Tahoma2 = Renderer.CreateFont("Tahoma", 9);
            Tahoma3 = Renderer.CreateFont("Tahoma", 12);
            Tahoma4 = Renderer.CreateFont("Tahoma", 10);
            new Thread(delegate ()
            {
                
                Render();
            }).Start();
        }
        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return screenPoint.z > 0.01f && screenPoint.x > -5f && screenPoint.y > -5f && screenPoint.x < (float)Screen.width && screenPoint.y < (float)Screen.height;
        }

        // we call this to jump to it to prevent code breaking when calling break, ironic right?
        private void DoNothing()
        { }
   
        void DrawRadarEntity(BasePlayer BasePlayer, Direct2DRenderer Renderer)
        {
            try
            {
                if (!Globals.Config.LocalPlayerInfo.Radar)
                    return;
                EFT.Player player = BasePlayer.Player;
                if (player == null)
                    return;
                if (!player.HealthController.IsAlive)
                    return;

                bool Scav = player.Profile.Info.RegistrationDate <= 0;
                bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;

                Vector3 centerPos = Globals.LocalPlayer.Transform.position;
                Vector3 extPos = player.Transform.position;

                float dist = Vector3.Distance(centerPos, extPos);

                float dx = centerPos.x - extPos.x;
                float dz = centerPos.z - extPos.z;

                float deltay = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg - 270 - Globals.LocalPlayer.Transform.eulerAngles.y;

                float bX = dist * Mathf.Cos(deltay * Mathf.Deg2Rad);
                float bY = dist * Mathf.Sin(deltay * Mathf.Deg2Rad);
                // 400 is the max distance
                // 250 is radius
                bX = bX * ((float)Globals.Config.LocalPlayerInfo.RadarSize / (float)Globals.Config.LocalPlayerInfo.RadarMaxDistance) / 2f;
                bY = bY * ((float)Globals.Config.LocalPlayerInfo.RadarSize / (float)Globals.Config.LocalPlayerInfo.RadarMaxDistance) / 2f;
         //       if (dist > Globals.Config.LocalPlayerInfo.RadarSize *5)
     //               return;
                if (Vector2.Distance(new Vector2(Globals.Config.LocalPlayerInfo.Radarx + bX, Globals.Config.LocalPlayerInfo.Radary + bY), new Vector2(Globals.Config.LocalPlayerInfo.Radarx, Globals.Config.LocalPlayerInfo.Radary)) > (float)Globals.Config.LocalPlayerInfo.RadarSize - 10)
                    return;


                if (Scav && Globals.Config.Scav.Enable)
                {
                    Renderer.FillCircle(Globals.Config.LocalPlayerInfo.Radarx + bX, Globals.Config.LocalPlayerInfo.Radary + bY, 4, new Direct2DColor(BasePlayer.Colour.r, BasePlayer.Colour.g, BasePlayer.Colour.b, BasePlayer.Colour.a));
                }
                if (ScavPlayer && Globals.Config.ScavPlayer.Enable)
                {
                    Renderer.FillCircle(Globals.Config.LocalPlayerInfo.Radarx + bX, Globals.Config.LocalPlayerInfo.Radary + bY, 4, new Direct2DColor(BasePlayer.Colour.r, BasePlayer.Colour.g, BasePlayer.Colour.b, BasePlayer.Colour.a));
                }
                if (!(Scav || ScavPlayer) && Globals.Config.Player.Enable)
                {
                    Renderer.FillCircle(Globals.Config.LocalPlayerInfo.Radarx + bX, Globals.Config.LocalPlayerInfo.Radary + bY, 4, new Direct2DColor(BasePlayer.Colour.r, BasePlayer.Colour.g, BasePlayer.Colour.b, BasePlayer.Colour.a));
                }
            }
            catch { }
        }
      
        private void Render()
        {


            while (true)
            {
                try
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    #region Start

                    try
                    {
                        Renderer.BeginScene();
                        Renderer.ClearScene();
                    }
                    catch { }

                    #endregion

                    //  Renderer.DrawRectangle(100, 100, 1000, 1000 ,1, whiteSolid);

                    // reorganize the order of these so player is drawn above everything. item currently wont draw somtimes but players will
                    if (Globals.MainCamera == null || Globals.GameWorld == null)
                        goto EndDraw;
                    try
                    {
                        #region Exfil
                        try
                        {
                            foreach (BaseExfil be in Globals.ExfilList)
                            {
                                ExfiltrationPoint item = be.Exfil;
                                if (item == null)
                                    continue;
                                Vector3 ScreenPos = be.W2S;


                                string DistanceString = Globals.Config.Exfil.DrawDistance ? $"[{be.Distance}m]" : "";

                                if (be.Distance > Globals.Config.Exfil.MaxDistance)
                                    continue;
                                if (!Globals.Config.Exfil.Enable)
                                    continue;
                                if (be.Exfil.Status == EFT.Interactive.EExfiltrationStatus.NotPresent)
                                    continue;
                                //               if ((be.Exfil.Status == EFT.Interactive.EExfiltrationStatus.RegularMode || be.Exfil.Status == EFT.Interactive.EExfiltrationStatus.AwaitsManualActivation || be.Exfil.Status == EFT.Interactive.EExfiltrationStatus.) )
                                //            continue;
                                if (be.Exfil.Status == EExfiltrationStatus.Pending)
                                    continue;
                                if (IsScreenPointVisible(ScreenPos))
                                {
                                    Renderer.DrawTextCentered($"{be.Name}" + DistanceString, ScreenPos.x, ScreenPos.y, Tahoma3, new Direct2DColor(be.Colour.r, be.Colour.g, be.Colour.b, be.Colour.a));
                                }
                            }

                        }
                        catch { }
                        #endregion
                        #region Container
                        try
                        {
                            foreach (BaseContainer bc in Globals.ContainerList)
                            {

                                if (bc.Distance > Globals.Config.Container.MaxDistance)
                                    continue;
                                if (bc.Value < Globals.Config.Container.MinValue)
                                    continue;
                                if (!IsScreenPointVisible(bc.W2S))
                                    continue;

                                string tagstring = Globals.Config.Container.Tag ? $"{bc.Container.Template.LocalizedShortName()}" : "";
                                string distancestring = Globals.Config.Container.Distance ? $"[{bc.Distance}m]" : "";
                                string valuestring = Globals.Config.Container.Value ? $"[{bc.Value / 1000}k]" : "";

                                int index = 0;
                                if (Globals.Config.Container.DrawContents)
                                {


                                    foreach (EFT.InventoryLogic.Item itm in bc.Items)
                                    {
                                        OurItem ouritem = OurItems.list[itm.Template._id];
                                        if (ouritem.price < Globals.Config.Corpse.ContentMinValue)
                                            continue;
                                        string subitemstring = $"[{ouritem.name} | {ouritem.price / 1000}k]";
                                        index++;
                                        Renderer.DrawTextCentered(subitemstring, bc.W2S.x, (bc.W2S.y - 4) - (index * 12), Tahoma2, new Direct2DColor(bc.Colour.r, bc.Colour.g, bc.Colour.b, bc.Colour.a));
                                    }
                                    if (index > 0)
                                        Renderer.DrawLine(bc.W2S.x - 45, bc.W2S.y - 2, bc.W2S.x + 45, bc.W2S.y - 2, 1, new Direct2DColor(bc.Colour.r, bc.Colour.g, bc.Colour.b, bc.Colour.a));
                                }
                                if ((Globals.Config.Container.OnlyDrawWithSubItems && index <= 0) && Globals.Config.Container.DrawContents)
                                    continue;

                                Renderer.DrawTextCentered(tagstring + distancestring + valuestring, bc.W2S.x, bc.W2S.y, Tahoma2, new Direct2DColor(bc.Colour.r, bc.Colour.g, bc.Colour.b, bc.Colour.a));

                            }

                        }
                        catch { }
                        #endregion
                        #region Corpse
                        try
                        {
                            foreach (BaseCorpse bc in Globals.CorpseList)
                            {
                                if (bc.Distance > Globals.Config.Corpse.MaxDistance)
                                    continue;
                                if (bc.Value < Globals.Config.Corpse.MinValue)
                                    continue;
                                if (!IsScreenPointVisible(bc.W2S))
                                    continue;
                                string tagstring = Globals.Config.Corpse.Tag ? "Corpse" : "";
                                string distancestring = Globals.Config.Corpse.Distance ? $"[{bc.Distance}m]" : "";
                                string valuestring = Globals.Config.Corpse.Value ? $"[{bc.Value / 1000}k]" : "";
                                Renderer.DrawTextCentered(tagstring + distancestring + valuestring, bc.W2S.x, bc.W2S.y, Tahoma2, new Direct2DColor(bc.Colour.r, bc.Colour.g, bc.Colour.b, bc.Colour.a));

                                if (Globals.Config.Corpse.DrawCorpseContents)
                                {

                                    int index = 0;
                                    foreach (string itm in bc.Items)
                                    {
                                        OurItem ouritem = OurItems.list[itm];
                                        if (ouritem.price < Globals.Config.Corpse.ContentMinValue)
                                            continue;
                                        string subitemstring = $"[{ouritem.name} | {ouritem.price / 1000}k]";
                                        index++;
                                        Renderer.DrawTextCentered(subitemstring, bc.W2S.x, (bc.W2S.y - 10) - (index * 15), Tahoma2, new Direct2DColor(bc.Colour.r, bc.Colour.g, bc.Colour.b, bc.Colour.a));
                                    }
                                    if (index > 0)
                                        Renderer.DrawLine(bc.W2S.x - 45, bc.W2S.y - 8, bc.W2S.x + 45, bc.W2S.y - 8, 1, new Direct2DColor(bc.Colour.r, bc.Colour.g, bc.Colour.b, bc.Colour.a));
                                }
                            }
                        }
                        catch { }
                        #endregion
                    }
                    catch (Exception ex) { File.WriteAllText("Container.txt", ex.ToString()); }
                    try
                    {
                        #region Item
                        try
                        {
                            // currently ouritem causes an exception as it wont create a valid item or something. need to fix this.
                            foreach (BaseItem bi in Globals.LootList)
                            {
                                try
                                {
                                    LootItem item = bi.Item;

                                    if (item == null)
                                        continue;

                                    Vector3 ScreenPos = bi.W2S;

                                    OurItem ouritem;
                                    ouritem = OurItems.list[item.Item.Template._id];




                                    ResourceKey prefab = item.Item.Prefab;

                                    // add the itemesp config checks for these ffsvvwd
                                    string NameString = Globals.Config.Item.DrawName ? $"{bi.Name}" : "";
                                    string DistanceString = Globals.Config.Item.DrawDistance ? $"[{bi.Distance}m]" : "";
                                    string ValueString = Globals.Config.Item.DrawValue ? $"[{ouritem.price / 1000}k]" : "";
                                    string ItemType = Globals.Config.Item.Type ? $"[{ouritem.type.ToString()}][{ouritem.subtype.ToString()}]" : "";

                                    if (!IsScreenPointVisible(ScreenPos))
                                        continue;

                                    if (ouritem.subtype != item_subtype.Keycard)
                                        if (bi.Distance > Globals.Config.Item.MaxDistance)
                                            continue;

                                    if (Globals.Config.Item.DrawAll)
                                    {
                                        if (ouritem.price < Globals.Config.Item.MinValue && bi.ValueCheckIgnored == false) // bypass this for quest items
                                            continue;
                                        Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));

                                    }
                                    else
                                    {
                                        // honestly this isn't really meant for use.

                                        #region InaccurateFilters
                                        if (Globals.Config.Item.EnableInaccurateEsp)
                                        {

                                            if (item.Item.Template.Rarity == JsonType.ELootRarity.Superrare && Globals.Config.Item.DrawSuperRare)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));

                                                goto EndOfLoot;
                                            }
                                            if (ouritem.price >= Globals.Config.Item.BypassFilterOverValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                goto EndOfLoot;
                                            }

                                            if (item.Item.Template.QuestItem && Globals.Config.Item.DrawQuestItems)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                goto EndOfLoot;
                                            }

                                            if (item.Item.Template.Rarity == JsonType.ELootRarity.Rare && Globals.Config.Item.DrawRare)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                goto EndOfLoot;
                                            }

                                        }
                                        #endregion
                                        #region Accurate Filters
                                        if (!Globals.Config.Item.EnableAccurateEsp)
                                            break;
                                        if (item.Item.Template.QuestItem)
                                            continue;
                                        bool mod = ouritem.type == item_type.FunctionalMod || ouritem.type == item_type.Muzzle || ouritem.type == item_type.Sights || ouritem.type == item_type.SpecialScope || ouritem.type == item_type.SpecialScope || ouritem.type == item_type.Mod || ouritem.type == item_type.GearMod || ouritem.type == item_type.Magazine || ouritem.type == item_type.MasterMod;
                                        if (Globals.Config.ItemAmmo.Enable && ouritem.type == item_type.StackableItem)
                                        {
                                            if (ouritem.price > Globals.Config.ItemAmmo.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemArmour.Enable && ouritem.type == item_type.ArmoredEquipment)
                                        {
                                            if (ouritem.price > Globals.Config.ItemArmour.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemBackpacks.Enable && ouritem.type == item_type.SearchableItem && ouritem.subtype == item_subtype.Backpack)
                                        {
                                            if (ouritem.price > Globals.Config.ItemBackpacks.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemBarterItem.Enable && ouritem.type == item_type.BarterItem)
                                        {
                                            if (ouritem.price > Globals.Config.ItemBarterItem.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemCases.Enable && ouritem.type == item_type.CompoundItem)
                                        {
                                            if (ouritem.price > Globals.Config.ItemCases.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemClothing.Enable && ouritem.type == item_type.Equipment)
                                        {
                                            if (ouritem.price > Globals.Config.ItemClothing.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemFoodAndDrink.Enable && ouritem.type == item_type.FoodDrink)
                                        {
                                            if (ouritem.price > Globals.Config.ItemFoodAndDrink.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemFuel.Enable && ouritem.type == item_type.Lubricant)
                                        {
                                            if (ouritem.price > Globals.Config.ItemFuel.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemKey.Enable && ouritem.type == item_type.Key)
                                        {
                                            if (ouritem.price > Globals.Config.ItemKey.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemMeds.Enable && ouritem.type == item_type.Meds)
                                        {
                                            if (ouritem.price > Globals.Config.ItemMeds.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemRepairKits.Enable && ouritem.type == item_type.RepairKits)
                                        {
                                            if (ouritem.price > Globals.Config.ItemRepairKits.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemSpecialItem.Enable && ouritem.type == item_type.Item)
                                        {
                                            if (ouritem.price > Globals.Config.ItemSpecialItem.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemWeapon.Enable && ouritem.type == item_type.Weapon)
                                        {
                                            if (ouritem.price > Globals.Config.ItemWeapon.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        if (Globals.Config.ItemMods.Enable && mod)
                                        {
                                            if (ouritem.price > Globals.Config.ItemMods.MinValue)
                                            {
                                                Renderer.DrawTextCentered(NameString + DistanceString + ValueString, ScreenPos.x, ScreenPos.y, Tahoma, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                                Renderer.DrawTextCentered(ItemType, ScreenPos.x, ScreenPos.y + 12, Tahoma2, new Direct2DColor(bi.Colour.r, bi.Colour.g, bi.Colour.b, bi.Colour.a));
                                            }
                                        }
                                        #endregion

                                    }
                                    EndOfLoot:
                                    DoNothing();

                                }
                                catch (Exception ex)
                                {
                                    //    File.WriteAllText("ccc.txt", ex.ToString());
                                }

                                // draw item type







                            }

                        }
                        catch { }
                        #endregion
                    }
                    catch (Exception ex) { File.WriteAllText("Item.txt", ex.ToString()); }
                    try
                    {
                        #region PlayerEsp
                        try
                        {
                            // skelton with vischecks needed.  and admin checks and boss stuff
                            foreach (BasePlayer bp in Globals.PlayerList)
                            {

                                Player player = bp.Player;
                                if (player.HealthController.IsAlive == false || player == null)
                                    continue;
                                Vector3 ScreenPos = bp.W2S;
                                Vector3 HeadPos = bp.HeadW2S - new Vector3(0f, 2, 0f); // for top of head
                                if (!IsScreenPointVisible(ScreenPos))
                                    continue;

                                #region KD Stuff
                                long allLong5 = player.Profile.Stats.OverallCounters.GetAllLong(new object[]
                                           {
                                     EFT.Counters.CounterTag.KilledPmc
                                           });
                                long allLong6 = player.Profile.Stats.OverallCounters.GetAllLong(new object[]
                                 {
                                    EFT.Counters.CounterTag.ExitStatus,
                            ExitStatus.Killed
                                 });
                                #endregion
                                string KDString = Globals.Config.Player.KD ? $"[KD:{(Math.Round(((float)allLong5 / (float)allLong6), 2))}]" : "";
                                string LevelString = Globals.Config.Player.Level ? $"[LVL:{player.Profile.Info.Level}]" : "";
                                bool Scav = player.Profile.Info.RegistrationDate <= 0;
                                bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
                                string ScavTag = Globals.Config.Scav.Tag ? "Scav" : "";
                                string ScavPlayerTag = Globals.Config.ScavPlayer.Tag ? "Scav Player" : "";
                                string PlayerName = Globals.Config.Player.Name ? player.Profile.Info.Nickname.Localized() : "";
                                string Weapon = "Empty";
                                string Ammo = "";
                                int AmmoCurrent = 0;
                                int AmmoMax = 0;



                                float Health = (player.HealthController.GetBodyPartHealth(EBodyPart.Common, false).Current / player.HealthController.GetBodyPartHealth(EBodyPart.Common).Maximum) * 100f;
                                float Height = (ScreenPos.y - bp.HeadW2S.y);
                                float Width = Height / 2;
                                float HalfWidth = Width / 2;
                                #region Scav
                                if (Scav && Globals.Config.Scav.Enable)
                                {
                                    // make it left thigh1 to 2
                                    if (bp.Distance > Globals.Config.Scav.MaxDistance)
                                        continue;
                                    if (player.Profile.Info.Settings.IsBoss())
                                    {

                                        switch (player.Profile.Info.Settings.Role)
                                        {
                                            case WildSpawnType.bossKilla:
                                                ScavTag = "Killa";
                                                break;
                                            case WildSpawnType.bossSanitar:
                                                ScavTag = "Sanitar";
                                                break;
                                            case WildSpawnType.bossGluhar:
                                                ScavTag = "Glukhar";
                                                break;
                                            case WildSpawnType.bossKojaniy:
                                                ScavTag = "Shturman";
                                                break;
                                            case WildSpawnType.bossBully:
                                                ScavTag = "Reshala";
                                                break;
                                            case WildSpawnType.bossTagilla:
                                                ScavTag = "Tagilla";
                                                break;
                                            case WildSpawnType.bossKnight:
                                                ScavTag = "Knight";
                                                break;
                                            case WildSpawnType.followerBigPipe:
                                                ScavTag = "Big Pipe ;)";
                                                break;
                                            case WildSpawnType.followerBirdEye:
                                                ScavTag = "Bird Eye";
                                                break;
                                            case WildSpawnType.sectantPriest:
                                            case WildSpawnType.sectantWarrior:
                                                ScavTag = "Cultist";
                                                break;
                                            case WildSpawnType.assaultGroup:
                                            case WildSpawnType.pmcBot:
                                            case WildSpawnType.cursedAssault:
                                            case WildSpawnType.exUsec:
                                                ScavTag = "Raider";
                                                break;
                                            case WildSpawnType.followerBully:
                                            case WildSpawnType.followerGluharAssault:
                                            case WildSpawnType.followerGluharScout:
                                            case WildSpawnType.followerGluharSnipe:
                                            case WildSpawnType.followerSanitar:
                                            case WildSpawnType.followerTest:
                                            case WildSpawnType.followerKojaniy:
                                            case WildSpawnType.followerTagilla:
                                                ScavTag = "Boss Follower";
                                                break;
                                            default:
                                                ScavTag = "Raider";
                                                break;
                                        }
                                    }
                                    try
                                    {

                                        Weapon = Globals.Config.Scav.Weapon ? bp.Weapon.ShortName.Localized() : "";
                                        AmmoCurrent = bp.Weapon.GetCurrentMagazine().Count;
                                        AmmoMax = bp.Weapon.GetCurrentMagazine().MaxCount;
                                        Ammo = Globals.Config.Scav.Ammo ? $"[{AmmoCurrent}/{AmmoMax}]" : "";
                                    }
                                    catch { }
                                    string DistanceString = Globals.Config.Scav.Distance ? $"[{bp.Distance}m]" : "";
                                    string ValueString = Globals.Config.Scav.Value ? $"[{bp.Value / 1000}k]" : "";
                                    #region Boxes
                                    if (Globals.Config.Scav.Box && bp.Distance <= 150)
                                    {
                                        if (Globals.Config.Scav.FillBox)
                                            Renderer.FillRectangle((ScreenPos.x - HalfWidth + 1), HeadPos.y + 1, Width + 1, Height - 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        Renderer.DrawRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1f, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    if (Globals.Config.Scav.RoundedBox && bp.Distance <= 150)
                                    {
                                        if (Globals.Config.Scav.FillBox)
                                            Renderer.FillRoundedRectangle((ScreenPos.x - HalfWidth + 1), HeadPos.y + 1, Width + 1, Height - 1, 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        Renderer.DrawRoundedRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1, 3, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRoundedRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1, 1, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    if (Globals.Config.Scav.CornerBox && bp.Distance <= 150)
                                    {
                                        // fix these sometime
                                        if (Globals.Config.Scav.FillBox)
                                            Renderer.FillRectangle((ScreenPos.x - HalfWidth), HeadPos.y, Width, Height + 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        //  Renderer.DrawRectangleEdges(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRectangleEdges(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1f, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    #endregion

                                    if (Globals.Config.Scav.HealthBar && bp.Distance <= 150)
                                        Renderer.DrawHorizontalBar(Health, (ScreenPos.x - HalfWidth) - 5, HeadPos.y, 2, Height + 1, 1, new Direct2DColor(0, 255, 0, 255), new Direct2DColor(15, 15, 15, 185));
                                    Renderer.DrawTextCentered(DistanceString + ValueString, ScreenPos.x, HeadPos.y - 14, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(ScavTag, ScreenPos.x, ScreenPos.y + 3, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(Weapon + Ammo, ScreenPos.x, ScreenPos.y + 15, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                }
                                #endregion
                                #region ScavPlayer
                                if (ScavPlayer && Globals.Config.ScavPlayer.Enable)
                                {
                                    try
                                    {
                                        if (bp.Distance > Globals.Config.ScavPlayer.MaxDistance)
                                            continue;
                                        Weapon = Globals.Config.ScavPlayer.Weapon ? bp.Weapon.ShortName.Localized() : "";
                                        AmmoCurrent = bp.Weapon.GetCurrentMagazine().Count;
                                        AmmoMax = bp.Weapon.GetCurrentMagazine().MaxCount;
                                        Ammo = Globals.Config.ScavPlayer.Ammo ? $"[{AmmoCurrent}/{AmmoMax}]" : "";
                                    }
                                    catch { }
                                    #region Boxes
                                    if (Globals.Config.ScavPlayer.Box && bp.Distance <= 150)
                                    {
                                        if (Globals.Config.ScavPlayer.FillBox)
                                            Renderer.FillRectangle((ScreenPos.x - HalfWidth + 1), HeadPos.y + 1, Width + 1, Height - 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        Renderer.DrawRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1f, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    if (Globals.Config.ScavPlayer.RoundedBox && bp.Distance <= 150)
                                    {
                                        if (Globals.Config.ScavPlayer.FillBox)
                                            Renderer.FillRoundedRectangle((ScreenPos.x - HalfWidth + 1), HeadPos.y + 1, Width + 1, Height - 1, 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        Renderer.DrawRoundedRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1, 3, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRoundedRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1, 1, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    #endregion
                                    string DistanceString = Globals.Config.ScavPlayer.Distance ? $"[{bp.Distance}m]" : "";
                                    string ValueString = Globals.Config.ScavPlayer.Value ? $"[{bp.Value / 1000}k]" : "";
                                    if (Globals.Config.ScavPlayer.HealthBar && bp.Distance <= 150)
                                        Renderer.DrawHorizontalBar(Health, (ScreenPos.x - HalfWidth) - 5, HeadPos.y, 2, Height + 1, 1, new Direct2DColor(0, 255, 0, 255), new Direct2DColor(15, 15, 15, 185));
                                    Renderer.DrawTextCentered(DistanceString + ValueString, ScreenPos.x, HeadPos.y - 14, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(ScavPlayerTag, ScreenPos.x, ScreenPos.y + 3, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(Weapon + Ammo, ScreenPos.x, ScreenPos.y + 15, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                }
                                #endregion
                                #region Player
                                if (!Scav && !ScavPlayer)
                                {
                                    if (bp.Distance > Globals.Config.Player.MaxDistance)
                                        continue;
                                    try
                                    {

                                        Weapon = Globals.Config.Player.Weapon ? bp.Weapon.ShortName.Localized() : "";
                                        AmmoCurrent = bp.Weapon.GetCurrentMagazine().Count;
                                        AmmoMax = bp.Weapon.GetCurrentMagazine().MaxCount;
                                        Ammo = Globals.Config.Player.Ammo ? $"[{AmmoCurrent}/{AmmoMax}]" : "";
                                    }
                                    catch { }
                                    #region Boxes
                                    if (Globals.Config.Player.Box && bp.Distance <= 150)
                                    {
                                        if (Globals.Config.Player.FillBox)
                                            Renderer.FillRectangle((ScreenPos.x - HalfWidth + 1), HeadPos.y + 1, Width + 1, Height - 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        Renderer.DrawRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 3f, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1f, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    if (Globals.Config.Player.RoundedBox && bp.Distance <= 150)
                                    {
                                        if (Globals.Config.Player.FillBox)
                                            Renderer.FillRoundedRectangle((ScreenPos.x - HalfWidth + 1), HeadPos.y + 1, Width + 1, Height - 1, 1, new Direct2DColor(bp.FilledBoxColour.r, bp.FilledBoxColour.g, bp.FilledBoxColour.b, bp.FilledBoxColour.a));

                                        Renderer.DrawRoundedRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1, 3, new Direct2DColor(0, 0, 0, 255)); // background
                                        Renderer.DrawRoundedRectangle(ScreenPos.x - HalfWidth, HeadPos.y, Width, Height, 1, 1, new Direct2DColor(bp.BoxColour.r, bp.BoxColour.g, bp.BoxColour.b, bp.BoxColour.a));
                                    }
                                    #endregion
                                    string Flag = Globals.Config.Player.Flag && bp.Flag != null ? $"[Flag: {bp.Flag}]" : ""; // bosses seem to be scav players
                                    string DistanceString = Globals.Config.Player.Distance ? $"[{bp.Distance}m]" : "";
                                    string ValueString = Globals.Config.Player.Value ? $"[{bp.Value / 1000}k]" : "";
                                    if (Globals.Config.Player.HealthBar && bp.Distance <= 150)
                                        Renderer.DrawHorizontalBar(Health, (ScreenPos.x - HalfWidth) - 5, HeadPos.y, 2, Height + 1, 1, new Direct2DColor(0, 255, 0, 255), new Direct2DColor(15, 15, 15, 185));
                                    Renderer.DrawTextCentered(DistanceString + ValueString, ScreenPos.x, HeadPos.y - 14, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(PlayerName + KDString + LevelString, ScreenPos.x, ScreenPos.y + 3, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(Weapon + Ammo, ScreenPos.x, ScreenPos.y + 15, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                    Renderer.DrawTextCentered(Flag, ScreenPos.x, ScreenPos.y + 30, Tahoma4, new Direct2DColor(bp.Colour.r, bp.Colour.g, bp.Colour.b, bp.Colour.a));
                                }
                                #endregion

                            }
                        }
                        catch (Exception ex) { }
                        #endregion
                    }
                    catch (Exception ex) { File.WriteAllText("Player.txt", ex.ToString()); }
                    try
                    {
                        #region Grenade
                        try
                        {
                            foreach (BaseGrenade bg in Globals.GrenadeList)
                            {
                                Throwable grenade = bg.Grenade;
                                if (grenade == null)
                                    continue;
                                if (grenade.transform.position == null)
                                    continue;
                                Vector3 ScreenPos = bg.W2S;
                                string DistanceString = $"({bg.Distance}m)";
                                if (bg.Distance > 300)
                                    continue;
                                if (IsScreenPointVisible(ScreenPos))
                                {
                                    Renderer.DrawTextCentered($"Grenade" + DistanceString, ScreenPos.x, ScreenPos.y, infoFont, whiteSolid);
                                }
                            }

                        }
                        catch { }
                        #endregion
                    }
                    catch { }


                    EndDraw:
                   try
                    {
                        #region Radar
                        if (Globals.Config.LocalPlayerInfo.Radar)
                        {
                            UnityEngine.Color32 radarcolour = Helpers.ColourHelper.GetColour("Radar Colour");
                            Renderer.FillCircle(Globals.Config.LocalPlayerInfo.Radarx, Globals.Config.LocalPlayerInfo.Radary, Globals.Config.LocalPlayerInfo.RadarSize, new Direct2DColor(radarcolour.r, radarcolour.g, radarcolour.b, radarcolour.a));
                            Renderer.DrawCircle(Globals.Config.LocalPlayerInfo.Radarx, Globals.Config.LocalPlayerInfo.Radary, Globals.Config.LocalPlayerInfo.RadarSize + 1, 1, new Direct2DColor(255, 255, 255, 255)); // outline
                            Renderer.DrawLine(Globals.Config.LocalPlayerInfo.Radarx, Globals.Config.LocalPlayerInfo.Radary - (Globals.Config.LocalPlayerInfo.RadarSize), Globals.Config.LocalPlayerInfo.Radarx, Globals.Config.LocalPlayerInfo.Radary + (Globals.Config.LocalPlayerInfo.RadarSize), 1, new Direct2DColor(255, 255, 255, 255));
                            Renderer.DrawLine(Globals.Config.LocalPlayerInfo.Radarx - (Globals.Config.LocalPlayerInfo.RadarSize), Globals.Config.LocalPlayerInfo.Radary, Globals.Config.LocalPlayerInfo.Radarx + (Globals.Config.LocalPlayerInfo.RadarSize), Globals.Config.LocalPlayerInfo.Radary, 1, new Direct2DColor(255, 255, 255, 255));
                            foreach (BasePlayer bp in Globals.PlayerList)
                            {
                                DrawRadarEntity(bp, Renderer);
                            }

                        }
                        #endregion
                    }
                    catch (Exception ex) { File.WriteAllText("Radar.txt", ex.ToString()); }
                    try
                    {

                        Renderer.DrawText($"Redd Private | {windowSize.Right - windowSize.Left}x{ windowSize.Bottom - windowSize.Top} - FPS: {Renderer.FPS}[{DrawTime}ms]{Helpers.ShaderHelper.SpiritesList.Count()}", 5, 5, infoFont, whiteSolid);
                        Renderer.DrawText($"Players: {Globals.PlayerList.Count()}", 5, 25, infoFont, whiteSolid);
                        Renderer.DrawText($"Loot: {Globals.LootList.Count()}", 5, 45, infoFont, whiteSolid);
                        Renderer.DrawText($"Corpse: {Globals.CorpseList.Count()}", 5, 65, infoFont, whiteSolid);
                        Renderer.DrawText($"Exfil: {Globals.ExfilList.Count()}", 5, 85, infoFont, whiteSolid);
                        Renderer.DrawText($"Container: {Globals.ContainerList.Count()}", 5, 105, infoFont, whiteSolid);
                        //  Renderer.DrawText($"Grenade: {Globals.GrenadeList.Count()}", 5, 125, infoFont, whiteSolid);


                        if (Globals.Config.Aimbot.DrawFov)
                        {
                            Renderer.DrawCircle((float)Screen.width / 2, (float)Screen.height / 2, Globals.Config.Aimbot.Fov, 1f, new Direct2DColor(255, 255, 255, 255));

                        }
                    }
                    catch { }

                    try
                    {
                        Menu.RenderMenu.Render(Renderer);
                        #region LocalPlayerInfo
                        try
                        {
                            if (Globals.Config.LocalPlayerInfo.Enable)
                            { // add kd

                                Color32 backgroundcolour = Helpers.ColourHelper.GetColour("Radar Colour");
                                Color32 primary = Helpers.ColourHelper.GetColour("Menu Primary Colour");
                                Color32 secondary = Helpers.ColourHelper.GetColour("Menu Secondary Colour");

                                string health = Globals.LocalPlayer != null ? "[" + (Globals.LocalPlayer.HealthController.GetBodyPartHealth(EBodyPart.Common, true).Current.ToString() + "/" + Globals.LocalPlayer.HealthController.GetBodyPartHealth(EBodyPart.Common, true).Maximum.ToString()) + "]" : "";
                                string weapon = Globals.LocalPlayerWeapon != null ? "[" + Globals.LocalPlayerWeapon.ShortName.Localized() + "]" : "";
                                string ammo = Globals.LocalPlayerWeapon != null ? "[" + Globals.LocalPlayerWeapon.GetCurrentMagazineCount().ToString() + "/" + Globals.LocalPlayerWeapon.GetMaxMagazineCount().ToString() + "]" : "";
                                string firemode = Globals.LocalPlayerWeapon != null ? "[" + Globals.LocalPlayerWeapon.SelectedFireMode + "]" : "";
                                string bulletspeed = Globals.LocalPlayerWeapon != null && Globals.LocalPlayerWeapon.CurrentAmmoTemplate != null ? "[" + Globals.LocalPlayerWeapon.CurrentAmmoTemplate.InitialSpeed.ToString() + "]" + "|" + "[" + Globals.LocalPlayerWeapon.SpeedFactor.ToString() + "]" : "";
                                string ammotype = Globals.LocalPlayerWeapon.Chambers[0].ContainedItem != null && Globals.LocalPlayerWeapon != null ? "[" + Globals.LocalPlayerWeapon.Chambers[0].ContainedItem.ShortName.Localized() + "]" : "";
                                string value = Globals.LocalPlayer != null ? $"[{Globals.LocalPlayerValue / 1000}K]" : "";
                                string fly = Globals.Config.Movement.FlyHack == true && Globals.LocalPlayer != null ? $"[On : {Math.Round(Globals.FlyHackValue, 3)}]" : "[Off]";

                                Renderer.FillRectangle(Globals.Config.LocalPlayerInfo.x, Globals.Config.LocalPlayerInfo.y - 15, 225, 15, new Direct2DColor(backgroundcolour.r, backgroundcolour.g, backgroundcolour.b, backgroundcolour.a - 20));
                                Renderer.DrawRectangle(Globals.Config.LocalPlayerInfo.x, Globals.Config.LocalPlayerInfo.y - 15, 225, 15, 1, new Direct2DColor(255, 255, 255, 255));
                                Renderer.DrawTextCentered("Localplayer:", Globals.Config.LocalPlayerInfo.x + (225 / 2), Globals.Config.LocalPlayerInfo.y - 15, Tahoma3, new Direct2DColor(255, 255, 255, 255));
                                //bottom
                                Renderer.FillRectangle(Globals.Config.LocalPlayerInfo.x, Globals.Config.LocalPlayerInfo.y, 225, 165, new Direct2DColor(backgroundcolour.r, backgroundcolour.g, backgroundcolour.b, backgroundcolour.a - 20));
                                Renderer.DrawRectangle(Globals.Config.LocalPlayerInfo.x, Globals.Config.LocalPlayerInfo.y, 225, 165, 1, new Direct2DColor(255, 255, 255, 255));

                                Renderer.DrawText("Health:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 5, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(health, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 5, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("Weapon:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 25, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(weapon, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 25, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("Ammo:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 45, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(ammo, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 45, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("Ammo Type:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 65, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(ammotype, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 65, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("Speed:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 85, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(bulletspeed, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 85, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("FireMode:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 105, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(firemode, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 105, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("Value:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 125, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(value, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 125, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));

                                Renderer.DrawText("FlyHack:", Globals.Config.LocalPlayerInfo.x + 5, Globals.Config.LocalPlayerInfo.y + 145, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                Renderer.DrawTextCentered(fly, Globals.Config.LocalPlayerInfo.x + ((225 / 2) + (225 / 4)), Globals.Config.LocalPlayerInfo.y + 145, Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));



                            }
                        }
                        catch { }



                        #endregion
                        #region PlayerInformation
                        try
                        {
                            if (Globals.Config.PlayerInformation.Enable)
                            {
                                Color32 backgroundcolour = Helpers.ColourHelper.GetColour("Radar Colour");
                                Color32 primary = Helpers.ColourHelper.GetColour("Menu Primary Colour");
                                Color32 secondary = Helpers.ColourHelper.GetColour("Menu Secondary Colour");
                                Player player = Updating.InformationPlayer; // just easier access
                                bool Scav = player.Profile.Info.RegistrationDate <= 0;
                                bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
                                List<string> drawlist = new List<string>();
                                #region Adding To List
                                if (player != null)
                                {

                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Primary Weapon:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.Template._id];
                                            EFT.InventoryLogic.Weapon weapon = player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem as EFT.InventoryLogic.Weapon;
                                            string ammo = weapon != null ? "[" + weapon.GetCurrentMagazineCount().ToString() + "/" + weapon.GetMaxMagazineCount().ToString() + "]" : "";
                                            string firemode = "[" + weapon.SelectedFireMode + "]";
                                            drawlist.Add($"[{ouritem.name}]{ammo}{firemode} | [{ouritem.price / 1000}k]");
                                        }
                                        catch { }

                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.SecondPrimaryWeapon).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Secondary Weapon:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.Template._id];
                                            EFT.InventoryLogic.Weapon weapon = player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.SecondPrimaryWeapon).ContainedItem as EFT.InventoryLogic.Weapon;
                                            string ammo = weapon != null ? "[" + weapon.GetCurrentMagazineCount().ToString() + "/" + weapon.GetMaxMagazineCount().ToString() + "]" : "";
                                            string firemode = "[" + weapon.SelectedFireMode + "]";
                                            drawlist.Add($"[{ouritem.name}]{ammo}{firemode} | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Holster).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Pistol:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Holster).ContainedItem.Template._id];
                                            EFT.InventoryLogic.Weapon weapon = player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Holster).ContainedItem as EFT.InventoryLogic.Weapon;
                                            string ammo = weapon != null ? "[" + weapon.GetCurrentMagazineCount().ToString() + "/" + weapon.GetMaxMagazineCount().ToString() + "]" : "";
                                            string firemode = "[" + weapon.SelectedFireMode + "]";
                                            drawlist.Add($"[{ouritem.name}]{ammo}{firemode} | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Headwear).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Helmet:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Headwear).ContainedItem.Template._id];
                                            drawlist.Add($"[{ouritem.name}] | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FaceCover).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Face:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FaceCover).ContainedItem.Template._id];
                                            drawlist.Add($"[{ouritem.name}] | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.ArmorVest).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Armour:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.ArmorVest).ContainedItem.Template._id];
                                            drawlist.Add($"[{ouritem.name}] | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.TacticalVest).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Vest:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.TacticalVest).ContainedItem.Template._id];
                                            drawlist.Add($"[{ouritem.name}] | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Earpiece).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Headset:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Earpiece).ContainedItem.Template._id];
                                            drawlist.Add($"[{ouritem.name}] | [{ouritem.price / 1000}k]");
                                            // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                        }
                                        catch { }
                                    }
                                    if (player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Backpack).ContainedItem != null)
                                    {
                                        try
                                        {
                                            drawlist.Add("Backpack:");
                                            OurItem ouritem = OurItems.list[player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.Backpack).ContainedItem.Template._id];
                                            drawlist.Add($"[{ouritem.name}] | [{ouritem.price / 1000}k]");
                                        }
                                        catch { }
                                        // drawlist.Add(player.Profile.Inventory.Equipment.GetSlot(EFT.InventoryLogic.EquipmentSlot.FirstPrimaryWeapon).ContainedItem.item.ShortName.Localized());
                                    }
                                    #endregion
                                    int sizey = (drawlist.Count() * 16);
                                    string playername = string.Empty;
                                    if (Scav)
                                        playername = "Scav:";
                                    if (ScavPlayer)
                                        playername = "Scav Player:";
                                    if (!Scav && !ScavPlayer)
                                        playername = player.Profile.Nickname.Localized() + ":";
                                    if (!Globals.Config.PlayerInformation.AutoSizeToRadar)
                                    {
                                        Renderer.FillRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y, 201, 15, new Direct2DColor(backgroundcolour.r, backgroundcolour.g, backgroundcolour.b, backgroundcolour.a - 20));
                                        Renderer.DrawRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y, 201, 15, 1, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                        Renderer.DrawTextCentered(playername, Globals.Config.PlayerInformation.X + (201 / 2), Globals.Config.PlayerInformation.Y - 1, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                        //bottom
                                        Renderer.FillRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y + 15, 201, sizey, new Direct2DColor(backgroundcolour.r, backgroundcolour.g, backgroundcolour.b, backgroundcolour.a - 20));
                                        Renderer.DrawRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y + 15, 201, sizey, 1, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                    }
                                    else
                                    {
                                        Renderer.FillRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y, Globals.Config.LocalPlayerInfo.RadarSize * 2, 15, new Direct2DColor(backgroundcolour.r, backgroundcolour.g, backgroundcolour.b, backgroundcolour.a - 20));
                                        Renderer.DrawRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y, Globals.Config.LocalPlayerInfo.RadarSize * 2, 15, 1, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                        Renderer.DrawTextCentered(playername, Globals.Config.PlayerInformation.X + Globals.Config.LocalPlayerInfo.RadarSize, Globals.Config.PlayerInformation.Y - 1, Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                        //bottom
                                        Renderer.FillRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y + 15, Globals.Config.LocalPlayerInfo.RadarSize * 2, sizey, new Direct2DColor(backgroundcolour.r, backgroundcolour.g, backgroundcolour.b, backgroundcolour.a - 20));
                                        Renderer.DrawRectangle(Globals.Config.PlayerInformation.X, Globals.Config.PlayerInformation.Y + 15, Globals.Config.LocalPlayerInfo.RadarSize * 2, sizey, 1, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                    }
                                    foreach (string str in drawlist)
                                    {


                                        if (Globals.Config.PlayerInformation.AutoSizeToRadar)
                                        {
                                            if (drawlist.IndexOf(str) % 2 == 0)
                                                Renderer.DrawTextCentered(str, Globals.Config.PlayerInformation.X + Globals.Config.LocalPlayerInfo.RadarSize, Globals.Config.PlayerInformation.Y + 15 + (drawlist.IndexOf(str) * 15), Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                            else
                                                Renderer.DrawTextCentered(str, Globals.Config.PlayerInformation.X + Globals.Config.LocalPlayerInfo.RadarSize, Globals.Config.PlayerInformation.Y + 15 + (drawlist.IndexOf(str) * 15), Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));
                                        }
                                        else
                                        {
                                            if (drawlist.IndexOf(str) % 2 == 0)
                                                Renderer.DrawTextCentered(str, Globals.Config.PlayerInformation.X + Globals.Config.LocalPlayerInfo.RadarSize, Globals.Config.PlayerInformation.Y + 15 + (drawlist.IndexOf(str) * 15), Tahoma3, new Direct2DColor(secondary.r, secondary.g, secondary.b, secondary.a));
                                            else
                                                Renderer.DrawTextCentered(str, Globals.Config.PlayerInformation.X + Globals.Config.LocalPlayerInfo.RadarSize, Globals.Config.PlayerInformation.Y + 15 + (drawlist.IndexOf(str) * 15), Tahoma3, new Direct2DColor(primary.r, primary.g, primary.b, primary.a));
                                        }
                                    }
                                    drawlist.Clear();
                                }
                            }


                        }
                        catch { }
                        #endregion
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Globals.Config.LocalPlayerInfo.Crosshair)
                            Renderer.DrawCrosshair(CrosshairStyle.Gap, Screen.width / 2, Screen.height / 2, 6, 1, CrosshairColor);
                    }
                    catch { }
                    #region End
                    try
                    {
                        Renderer.EndScene();
                    }
                    catch { }
                    #endregion
                    timer.Stop();
                }
                catch { }


            }

        }
      
    }
}
