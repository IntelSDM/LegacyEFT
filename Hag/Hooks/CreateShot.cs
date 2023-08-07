using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using EFT;
using UnityEngine;
using EFT.InventoryLogic;
using EFT.Ballistics;
using System.IO;
using Hag.Esp;

namespace Hag.Hooks
{
    class CreateShot : MonoBehaviour
    {
        private static DumbHook SilentAimHook;
        public static bool Autoshoot = false;
        void Awake()
        {
            SilentAimHook = new DumbHook();
            SilentAimHook.Init(typeof(EFT.Ballistics.BallisticsCalculator).GetMethod("CreateShot"), typeof(CreateShot).GetMethod("CreateShotHook"));
            SilentAimHook.Hook();
        }
     
        Vector3 Prediction(Player player)
        {

            // playervelocity 
            Vector3 Velocity = player.Velocity;
            float num = Globals.LocalPlayerWeapon.CurrentAmmoTemplate.BulletMassGram / 1000f;
            float num2 = Globals.LocalPlayerWeapon.CurrentAmmoTemplate.BulletDiameterMilimeters / 1000f;
            float num3 = num2 * num2 * 3.1415927f / 4f;
            float num4 = 0.01f;
            
            Vector3 ret = Vector3.zero;

            return ret;
        }
        public static bool Penetrate(BallisticCollider collider)
        {
            // File.WriteAllText("test.txt", collider.PenetrationLevel.ToString());
            

            if (collider.PenetrationChance >= 1E-45f && collider.PenetrationLevel <= 15)
            {
                
                if (Globals.LocalPlayerWeapon.CurrentAmmoTemplate.PenetrationPower > collider.PenetrationLevel )
                {
                    return true;
                }
            }
            return false;
        }
        public static bool PenetrateAutoShoot(BallisticCollider collider)
        {
            // File.WriteAllText("test.txt", collider.PenetrationLevel.ToString());


            if (collider.PenetrationChance >= 1E-45f && collider.PenetrationLevel <= 5)
            {

                if (Globals.LocalPlayerWeapon.CurrentAmmoTemplate.PenetrationPower > collider.PenetrationLevel)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Ricochet( BallisticCollider collider)
        {
            if (collider.RicochetChance >= 1E-45f && Vector3.Angle(-Globals.LocalPlayer.Fireport.forward.normalized, Helpers.RaycastHelper.BarrelRaycast().normalized) > 42.5f)
            {
                
                    return true;
                
            }
            return false;
        }
        public static Player GetTargetPlayer()
        {
            Player baseplayer = new Player();
            try
            {

                if (Globals.GameWorld == null)
                    return baseplayer;
                List<Player> playerlist = Hag.Aimbot.General.SortClosestToCrosshair(Globals.GameWorld.RegisteredPlayers);

                foreach (Player player in playerlist)
                {   

                    if (player == null)
                        continue;
                    bool Scav =player.Profile.Info.RegistrationDate <= 0;
                    bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
                    bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == player.Profile.Info.GroupId
                        && player.Profile.Info.GroupId != "0"
                        && player.Profile.Info.GroupId != ""
                        && player.Profile.Info.GroupId != null;
                    Vector2 vector = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
                    int num = (int)Vector2.Distance(Globals.MainCamera.WorldToScreenPoint(player.PlayerBones.Head.position), vector); // fov check
                    int num2 = (int)Vector3.Distance(Globals.MainCamera.transform.position, player.Transform.position); // distance from player

                    if (num > Globals.Config.Aimbot.Fov && !Globals.Config.Aimbot.IgnoreFov)
                        continue;
                   
                    if (friendly)
                        continue;
                    if (!player.HealthController.IsAlive)
                        continue;
                    if (player == Globals.LocalPlayer)
                        continue;
                    #region Scav
                    if (Scav && Globals.Config.ScavAimbot.RageAimbot && num2 < Globals.Config.ScavAimbot.MaxDistance)
                    {
                        if (RaycastHelper.Head(player,Globals.Config.Aimbot.AutoWall) || RaycastHelper.IsPointVisible(player, player.PlayerBones.LeftShoulder.position, Globals.Config.Aimbot.AutoWall) || RaycastHelper.IsPointVisible(player, player.PlayerBones.RightShoulder.position, Globals.Config.Aimbot.AutoWall) || RaycastHelper.Spine1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.Spine3(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.RightThigh1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.RightThigh2(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.LeftThigh1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.LeftThigh2(player, Globals.Config.Aimbot.AutoWall))
                        {
                            return player;

                        }
                    }
                    #endregion
                    #region ScavPlayer
                    if (ScavPlayer && Globals.Config.ScavPlayerAimbot.RageAimbot && num2 < Globals.Config.ScavPlayerAimbot.MaxDistance)
                    {
                        if (RaycastHelper.Head(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.IsPointVisible(player, player.PlayerBones.LeftShoulder.position, Globals.Config.Aimbot.AutoWall) || RaycastHelper.IsPointVisible(player, player.PlayerBones.RightShoulder.position, Globals.Config.Aimbot.AutoWall) || RaycastHelper.Spine1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.Spine3(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.RightThigh1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.RightThigh2(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.LeftThigh1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.LeftThigh2(player, Globals.Config.Aimbot.AutoWall))
                        {
                            return player;

                        }
                    }
                    #endregion
                    #region Player
                    if ((!Scav && !ScavPlayer) && Globals.Config.PlayerAimbot.RageAimbot && num2 < Globals.Config.PlayerAimbot.MaxDistance)
                    {
                        if (RaycastHelper.Head(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.IsPointVisible(player, player.PlayerBones.LeftShoulder.position, Globals.Config.Aimbot.AutoWall) || RaycastHelper.IsPointVisible(player, player.PlayerBones.RightShoulder.position, Globals.Config.Aimbot.AutoWall) || RaycastHelper.Spine1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.Spine3(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.RightThigh1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.RightThigh2(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.LeftThigh1(player, Globals.Config.Aimbot.AutoWall) || RaycastHelper.LeftThigh2(player, Globals.Config.Aimbot.AutoWall))
                        {
                            return player;

                        }
                    }
                    #endregion
                }



            }
            catch
            { }
            return baseplayer;
        }
   
      
        public static Player GetAutoShootTargetPlayer()
        {
            
                Player baseplayer = new Player();
            try
            {
                foreach (Esp.BasePlayer bp in Globals.PlayerList)   
                {
                    Player player = bp.Player;
                    bool Scav = player.Profile.Info.RegistrationDate <= 0;
                    bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
                    bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == player.Profile.Info.GroupId
                        && player.Profile.Info.GroupId != "0"
                        && player.Profile.Info.GroupId != ""
                        && player.Profile.Info.GroupId != null;
                    int distance = (int)Vector3.Distance(player.Transform.position, Globals.MainCamera.transform.position);
                 
                    if (!Globals.Config.ScavAimbot.AutoShoot && Scav || distance > Globals.Config.ScavAimbot.AutoShootMaxDistance)
                        continue;
                    if (!Globals.Config.ScavPlayerAimbot.AutoShoot && ScavPlayer || distance > Globals.Config.ScavPlayerAimbot.AutoShootMaxDistance)
                        continue;
                    if (!Globals.Config.PlayerAimbot.AutoShoot && !(Scav || ScavPlayer) || distance > Globals.Config.PlayerAimbot.AutoShootMaxDistance)
                        continue;
                    if (friendly)
                        continue;
                    if (player == Globals.LocalPlayer)
                        continue;
                    if (!player.HealthController.IsAlive)
                        continue;
                    if (Helpers.RaycastHelper.AutoShootHead(player, Globals.Config.Aimbot.AutoWall))
                        return player;
                }
            }
            catch { }
            return baseplayer;
        }
        public object CreateShotHook(object ammo, Vector3 origin, Vector3 direction, int fireIndex, Player player1, Item weapon, float speedFactor = 1f, int fragmentIndex = 0)
        {
           
            if(Globals.Config.Aimbot.InstantHit && !(speedFactor >= 5))
                speedFactor = 5;
                #region Rage Aimbot
            if (!Autoshoot)
            {
                if (GetTargetPlayer() != null)
                {
                    Weapon wpn = Globals.LocalPlayerWeapon;
                    System.Random hitchance = new System.Random();
                    //  speedFactor = 1000;
                    Player targetplayer = GetTargetPlayer();
                    bool Scav = targetplayer.Profile.Info.RegistrationDate <= 0;
                    bool ScavPlayer = targetplayer.Profile.Side == EPlayerSide.Savage && !Scav && !targetplayer.IsAI;
                    #region ScavRageAimbot
                    if ((Scav) && Globals.Config.ScavAimbot.RageAimbot)
                    {



                        if (hitchance.Next(1, 100) <= Globals.Config.ScavAimbot.RageAimbotHitchance)
                        {
                            // we do this from decending order, least important to most important limb so then if a more important one is visible it will go through the code and get to the most important one that is visible.
                            if (RaycastHelper.RightThigh1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                              
                                Vector3 hitvector = targetplayer.PlayerBones.RightThigh1.position;

                                direction = (hitvector - origin).normalized;
                                // direction = (targetplayer.PlayerBones.RightThigh1.position - origin).normalized;
                            }
                            if (RaycastHelper.LeftThigh1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftThigh1.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.RightThigh2(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightThigh2.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.LeftThigh2(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftThigh2.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.IsPointVisible(targetplayer, targetplayer.PlayerBones.RightShoulder.position, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightShoulder.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.IsPointVisible(targetplayer, targetplayer.PlayerBones.LeftShoulder.position, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftShoulder.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Spine3(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Spine3.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Spine1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Spine1.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Head(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                               // Vector3 Scaled = Vector3.Scale(targetplayer.PlayerBones.RightThigh1.position, targetplayer.Velocity);
                                //Vector3 Difference = Scaled - targetplayer.PlayerBones.Head.position;
                                Vector3 hitvector = targetplayer.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);

                                direction = (hitvector - origin).normalized;

                            }
                        }



                    }
                    #endregion
                    #region ScavPlayerRageAimbot
                    if ((ScavPlayer) && Globals.Config.ScavPlayerAimbot.RageAimbot)
                    {



                        if (hitchance.Next(1, 100) <= Globals.Config.ScavPlayerAimbot.RageAimbotHitchance)
                        {
                            // we do this from decending order, least important to most important limb so then if a more important one is visible it will go through the code and get to the most important one that is visible.
                            if (RaycastHelper.RightThigh1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightThigh1.position;

                                direction = (hitvector - origin).normalized;
                                // direction = (targetplayer.PlayerBones.RightThigh1.position - origin).normalized;
                            }
                            if (RaycastHelper.LeftThigh1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftThigh1.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.RightThigh2(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightThigh2.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.LeftThigh2(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftThigh2.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.IsPointVisible(targetplayer, targetplayer.PlayerBones.RightShoulder.position, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightShoulder.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.IsPointVisible(targetplayer, targetplayer.PlayerBones.LeftShoulder.position, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftShoulder.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Spine3(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Spine3.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Spine1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Spine1.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Head(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);

                                direction = (hitvector - origin).normalized;

                            }
                        }



                    }
                    #endregion
                    #region PlayerRageAimbot
                    if ((!Scav && !ScavPlayer) && Globals.Config.PlayerAimbot.RageAimbot)
                    {



                        if (hitchance.Next(1, 100) <= Globals.Config.PlayerAimbot.RageAimbotHitchance)
                        {
                            // we do this from decending order, least important to most important limb so then if a more important one is visible it will go through the code and get to the most important one that is visible.
                            if (RaycastHelper.RightThigh1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightThigh1.position;

                                direction = (hitvector - origin).normalized;
                                // direction = (targetplayer.PlayerBones.RightThigh1.position - origin).normalized;
                            }
                            if (RaycastHelper.LeftThigh1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftThigh1.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.RightThigh2(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightThigh2.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.LeftThigh2(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftThigh2.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.IsPointVisible(targetplayer, targetplayer.PlayerBones.RightShoulder.position, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.RightShoulder.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.IsPointVisible(targetplayer, targetplayer.PlayerBones.LeftShoulder.position, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.LeftShoulder.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Spine3(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Spine3.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Spine1(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Spine1.position;

                                direction = (hitvector - origin).normalized;
                            }
                            if (RaycastHelper.Head(targetplayer, Globals.Config.Aimbot.AutoWall))
                            {
                                Vector3 hitvector = targetplayer.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);

                                direction = (hitvector - origin).normalized;

                            }
                        }



                    }
                }
                #endregion
            }
            #endregion
            #region AutoShoot
            if (Autoshoot)
            {
                Player targetplayer = GetAutoShootTargetPlayer();
                if (targetplayer != null)
                {
                    if (RaycastHelper.AutoShootHead(targetplayer, Globals.Config.Aimbot.AutoWall))
                    {
                        Vector3 hitvector = targetplayer.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);

                        direction = (hitvector - origin).normalized;

                    }
                }
                Autoshoot = false;
            }
            #endregion
            SilentAimHook.Unhook();


            object[] parameters = new object[]
               {
                    ammo,
                    origin,
                    direction,
                    fireIndex,
                    player1,
                    weapon,
                    speedFactor,
                    fragmentIndex
               };
            object result = SilentAimHook.OriginalMethod.Invoke(this, parameters);

            SilentAimHook.Hook();
            return result;
        }

        private static float NextShot;


    
    }
}
