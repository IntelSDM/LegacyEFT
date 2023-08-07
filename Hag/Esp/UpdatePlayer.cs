using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT;
using EFT.Interactive;
using Hag.Helpers;
using EFT.InventoryLogic;


namespace Hag.Esp
{
    class UpdatePlayer : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Player());
        }

        private IEnumerator Player()
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

                        bp.W2S = Updating.WorldPointToScreenPoint(bp.Player.Transform.position);
                        bp.HeadW2S = Updating.WorldPointToScreenPoint(bp.Player.PlayerBones.Head.position);
                        bp.Distance = (int)Vector3.Distance(Globals.MainCamera.transform.position, bp.Player.Transform.position);
                        Updating.ApplyShader(bp.Player);

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
    }
}
