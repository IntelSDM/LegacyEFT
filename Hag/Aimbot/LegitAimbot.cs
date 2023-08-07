using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hag.Esp;
using EFT;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using Hag.Helpers;

namespace Hag.Aimbot
{
    class LegitAimbot : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Aim());
        }
        IEnumerator Aim()
        {
            for (; ; )
            {
                //General.CalculateBulletDrop();
                try
                {
                    Aimbot();
                }
                catch { }
                yield return new WaitForFixedUpdate();
            }
        }
        #region Logic Functions
        private static bool IsLegitBoneVisible(int bone, Player player)
        {
            switch (bone)
            {
                case 0:
                    return RaycastHelper.Head(player, false);
                case 1:
                    return RaycastHelper.IsPointVisible(player, player.PlayerBones.Neck.position, false);
                case 2:
                    return RaycastHelper.IsPointVisible(player, player.PlayerBones.Spine1.position, false);
                case 3:
                    return RaycastHelper.IsPointVisible(player, player.PlayerBones.Spine3.position, false);
                case 4:
                    return RaycastHelper.IsPointVisible(player, player.PlayerBones.Pelvis.position, false);
            }

            return false;
        }
        public static Player GetLegitTargetPlayer()
        {
            Player targetplayer = new Player();
            try
            {
                if (Globals.GameWorld == null)
                    return targetplayer;
                List<Player> playerlist = General.SortClosestToCrosshair(Globals.GameWorld.RegisteredPlayers);
                foreach (Player player in playerlist)
                {

                    if (player == null)
                        continue;
                    bool Scav = player.Profile.Info.RegistrationDate <= 0;
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
                    if (!Globals.Config.ScavAimbot.LegitAimbot && Scav || num2 > Globals.Config.ScavAimbot.LegitAimbotMaxDistance)
                        continue;
                    if (!Globals.Config.ScavPlayerAimbot.LegitAimbot && ScavPlayer || num2 > Globals.Config.ScavPlayerAimbot.LegitAimbotMaxDistance)
                        continue;
                    if (!Globals.Config.PlayerAimbot.LegitAimbot && !(Scav || ScavPlayer) || num2 > Globals.Config.PlayerAimbot.LegitAimbotMaxDistance)
                        continue;
                    if (friendly)
                        continue;
                    if (!player.HealthController.IsAlive)
                        continue;
                    if (player == Globals.LocalPlayer)
                        continue;
                    if (Scav && !General.IsBoneOnScreen(Globals.Config.ScavAimbot.HitBone, player))
                        continue;
                    if (ScavPlayer && !General.IsBoneOnScreen(Globals.Config.ScavPlayerAimbot.HitBone, player))
                        continue;
                    if (!(ScavPlayer || Scav) && !General.IsBoneOnScreen(Globals.Config.PlayerAimbot.HitBone, player))
                        continue;
                    if (Scav && Globals.Config.ScavAimbot.LegitVischecks && !IsLegitBoneVisible(Globals.Config.ScavAimbot.HitBone, player))
                        continue;
                    if (ScavPlayer && Globals.Config.ScavPlayerAimbot.LegitVischecks && !IsLegitBoneVisible(Globals.Config.ScavPlayerAimbot.HitBone, player))
                        continue;
                    if (!(ScavPlayer || Scav) && Globals.Config.PlayerAimbot.LegitVischecks && !IsLegitBoneVisible(Globals.Config.PlayerAimbot.HitBone, player))
                        continue;
                    targetplayer = player;
                    return targetplayer;
                }

            }
            catch { }

            return targetplayer;
        }
