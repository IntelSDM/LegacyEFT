using Comfort.Common;
using EFT;
using Hag.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using EFT.InventoryLogic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Hag.Esp
{
    class Updating : MonoBehaviour
    {
        private static List<BasePlayer> SortClosestToCrosshair(List<BasePlayer> p)
        {
            return (from tempPlayer in p
                    orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), Camera.main.WorldToScreenPoint(tempPlayer.Player.Transform.position))
                    select tempPlayer).ToList<BasePlayer>();
        }
        public static float ConvertToRadians(float angle)
        {
            return (float)((Math.PI / 180f) * angle);
        }

        private float NextShot = 0;
        void ApplyShader(Player player)
        {
            bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == player.Profile.Info.GroupId
                       && player.Profile.Info.GroupId != "0"
                       && player.Profile.Info.GroupId != ""
                       && player.Profile.Info.GroupId != null;
            bool Scav = player.Profile.Info.RegistrationDate <= 0;
            bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
            bool Player = (player.Profile.Side == EPlayerSide.Usec || player.Profile.Side == EPlayerSide.Bear) && !ScavPlayer && !Scav;

            if (!((player && Globals.Config.Player.Chams) || (Scav && Globals.Config.Scav.Chams) || (ScavPlayer && Globals.Config.ScavPlayer.Chams)))
                return;
            if (!((player && Globals.Config.Player.ChamGear) || (Scav && Globals.Config.Scav.ChamGear) || (ScavPlayer && Globals.Config.ScavPlayer.ChamGear)))
            {
                #region No Gear
                var skinDictionary = player?.PlayerBody?.BodySkins;
                if (skinDictionary != null)
                {
                    foreach (var renderer in player.GetComponentsInChildren<UnityEngine.Renderer>())
                    {

                        if (ScavPlayer)
                        {
                            switch (Globals.Config.ScavPlayer.ChamType)
                            {
                                case 0:
                                    if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                        continue;

                                    renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                    if (Globals.Config.ScavPlayer.ChamRGB)
                                    {
                                        renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    }
                                    else
                                    {
                                        renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Chams Visible Colour"));
                                        renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Chams Invisible Colour"));
                                    }
                                    break;
                                case 1:
                                    if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                        continue;
                                    renderer.material.shader = ShaderHelper.TransparentShader;
                                    if (Globals.Config.ScavPlayer.ChamTopmost)
                                    {
                                        renderer.material.SetInt("_Cull", 0);
                                        renderer.material.SetInt("_ZWrite", 0);
                                        renderer.material.SetInt("_ZTest", 8);
                                    }
                                    if (Globals.Config.ScavPlayer.ChamRGB)
                                    {
                                        renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    }
                                    else
                                    {
                                        renderer.material.SetColor("_Color", ColourHelper.GetColour("Chams Transparent Colour"));
                                    }
                                    break;
                                case 2:
                                    renderer.material = ShaderHelper.GalaxyMat;
                                    if (Globals.Config.ScavPlayer.ChamTopmost)
                                    {
                                        renderer.material.SetInt("_Cull", 0);
                                        renderer.material.SetInt("_ZWrite", 0);
                                        renderer.material.SetInt("_ZTest", 8);
                                    }
                                    break;
                            }
                        }
                        if (Scav)
                        {
                            switch (Globals.Config.Scav.ChamType)
                            {
                                case 0:
                                    if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                        continue;

                                    renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                    if (Globals.Config.Scav.ChamRGB)
                                    {
                                        renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    }
                                    else
                                    {
                                        renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Chams Visible Colour"));
                                        renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Chams Invisible Colour"));
                                    }
                                    break;
                                case 1:
                                    if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                        continue;
                                    renderer.material.shader = ShaderHelper.TransparentShader;
                                    if (Globals.Config.Scav.ChamTopmost)
                                    {
                                        renderer.material.SetInt("_Cull", 0);
                                        renderer.material.SetInt("_ZWrite", 0);
                                        renderer.material.SetInt("_ZTest", 8);
                                    }
                                    if (Globals.Config.Scav.ChamRGB)
                                    {
                                        renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    }
                                    else
                                    {
                                        renderer.material.SetColor("_Color", ColourHelper.GetColour("Chams Transparent Colour"));
                                    }
                                    break;
                                case 2:
                                    renderer.material = ShaderHelper.GalaxyMat;
                                    if (Globals.Config.Scav.ChamTopmost)
                                    {
                                        renderer.material.SetInt("_Cull", 0);
                                        renderer.material.SetInt("_ZWrite", 0);
                                        renderer.material.SetInt("_ZTest", 8);
                                    }
                                    break;
                            }
                        }
                        if (Player)
                        {
                            switch (Globals.Config.Player.ChamType)
                            {
                                case 0:
                                    if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                        continue;

                                    renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                    if (Globals.Config.Player.ChamRGB)
                                    {
                                        renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    }
                                    else
                                    {
                                        renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Chams Visible Colour"));
                                        renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Chams Invisible Colour"));
                                    }
                                    break;
                                case 1:
                                    if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                        continue;
                                    if (Globals.Config.Player.ChamTopmost)
                                    {
                                        renderer.material.SetInt("_Cull", 0);
                                        renderer.material.SetInt("_ZWrite", 0);
                                        renderer.material.SetInt("_ZTest", 8);
                                    }
                                    renderer.material.shader = ShaderHelper.TransparentShader;
                                    if (Globals.Config.Player.ChamRGB)
                                    {
                                        renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    }
                                    else
                                    {
                                        renderer.material.SetColor("_Color", ColourHelper.GetColour("Chams Transparent Colour"));
                                    }
                                    break;
                                case 2:
                                    if (Globals.Config.Player.ChamTopmost)
                                    {
                                        renderer.material.SetInt("_Cull", 0);
                                        renderer.material.SetInt("_ZWrite", 0);
                                        renderer.material.SetInt("_ZTest", 8);
                                    }
                                    renderer.material = ShaderHelper.GalaxyMat;
                                    break;
                            }
                        }



                    }
                }
                #endregion
            }
            else
            {
                #region Gear
                var skinDictionary = player?.PlayerBody?.BodySkins;
                if (skinDictionary != null)
                {
                    foreach (var skin in skinDictionary.Values)
                    {
                        if (skin == null)
                            continue;

                        foreach (var renderer in skin.GetRenderers())
                        {

                            if (ScavPlayer)
                            {
                                switch (Globals.Config.ScavPlayer.ChamType)
                                {
                                    case 0:
                                        if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                            continue;

                                        renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                        if (Globals.Config.ScavPlayer.ChamRGB)
                                        {
                                            renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                            renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        }
                                        else
                                        {
                                            renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Chams Visible Colour"));
                                            renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Chams Invisible Colour"));
                                        }
                                        break;
                                    case 1:
                                        if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                            continue;
                                        renderer.material.shader = ShaderHelper.TransparentShader;
                                        if (Globals.Config.ScavPlayer.ChamTopmost)
                                        {
                                            renderer.material.SetInt("_SrcBlend", 5);
                                            renderer.material.SetInt("_DstBlend", 10);
                                            renderer.material.SetInt("_Cull", 0);
                                            renderer.material.SetInt("_ZWrite", 0);
                                            renderer.material.SetInt("_ZTest", 8);
                                        }
                                        if (Globals.Config.ScavPlayer.ChamRGB)
                                        {
                                            renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        }
                                        else
                                        {
                                            renderer.material.SetColor("_Color", ColourHelper.GetColour("Chams Transparent Colour"));
                                        }
                                        break;
                                    case 2:
                                        renderer.material = ShaderHelper.GalaxyMat;
                                        if (Globals.Config.ScavPlayer.ChamTopmost)
                                        {
                                            renderer.material.SetInt("_SrcBlend", 5);
                                            renderer.material.SetInt("_DstBlend", 10);
                                            renderer.material.SetInt("_Cull", 0);
                                            renderer.material.SetInt("_ZWrite", 0);
                                            renderer.material.SetInt("_ZTest", 8);
                                        }
                                        break;
                                }
                            }
                            if (Scav)
                            {
                                switch (Globals.Config.Scav.ChamType)
                                {
                                    case 0:
                                        if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                            continue;

                                        renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                        if (Globals.Config.Scav.ChamRGB)
                                        {
                                            renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                            renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        }
                                        else
                                        {
                                            renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Chams Visible Colour"));
                                            renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Chams Invisible Colour"));
                                        }
                                        break;
                                    case 1:
                                        if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                            continue;
                                        renderer.material.shader = ShaderHelper.TransparentShader;
                                        if (Globals.Config.Scav.ChamTopmost)
                                        {
                                            renderer.material.SetInt("_SrcBlend", 5);
                                            renderer.material.SetInt("_DstBlend", 10);
                                            renderer.material.SetInt("_Cull", 0);
                                            renderer.material.SetInt("_ZWrite", 0);
                                            renderer.material.SetInt("_ZTest", 8);
                                        }
                                        if (Globals.Config.Scav.ChamRGB)
                                        {
                                            renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        }
                                        else
                                        {
                                            renderer.material.SetColor("_Color", ColourHelper.GetColour("Chams Transparent Colour"));
                                        }
                                        break;
                                    case 2:
                                        renderer.material = ShaderHelper.GalaxyMat;
                                        if (Globals.Config.Scav.ChamTopmost)
                                        {
                                            renderer.material.SetInt("_SrcBlend", 5);
                                            renderer.material.SetInt("_DstBlend", 10);
                                            renderer.material.SetInt("_Cull", 0);
                                            renderer.material.SetInt("_ZWrite", 1);
                                            renderer.material.SetInt("_ZTest", 80);
                                        }
                                        break;
                                }
                            }
                            if (Player)
                            {
                                switch (Globals.Config.Player.ChamType)
                                {
                                    case 0:
                                        if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                            continue;

                                        renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                        if (Globals.Config.Player.ChamRGB)
                                        {
                                            renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                            renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        }
                                        else
                                        {
                                            renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Chams Visible Colour"));
                                            renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Chams Invisible Colour"));
                                        }
                                        break;
                                    case 1:
                                        if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                            continue;
                                        if (Globals.Config.Player.ChamTopmost)
                                        {
                                            renderer.material.SetInt("_SrcBlend", 5);
                                            renderer.material.SetInt("_DstBlend", 10);
                                            renderer.material.SetInt("_Cull", 0);
                                            renderer.material.SetInt("_ZWrite", 0);
                                            renderer.material.SetInt("_ZTest", 8);
                                        }
                                        renderer.material.shader = ShaderHelper.TransparentShader;
                                        if (Globals.Config.Player.ChamRGB)
                                        {
                                            renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                        }
                                        else
                                        {
                                            renderer.material.SetColor("_Color", ColourHelper.GetColour("Chams Transparent Colour"));
                                        }
                                        break;
                                    case 2:
                                        if (Globals.Config.Player.ChamTopmost)
                                        {
                                            renderer.material.SetInt("_SrcBlend", 5);
                                            renderer.material.SetInt("_DstBlend", 10);
                                            renderer.material.SetInt("_Cull", 0);
                                            renderer.material.SetInt("_ZWrite", 0);
                                            renderer.material.SetInt("_ZTest", 8);
                                        }
                                        renderer.material = ShaderHelper.GalaxyMat;
                                        break;
                                }
                            }
                        }
                    }
                }
                #endregion
            }


        }
        public struct KnifeHit
        {
            public KnifeHit(RaycastHit hit)
			{
                this.collider = hit.collider;
                this.point = hit.point;
                this.normal = hit.normal;
            }
            public Collider collider;
            public Vector3 point;
            public Vector3 normal;
        };
        void SilentMelee()
        {
            if (Globals.GameWorld == null)
                return;
            foreach (BasePlayer baseplayer in SortClosestToCrosshair(Globals.PlayerList))
            {
                EFT.Player player = baseplayer.Player;

                bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == player.Profile.Info.GroupId
                          && player.Profile.Info.GroupId != "0"
                          && player.Profile.Info.GroupId != ""
                          && player.Profile.Info.GroupId != null;
                bool Scav = player.Profile.Info.RegistrationDate <= 0;
                bool ScavPlayer = player.Profile.Side == EPlayerSide.Savage && !Scav && !player.IsAI;
                bool Player = (player.Profile.Side == EPlayerSide.Usec || player.Profile.Side == EPlayerSide.Bear) && !ScavPlayer && !Scav;

                if (friendly || player == null)
                    continue;
                try
                {
                    BodyPartCollider[] hitcolliders = player.GetComponentsInChildren<BodyPartCollider>();
                    BodyPartCollider bodypartcollider = hitcolliders.First();
                    Collider collider = bodypartcollider.Collider;
                    Vector3 colliderpos = collider.gameObject.transform.position;
                    KnifeHit hit;
                    hit.collider = collider;
                    hit.point = colliderpos;
                    hit.normal = Vector3.zero;

                    try
                    {
                        Globals.LocalPlayer.HandsController.CallPrivateMethod("", null);
                    }
                    catch (Exception ex) { File.WriteAllText("dsgsg.txt", ex.ToString()); }
                }
                catch (Exception ex)
                {
                    File.WriteAllText("dsgsg1.txt", ex.ToString());
                }
                //  Globals.LocalPlayer.HandsController.CallPrivateMethod("",);
                if (Globals.Config.ScavAimbot.SilentMelee && Scav)
                { 
                
                }
                if (Globals.Config.ScavPlayerAimbot.SilentMelee && ScavPlayer)
                { }
                if (Globals.Config.PlayerAimbot.SilentMelee && Player)
                { }

            }
        }
        void ApplyHandChams()
        {
           

            if (!Globals.LocalPlayer.HealthController.IsAlive)
                return;
            
            //   Globals.LocalPlayer.MovementContext.CurrentState.AuthoritySpeed = 1000;
            //   Globals.LocalPlayer.MovementContext.CurrentState.ChangeSpeed(1000);
            //    Globals.LocalPlayer.MovementContext.CurrentState.Prone();
            //     Globals.LocalPlayer.MovementContext.CheckGroundedRayDistance = 100;
            //   Globals.LocalPlayer.MovementContext.IsInPronePose = true;
          //  Globals.LocalPlayer.MovementContext.SetPoseLevel(0, true);
            
            //   Globals.LocalPlayer.MovementContext.CurrentState.Jump();
            // Globals.LocalPlayer.MovementContext.PoseLevel = 0;
            // Globals.LocalPlayer.MovementContext.States.Values
            // Time.timeScale = 10;
            if (Globals.Config.Visuals.HandChamRGB)
            {
                switch (Helpers.ColourHelper.Cases)
                {
                    // Adds And Sets Values To Cycle Through RGB Colours 
                    case 0: { ColourHelper.R -= 0.0150f; if (ColourHelper.R <= 0) ColourHelper.Cases += 1; break; }
                    case 1: { ColourHelper.G += 0.0150f; ColourHelper.B -= 0.0150f; if (ColourHelper.G >= 1) ColourHelper.Cases += 1; break; }
                    case 2: { ColourHelper.R += 0.0150f; if (ColourHelper.R >= 1) ColourHelper.Cases += 1; break; }
                    case 3: { ColourHelper.B += 0.0150f; ColourHelper.G -= 0.0150f; if (ColourHelper.B >= 1) ColourHelper.Cases = 0; break; }
                    default: { ColourHelper.R = 1.00f; ColourHelper.G = 0.00f; ColourHelper.B = 1.00f; break; }
                }
            }

            if (Globals.Config.Visuals.HandChamOnlyArms)
            {
                var skinDictionary = Globals.LocalPlayer?.PlayerBody?.BodySkins;
                if (skinDictionary == null)
                    return;
                // Loop Your Player's Skin Dictionary 
                foreach (var skin in skinDictionary.Values)
                {
                    if (skin == null)
                        continue;
                  // Loop The Renderers In The Skins
                    foreach (var renderer in skin.GetRenderers())
                    {
                        if (renderer?.material?.shader == null)
                            continue;

                        switch (Globals.Config.Visuals.HandChamType)
                        {
                            /*
                            0 = Chams Shader
                            1 = Transparent Shader
                            2 = Galaxy Material
                            */
                            case 0:
                                if (renderer?.material?.shader == ShaderHelper.Shaders["Chams"])
                                    continue;
                                renderer.material.shader = ShaderHelper.Shaders["Chams"];
                                if (Globals.Config.Visuals.HandChamRGB)
                                {
                                    renderer.material.SetColor("_ColorVisible", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                    renderer.material.SetColor("_ColorBehind", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                }
                                else
                                {
                                    renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Hand Chams Colour"));
                                    renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Hand Chams Colour"));
                                }
                                break;
                            case 1:
                                if (renderer?.material?.shader == ShaderHelper.TransparentShader)
                                    continue;
                                renderer.material.shader = ShaderHelper.TransparentShader;
                                if (Globals.Config.Visuals.HandChamRGB)
                                {
                                    renderer.material.SetColor("_Color", new Color(ColourHelper.R, ColourHelper.G, ColourHelper.B, 1));
                                }
                                else
                                {
                                    renderer.material.SetColor("_Color", ColourHelper.GetColour("Hand Chams Colour"));
                                }
                                break;
                            case 2:
                                renderer.material = ShaderHelper.GalaxyMat;
                                break;
                        }
                    }
                }

            }
            else
            {
                // Loop The Entire Hand's Renderers
                foreach (var renderer in Globals.LocalPlayer.HandsController.GetComponentsInChildren<UnityEngine.Renderer>())
                {
                    if (renderer == null)
                        continue;
                    if (renderer.material == null)
                        continue;

                    switch (Globals.Config.Visuals.HandChamType)
                    {       
                            /*
                            0 = Chams Shader
                            1 = Transparent Shader
                            2 = Galaxy Material
                            */
                        case 0:
                            renderer.material.shader = ShaderHelper.Shaders["Chams"];
                            if (Globals.Config.Visuals.HandChamRGB)
                            {

                            }
                            else
                            {
                                renderer.material.SetColor("_ColorVisible", ColourHelper.GetColour("Hand Chams Colour"));
                                renderer.material.SetColor("_ColorBehind", ColourHelper.GetColour("Hand Chams Colour"));
                            }
                            break;
                        case 1:
                            renderer.material.shader = ShaderHelper.TransparentShader;
                            if (Globals.Config.Visuals.HandChamRGB)
                            {

                            }
                            else
                            {
                                renderer.material.SetColor("_Color", ColourHelper.GetColour("Hand Chams Colour"));
                            }
                            break;
                        case 2:
                            renderer.material = ShaderHelper.GalaxyMat;

                            break;
                    }

                }
            }

        }

        public static Player InformationPlayer;
        void GetPlayerInformation()
        {
            // Define 2 Lists For Closest Players To Crosshair And Local Player
            List<Player> SortClosestToCrosshair(List<Player> p)
            {
                return (from tempPlayer in p
                        orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), Camera.main.WorldToScreenPoint(tempPlayer.Transform.position))
                        select tempPlayer).ToList<Player>();
            }

            List<Player> SortClosestToLocalPlayer(List<Player> p)
            {
                return (from tempPlayer in p
                        orderby Vector3.Distance(Globals.MainCamera.transform.position, tempPlayer.Transform.position)
                        select tempPlayer).ToList<Player>();
            }
            //Loop Our Defined Lists And Check If The Player Reaches Our Requirements
            Player player = new Player();
            if (Globals.Config.PlayerInformation.ClosestToCrosshair)
            {
                foreach (Player players in SortClosestToCrosshair(Globals.GameWorld.RegisteredPlayers))
                {
                    if (players == Globals.LocalPlayer)
                        continue;
                    if (!players.HealthController.IsAlive)
                        continue;
                    bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == players.Profile.Info.GroupId
                      && players.Profile.Info.GroupId != "0"
                      && players.Profile.Info.GroupId != ""
                      && players.Profile.Info.GroupId != null;
                    if (Globals.Config.PlayerInformation.IgnoreTeam && friendly)
                        continue;
                    player = players; // returns the first player that meets requirements
                    break;
                }
            }
            else
            {
                foreach (Player players in SortClosestToLocalPlayer(Globals.GameWorld.RegisteredPlayers))
                {
                    if (players == Globals.LocalPlayer)
                        continue;
                    if (!players.HealthController.IsAlive)
                        continue;
                    bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == players.Profile.Info.GroupId
                      && players.Profile.Info.GroupId != "0"
                      && players.Profile.Info.GroupId != ""
                      && players.Profile.Info.GroupId != null;
                    if (!Globals.Config.PlayerInformation.IgnoreTeam && friendly)
                        continue;
                    player = players; // returns the first player that meets requirements
                    break;
                }
            }
            InformationPlayer = player; // Set The Player For Use In Drawing.cs


        }
     
        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {

            //  This is an attempt to get the scope matrix, Doesn't work but i am keeping it here for future use.
            /*    if (Globals.LocalPlayer.ProceduralWeaponAnimation.Breath.IsAiming)
                {

                    try
                    {
                        Camera cam = new Camera();
                        Camera fpscam = new Camera();
                        foreach (Camera camera in Camera.allCameras)
                        {
                            if (camera.name == "BaseOpticCamera(Clone)")
                                cam = camera;

                            if (camera.name == "FPS Camera")
                                fpscam = camera;
                        }
                        if (cam == null)
                        {
                            Vector3 vector = Globals.MainCamera.WorldToScreenPoint(worldPoint);
                            vector.y = (float)Screen.height - vector.y;
                            return vector;
                        }

                      /*   Matrix4x4 mat = cam.projectionMatrix;
                         Matrix4x4 matTransposed = Matrix4x4.Transpose(mat);
                         Vector3 result = cam.WorldToViewportPoint(worldPoint);
                         Vector3 ScreenPos = Vector3.zero;
                         float angle_rad_half = (float)ConvertToRadians(fpscam.fieldOfView) * 0.5f;
                         float angle_ctg = (float)(Math.Cos((double)angle_rad_half) / Math.Sin((double)angle_rad_half));
                         result.x /= angle_ctg * (fpscam.aspect * 0.5f);
                         result.y /= angle_ctg * 0.5f;
                         ScreenPos.x = (1.0f + result.x) * (Screen.width * 0.5f);
                         ScreenPos.y = (1.0f - result.y) * (Screen.height * 0.5f);
                         ScreenPos.z = result.z;
                         return ScreenPos;*/

            /*      Matrix4x4 mat = Globals.MainCamera.projectionMatrix;
                   Matrix4x4 temp = Matrix4x4.Transpose(mat);

                   Vector3 translation = new Vector3(temp.m30, temp.m31, temp.m32);
                   Vector3 up = new Vector3(temp.m20, temp.m21, temp.m22);
                   Vector3 right = new Vector3(temp.m10, temp.m11, temp.m12);
                   float z = Vector3.Dot(worldPoint,translation) + temp.m33;
               //    File.WriteAllText("test.txt", z.ToString());
               //    if (z < 0.0f)
              //        return Vector3.zero;
                   float x = (Vector3.Dot(worldPoint, right) + temp.m03) / z;
                   float y = (Vector3.Dot(worldPoint,up) + temp.m13) / z;

                   float angle_rad_half = (cam.fieldOfView * (float)(Math.PI / 180)) * 0.5f;
                   float angle_ctg = (float)(Math.Cos(angle_rad_half) / Math.Sin(angle_rad_half));

                   x /= (angle_ctg * (cam.aspect * 0.5f));
                   y /= (angle_ctg * 0.5f);
                   Vector3 Return = new Vector3((Screen.width * 0.5f) * (1.0f + x), (Screen.height * 0.5f) * (1.0f - y), z);
                   return Return;

               }
               catch
               {

               }

           }
   */
            
            Camera optic = new Camera();
            Camera fpscam = new Camera();
            foreach (Camera camera in Camera.allCameras)
            {
                if (camera.name == "BaseOpticCamera(Clone)")
                    optic = camera;

                if (camera.name == "FPS Camera")
                    fpscam = camera;
            }
         //   optic.Reset();
            if (optic != null)
            {
  /*       Vector3 screenpos = Vector3.zero;
                     Matrix4x4 mat = optic.projectionMatrix;                  // ViewProjection = Camera + 0xD8				
                     Matrix4x4 matTransposed = Matrix4x4.Transpose(mat);
                     Vector3 result = matTransposed.MultiplyPoint(worldPoint);

                     if (result.z >= 1.0f)
                         goto FPSCAM;

                     float angle_rad_half = ConvertToRadians(fpscam.fieldOfView) * 0.5f;     // FOV = Camera +  0x158 (default 50.0 and 35.0 when aiming)
                     float angle_ctg = (float)(Math.Cos(angle_rad_half) / Math.Sin(angle_rad_half));

                     result.x /= angle_ctg * (fpscam.aspect * 0.5f);                // Aspect = Camera +  0x4C8 (1.778 for resoluttion 1600x900 px)
                     result.y /= angle_ctg * 0.5f;

                     screenpos.x = (1.0f + result.x) * (Screen.width * 0.5f);
                     screenpos.y = (1.0f - result.y) * (Screen.height * 0.5f);

                     return screenpos;
  */
                /*   Matrix4x4 mat = fpscam.projectionMatrix;
                   Matrix4x4 temp = Matrix4x4.Transpose(mat);

                   Vector3 translation = new Vector3(temp.m30, temp.m31, temp.m32);
                   Vector3 up = new Vector3(temp.m20, temp.m21, temp.m22);
                   Vector3 right = new Vector3(temp.m10, temp.m11, temp.m12);
                   float z = Vector3.Dot(worldPoint, translation) + temp.m33;

                   if (z < 0.0f)
                       goto FPSCAM;
                   float x = (Vector3.Dot(worldPoint, right) + temp.m03) / z;
                   float y = (Vector3.Dot(worldPoint, up) + temp.m13) / z;

                   float angle_rad_half = (optic.fieldOfView * (float)(Math.PI / 180)) * 0.5f;
                   float angle_ctg = (float)(Math.Cos(angle_rad_half) / Math.Sin(angle_rad_half));

                   x /= (angle_ctg * (optic.aspect * 0.5f));
                   y /= (angle_ctg * 0.5f);

                   Vector3 Return = new Vector3((Screen.width * 0.5f) * (1.0f + x), (Screen.height * 0.5f) * (1.0f - y), z);
                   return Return;*/
                Vector3 screen = fpscam.WorldToScreenPoint(worldPoint);
                screen.y = (float)Screen.height - screen.y;
                return screen;
            }
            else
            {
                
                Vector3 vector2 = fpscam.WorldToScreenPoint(worldPoint);
                vector2.y = (float)Screen.height - vector2.y;
                return vector2;
            }
            FPSCAM:
            Vector3 vector3 = fpscam.WorldToScreenPoint(worldPoint);
            vector3.y = (float)Screen.height - vector3.y;
            return vector3;
        }
        void Start()
        {
            // these are substitutes for update calls as update calls on eft are slow for some odd reason
            StartCoroutine(World());
            StartCoroutine(LocalPlayer());
            StartCoroutine(Player());
            StartCoroutine(Corpse());
            StartCoroutine(Container());
            StartCoroutine(Exfil());
            StartCoroutine(Grenade());
            StartCoroutine(Item());

        }
        IEnumerator Grenade()
        {
            for (; ; )
            {
                foreach (BaseGrenade bg in Globals.GrenadeList)
                {
                    if (bg.Grenade == null)
                        continue;
                    try
                    {
                        Vector3 W2S;

                        W2S = WorldPointToScreenPoint(bg.Grenade.transform.position);
                        float fov = Globals.MainCamera.fieldOfView;

                        bg.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, bg.Grenade.transform.position);
                    }
                    catch { }

                }

                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator Exfil()
        {
            for (; ; )
            {
                foreach (BaseExfil be in Globals.ExfilList)
                {
                    if (be.Exfil == null)
                        continue;
                    try
                    {

                        be.W2S = WorldPointToScreenPoint(be.Exfil.transform.position);
                        be.Colour = ColourHelper.GetColour("Exfil Colour");
                        be.Name = be.Exfil.Settings.Name;
                        be.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, be.Exfil.transform.position);
                    }
                    catch { }

                }

                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator Container()
        {
            for (; ; )
            {
                if (UnityEngine.Input.GetKeyDown(Globals.Config.Container.ContentsKey))
                    Globals.Config.Container.DrawContents = !Globals.Config.Container.DrawContents;
                foreach (BaseContainer bc in Globals.ContainerList)
                {
                    if (bc.Container == null)
                        continue;
                    try
                    {

                        bc.Colour = ColourHelper.GetColour("Container Colour");

                        //    bc.Value = (int)bc.CorpseValue(out bc.Items);
                        bc.W2S = WorldPointToScreenPoint(bc.Container.transform.position);

                        bc.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, bc.Container.transform.position);


                    }
                    catch { }

                }

                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator Corpse()
        {
            for (; ; )
            {
                if (Input.GetKeyDown(Globals.Config.Corpse.ContentsKey))
                    Globals.Config.Corpse.DrawCorpseContents = !Globals.Config.Corpse.DrawCorpseContents;
                foreach (BaseCorpse bc in Globals.CorpseList)
                {
                    if (bc.Corpse == null)
                        continue;
                    try
                    {
                        bc.Colour = ColourHelper.GetColour("Corpse Colour");

                        //      bc.Value = (int)bc.CorpseValue(out bc.Items);
                        bc.W2S = WorldPointToScreenPoint(bc.Corpse.transform.position);

                        bc.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, bc.Corpse.transform.position);
                    }
                    catch { }

                }

                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator Item()
        {
            for (; ; )
            {
                foreach (BaseItem bi in Globals.LootList)
                {
                    if (bi.Item == null)
                        continue;
                    try
                    {
                        bi.Item.Item.Template.ExamineTime = 0f;
                        bi.W2S = WorldPointToScreenPoint(bi.Item.transform.position);

                        bi.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, bi.Item.transform.position);
                        bi.Colour = ColourHelper.GetColour("Item Colour");
                        if (Globals.Config.Item.EnableInaccurateEsp)
                        {
                            if (bi.Item.Item.Template.Rarity == JsonType.ELootRarity.Rare)
                                bi.Colour = ColourHelper.GetColour("Rare Item Colour");
                            if (bi.Item.Item.Template.Rarity == JsonType.ELootRarity.Superrare)
                            {
                                bi.Colour = ColourHelper.GetColour("Super Rare Item Colour");
                                if (Globals.Config.Item.SuperRareBypassMinValue)
                                    bi.ValueCheckIgnored = true;
                            }
                            if (bi.Whitelisted)
                                bi.Colour = ColourHelper.GetColour("Whitelisted Item Colour");
                            if (bi.Item.Item.QuestItem)
                            {
                                bi.Colour = ColourHelper.GetColour("Quest Item Colour");
                                bi.ValueCheckIgnored = true;
                            }
                        }
                        OurItem itm = OurItems.list[bi.Item.Item.Template._id];
                        if (itm.subtype == item_subtype.Keycard)
                        {
                            bi.Colour = ColourHelper.GetColour("Keycard Item Colour");
                        }
                        if (Globals.Config.Item.EnableAccurateEsp)
                        {
                            // could do colours for shit i guess
                        }
                    }
                    catch { }

                }

                yield return new WaitForEndOfFrame();
            }
        }

        public static void TurnOnAllSwitch()
        {
            foreach (EFT.Interactive.Switch Switch in FindObjectsOfType<EFT.Interactive.Switch>())
            {
                Switch.DoorState = EFT.Interactive.EDoorState.Open;

            }
        }
        void AutoShoot()
        {
            if (Hooks.CreateShot.GetAutoShootTargetPlayer() != null)
            {
                if (NextShot < Time.time)
                {

                    Hooks.CreateShot.Autoshoot = true;
                    Globals.LocalPlayer.GetComponent<Player.FirearmController>().SetTriggerPressed(true);
                    
                    Hooks.CreateShot.Autoshoot = false;
                    NextShot = Time.time + 0.064f;
                    Globals.LocalPlayer.GetComponent<Player.FirearmController>().SetTriggerPressed(false);
                }

            }
        }

     
        IEnumerator LocalPlayer()
        {
            for (; ; )
            {
                try
                {
                   
                    #region AutoShoot
                    AutoShoot();
                    #endregion
                    
                }
                catch { }
                try
                {


                    if (Globals.LocalPlayer != null)
                    {
                        BasePlayer tempbp = new BasePlayer(Globals.LocalPlayer);
                        Globals.LocalPlayerValue = (int)tempbp.GetPlayerValue();
                    }
                    if (Globals.LocalPlayer != null)
                        if (Globals.LocalPlayer?.HandsController?.Item is Weapon)
                            Globals.LocalPlayerWeapon = (Weapon)Globals.LocalPlayer?.HandsController?.Item;
            
                    
                }
                catch { }
                #region Weapons
                try
                {
                    try
                    {
                      
                        if (Globals.Config.Aimbot.AutoWall) // a crash might be coming from here
                        {
                            EFT.Ballistics.BallisticCollider collider = Helpers.RaycastHelper.BarrelRaycastCollider();
                            try
                            {// red = shoot through and blue is no shoot
                                if (Hooks.CreateShot.Penetrate(collider))
                                    Drawing.CrosshairColor = new Renderer.Direct2DColor(255, 0, 0, 255);
                                else Drawing.CrosshairColor = new Renderer.Direct2DColor(111, 0, 255, 255);
                            }
                            catch
                            {
                                Drawing.CrosshairColor = new Renderer.Direct2DColor(111, 0, 255, 255);
                            }
                        }
                        else 
                        {
                            Drawing.CrosshairColor = new Renderer.Direct2DColor(111, 0, 255, 255);
                        }
                    }
                    catch (Exception ex){  }
                  
                    if (Globals.Config.Weapon.FullAuto)
                    {
                        //          EPlayerState[] states = new EPlayerState[0];
                        //        EFTHardSettings.Instance.UnsuitableStates = states; // external run and shoot
                      
                        EFT.InventoryLogic.Weapon.EFireMode[] firemodes = new EFT.InventoryLogic.Weapon.EFireMode[3]
                            {
                            EFT.InventoryLogic.Weapon.EFireMode.single, 
                            EFT.InventoryLogic.Weapon.EFireMode.burst,
                            EFT.InventoryLogic.Weapon.EFireMode.fullauto
                            };
                        Globals.LocalPlayerWeapon.Template.BoltAction = false;
                        Globals.LocalPlayerWeapon.Template.weapFireType = firemodes;
                       
                    }
                    if (Globals.Config.Weapon.FastFire)
                    {
                        Globals.LocalPlayerWeapon.Template.bFirerate = Globals.Config.Weapon.FastFireRate;
                    }
                    if (Globals.Config.Weapon.NoRecoil)
                    {
                        Globals.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = Globals.Config.Weapon.NoRecoilAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = Globals.Config.Weapon.NoRecoilAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.Pitch = Globals.Config.Weapon.NoRecoilAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.SwayFalloff = Globals.Config.Weapon.NoRecoilAmount;
                    }
                    if (Globals.Config.Weapon.NoSway)
                    {
                        Globals.LocalPlayer.ProceduralWeaponAnimation.Breath.Intensity = Globals.Config.Weapon.NoSwayAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = false;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.Walk.Intensity = Globals.Config.Weapon.NoSwayAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = Globals.Config.Weapon.NoSwayAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.MotionReact.Velocity = new Vector3(Globals.Config.Weapon.NoSwayAmount, Globals.Config.Weapon.NoSwayAmount, Globals.Config.Weapon.NoSwayAmount);
                        Globals.LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = Globals.Config.Weapon.NoSwayAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.Breath.Overweight = Globals.Config.Weapon.NoSwayAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.AimingDisplacementStr = Globals.Config.Weapon.NoSwayAmount;
                        Globals.LocalPlayer.ProceduralWeaponAnimation.Breath.HipPenalty = Globals.Config.Weapon.NoSwayAmount;
                    }
                    if (Globals.Config.Weapon.NoMalfunction)
                    {
                        Globals.LocalPlayerWeapon.CurrentAmmoTemplate.MalfFeedChance = Globals.Config.Weapon.NoMalfunctionChance;
                        Globals.LocalPlayerWeapon.CurrentAmmoTemplate.MalfMisfireChance = Globals.Config.Weapon.NoMalfunctionChance;
                       Globals.LocalPlayerWeapon.Template.BaseMalfunctionChance = Globals.Config.Weapon.NoMalfunctionChance;
                     
                    }
                    
                }
                catch { }
                #endregion
                #region Movement
                try
                {
           // \ue805  has some good shit for body and health
             //       Globals.LocalPlayer.BlindnessDuration.
            
                    if (Globals.Config.Movement.FlyHack)
                        Globals.LocalPlayer.MovementContext.FreefallTime = Globals.FlyHackValue;
                   
                    EFTHardSettings.Instance.DIRECTION_LERP_SPEED = float.MaxValue;
                    if (Globals.Config.Movement.InstantCrouch)
                    {
                        EFTHardSettings.Instance.POSE_CHANGING_SPEED = 100;
                        EFTHardSettings.Instance.PRONE_ALIGN_SPEED = 100;
                    }
                    if (Globals.Config.Movement.MedAndRun)
                    {
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.UsingMeds, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.Tremor, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.SprintDisabled, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.JumpDisabled, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.LeftArmDamaged, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.LeftLegDamaged, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.RightArmDamaged, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.RightLegDamaged, false);
                        Globals.LocalPlayer.MovementContext.SetPhysicalCondition(EPhysicalCondition.HealingLegs, false);
                    }
                    if (Globals.Config.Movement.FakeProne)
                        Globals.LocalPlayer.MovementContext.CurrentState.Name = EPlayerState.ProneIdle;
                    // Globals.LocalPlayer.MovementContext.ChangeSpeedLimit

                    //       EFTHardSettings.Instance.HIT_FORCE = 100000; // corpses go flying
                    //  EFTHardSettings.Instance.

                    if (Input.GetKeyDown(Globals.Config.Movement.FlyHackKey))
                    {
                        Globals.Config.Movement.FlyHack = !Globals.Config.Movement.FlyHack;
                        if (Globals.Config.Movement.FlyHack)
                            Globals.LocalPlayer.MovementContext.IsGrounded = false;
                        else
                            Globals.LocalPlayer.MovementContext.IsGrounded = true;
                    }
                    if (Input.GetKeyDown(Globals.Config.Movement.FlyDownWardsKey))
                    {
                        if (Globals.FlyHackValue >= 0)
                            Globals.FlyHackValue = Globals.FlyHackValue + 0.2f;
                        else
                            Globals.FlyHackValue = 0;
                    }
                    if (Input.GetKeyDown(Globals.Config.Movement.FlyUpwardsKey))
                    {
                        if (Globals.FlyHackValue <= 0)
                            Globals.FlyHackValue = Globals.FlyHackValue - 0.2f;
                        else
                            Globals.FlyHackValue = 0;
                    }
                    if (Globals.Config.Movement.UnlimitedStamina)
                    {
                        Globals.LocalPlayer.Physical.HandsStamina.Current = 99;
                        Globals.LocalPlayer.Physical.Stamina.Current = 99;
                    }
                    if (Globals.Config.Movement.NoIntertia)
                    {
                        Globals.LocalPlayer.Physical.Inertia = 0;
                //        Globals.LocalPlayer.MovementContext.InertiaSettings.MoveTime = 0;
                        //EFTHardSettings.Instance.MOVEMENT_MASK
                  //      Globals.LocalPlayer.MovementContext.InertiaSettings.
                     Globals.LocalPlayer.MovementContext.InertiaSettings.MinDirectionBlendTime = 0;
                        Globals.LocalPlayer.MovementContext.InertiaSettings.PenaltyPower = 0;
                        Globals.LocalPlayer.MovementContext.InertiaSettings.BaseJumpPenalty = 0;
                        Globals.LocalPlayer.MovementContext.InertiaSettings.DurationPower = 0;
                        Globals.LocalPlayer.MovementContext.InertiaSettings.BaseJumpPenaltyDuration = 0;
                        Globals.LocalPlayer.MovementContext.InertiaSettings.FallThreshold = float.MaxValue;
                    }
                    if (Globals.Config.Movement.HighJump)
                    {
                        Globals.LocalPlayer.Skills.StrengthBuffJumpHeightInc.Value = Globals.Config.Movement.HighJumpAmount;
                       
                    }
                    if (Globals.Config.Movement.NoFall)
                    {
                        Globals.LocalPlayer.Physical.FallDamageMultiplier = 0;
                    }
                    if (Globals.Config.Movement.Bhop)
                    {
                        if (Input.GetKey(KeyCode.Space))
                            Globals.LocalPlayer.MovementContext.TryJump();
                    }
                }
                catch { }
                #endregion
                #region World
                try
                {
                    GetPlayerInformation();
                    if (Globals.Config.LocalPlayerWorld.UnloadLoadMags)
                    {
                        
                        Globals.LocalPlayer.Skills.MagDrillsLoadSpeed.Value = 98;
                        Globals.LocalPlayer.Skills.MagDrillsUnloadSpeed.Value = 90;
                    }
                    if (Globals.Config.LocalPlayerWorld.BetterHearing)
                    {
                        Globals.LocalPlayer.Skills.PerceptionHearing.Value = 100;
                    }
                    if (Globals.Config.LocalPlayerWorld.NoOpenDelay)
                    {
                        EFTHardSettings.Instance.DelayToOpenContainer = 0;
                        EFTHardSettings.Instance.PICKUP_DELAY = 0;
                    }
                    if (Globals.Config.LocalPlayerWorld.InstantReload)
                    {
                       
                        Globals.LocalPlayer.Skills.BotReloadSpeed.Value = 90;
                        Globals.LocalPlayer.Skills.WeaponReloadAction.FactorValue = 1000;
                    }
                    if (Globals.Config.LocalPlayerWorld.FarThrowGrenades)
                    {
                        EFTHardSettings.Instance.GrenadeForce = 25;
                    }
                   if(Globals.Config.LocalPlayerWorld.InstantADS)
                        Globals.LocalPlayer.ProceduralWeaponAnimation.SetPrivateField("", 10f);

                }
                catch { }

                
                #endregion
                #region Visuals
                try
                {   

                  //  SilentMelee();
                    if (Globals.Config.Visuals.ThirdPerson)
                        Globals.LocalPlayer.PointOfView = EPointOfView.ThirdPerson;
                    else
                        Globals.LocalPlayer.PointOfView = EPointOfView.FirstPerson;

                    if (Globals.Config.Visuals.NoVisor)
                    {
                        Globals.MainCamera.GetComponent<VisorEffect>().Intensity = 0f;
                        Globals.MainCamera.GetComponent<VisorEffect>().enabled = true;
                    }
                    if (Globals.Config.Visuals.UnlockViewAngles)
                    {
                        Globals.LocalPlayer.MovementContext.PitchLimit = new Vector2(-90, 90);
                        Globals.LocalPlayer.MovementContext.SetYawLimit(new Vector2(-360, 360));
                    }
                    if (Globals.Config.Visuals.HandChams)
                    {
                        ApplyHandChams();
                    }
                    if (Globals.Config.Visuals.NightVision)
                        Globals.MainCamera.GetComponent<BSG.CameraEffects.NightVision>().SetPrivateField("_on", true);
                    else
                        Globals.MainCamera.GetComponent<BSG.CameraEffects.NightVision>().SetPrivateField("_on", false);

                    if (Globals.Config.Visuals.ThermalVision)
                        Globals.MainCamera.GetComponent<ThermalVision>().On = true;
                    else
                        Globals.MainCamera.GetComponent<ThermalVision>().On = false;
//
                   
                        FullBright_UpdateObject(Globals.Config.Visuals.AlwaysDay);
                        FullBright_SpawnObject();


                }
                catch { }
                #endregion
                //            BasePlayer tempbp = new BasePlayer(Globals.LocalPlayer);
                //          Globals.LocalPlayerValue = (int)tempbp.GetPlayerValue();
                yield return new WaitForEndOfFrame();
            }
        }
         IEnumerator Player()
         {
            for (; ; )
            {

                
                foreach (BasePlayer bp in Globals.PlayerList)
                {
                    if (bp.Player == null || !bp.Player.HealthController.IsAlive)
                        continue;
                    try
                    {
                       bp.Weapon = (Weapon)bp.Player?.HandsController?.Item;
                    }
                    catch { }
                    try
                    {
                        bool friendly = Globals.LocalPlayer.Profile.Info.GroupId == bp.Player.Profile.Info.GroupId
                        && bp.Player.Profile.Info.GroupId != "0"
                        && bp.Player.Profile.Info.GroupId != ""
                        && bp.Player.Profile.Info.GroupId != null;

                        bool Scav = bp.Player.Profile.Info.RegistrationDate <= 0;
                        bool ScavPlayer = bp.Player.Profile.Side == EPlayerSide.Savage && !Scav && !bp.Player.IsAI;
                        bool Player = (bp.Player.Profile.Side == EPlayerSide.Usec || bp.Player.Profile.Side == EPlayerSide.Bear) && !ScavPlayer && !Scav;
                        if (ScavPlayer)
                        {
                            bp.Colour = ColourHelper.GetColour("Scav Player Colour");
                            bp.BoxColour = ColourHelper.GetColour("Scav Player Box Colour");
                        }
                        if (Scav)
                        {
                            bp.Colour = ColourHelper.GetColour("Scav Colour");
                            bp.BoxColour = ColourHelper.GetColour("Scav Box Colour");
                        }
                        if (Player)
                        {
                            bp.Colour = ColourHelper.GetColour("Player Colour");
                            bp.BoxColour = ColourHelper.GetColour("Player Box Colour");
                            if (friendly)
                            {
                                bp.Colour = ColourHelper.GetColour("Team Colour");
                                bp.BoxColour = ColourHelper.GetColour("Team Colour");
                            }
                        }
                        bp.FilledBoxColour = ColourHelper.GetColour("Filled Box Colour");
                        bp.VisibleColour = ColourHelper.GetColour("Visible Colour");
                        bp.InvisibleColour = ColourHelper.GetColour("Invisible Colour");
                        if (bp.Player.Profile.Info.Settings.IsBoss())
                            bp.Colour = ColourHelper.GetColour("Scav Boss Colour");

                        bp.W2S = WorldPointToScreenPoint(bp.Player.Transform.position);
                        bp.HeadW2S = WorldPointToScreenPoint(bp.Player.PlayerBones.Head.position);
                        bp.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, bp.Player.Transform.position);
                            ApplyShader(bp.Player);
                           
                        //          bp.Value = (int)bp.GetPlayerValue( out bp.Items);
                        switch (bp.Player.Profile.Info.MemberCategory)
                        {
                            case EMemberCategory.ChatModerator:
                                bp.Flag = "Mod";
                                break;
                            case EMemberCategory.ChatModeratorWithPermanentBan:
                                bp.Flag = "Mod+";
                                break;
                            case EMemberCategory.Developer:
                                bp.Flag = "Developer";
                                break;
                            case EMemberCategory.Emissary:
                                bp.Flag = "Emissary";
                                break;
                            case EMemberCategory.Sherpa:
                                bp.Flag = "Sherpa";
                                break;
                            default:
                                bp.Flag = "";
                                break;
                        }
                    }
                        catch { }
                    }
                

                yield return new WaitForEndOfFrame();
            }
            

        }
       IEnumerator World()
       {
            for (; ; )
            {
                if(Globals.MainCamera == null)
                    Globals.MainCamera = Camera.main;
                //if(Globals.GameWorld == null)
                  //  Globals.GameWorld = Singleton<GameWorld>.Instance;

                yield return new WaitForSeconds(3f);
            }

       }
        #region fullbright from maoci
        public bool Enabled = false;

        public GameObject lightGameObject;

        public Light FullBrightLight;

        public bool _LightEnabled = true;

        public bool lightCalled;

        private Vector3 tempPosition = Vector3.zero;
        public void FullBright_SpawnObject()
        {
            if (Globals.LocalPlayer != null && !lightCalled && Enabled)
            {
                lightGameObject = new GameObject("Fullbright");
                FullBrightLight = lightGameObject.AddComponent<Light>();
                FullBrightLight.color = new Color(1f, 0.839f, 0.66f, 1f);
               // FullBrightLight.color = new Color(1f, 0, 0, 1f);
                FullBrightLight.range = 2000f;
                FullBrightLight.intensity = 0.6f;
                lightCalled = true;
            }
        }

        public void FullBright_UpdateObject(bool set)
        {
            if (Globals.LocalPlayer != null)
            {
                Enabled = set;
                if (set)
                {
                    if (FullBrightLight == null)
                    {
                        return;
                    }
                    tempPosition = Globals.LocalPlayer.Transform.position;
                    tempPosition.y = tempPosition.y + 0.2f;
                    lightGameObject.transform.position = tempPosition;
                    return;
                }
                else
                {
                    if (FullBrightLight != null)
                    {
                        UnityEngine.Object.Destroy(FullBrightLight);
                    }
                    lightCalled = false;
                }
            }
        }
        #endregion
    }
}
