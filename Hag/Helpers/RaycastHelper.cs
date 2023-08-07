using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT;
namespace Hag.Helpers
{
    class RaycastHelper
    {
        private static RaycastHit RaycastHit;
        private static readonly LayerMask Mask = 1 << 12 | 1 << 16 | 1 << LayerMask.GetMask("Foliage") | 1 << LayerMask.GetMask("Grass");
        public static bool IsPointVisible(Player player, Vector3 BonePos,bool autowall)
        {
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                    BonePos,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                   BonePos,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        BonePos,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }

        #region Temp
        public static Vector3 BarrelRaycast()
        {
            try
            {
                if (Globals.LocalPlayer.Fireport == null)
                    return Vector3.zero;

                Physics.Linecast(
                    Globals.LocalPlayer.Fireport.position,
                    Globals.LocalPlayer.Fireport.position - Globals.LocalPlayer.Fireport.up * 1000f,
                    out RaycastHit,
                    Mask);
               
                return RaycastHit.point;
            }
            catch
            {
                return Vector3.zero;
            }
        }
        public static EFT.Ballistics.BallisticCollider BarrelRaycastCollider()
        {
            try
            {
                if (Globals.LocalPlayer.Fireport == null)
                    return null;

                Physics.Linecast(
                    Globals.LocalPlayer.Fireport.position,
                    Globals.LocalPlayer.Fireport.position - Globals.LocalPlayer.Fireport.up * 1000f,
                    out RaycastHit,
                    Mask);
                BaseBallistic component = RaycastHit.collider.GetComponent<BaseBallistic>();
                if (component == null)
                    return null;

                return component.Get(RaycastHit.point);
            }
            catch
            {
                return null;
            }
        }
        public static EFT.Ballistics.BallisticCollider BarrelRaycastCollider(out Vector3 pos)
        {
            pos = Vector3.zero;
            try
            {
                if (Globals.LocalPlayer.Fireport == null)
                    return null;

                Physics.Linecast(
                    Globals.LocalPlayer.Fireport.position,
                    Globals.LocalPlayer.Fireport.position - Globals.LocalPlayer.Fireport.up * 1000f,
                    out RaycastHit,
                    Mask);
                pos = RaycastHit.point;
                BaseBallistic component = RaycastHit.collider.GetComponent<BaseBallistic>();
                if (component == null)
                    return null;
                
                return component.Get(RaycastHit.point);
            }
            catch
            {
                return null;
            }
        }
        public static bool Spine1(Player player,bool autowall)
        {

            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                     player.PlayerBones.Spine1.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.Spine1.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.Spine1.position,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        public static bool Spine3(Player player, bool autowall)
        {
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                     player.PlayerBones.Spine3.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.Spine3.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.Spine3.position,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        public static bool LeftThigh1(Player player,bool autowall)
        {
           
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                     player.PlayerBones.LeftThigh1.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.LeftThigh1.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.LeftThigh1.position,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        public static bool LeftThigh2(Player player, bool autowall)
        {
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                     player.PlayerBones.LeftThigh2.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.LeftThigh2.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.LeftThigh2.position,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        public static bool RightThigh1(Player player,bool autowall)
        {
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                     player.PlayerBones.RightThigh1.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.RightThigh1.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.RightThigh1.position,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        public static bool RightThigh2(Player player,bool autowall)
        {
           

            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                     player.PlayerBones.RightThigh2.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.RightThigh2.position,
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.RightThigh2.position,
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }

        public static bool Head(Player player, bool autowall)
        {
            // passing through so i can just decide not to pass through for some form of none awall vischecks while having it on in the config
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                    player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0),
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
               // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0),
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject) 
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.Penetrate(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0),
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        public static bool AutoShootHead(Player player, bool autowall)
        {
            // passing through so i can just decide not to pass through for some form of none awall vischecks while having it on in the config
            if (autowall)
            {
                // initial raycast incase they are normally visible and to set the raycast
                if (Physics.Linecast(
                    Camera.main.transform.position,
                    player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0),
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                    return true;
                // 4 passes, let the user change the amount for performance
                for (int i = 0; i < Globals.Config.Aimbot.AutoWallPasses; i++)
                {
                    if (!Hooks.CreateShot.PenetrateAutoShoot(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // one up here for the first pass to prevent it pushing into walls and shit with the +1 on the z axis we give it. 
                    if (Physics.Linecast(
                     RaycastHit.transform.position + new Vector3(0, 0, 1f),
                    player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0),
                    out RaycastHit,
                    Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject)
                        return true; // hit player
                    if (RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>() == null)
                        return false;
                    if (!Hooks.CreateShot.PenetrateAutoShoot(RaycastHit.collider.GetComponent<EFT.Ballistics.BallisticCollider>()))
                        return false; // hit bad object
                }
                return false;
            }
            else
            {
                return (Physics.Linecast(
                        Camera.main.transform.position,
                        player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0),
                        out RaycastHit,
                        Mask) && RaycastHit.collider && RaycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject);
            }
        }
        #endregion
    }
}