#endregion
        void Aimbot()
        {
            try
            {
                Player player = GetLegitTargetPlayer();
                if (player == null)
                    return;
                if (!(Globals.Config.ScavAimbot.LegitAimbot || Globals.Config.ScavPlayerAimbot.LegitAimbot || Globals.Config.PlayerAimbot.LegitAimbot))
                    return;
                if (!(Input.GetKey(Globals.Config.Aimbot.AimbotKey)))
                    return;
                if (Globals.LocalPlayerWeapon == null)
                    return;
                bool Scav = player.Profile.Info.RegistrationDate <= 0;
                bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
                Vector3 pos = Vector3.zero;
                float smooth = 0;
                bool dosmoth = false;
                if (Scav)
                {
                    smooth = Globals.Config.ScavAimbot.LegitSmoothing;
                    dosmoth = Globals.Config.ScavAimbot.LegitSmoothing > 0;
                    switch (Globals.Config.ScavAimbot.LegitHitbox)
                    {
                        case 0:
                            pos = player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);
                            break;
                        case 1:
                            pos = player.PlayerBones.Neck.position;
                            break;
                        case 2:
                            pos = player.PlayerBones.Spine3.position;
                            break;
                        case 3:
                            pos = player.PlayerBones.Spine1.position;
                            break;
                        case 4:
                            pos = player.PlayerBones.Pelvis.position;
                            break;
                    }
                }
                if (ScavPlayer)
                {
                    smooth = Globals.Config.ScavPlayerAimbot.LegitSmoothing;
                    dosmoth = Globals.Config.ScavPlayerAimbot.LegitSmoothing > 0;
                    switch (Globals.Config.ScavPlayerAimbot.LegitHitbox)
                    {
                        case 0:
                            pos = player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);
                            break;
                        case 1:
                            pos = player.PlayerBones.Neck.position;
                            break;
                        case 2:
                            pos = player.PlayerBones.Spine3.position;
                            break;
                        case 3:
                            pos = player.PlayerBones.Spine1.position;
                            break;
                        case 4:
                            pos = player.PlayerBones.Pelvis.position;
                            break;
                    }
                }
                if (!(Scav || ScavPlayer))
                {
                    smooth = Globals.Config.PlayerAimbot.LegitSmoothing;
                    dosmoth = Globals.Config.PlayerAimbot.LegitSmoothing > 0;
                    switch (Globals.Config.PlayerAimbot.LegitHitbox)
                    {
                        case 0:
                            pos = player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0);
                            break;
                        case 1:
                            pos = player.PlayerBones.Neck.position;
                            break;
                        case 2:
                            pos = player.PlayerBones.Spine3.position;
                            break;
                        case 3:
                            pos = player.PlayerBones.Spine1.position;
                            break;
                        case 4:
                            pos = player.PlayerBones.Pelvis.position;
                            break;
                    }
                }

                float ScreenCenterX = (Screen.width / 2);
                float ScreenCenterY = (Screen.height / 2);
                float TargetX = 0;
                float TargetY = 0;
                //  pos.y += General.CalculateDrop(pos);
           //     General.CalculateDrop(pos);
                float x = Updating.WorldPointToScreenPoint(pos).x;
                float y = Updating.WorldPointToScreenPoint(pos).y;
                float AimSpeed = ((100 - smooth) + 1) * 1000;

                if (x != 0)
                {
                    if (x > ScreenCenterX)
                    {
                        TargetX = -(ScreenCenterX - x);
                        if (dosmoth)
                            TargetX /= AimSpeed;
                        if (TargetX + ScreenCenterX > ScreenCenterX * 2) TargetX = 0;
                    }
                    if (x < ScreenCenterX)
                    {
                        TargetX = x - ScreenCenterX;
                        if (dosmoth)
                            TargetX /= AimSpeed;
                        if (TargetX + ScreenCenterX < 0) TargetX = 0;
                    }
                }
                if (y != 0)
                {
                    if (y > ScreenCenterY)
                    {
                        TargetY = -(ScreenCenterY - y);
                        if (dosmoth)
                            TargetY /= AimSpeed;
                        if (TargetY + ScreenCenterY > ScreenCenterY * 2) TargetY = 0;
                    }
                    if (y < ScreenCenterY)
                    {
                        TargetY = y - ScreenCenterY;
                        if (dosmoth)
                            TargetY /= AimSpeed;
                        if (TargetY + ScreenCenterY < 0) TargetY = 0;
                    }
                }
                if (dosmoth)
                {
                    TargetX /= 10;
                    TargetY /= 10;
                    if (Math.Abs(TargetX) < 1)
                    {
                        if (TargetX > 0)
                            TargetX = 1;
                        if (TargetX < 0)
                            TargetX = -1;
                    }
                    if (Math.Abs(TargetY) < 1)
                    {
                        if (TargetY > 0)
                            TargetY = 1;
                        if (TargetY < 0)
                            TargetY = -1;
                    }
                    mouse_event(0x0001, (uint)TargetX, (uint)TargetY, 0, UIntPtr.Zero);
                }
                else
                {
                    mouse_event(0x0001, (uint)(TargetX), (uint)(TargetY), 0, UIntPtr.Zero);
                }
            }
            catch { }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
    }
}
