using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using Hag.Renderer;
using UnityEngine;
using System.Collections;
using Hag.Helpers;
using System.IO;
using UnityEngine;

namespace Hag.Menu
{
    class RenderMenu : MonoBehaviour
    {
        //     public static Direct2DRenderer Renderer;
        static bool ShowGUI = true;
        void Start()
        {
            
      
                StartCoroutine(KeyControls());

                MainMenu.Items.Add(Esp);
                MainMenu.Items.Add(Aimbot);
                MainMenu.Items.Add(LocalPlayer);
                MainMenu.Items.Add(Colours);
                MainMenu.Items.Add(Config);



                ColourPicker();
                Configs();

                ItemEsps();
                PlayerEsps();
                PlayerScavEsps();
                ScavEsps();
                CorpseEsps();
                Exfils();
                ContainerEsps();

                LocalPlayerInfo();
                Weapons();
                Movements();
                Visual();
                Worlds();

                Generals();
                PlayerAimbots();
                ScavPlayerAimbots();
                ScaAimbots();

                MenuHistory.Add(MainMenu);
                CurrentMenu = MainMenu;
                Selected = Esp;

              
           
        }
        void Configs()
        {
            Config.Items.Add(SaveConfig);
            Config.Items.Add(LoadConfig);
            Button LoadDefault = new Button("Load Default", "", () => ConfigHelper.LoadConfig("Default"));
            Button LoadLegit = new Button("Load Legit", "", () => ConfigHelper.LoadConfig("Legit"));
            Button LoadSemi = new Button("Load Semi-Legit", "", () => ConfigHelper.LoadConfig("SemiLegit"));
            Button LoadNearlyage = new Button("Load Nearly Rage", "", () => ConfigHelper.LoadConfig("NearlyRage"));
            Button LoadRage = new Button("Load Rage", "", () => ConfigHelper.LoadConfig("Rage"));
            Button LoadHVH = new Button("Load HVH", "", () => ConfigHelper.LoadConfig("HVH"));

            Button SaveDefault = new Button("Save Default", "", () => ConfigHelper.SaveConfig("Default"));
            Button SaveLegit = new Button("Save Legit", "", () => ConfigHelper.SaveConfig("Legit"));
            Button SavedSemi = new Button("Save Semi-Legit", "", () => ConfigHelper.SaveConfig("SemiLegit"));
            Button SaveNearlyage = new Button("Save Nearly Rage", "", () => ConfigHelper.SaveConfig("NearlyRage"));
            Button SaveRage = new Button("Save Rage", "", () => ConfigHelper.SaveConfig("Rage"));
            Button SaveHVH = new Button("Save HVH", "", () => ConfigHelper.SaveConfig("HVH"));

            SaveConfig.Items.Add(LoadDefault);
            SaveConfig.Items.Add(LoadLegit);
            SaveConfig.Items.Add(LoadSemi);
            SaveConfig.Items.Add(LoadNearlyage);
            SaveConfig.Items.Add(LoadRage);
            SaveConfig.Items.Add(LoadHVH);

            LoadConfig.Items.Add(SaveDefault);
            LoadConfig.Items.Add(SaveLegit);
            LoadConfig.Items.Add(SavedSemi);
            LoadConfig.Items.Add(SaveNearlyage);
            LoadConfig.Items.Add(SaveRage);
            LoadConfig.Items.Add(SaveHVH);
        }
        void PlayerAimbots()
        {
            Aimbot.Items.Add(PlayerAimbot);
            #region Rage
            PlayerAimbot.Items.Add(PlayerRageAimbot);
            Toggle rageaimbot = new Toggle("Rage Aimbot", "Master Toggle For Rage Aimbot", ref Globals.Config.PlayerAimbot.RageAimbot);
            IntSlider ragehitchane = new IntSlider("Rage Aimbot Hitchance", "Master Toggle For Rage Aimbot", ref Globals.Config.PlayerAimbot.RageAimbotHitchance,0,100,5);
            PlayerRageAimbot.Items.Add(rageaimbot);
            PlayerRageAimbot.Items.Add(ragehitchane);
            #endregion
            #region AutoShoot
            PlayerAimbot.Items.Add(PlayerAutoAimbot);
            Toggle autoshoot = new Toggle("Auto Shoot Aimbot", "Master Toggle For Auto Shoot Aimbot", ref Globals.Config.PlayerAimbot.AutoShoot);
            IntSlider autodistance = new IntSlider("Auto Shoot Max Distance", "Auto Shoot Wont Function Under This Distance", ref Globals.Config.PlayerAimbot.AutoShootMaxDistance, 0, 800, 20);
            PlayerAutoAimbot.Items.Add(autoshoot);
            PlayerAutoAimbot.Items.Add(autodistance);
            #endregion
            #region Static Aimbot
            PlayerAimbot.Items.Add(PlayerStaticAimbot);
            Toggle staticaimbot = new Toggle("Static Aimbot", "Master Toggle For Static Aimbot", ref Globals.Config.PlayerAimbot.LegitAimbot);
            IntSlider staticmaxdistance = new IntSlider("Static Aimbot Max Distance", "Static Aimbot Wont Lock On Under This Distance", ref Globals.Config.PlayerAimbot.LegitAimbotMaxDistance, 0, 800, 20);
            IntSlider statichitbox = new IntSlider("Static Aimbot Hitbox", "HitBone | (0)Head | (1)Neck | (2)UpperSpine | (3)LowerSpine | (4)Pelvis", ref Globals.Config.PlayerAimbot.LegitHitbox, 0, 4, 1);
            Toggle staticvischecks = new Toggle("Static Aimbot Vischecks", "Visibility Checks For Aimbot", ref Globals.Config.PlayerAimbot.LegitVischecks);
            IntSlider staticsmoothing = new IntSlider("Static Aimbot Smoothing", "Higher Smoothing The More Legit Aimbot Looks", ref Globals.Config.PlayerAimbot.LegitSmoothing, 0, 100, 1);
            PlayerStaticAimbot.Items.Add(staticaimbot);
            PlayerStaticAimbot.Items.Add(staticmaxdistance);
            PlayerStaticAimbot.Items.Add(statichitbox);
            PlayerStaticAimbot.Items.Add(staticvischecks);
            PlayerStaticAimbot.Items.Add(staticsmoothing);
            #endregion

        }
        void ScavPlayerAimbots()
        {
            Aimbot.Items.Add(ScavPlayerAimbot);
            #region Rage
            ScavPlayerAimbot.Items.Add(ScavPlayerRageAimbot);
            Toggle rageaimbot = new Toggle("Rage Aimbot", "Master Toggle For Rage Aimbot", ref Globals.Config.ScavPlayerAimbot.RageAimbot);
            IntSlider ragehitchane = new IntSlider("Rage Aimbot Hitchance", "Master Toggle For Rage Aimbot", ref Globals.Config.ScavPlayerAimbot.RageAimbotHitchance, 0, 100, 5);
            ScavPlayerRageAimbot.Items.Add(rageaimbot);
            ScavPlayerRageAimbot.Items.Add(ragehitchane);
            #endregion
            #region AutoShoot
            ScavPlayerAimbot.Items.Add(ScavPlayerAutoAimbot);
            Toggle autoshoot = new Toggle("Auto Shoot Aimbot", "Master Toggle For Auto Shoot Aimbot", ref Globals.Config.ScavPlayerAimbot.AutoShoot);
            IntSlider autodistance = new IntSlider("Auto Shoot Max Distance", "Auto Shoot Wont Function Under This Distance", ref Globals.Config.ScavPlayerAimbot.AutoShootMaxDistance, 0, 800, 20);
            ScavPlayerAutoAimbot.Items.Add(autoshoot);
            ScavPlayerAutoAimbot.Items.Add(autodistance);
            #endregion
            #region Static Aimbot
            ScavPlayerAimbot.Items.Add(ScavPlayerStaticAimbot);
            Toggle staticaimbot = new Toggle("Static Aimbot", "Master Toggle For Static Aimbot", ref Globals.Config.ScavPlayerAimbot.LegitAimbot);
            IntSlider staticmaxdistance = new IntSlider("Static Aimbot Max Distance", "Static Aimbot Wont Lock On Under This Distance", ref Globals.Config.ScavPlayerAimbot.LegitAimbotMaxDistance, 0, 800, 20);
            IntSlider statichitbox = new IntSlider("Static Aimbot Hitbox", "HitBone | (0)Head | (1)Neck | (2)UpperSpine | (3)LowerSpine | (4)Pelvis", ref Globals.Config.ScavPlayerAimbot.LegitHitbox, 0, 4, 1);
            Toggle staticvischecks = new Toggle("Static Aimbot Vischecks", "Visibility Checks For Aimbot", ref Globals.Config.ScavPlayerAimbot.LegitVischecks);
            IntSlider staticsmoothing = new IntSlider("Static Aimbot Smoothing", "Higher Smoothing The More Legit Aimbot Looks", ref Globals.Config.ScavPlayerAimbot.LegitSmoothing, 0, 100, 1);
            ScavPlayerStaticAimbot.Items.Add(staticaimbot);
            ScavPlayerStaticAimbot.Items.Add(staticmaxdistance);
            ScavPlayerStaticAimbot.Items.Add(statichitbox);
            ScavPlayerStaticAimbot.Items.Add(staticvischecks);
            ScavPlayerStaticAimbot.Items.Add(staticsmoothing);
            #endregion
        }
        void ScaAimbots()
        {
            Aimbot.Items.Add(ScavAimbot);
            #region Rage
            ScavAimbot.Items.Add(ScavRageAimbot);
            Toggle rageaimbot = new Toggle("Rage Aimbot", "Master Toggle For Rage Aimbot", ref Globals.Config.ScavAimbot.RageAimbot);
            IntSlider ragehitchane = new IntSlider("Rage Aimbot Hitchance", "Master Toggle For Rage Aimbot", ref Globals.Config.ScavAimbot.RageAimbotHitchance, 0, 100, 5);
            ScavRageAimbot.Items.Add(rageaimbot);
            ScavRageAimbot.Items.Add(ragehitchane);
            #endregion
            #region AutoShoot
            ScavAimbot.Items.Add(ScavAutoAimbot);
            Toggle autoshoot = new Toggle("Auto Shoot Aimbot", "Master Toggle For Auto Shoot Aimbot", ref Globals.Config.ScavAimbot.AutoShoot);
            IntSlider autodistance = new IntSlider("Auto Shoot Max Distance", "Auto Shoot Wont Function Under This Distance", ref Globals.Config.ScavAimbot.AutoShootMaxDistance, 0, 800, 20);
            ScavAutoAimbot.Items.Add(autoshoot);
            ScavAutoAimbot.Items.Add(autodistance);
            #endregion
            #region Static Aimbot
            ScavAimbot.Items.Add(ScavStaticAimbot);
            Toggle staticaimbot = new Toggle("Static Aimbot", "Master Toggle For Static Aimbot", ref Globals.Config.ScavAimbot.LegitAimbot);
            IntSlider staticmaxdistance = new IntSlider("Static Aimbot Max Distance", "Static Aimbot Wont Lock On Under This Distance", ref Globals.Config.ScavAimbot.LegitAimbotMaxDistance, 0, 800, 20);
            IntSlider statichitbox = new IntSlider("Static Aimbot Hitbox", "HitBone | (0)Head | (1)Neck | (2)UpperSpine | (3)LowerSpine | (4)Pelvis", ref Globals.Config.ScavAimbot.LegitHitbox, 0, 4, 1);
            Toggle staticvischecks = new Toggle("Static Aimbot Vischecks", "Visibility Checks For Aimbot", ref Globals.Config.ScavAimbot.LegitVischecks);
            IntSlider staticsmoothing = new IntSlider("Static Aimbot Smoothing", "Higher Smoothing The More Legit Aimbot Looks", ref Globals.Config.ScavAimbot.LegitSmoothing, 0, 100, 1);
            ScavStaticAimbot.Items.Add(staticaimbot);
            ScavStaticAimbot.Items.Add(staticmaxdistance);
            ScavStaticAimbot.Items.Add(statichitbox);
            ScavStaticAimbot.Items.Add(staticvischecks);
            ScavStaticAimbot.Items.Add(staticsmoothing);
            #endregion
        }
        void Generals()
        {
            Aimbot.Items.Add(General);
            Toggle drawfov = new Toggle("Draw Fov", "Draws Aimbot Fov Bounds", ref Globals.Config.Aimbot.DrawFov);
            IntSlider fov = new IntSlider("Fov", "Fov Distance For Aimbot", ref Globals.Config.Aimbot.Fov,0,2000,20);
            Toggle ignorefov = new Toggle("Ignore Fov", "Shoot People Outside Of The Fov", ref Globals.Config.Aimbot.IgnoreFov);
            Keybind bind = new Keybind("Aimbot Bind", "Key To Press To Use Aimbot", ref Globals.Config.Aimbot.AimbotKey);
            Toggle insta = new Toggle("Instant Hit", "Bullet Speed Go Zooom", ref Globals.Config.Aimbot.InstantHit);
            Toggle awall = new Toggle("AutoWall", "Shoots People Through Walls", ref Globals.Config.Aimbot.AutoWall);
            IntSlider awallpasses = new IntSlider("AutoWall Passes", "How Many Walls Auto Wall Will Check", ref Globals.Config.Aimbot.AutoWallPasses,1,5,1);
            General.Items.Add(drawfov);
            General.Items.Add(fov);
            General.Items.Add(ignorefov);
            General.Items.Add(bind);
            General.Items.Add(insta);
            General.Items.Add(awall);
            General.Items.Add(awallpasses);
        }
        void Worlds()
        {
            LocalPlayer.Items.Add(World);
            Toggle loot = new Toggle("Loot Through Walls", "Loot Items Through Walls", ref Globals.Config.LocalPlayerWorld.LootThroughWalls);
            Toggle examine = new Toggle("Instant Examine", "Examine Contents Of Containers Instantly", ref Globals.Config.LocalPlayerWorld.InstantExamine);
            Toggle fast = new Toggle("Fast Search", "Search Items In Containers Instantly", ref Globals.Config.LocalPlayerWorld.FastSearch);
            Toggle bypasssearch = new Toggle("Insta Search", "Searches Containers Instantly(Wont Give XP)", ref Globals.Config.LocalPlayerWorld.BypassSearch);
            Toggle grenade = new Toggle("Far Throw Grenade", "Yeet Grenades 100 Miles", ref Globals.Config.LocalPlayerWorld.FarThrowGrenades);
            Toggle reload = new Toggle("Instant Load/Unload Mags", "Instantly Unload And Load Ammo Into Mags", ref Globals.Config.LocalPlayerWorld.UnloadLoadMags);
            Toggle instantads = new Toggle("Instant ADS", "Aim Instantly", ref Globals.Config.LocalPlayerWorld.InstantADS);
            Toggle mods = new Toggle("Unlock Mods", "Change Mods Mid Raid", ref Globals.Config.LocalPlayerWorld.UnlockMods);
            Toggle damage = new Toggle("No Physical Damage", "Don't Take Damage To Body Parts", ref Globals.Config.LocalPlayerWorld.NoPhysicalDamage);
            Toggle nodelay = new Toggle("No Open Delay", "Drop Items And Close Containers Instantly", ref Globals.Config.LocalPlayerWorld.NoOpenDelay);
            Toggle pockets = new Toggle("Loot Pockets", "Take A Player Or Bosses' Pockets", ref Globals.Config.LocalPlayerWorld.LootPockets);
            Button switches = new Button("Turn On All Switches", "Turns On Power And Raider Switches", () => Hag.Esp.Updating.TurnOnAllSwitch());
            World.Items.Add(loot);
            World.Items.Add(examine);
            World.Items.Add(fast);
            World.Items.Add(bypasssearch);
            World.Items.Add(grenade);
            World.Items.Add(reload);
            World.Items.Add(instantads);
            World.Items.Add(mods);
            World.Items.Add(nodelay);
            World.Items.Add(pockets);
            World.Items.Add(switches);
           
        }
        void Visual()
        {
            LocalPlayer.Items.Add(Visuals);
            Toggle novisor = new Toggle("No Visor", "Master Switch For No Visor", ref Globals.Config.Visuals.NoVisor);
            Toggle nightvision = new Toggle("NightVision", "Master Switch For NightVision", ref Globals.Config.Visuals.NightVision);
            Toggle thermalvision = new Toggle("ThermalVision", "Master Switch For ThermalVision", ref Globals.Config.Visuals.ThermalVision);
            Toggle alwaysday = new Toggle("Always Day", "Master Switch For Always Day", ref Globals.Config.Visuals.AlwaysDay);
            Toggle third = new Toggle("3rd person", "Master Switch For 3rd Person", ref Globals.Config.Visuals.ThirdPerson);
            Toggle flash = new Toggle("No Flash", "Removes Flashbang Effects", ref Globals.Config.Visuals.NoFlash);
            Toggle viewangles = new Toggle("Unlock Viewangles", "Look And And Down As Much As You Want", ref Globals.Config.Visuals.UnlockViewAngles);
            Toggle handchams = new Toggle("Hand Chams", "Changes Colour/Texture Of Your Hands", ref Globals.Config.Visuals.HandChams);
            Toggle handchamsonlyarms = new Toggle("Hand Chams Only Arms", "Doesn't Apply Hand Chams To Your Gun", ref Globals.Config.Visuals.HandChamOnlyArms);
            Toggle handchamsrgb = new Toggle("Hand Chams RGB", "Changes Hand Cham Colour Constantly", ref Globals.Config.Visuals.HandChamRGB);
            IntSlider handchamtype = new IntSlider("Hand Chams RGB", "0) Solid Colour | 1) Transparent Colour | 2) Galaxy", ref Globals.Config.Visuals.HandChamType,0,2,1);
            Visuals.Items.Add(novisor);
            Visuals.Items.Add(nightvision);
            Visuals.Items.Add(thermalvision);
            Visuals.Items.Add(alwaysday);
            Visuals.Items.Add(third);
            Visuals.Items.Add(flash);
            Visuals.Items.Add(viewangles);
            Visuals.Items.Add(handchams);
            Visuals.Items.Add(handchamsonlyarms);
            Visuals.Items.Add(handchamsrgb);
            Visuals.Items.Add(handchamtype);
        }
        void Movements()
        {
            LocalPlayer.Items.Add(Movement);
            Toggle intertia = new Toggle("No Intertia", "Master Switch For No Interia", ref Globals.Config.Movement.NoIntertia);
            Toggle stamina = new Toggle("Unlimited Stamina", "Master Switch For Unlimited Stamina", ref Globals.Config.Movement.UnlimitedStamina);
            Keybind flykey = new Keybind("Fly Hack Key", "Toggles On FlyHack", ref Globals.Config.Movement.FlyHackKey);
            Keybind flyupwards = new Keybind("Fly Up Key", "Makes Fly Hack Go Up", ref Globals.Config.Movement.FlyUpwardsKey);
            Keybind flydownwards = new Keybind("Fly Down Key", "Makes Fly Hack Go Down", ref Globals.Config.Movement.FlyDownWardsKey);
            Toggle highjump = new Toggle("High Jump", "Jump Higher", ref Globals.Config.Movement.HighJump);
            FloatSlider highjumpamount = new FloatSlider("High Jump Amount", "The Height Of Your High Jump", ref Globals.Config.Movement.HighJumpAmount, 0f, 3f, 0.1f);
            Toggle nofall = new Toggle("No Fall Damage", "Removes Fall Damage From Legs", ref Globals.Config.Movement.NoFall);
            Toggle bhop = new Toggle("Bhop", "Auto Jumps When You Hold Space", ref Globals.Config.Movement.Bhop);
            Toggle runandshoot = new Toggle("Run And Shoot", "Allows You To Shoot And Run", ref Globals.Config.Movement.RunAndShoot);
            Toggle crouch = new Toggle("Instant Crouch", "Crouch And Stand Instantly", ref Globals.Config.Movement.InstantCrouch);
            Toggle prone = new Toggle("Fake Prone", "Serverside Prone While Client Side Standing", ref Globals.Config.Movement.FakeProne);
            Movement.Items.Add(intertia);
            Movement.Items.Add(stamina);
            Movement.Items.Add(flykey);
            Movement.Items.Add(flyupwards);
            Movement.Items.Add(flydownwards);
            Movement.Items.Add(highjump);
            Movement.Items.Add(highjumpamount);
            Movement.Items.Add(nofall);
            Movement.Items.Add(bhop);
            Movement.Items.Add(runandshoot);
            Movement.Items.Add(crouch);
            Movement.Items.Add(prone);
        }
        void Weapons()
        {

            LocalPlayer.Items.Add(Weapon);
            Toggle norecoil = new Toggle("No Recoil", "Master Switch For No Recoil", ref Globals.Config.Weapon.NoRecoil);
            FloatSlider norecoilamount = new FloatSlider("No Recoil Amount", "Sets The Amount Of Recoil", ref Globals.Config.Weapon.NoRecoilAmount, 0f, 1f, 0.1f);
            Toggle nosway = new Toggle("No Sway", "Master Switch For No Sway", ref Globals.Config.Weapon.NoSway);
            FloatSlider noswayamount = new FloatSlider("No Sway Amount", "Sets The Amount Of Sway", ref Globals.Config.Weapon.NoSwayAmount, 0f, 1f, 0.1f);
            Toggle nomalfunction = new Toggle("No Malfunction", "Master Switch For No Malfunction", ref Globals.Config.Weapon.NoMalfunction);
            FloatSlider nomalfunctionamount = new FloatSlider("No Malfunction Chance", "Sets The Chance You Have Of Malfunctioning", ref Globals.Config.Weapon.NoMalfunctionChance, 0f, 1f, 0.1f);
            Toggle reload = new Toggle("Instant Reload", "Instantly Reload Weapons", ref Globals.Config.LocalPlayerWorld.InstantReload);
            Toggle fastfire = new Toggle("Fast Fire", "Increases Your FireRate", ref Globals.Config.Weapon.FastFire);
            IntSlider fastfirerate = new IntSlider("Fast Fire Rate", "How Fast Fast Fire Shoots", ref Globals.Config.Weapon.FastFireRate, 0, 100000, 100);
            Weapon.Items.Add(norecoil);
            Weapon.Items.Add(norecoilamount);
            Weapon.Items.Add(nosway);
            Weapon.Items.Add(noswayamount);
            Weapon.Items.Add(nomalfunction);
            Weapon.Items.Add(nomalfunctionamount);
            Weapon.Items.Add(reload);
            Weapon.Items.Add(fastfire);
            Weapon.Items.Add(fastfirerate);
        }
        void Exfils()
        {
            Esp.Items.Add(ExfilEsp);
            Toggle toggle = new Toggle("Enable", "Master Switch For Exfil Esp", ref Globals.Config.Exfil.Enable);
            Toggle distance = new Toggle("Distance", "Distance From Exfil Point", ref Globals.Config.Exfil.DrawDistance);
            IntSlider maxdistance = new IntSlider("Max Distance", "How Far Exfil Points Will Be Drawn",ref Globals.Config.Exfil.MaxDistance, 0, 2000, 1000);
            ExfilEsp.Items.Add(toggle);
            ExfilEsp.Items.Add(distance);
            ExfilEsp.Items.Add(maxdistance);
        }
        void ItemEsps()
        {
            Esp.Items.Add(ItemEsp);
            
            Toggle toggle = new Toggle("Enable", "Master Switch For Item Esp", ref Globals.Config.Item.Enabled);
            Toggle name = new Toggle("Draw Item Name", "Draws The Name Of Items", ref Globals.Config.Item.DrawName);
            Toggle distance = new Toggle("Draw Item Distance", "Draws The Distance From Item", ref Globals.Config.Item.DrawDistance);
            Toggle value = new Toggle("Draw Item Value", "Draws The Value Of Item", ref Globals.Config.Item.DrawValue);
            Toggle type = new Toggle("Draw Type Value", "Draws The Type Of Item", ref Globals.Config.Item.Type);
            IntSlider maxdistance = new IntSlider("Max Distance", "Max Distance The Esp Will Render Items", ref Globals.Config.Item.MaxDistance, 0, 2000, 50);
            IntSlider minvalue = new IntSlider("Min Value For Draw All", "Minimum Value Needed For Esp To Draw Item", ref Globals.Config.Item.MinValue, 0, 20000000, 1000);
            Toggle allitems = new Toggle("Draw All Items", "Ignores Filters And Draws All Items", ref Globals.Config.Item.DrawAll);
            Toggle enableinaccurate = new Toggle("Enable Inaccurate Filters", "Ignores Filters And Draws All Items", ref Globals.Config.Item.EnableInaccurateEsp);
            Toggle enableaccurate = new Toggle("Enable Accurate Filters", "Ignores Filters And Draws All Items", ref Globals.Config.Item.EnableAccurateEsp);
            SubMenu filters = new SubMenu("Inaccurate Filters", "Select Filters For Item Esp");
            SubMenu filters2 = new SubMenu("Accurate Filters", "Select Filters For Each Item Type");
            ItemEsp.Items.Add(toggle);
            ItemEsp.Items.Add(name);
            ItemEsp.Items.Add(distance);
            ItemEsp.Items.Add(value);
            ItemEsp.Items.Add(type);
            ItemEsp.Items.Add(maxdistance);
            ItemEsp.Items.Add(minvalue);
            ItemEsp.Items.Add(allitems);
            ItemEsp.Items.Add(enableinaccurate);
            ItemEsp.Items.Add(enableaccurate);
            ItemEsp.Items.Add(filters);
            ItemEsp.Items.Add(filters2);

            Toggle quest = new Toggle("Quest Items", "Draws Quest Items", ref Globals.Config.Item.DrawQuestItems);
            Toggle rare = new Toggle("Rare Items", "Draws Game Marked Rare Items", ref Globals.Config.Item.DrawRare);
            Toggle superrare = new Toggle("Super Rare Items", "Draws Game Marked Super Rare Items", ref Globals.Config.Item.DrawSuperRare);
            Toggle whitelisted = new Toggle("Whitelisted Items", "Draws Imported Whitelisted Items", ref Globals.Config.Item.WhitelistedItems);
            Toggle superrarevalue = new Toggle("Bypass Super Rare Min Value", "Draws Super Rare Items Despite Being Below Set Min Value", ref Globals.Config.Item.SuperRareBypassMinValue);
            IntSlider minvalueoverride = new IntSlider("Min Value Override", "Ignores Filters If The Item Value Is Over This Value", ref Globals.Config.Item.BypassFilterOverValue, 0, 20000000, 1000);
            filters.Items.Add(quest);
            filters.Items.Add(rare);
            filters.Items.Add(superrare);
            filters.Items.Add(whitelisted);
            filters.Items.Add(superrarevalue);
            filters.Items.Add(minvalueoverride);

            #region ItemFilter
            SubMenu BarterItem = new SubMenu("Barter Items", "");
            filters2.Items.Add(BarterItem);
            BarterItem.Items.Add(new Toggle("Enable", "Enables Barter Item Filter", ref Globals.Config.ItemBarterItem.Enable));
            BarterItem.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Barter Items To Show On Esp", ref Globals.Config.ItemBarterItem.MinValue, 0, 200000, 1000));

            SubMenu SpecialItem = new SubMenu("Special Items", "");
            filters2.Items.Add(SpecialItem);
            SpecialItem.Items.Add(new Toggle("Enable", "Enables Special Items Filter", ref Globals.Config.ItemSpecialItem.Enable));
            SpecialItem.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Special Items To Show On Esp", ref Globals.Config.ItemSpecialItem.MinValue, 0, 200000, 1000));

            SubMenu CaseItems = new SubMenu("Cases", "");
            filters2.Items.Add(CaseItems);
            CaseItems.Items.Add(new Toggle("Enable", "Enables Case Filter", ref Globals.Config.ItemCases.Enable));
            CaseItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Cases To Show On Esp", ref Globals.Config.ItemCases.MinValue, 0, 200000, 1000));

            SubMenu KeyItems = new SubMenu("Keys", "");
            filters2.Items.Add(KeyItems);
            KeyItems.Items.Add(new Toggle("Enable", "Enables Keys Filter", ref Globals.Config.ItemKey.Enable));
            KeyItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Keys To Show On Esp", ref Globals.Config.ItemKey.MinValue, 0, 200000, 1000));

            SubMenu WeaponItems = new SubMenu("Weapons", "");
            filters2.Items.Add(WeaponItems);
            WeaponItems.Items.Add(new Toggle("Enable", "Enables Weapons Filter", ref Globals.Config.ItemWeapon.Enable));
            WeaponItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Weapons To Show On Esp", ref Globals.Config.ItemWeapon.MinValue, 0, 200000, 1000));

            SubMenu ArmourItems = new SubMenu("Armour", "");
            filters2.Items.Add(ArmourItems);
            ArmourItems.Items.Add(new Toggle("Enable", "Enables Armour Filter", ref Globals.Config.ItemArmour.Enable));
            ArmourItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Armour To Show On Esp", ref Globals.Config.ItemArmour.MinValue, 0, 200000, 1000));

            SubMenu BackpackItems = new SubMenu("Backpacks", "");
            filters2.Items.Add(BackpackItems);
            BackpackItems.Items.Add(new Toggle("Enable", "Enables Armour Filter", ref Globals.Config.ItemBackpacks.Enable));
            BackpackItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Backpacks To Show On Esp", ref Globals.Config.ItemBackpacks.MinValue, 0, 200000, 1000));

            SubMenu MedItems = new SubMenu("Meds", "");
            filters2.Items.Add(MedItems);
            MedItems.Items.Add(new Toggle("Enable", "Enables Meds Filter", ref Globals.Config.ItemMeds.Enable));
            MedItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Meds To Show On Esp", ref Globals.Config.ItemMeds.MinValue, 0, 200000, 1000));

            SubMenu AmmoItems = new SubMenu("Ammo", "");
            filters2.Items.Add(AmmoItems);
            AmmoItems.Items.Add(new Toggle("Enable", "Enables Ammo Filter", ref Globals.Config.ItemAmmo.Enable));
            AmmoItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Ammo To Show On Esp", ref Globals.Config.ItemAmmo.MinValue, 0, 200000, 1000));

            SubMenu FuelItems = new SubMenu("Fuel", "");
            filters2.Items.Add(FuelItems);
            FuelItems.Items.Add(new Toggle("Enable", "Enables Fuel Filter", ref Globals.Config.ItemFuel.Enable));
            FuelItems.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Fuel To Show On Esp", ref Globals.Config.ItemFuel.MinValue, 0, 200000, 1000));

            SubMenu FoodWater = new SubMenu("Food And Water", "");
            filters2.Items.Add(FoodWater);
            FoodWater.Items.Add(new Toggle("Enable", "Enables Food And Water Filter", ref Globals.Config.ItemFoodAndDrink.Enable));
            FoodWater.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Food And Water To Show On Esp", ref Globals.Config.ItemFoodAndDrink.MinValue, 0, 200000, 1000));

            SubMenu Clothing = new SubMenu("Clothing", "");
            filters2.Items.Add(Clothing);
            Clothing.Items.Add(new Toggle("Enable", "Enables Clothing Filter", ref Globals.Config.ItemClothing.Enable));
            Clothing.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Clothing To Show On Esp", ref Globals.Config.ItemClothing.MinValue, 0, 200000, 1000));

            SubMenu Repair = new SubMenu("Repair Kits", "");
            filters2.Items.Add(Repair);
            Repair.Items.Add(new Toggle("Enable", "Enables Repair Kit Filter", ref Globals.Config.ItemRepairKits.Enable));
            Repair.Items.Add(new IntSlider("Min Loot Value", "Minimum Value For Repair Kits To Show On Esp", ref Globals.Config.ItemRepairKits.MinValue, 0, 200000, 1000));
            #endregion
        }
        void PlayerEsps()
        {
            Esp.Items.Add(PlayerEsp);
            Toggle toggle = new Toggle("Enable", "Master Switch For Player Esp", ref Globals.Config.Player.Enable);
            Toggle name = new Toggle("Name", "Draw Player Name", ref Globals.Config.Player.Name);
            Toggle distance = new Toggle("Distance", "Draw Player Distance", ref Globals.Config.Player.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Max Distance Players Will Render Till", ref Globals.Config.Player.MaxDistance, 0, 2000, 50);
            Toggle value = new Toggle("Value", "Draw Player Value", ref Globals.Config.Player.Value);
            Toggle weapon = new Toggle("Weapon", "Draw Player Weapon", ref Globals.Config.Player.Weapon);
            Toggle ammo = new Toggle("Ammo", "Draw Player Ammo", ref Globals.Config.Player.Ammo);
            Toggle KD = new Toggle("KD", "Draw Player Kill Death Ratio", ref Globals.Config.Player.KD);
            Toggle level = new Toggle("Level", "Draw Player Level", ref Globals.Config.Player.Level);
            Toggle flag = new Toggle("Flag", "Draw Admin Flagged Players", ref Globals.Config.Player.Flag);
            Toggle box = new Toggle("Box", "Draws Bounding Box Around Player", ref Globals.Config.Player.Box);
            Toggle fillbox = new Toggle("Fill Box", "Fills Box With A Colour", ref Globals.Config.Player.FillBox);
            Toggle healthbar = new Toggle("Health Bar", "Draws The Player's Health", ref Globals.Config.Player.HealthBar);
            Toggle chams = new Toggle("Chams", "Enables Chams", ref Globals.Config.Player.Chams);
            Toggle chamgear = new Toggle("Chams Only On Skin", "Draws Chams Only On The Player's Skin And Not Gear", ref Globals.Config.Player.ChamGear);
            Toggle chamrgb = new Toggle("Chams RGB", "Chams Strobe Between Colours", ref Globals.Config.Player.ChamRGB);
            IntSlider chamtype = new IntSlider("Chams Type", "0) Solid Normal Chams | 1) Transparent Chams | 2) Galaxy Chams", ref Globals.Config.Player.ChamType,0,2,1);
            PlayerEsp.Items.Add(toggle);
            PlayerEsp.Items.Add(name);
            PlayerEsp.Items.Add(distance);
            PlayerEsp.Items.Add(maxdistance);
            PlayerEsp.Items.Add(value);
            PlayerEsp.Items.Add(weapon);
            PlayerEsp.Items.Add(ammo);
            PlayerEsp.Items.Add(KD);
            PlayerEsp.Items.Add(level);
            PlayerEsp.Items.Add(flag);
            PlayerEsp.Items.Add(box);
            PlayerEsp.Items.Add(fillbox);
            PlayerEsp.Items.Add(healthbar);
            PlayerEsp.Items.Add(chams);
            PlayerEsp.Items.Add(chamgear);
            PlayerEsp.Items.Add(chamrgb);
            PlayerEsp.Items.Add(chamtype);
        }
        void ScavEsps()
        {
            Esp.Items.Add(ScavEsp);
            Toggle toggle = new Toggle("Enable", "Master Switch For Scav Esp", ref Globals.Config.Scav.Enable);
            Toggle name = new Toggle("Tag", "Draw Scav Tag Or Boss Name", ref Globals.Config.Scav.Tag);
            Toggle distance = new Toggle("Distance", "Draw Scav Distance", ref Globals.Config.Scav.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Max Distance Scavs Will Render Till", ref Globals.Config.Scav.MaxDistance, 0, 2000, 50);
            Toggle value = new Toggle("Value", "Draw Scav Value", ref Globals.Config.Scav.Value);
            Toggle weapon = new Toggle("Weapon", "Draw Scav Weapon", ref Globals.Config.Scav.Weapon);
            Toggle ammo = new Toggle("Ammo", "Draw Scav Ammo", ref Globals.Config.Scav.Ammo);
            Toggle box = new Toggle("Box", "Draws Bounding Box Around the Scav", ref Globals.Config.Scav.Box);
            Toggle fillbox = new Toggle("Fill Box", "Fills Box With A Colour", ref Globals.Config.Scav.FillBox);
            Toggle healthbar = new Toggle("Health Bar", "Draws The Scav's Health", ref Globals.Config.Scav.HealthBar);
            Toggle chams = new Toggle("Chams", "Enables Chams", ref Globals.Config.Scav.Chams);
            Toggle chamgear = new Toggle("Chams Only On Skin", "Draws Chams Only On The Player's Skin And Not Gear", ref Globals.Config.Scav.ChamGear);
            Toggle chamrgb = new Toggle("Chams RGB", "Chams Strobe Between Colours", ref Globals.Config.Scav.ChamRGB);
            IntSlider chamtype = new IntSlider("Chams Type", "0) Solid Normal Chams | 1) Transparent Chams | 2) Galaxy Chams", ref Globals.Config.Scav.ChamType, 0, 2, 1);
            ScavEsp.Items.Add(toggle);
            ScavEsp.Items.Add(name);
            ScavEsp.Items.Add(distance);
            ScavEsp.Items.Add(maxdistance);
            ScavEsp.Items.Add(value);
            ScavEsp.Items.Add(weapon);
            ScavEsp.Items.Add(ammo);
            ScavEsp.Items.Add(box);
            ScavEsp.Items.Add(fillbox);
            ScavEsp.Items.Add(healthbar);

            ScavEsp.Items.Add(chams);
            ScavEsp.Items.Add(chamgear);
            ScavEsp.Items.Add(chamrgb);
            ScavEsp.Items.Add(chamtype);
        }

        void PlayerScavEsps()
        {
            Esp.Items.Add(PlayerScavEsp);
            Toggle toggle = new Toggle("Enable", "Master Switch For Player Scav Esp", ref Globals.Config.ScavPlayer.Enable);
            Toggle name = new Toggle("Tag", "Draw Player Scav Tag", ref Globals.Config.ScavPlayer.Tag);
            Toggle distance = new Toggle("Distance", "Draw Player Scav Distance", ref Globals.Config.ScavPlayer.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Max Distance Player Scavs Will Render Till", ref Globals.Config.ScavPlayer.MaxDistance, 0, 2000, 50);
            Toggle value = new Toggle("Value", "Draw Player Scav Value", ref Globals.Config.ScavPlayer.Value);
            Toggle weapon = new Toggle("Weapon", "Draw Player Scav Weapon", ref Globals.Config.ScavPlayer.Weapon);
            Toggle ammo = new Toggle("Ammo", "Draw Player Scav Ammo", ref Globals.Config.ScavPlayer.Ammo);
            Toggle box = new Toggle("Box", "Draws Bounding Box Around the Player Scav", ref Globals.Config.ScavPlayer.Box);
            Toggle fillbox = new Toggle("Fill Box", "Fills Box With A Colour", ref Globals.Config.ScavPlayer.FillBox);
            Toggle healthbar = new Toggle("Health Bar", "Draws The Player Scav's Health", ref Globals.Config.ScavPlayer.HealthBar);
            Toggle chams = new Toggle("Chams", "Enables Chams", ref Globals.Config.ScavPlayer.Chams);
            Toggle chamgear = new Toggle("Chams Only On Skin", "Draws Chams Only On The Player's Skin And Not Gear", ref Globals.Config.ScavPlayer.ChamGear);
            Toggle chamrgb = new Toggle("Chams RGB", "Chams Strobe Between Colours", ref Globals.Config.ScavPlayer.ChamRGB);
            IntSlider chamtype = new IntSlider("Chams Type", "0) Solid Normal Chams | 1) Transparent Chams | 2) Galaxy Chams", ref Globals.Config.ScavPlayer.ChamType, 0, 2, 1);
            PlayerScavEsp.Items.Add(toggle);
            PlayerScavEsp.Items.Add(name);
            PlayerScavEsp.Items.Add(distance);
            PlayerScavEsp.Items.Add(maxdistance);
            PlayerScavEsp.Items.Add(value);
            PlayerScavEsp.Items.Add(weapon);
            PlayerScavEsp.Items.Add(ammo);
            PlayerScavEsp.Items.Add(box);
            PlayerScavEsp.Items.Add(fillbox);
            PlayerScavEsp.Items.Add(healthbar);

            PlayerScavEsp.Items.Add(chams);
            PlayerScavEsp.Items.Add(chamgear);
            PlayerScavEsp.Items.Add(chamrgb);
            PlayerScavEsp.Items.Add(chamtype);
        }
        void CorpseEsps()
        {
            Esp.Items.Add(CorpseEsp);
            Toggle toggle = new Toggle("Enable", "Master Switch For Corpse Esp", ref Globals.Config.Corpse.Enable);
            Toggle name = new Toggle("Tag", "Draw Corpse Tag", ref Globals.Config.Corpse.Tag);
            Toggle distance = new Toggle("Distance", "Draw Distance From Corpse", ref Globals.Config.Corpse.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Max Distance Corpses Will Be Drawn From", ref Globals.Config.Corpse.MaxDistance, 0, 2000, 50);
            Toggle value = new Toggle("Value", "Draw Corpse Value", ref Globals.Config.Corpse.Value);
            IntSlider mincorpsevalue = new IntSlider("Min Corpse Value", "Minimum Value For A Corpse To Be Drawn", ref Globals.Config.Corpse.MinValue, 0, 1000000, 1000);
            Keybind keybind = new Keybind("Contents Toggle Key", "Key Which Will Toggle On And Off Corpse Content", ref Globals.Config.Corpse.ContentsKey);
            IntSlider subvalue = new IntSlider("Min Sub Content Value", "Minimum Value For A Corpse Sub Item To Be Drawn", ref Globals.Config.Corpse.ContentMinValue, 0, 1000000, 1000);

            CorpseEsp.Items.Add(toggle);
            CorpseEsp.Items.Add(name);
            CorpseEsp.Items.Add(distance);
            CorpseEsp.Items.Add(maxdistance);
            CorpseEsp.Items.Add(value);
            CorpseEsp.Items.Add(mincorpsevalue);
            CorpseEsp.Items.Add(keybind);
            CorpseEsp.Items.Add(subvalue);

        }
        void ContainerEsps()
        {
            Esp.Items.Add(ContainerEsp);
            Toggle toggle = new Toggle("Enable", "Master Switch For Container Esp", ref Globals.Config.Container.Enable);
            Toggle name = new Toggle("Name", "Draw Container Name", ref Globals.Config.Container.Tag);
            Toggle distance = new Toggle("Distance", "Draw Distance From Container", ref Globals.Config.Container.Distance);
            IntSlider maxdistance = new IntSlider("Max Distance", "Max Distance Containers Will Be Drawn From", ref Globals.Config.Container.MaxDistance, 0, 2000, 50);
            Toggle value = new Toggle("Value", "Draw Container Value", ref Globals.Config.Container.Value);
            IntSlider mincorpsevalue = new IntSlider("Min Container Value", "Minimum Value For A Container To Be Drawn", ref Globals.Config.Container.MinValue, 0, 1000000, 1000);
            Keybind keybind = new Keybind("Contents Toggle Key", "Key Which Will Toggle On And Off Container Content", ref Globals.Config.Container.ContentsKey);
            IntSlider subvalue = new IntSlider("Min Sub Content Value", "Minimum Value For A Container Sub Item To Be Drawn", ref Globals.Config.Container.ContentMinValue, 0, 1000000, 1000);
            Toggle exclusivedraw = new Toggle("Draw Container Whith Sub Items", "Only Draws Containers With A Suitable Sub Item", ref Globals.Config.Container.OnlyDrawWithSubItems);
            ContainerEsp.Items.Add(toggle);
            ContainerEsp.Items.Add(name);
            ContainerEsp.Items.Add(distance);
            ContainerEsp.Items.Add(maxdistance);
            ContainerEsp.Items.Add(value);
            ContainerEsp.Items.Add(mincorpsevalue);
            ContainerEsp.Items.Add(keybind);
            ContainerEsp.Items.Add(subvalue);
            ContainerEsp.Items.Add(exclusivedraw);
        }
        void LocalPlayerInfo()
        {
            LocalPlayer.Items.Add(LocalPlayerInformation);
            Toggle toggle = new Toggle("Enable Local Player Information", "Master Switch To Draw Local Player Info", ref Globals.Config.LocalPlayerInfo.Enable);
            IntSlider x = new IntSlider("X Position Of Local Player Information Box", "The X Axis Position Of Local Player Information Box", ref Globals.Config.LocalPlayerInfo.x, 0, Screen.width, 20);
            IntSlider y = new IntSlider("Y Position Of Local Player Information Box", "The Y Axis Position Of Local Player Information Box", ref Globals.Config.LocalPlayerInfo.y, 0, Screen.height, 20);
            Toggle crosshair = new Toggle("Crosshair", "Master Switch To Draw Crosshair", ref Globals.Config.LocalPlayerInfo.Crosshair);
            Toggle radar = new Toggle("Radar", "Master Switch For Radar", ref Globals.Config.LocalPlayerInfo.Radar);
            IntSlider radarx = new IntSlider("X Position Of Radar", "The X Axis Position Of Radar", ref Globals.Config.LocalPlayerInfo.Radarx, 0, Screen.width, 20);
            IntSlider radary = new IntSlider("Y Position Of Radar", "The Y Axis Position Of Radar", ref Globals.Config.LocalPlayerInfo.Radary, 0, Screen.height, 20);
            IntSlider radarsize = new IntSlider("Radar Size", "Radius Of The Radar", ref Globals.Config.LocalPlayerInfo.RadarSize, 0, 2000, 20);
            IntSlider radarmax = new IntSlider("Maximum Radar Distance", "How Far People Will Render On Radar", ref Globals.Config.LocalPlayerInfo.RadarMaxDistance, 0, 2000, 50);
            Toggle playetoggle = new Toggle("Enable Enemy Player Information", "Master Switch To Draw Information Of Enemey Player", ref Globals.Config.PlayerInformation.Enable);
            IntSlider playerx = new IntSlider("X Position Of Player Information Box", "The X Axis Position Of Player Information Box", ref Globals.Config.PlayerInformation.X, 0, Screen.width, 20);
            IntSlider playery = new IntSlider("Y Position Of Player Information Box", "The Y Axis Position Of Player Information Box", ref Globals.Config.PlayerInformation.Y, 0, Screen.height, 20);
            Toggle scaletoradar = new Toggle("Scale Player Information Box To Radar", "Makes The Player Information Box The Same Size As Radar", ref Globals.Config.PlayerInformation.AutoSizeToRadar);
            Toggle cloesttocrosshair = new Toggle("Sort Player By Cloest To Crosshair", "Gets Cloest Player To Crosshair For Information Box Rather Than Cloest Player To You", ref Globals.Config.PlayerInformation.ClosestToCrosshair);
            LocalPlayerInformation.Items.Add(toggle);
            LocalPlayerInformation.Items.Add(x);
            LocalPlayerInformation.Items.Add(y);
            LocalPlayerInformation.Items.Add(crosshair);
            LocalPlayerInformation.Items.Add(radar);
            LocalPlayerInformation.Items.Add(radarx);
            LocalPlayerInformation.Items.Add(radary);
            LocalPlayerInformation.Items.Add(radarsize);
            LocalPlayerInformation.Items.Add(radarmax);
            LocalPlayerInformation.Items.Add(playetoggle);
            LocalPlayerInformation.Items.Add(playerx);
            LocalPlayerInformation.Items.Add(playery);
            LocalPlayerInformation.Items.Add(scaletoradar);
            LocalPlayerInformation.Items.Add(cloesttocrosshair);
        }
        void ColourPicker()
        {
            foreach (KeyValuePair<string, Color32> value in Globals.Config.Colours.GlobalColors)
            {
                SubMenu colourmenu = new SubMenu(value.Key, "");
                int alpha = Helpers.ColourHelper.GetColour(value.Key).a;
                IntSlider slidera = new IntSlider("Alpha", "Change The Colour Opacity", ref alpha, 0, 255, 10);
                int red = Helpers.ColourHelper.GetColour(value.Key).r;
                IntSlider sliderr = new IntSlider("Red", "Change Amount Of Red In Colour", ref red, 0, 255, 10);
                int green = Helpers.ColourHelper.GetColour(value.Key).g;
                IntSlider sliderg = new IntSlider("Green", "Change Amount Of Green In Colour", ref green, 0, 255, 10);
                int blue = Helpers.ColourHelper.GetColour(value.Key).b;
                IntSlider sliderb = new IntSlider("Blue", "Change Amount Of Blue In Colour", ref blue, 0, 255, 10);
                colourmenu.Items.Add(slidera);
                colourmenu.Items.Add(sliderr);
                colourmenu.Items.Add(sliderg);
                colourmenu.Items.Add(sliderb);
                colourmenu.Items.Add(new Button("Save Colour", "Right Arrow To Save The Colour", () => Helpers.ColourHelper.SetColour(value.Key, new Color32((byte)red, (byte)green, (byte)blue, (byte)alpha))));
                Colours.Items.Add(colourmenu);
            }
        }
        static KeyCode SetKey()
        {
            KeyCode Key = new KeyCode();
            Event e = Event.current;
            if (e.keyCode != KeyCode.RightArrow)
            {
                Key = e.keyCode;


            }
            else
            {
                Key = KeyCode.None;

            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                Key = KeyCode.Mouse0;

            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Key = KeyCode.Mouse1;

            }
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Key = KeyCode.Mouse2;

            }
            if (Input.GetKeyDown(KeyCode.Mouse3))
            {
                Key = KeyCode.Mouse3;

            }
            if (Input.GetKeyDown(KeyCode.Mouse4))
            {
                Key = KeyCode.Mouse4;

            }
            if (Input.GetKeyDown(KeyCode.Mouse5))
            {
                Key = KeyCode.Mouse5;

            }
            if (Input.GetKeyDown(KeyCode.Mouse6))
            {
                Key = KeyCode.Mouse6;

            }
            return Key;
        }
        IEnumerator KeyControls()
        {
            
            for (; ; )
            {
             
                try
                {

                    if (Input.GetKeyDown(KeyCode.Insert))
                        ShowGUI = !ShowGUI;
                    if (Input.GetKeyDown(KeyCode.DownArrow) && CurrentMenu.index < CurrentMenu.Items.Count)
                        CurrentMenu.index++;
                    if (Input.GetKeyDown(KeyCode.UpArrow) && CurrentMenu.index > 0)
                        CurrentMenu.index--;
                    if (Input.GetKeyDown(KeyCode.Backspace) && MenuHistory.Count > 1)
                    {
                        CurrentMenu = MenuHistory[MenuHistory.Count - 2];
                        MenuHistory.Remove(MenuHistory.Last<SubMenu>());
                        goto End;
                    }
                    if (((Input.GetKeyDown(KeyCode.LeftArrow) && Selected is SubMenu)) && CurrentMenu.index < CurrentMenu.Items.Count)
                    {

                        CurrentMenu = MenuHistory[MenuHistory.Count - 2];
                        MenuHistory.Remove(MenuHistory.Last<SubMenu>());
                        goto End;
                    }
                    foreach (Entity entity in CurrentMenu.Items)
                    {

                        if (CurrentMenu.index == CurrentMenu.Items.IndexOf(entity))
                            Selected = entity;
                        if (entity != Selected)
                            continue;

                    }
                    if (Selected is Keybind)
                    {
                        Keybind bind = Selected as Keybind;
                        if (bind.Value == KeyCode.None)
                            bind.Value = SetKey();

                    }
                    if (Selected is SubMenu && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
                    {
                        CurrentMenu = Selected as SubMenu;
                        MenuHistory.Add(Selected as SubMenu);
                        goto End;// opens a new menu so we need to exit the loop to then render our new currentmenu
                    }
                    if (Selected is Toggle && Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Toggle toggle = Selected as Toggle;
                        toggle.Value = true;
                    }
                    if (Selected is Toggle && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Toggle toggle = Selected as Toggle;
                        toggle.Value = false;
                    }
                    if (Selected is Button && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
                    {
                        Button button = Selected as Button;
                        button.Method();
                    }
                    if (Selected is IntSlider && Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        IntSlider slider = Selected as IntSlider;
                        int result = slider.Value + slider.IncrementValue;

                        if (result > slider.MaxValue)
                            slider.Value = slider.MaxValue;
                        else
                            slider.Value = result;
                    }
                    if (Selected is IntSlider && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        IntSlider slider = Selected as IntSlider;
                        int result = slider.Value - slider.IncrementValue;

                        if (result < slider.MinValue)
                            slider.Value = slider.MinValue;
                        else
                            slider.Value = result;
                    }
                    if (Selected is FloatSlider && Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        FloatSlider slider = Selected as FloatSlider;
                        float result = slider.Value + slider.IncrementValue;

                        if (result > slider.MaxValue)
                            slider.Value = slider.MaxValue;
                        else
                            slider.Value = result;
                    }
                    if (Selected is FloatSlider && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        FloatSlider slider = Selected as FloatSlider;
                        float result = slider.Value - slider.IncrementValue;

                        if (result < slider.MinValue)
                            slider.Value = slider.MinValue;
                        else
                            slider.Value = result;
                    }
                    if (Selected is Keybind && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)))
                    {
                        Keybind bind = Selected as Keybind;
                        bind.Value = KeyCode.None;
                    }
                
                }
                catch (Exception ex) {  }
                End:
                yield return new WaitForEndOfFrame();
            }
        }
    

        static List<SubMenu> MenuHistory = new List<SubMenu>();
        static SubMenu CurrentMenu;
        static Entity Selected;
        static SubMenu MainMenu = new SubMenu("Main", "Menu");
        static SubMenu Esp = new SubMenu("ESP", "Draw Visuals");
        static SubMenu Aimbot = new SubMenu("Aimbot", "Lock Onto Enemies");
        static SubMenu LocalPlayer = new SubMenu("Local Player", "Modify Your Player");
        static SubMenu Colours = new SubMenu("Colour Menu", "Allows You To Change Colours On The Cheat");
        static SubMenu Config = new SubMenu("Config Menu", "Allows You To Save And Load Settings");

        static SubMenu SaveConfig = new SubMenu("Load Configs", "");
        static SubMenu LoadConfig = new SubMenu("Save Configs", "");

        static SubMenu ItemEsp = new SubMenu("Item", "Draw Items On Overlay");
        static SubMenu PlayerEsp = new SubMenu("Player", "Draw Player Information On Overlay");
        static SubMenu ScavEsp = new SubMenu("Scav", "Draw Scav Information On Overlay");
        static SubMenu PlayerScavEsp = new SubMenu("Player Scav", "Draw Player Scav Information On Overlay");
        static SubMenu CorpseEsp = new SubMenu("Corpse", "Draw Corpse And Contents On Overlay");
        static SubMenu ContainerEsp = new SubMenu("Container", "Draw Containers And Contents On Overlay");
        static SubMenu ExfilEsp = new SubMenu("Exfil", "Draw Exfil Locations On Overlay");

        static SubMenu LocalPlayerInformation = new SubMenu("Information", "Draws Information About Local Player");
        static SubMenu Weapon = new SubMenu("Weapon", "Change Weapon Values");
        static SubMenu Movement = new SubMenu("Movement", "Change How Your Player Moves");
        static SubMenu Visuals = new SubMenu("Visuals", "Change Your Player's View");
        static SubMenu World = new SubMenu("World", "Change Local Player World Values");

        static SubMenu General = new SubMenu("General", "General Aimbot Settings");
        static SubMenu PlayerAimbot = new SubMenu("Player Aimbot", "Settings For Player Aimbot");
        static SubMenu ScavPlayerAimbot = new SubMenu("Scav Player Aimbot", "Settings For Scav Player Aimbot");
        static SubMenu ScavAimbot = new SubMenu("Scav Aimbot", "Settings For Scav Aimbot");

        static SubMenu PlayerRageAimbot = new SubMenu("Player Rage Aimbot", "Configure The Rage Player Rage Silent Aim");
        static SubMenu ScavPlayerRageAimbot = new SubMenu("Scav Player Rage Aimbot", "Configure The Rage Scav Player Rage Silent Aim");
        static SubMenu ScavRageAimbot = new SubMenu("Scav Rage Aimbot", "Configure The Rage Scav Rage Silent Aim");

        static SubMenu PlayerAutoAimbot = new SubMenu("Player AutoShoot Aimbot", "Configure The Player AutoShoot");
        static SubMenu ScavPlayerAutoAimbot = new SubMenu("Scav Player AutoShoot Aimbot", "Configure The Scav Player AutoShoot");
        static SubMenu ScavAutoAimbot = new SubMenu("Scav AutoShoot Aimbot", "Configure The Scav AutoShoot");

        static SubMenu PlayerStaticAimbot = new SubMenu("Player Static Aimbot", "Configure The Player Static Aimbot");
        static SubMenu ScavPlayerStaticAimbot = new SubMenu("Scav Player Static Aimbot", "Configure The Scav Player Static Aimbot");
        static SubMenu ScavStaticAimbot = new SubMenu("Scav Static Aimbot", "Configure The Scav Static Aimbot");

        static Direct2DBrush PrimaryColour;
        static Direct2DBrush SecondaryColour;
        static Direct2DFont Verdana;
        public static void Render(Direct2DRenderer Renderer)
        {
           
            #region Brushes
            Color32 OriginalPrimaryColour = ColourHelper.GetColour("Menu Primary Colour");
            Color32 OriginalSecondaryColour = ColourHelper.GetColour("Menu Secondary Colour");
             PrimaryColour = Renderer.CreateBrush(OriginalPrimaryColour.r, OriginalPrimaryColour.g, OriginalPrimaryColour.b, OriginalPrimaryColour.a);
             SecondaryColour = Renderer.CreateBrush(OriginalSecondaryColour.r, OriginalSecondaryColour.g, OriginalSecondaryColour.b, OriginalSecondaryColour.a);
            #endregion
            #region Font
            Verdana = Renderer.CreateFont("Verdana", 18);
            #endregion
            #region MenuHistory
            try
            {
                if (!ShowGUI)
                    return;
                    string text = string.Empty;
                    if (MenuHistory.Count > 0)
                    {
                        foreach (SubMenu subMenu in MenuHistory)
                        {
                            if (subMenu != null)
                            {
                                if (subMenu == MenuHistory.Last<SubMenu>())
                                {
                                    text += subMenu.Name + " v ";
                                }
                                else
                                {
                                    text = text + subMenu.Name + " > ";
                                }
                            }
                        }
                    }
                    Renderer.DrawText(text, Globals.Config.Menu.Menux - 10, Globals.Config.Menu.Menuy - 20, 12, Verdana, PrimaryColour);
                    #endregion

                    foreach (Entity entity in CurrentMenu.Items)
                    {

                        if (Selected == entity)
                        {
                            if (entity.Description != null)
                                Renderer.DrawText(entity.Description, Globals.Config.Menu.Menux - 10, Globals.Config.Menu.Menuy + (20f * (float)CurrentMenu.Items.Count), 12, Verdana, PrimaryColour);
                            if (entity is Toggle)
                            {
                                Toggle toggle = entity as Toggle;
                                string ToggleStr = toggle.Value ? "Enabled" : "Disabled";
                                Renderer.DrawText($"- {entity.Name}: {ToggleStr}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                            }
                            if (entity is IntSlider)
                            {
                                IntSlider slider = entity as IntSlider;
                                Renderer.DrawText($"- {entity.Name}: {slider.Value}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                            }
                            if (entity is FloatSlider)
                            {
                                FloatSlider slider = entity as FloatSlider;
                                Renderer.DrawText($"- {entity.Name}: {Math.Round(slider.Value,2)}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                            }
                            if (entity is Keybind)
                            {
                                Keybind bind = entity as Keybind;
                                Renderer.DrawText($"- {entity.Name}: {bind.Value.ToString()}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                            }
                            if (entity is SubMenu)

                                Renderer.DrawText($"> {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                            if (entity is Button)
                                Renderer.DrawText($"- {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 14, Verdana, PrimaryColour);
                        }
                        else
                        {
                            if (entity is Toggle)
                            {
                                Toggle toggle = entity as Toggle;
                                string ToggleStr = toggle.Value ? "Enabled" : "Disabled";
                                Renderer.DrawText($"- {entity.Name}: {ToggleStr}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                            }
                            if (entity is IntSlider)
                            {
                                IntSlider slider = entity as IntSlider;
                                Renderer.DrawText($"- {entity.Name}: {slider.Value}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                            }
                            if (entity is FloatSlider)
                            {
                                FloatSlider slider = entity as FloatSlider;
                                Renderer.DrawText($"- {entity.Name}: {Math.Round(slider.Value, 2)}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                            }
                            if (entity is Keybind)
                            {
                                Keybind bind = entity as Keybind;
                                Renderer.DrawText($"- {entity.Name}: {bind.Value.ToString()}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                            }
                            if (entity is SubMenu)
                                Renderer.DrawText($"> {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                            if (entity is Button)
                                Renderer.DrawText($"- {entity.Name}", Globals.Config.Menu.Menux, Globals.Config.Menu.Menuy + (20 * CurrentMenu.Items.IndexOf(entity)), 12, Verdana, SecondaryColour);
                        }
                    }
                
            }
            catch (Exception ex) { }
      //      GC.Collect();
     //       GC.WaitForPendingFinalizers();
      //      GC.Collect();

        }

    }
}
