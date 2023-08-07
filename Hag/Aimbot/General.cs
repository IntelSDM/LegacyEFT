using EFT;
using Hag.Esp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Reflection;
using Hag.Helpers;
namespace Hag.Aimbot
{
    public struct G1Coefficent
    {
        // Token: 0x0600B23C RID: 45628 RVA: 0x00363FE0 File Offset: 0x003621E0
        public G1Coefficent(float _mach, float _ballist)
        {
            this.mach = _mach;
            this.ballist = _ballist;
        }

        // Token: 0x04009EEE RID: 40686
        public float mach;

        // Token: 0x04009EEF RID: 40687
        public float ballist;
    }
    public readonly struct TrajInfo
    {
        // Token: 0x0600A871 RID: 43121 RVA: 0x00332A5C File Offset: 0x00330C5C
        public TrajInfo(float time, Vector3 position, Vector3 velocity)
        {
            this.time = time;
            this.position = position;
            this.velocity = velocity;
        }

        // Token: 0x04009598 RID: 38296
        public readonly float time;

        // Token: 0x04009599 RID: 38297
        public readonly Vector3 position;

        // Token: 0x0400959A RID: 38298
        public readonly Vector3 velocity;
    }
    class General
    {
        #region Drag Calc


        private static readonly List<G1Coefficent> G1Coefficents = new List<G1Coefficent> { 
        new G1Coefficent(0f, 0.2629f),
		new G1Coefficent(0.05f, 0.2558f),
		new G1Coefficent(0.1f, 0.2487f),
		new G1Coefficent(0.15f, 0.2413f),
		new G1Coefficent(0.2f, 0.2344f),
		new G1Coefficent(0.25f, 0.2278f),
		new G1Coefficent(0.3f, 0.2214f),
		new G1Coefficent(0.35f, 0.2155f),
		new G1Coefficent(0.4f, 0.2104f),
		new G1Coefficent(0.45f, 0.2061f),
		new G1Coefficent(0.5f, 0.2032f),
		new G1Coefficent(0.55f, 0.202f),
		new G1Coefficent(0.6f, 0.2034f),
		new G1Coefficent(0.7f, 0.2165f),
		new G1Coefficent(0.725f, 0.223f),
		new G1Coefficent(0.75f, 0.2313f),
		new G1Coefficent(0.775f, 0.2417f),
		new G1Coefficent(0.8f, 0.2546f),
		new G1Coefficent(0.825f, 0.2706f),
		new G1Coefficent(0.85f, 0.2901f),
		new G1Coefficent(0.875f, 0.3136f),
		new G1Coefficent(0.9f, 0.3415f),
		new G1Coefficent(0.925f, 0.3734f),
		new G1Coefficent(0.95f, 0.4084f),
		new G1Coefficent(0.975f, 0.4448f),
		new G1Coefficent(1f, 0.4805f),
		new G1Coefficent(1.025f, 0.5136f),
		new G1Coefficent(1.05f, 0.5427f),
		new G1Coefficent(1.075f, 0.5677f),
		new G1Coefficent(1.1f, 0.5883f),
		new G1Coefficent(1.125f, 0.6053f),
		new G1Coefficent(1.15f, 0.6191f),
		new G1Coefficent(1.2f, 0.6393f),
		new G1Coefficent(1.25f, 0.6518f),
		new G1Coefficent(1.3f, 0.6589f),
		new G1Coefficent(1.35f, 0.6621f),
		new G1Coefficent(1.4f, 0.6625f),
		new G1Coefficent(1.45f, 0.6607f),
		new G1Coefficent(1.5f, 0.6573f),
		new G1Coefficent(1.55f, 0.6528f),
		new G1Coefficent(1.6f, 0.6474f),
		new G1Coefficent(1.65f, 0.6413f),
		new G1Coefficent(1.7f, 0.6347f),
		new G1Coefficent(1.75f, 0.628f),
		new G1Coefficent(1.8f, 0.621f),
		new G1Coefficent(1.85f, 0.6141f),
		new G1Coefficent(1.9f, 0.6072f),
		new G1Coefficent(1.95f, 0.6003f),
		new G1Coefficent(2f, 0.5934f),
		new G1Coefficent(2.05f, 0.5867f),
		new G1Coefficent(2.1f, 0.5804f),
		new G1Coefficent(2.15f, 0.5743f),
		new G1Coefficent(2.2f, 0.5685f),
		new G1Coefficent(2.25f, 0.563f),
		new G1Coefficent(2.3f, 0.5577f),
		new G1Coefficent(2.35f, 0.5527f),
		new G1Coefficent(2.4f, 0.5481f),
		new G1Coefficent(2.45f, 0.5438f),
		new G1Coefficent(2.5f, 0.5397f),
		new G1Coefficent(2.6f, 0.5325f),
		new G1Coefficent(2.7f, 0.5264f),
		new G1Coefficent(2.8f, 0.5211f),
		new G1Coefficent(2.9f, 0.5168f),
		new G1Coefficent(3f, 0.5133f),
		new G1Coefficent(3.1f, 0.5105f),
		new G1Coefficent(3.2f, 0.5084f),
		new G1Coefficent(3.3f, 0.5067f),
		new G1Coefficent(3.4f, 0.5054f),
		new G1Coefficent(3.5f, 0.504f),
		new G1Coefficent(3.6f, 0.503f),
		new G1Coefficent(3.7f, 0.5022f),
		new G1Coefficent(3.8f, 0.5016f),
		new G1Coefficent(3.9f, 0.501f),
		new G1Coefficent(4f, 0.5006f),
		new G1Coefficent(4.2f, 0.4998f),
		new G1Coefficent(4.4f, 0.4995f),
		new G1Coefficent(4.6f, 0.4992f),
		new G1Coefficent(4.8f, 0.499f),
		new G1Coefficent(5f, 0.4988f)
        
        };
        private static float CalculateDragCoefficient(float velocity)
        {
            int DragIndex = (int)Mathf.Floor(velocity / 343f / 0.05f);
            if (DragIndex <= 0)
                return 0f;

            if (DragIndex > G1Coefficents.Count() - 1)
            {
                return G1Coefficents[G1Coefficents.Count() - 1].ballist;
            }
            float PreviousDrag = G1Coefficents[DragIndex - 1].mach * 343f;
            float CurrentDrag = G1Coefficents[DragIndex].mach * 343f;
            float Ballist = G1Coefficents[DragIndex - 1].ballist;
            return (G1Coefficents[DragIndex].ballist - Ballist) / (CurrentDrag - PreviousDrag) * (velocity - PreviousDrag) + Ballist;
        }

      
#endregion
public static List<Player> SortClosestToCrosshair(List<Player> p)
        {
            return (from tempPlayer in p
                    orderby Vector2.Distance(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), Camera.main.WorldToScreenPoint(tempPlayer.Transform.position))
                    select tempPlayer).ToList<Player>();
        }
        //https://charbase.com/e011-unicode-invalid-character
        public static float CalculateBulletTime(Vector3 dir)
        {
            try
            {
                dir = (dir - Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position).normalized;
                Vector3 Direction = Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.up;
                Vector3 StartPosition = Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position;
                float num = Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position.y - Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.RootJoint.position.y;
                float height = num + Globals.LocalPlayer.ProceduralWeaponAnimation._agsDeltaHeight;
                float time = 0;
                //   Globals.LocalPlayer.ProceduralWeaponAnimation.GetPrivateField<Player.FirearmController>("").Item.ZeroLevelPosition(-Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.up,
                //      Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position,
                //     num + Globals.LocalPlayer.ProceduralWeaponAnimation._agsDeltaHeight,
                //    out time
                //   );

                Globals.LocalPlayer.ProceduralWeaponAnimation.GetPrivateField<Player.FirearmController>("").Item.ZeroLevelPosition(-dir,
                    Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position,
                    num + Globals.LocalPlayer.ProceduralWeaponAnimation._agsDeltaHeight,
                    out time
                    );

                return time;
                File.WriteAllText("Traj.txt", time.ToString());
            }
            catch (Exception ex)
            {
                File.WriteAllText("predex.txt", ex.ToString());
            }
            return 0f;
         //   Globals.LocalPlayer.ProceduralWeaponAnimation.CallPrivateMethod("");
         //     File.WriteAllText("test", $"ShotDIR: {Globals.LocalPlayer.ProceduralWeaponAnimation._shotDirection} Origin: {origin} DIR: {direction}");
        }
        //https://charbase.com/ea5b-unicode-invalid-character
        // explination so we need to basically use this function, go through all the stages of the projectile and find where it lands. then we find the distance from the landing position and the bone position
        // that distance is the distance the aimbot needs to calculate for.
        public static void CalculateDrop(Vector3 bonepos)
        {
            try
            {
                //\uEA5B.GetTrajectoryInfoArrayFromPool();
                Type trajtype = Type.GetType("");
                TrajInfo[] trajlist = (TrajInfo[])trajtype.GetMethod("GetTrajectoryInfoArrayFromPool", BindingFlags.Public | BindingFlags.Static).Invoke(trajtype, null);

                
                

                float num = Globals.LocalPlayerWeapon.CurrentAmmoTemplate.BulletMassGram / 1000f;
                float num2 = Globals.LocalPlayerWeapon.CurrentAmmoTemplate.BulletDiameterMilimeters / 1000f;
                float num3 = num2 * num2 * 3.1415927f / 4f;
                float num4 = 0.01f;

                // temp vars, remove later when we figure out how we should handle velocity and pos, ruskis probably meant speed not velocity.
                Vector3 Position = Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position; 
                Vector3 Velocity = bonepos * (Globals.LocalPlayerWeapon.SpeedFactor * Globals.LocalPlayerWeapon.CurrentAmmoTemplate.InitialSpeed);
                bonepos = (bonepos - Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position).normalized; // convert it from a pos to an angle
                trajlist[0] = new TrajInfo(0, Position, Velocity);
                for (int i = 1; i < trajlist.Length; i++)
                {
                    TrajInfo instance = trajlist[i - 1];
                    Vector3 velocity = instance.velocity;
                    Vector3 position = instance.position;

                    float num5 = num * CalculateDragCoefficient(velocity.magnitude) / Globals.LocalPlayerWeapon.CurrentAmmoTemplate.BallisticCoeficient / (num2 * num2) * 0.0014223f;
                    Vector3 a = Physics.gravity + velocity.normalized * (-num5 * 1.2f * num3 * velocity.magnitude * velocity.magnitude) / (2f * num);
                    Vector3 position2 = position + velocity * 0.01f + 5E-05f * a;
                    Vector3 velocity2 = velocity + a * 0.01f;
                    num4 += 0.01f;
                    File.WriteAllText("tt.txt", $"Pos = {position2} VEL = {velocity2} a = {a} num5 = {num5} bonepos = {bonepos}");
                    // return num5;
                }
            }
            catch(Exception ex) { File.WriteAllText("ex.txt", ex.Message); }
                //float distance = Vector3.Distance(Globals.LocalPlayer.ProceduralWeaponAnimation.HandsContainer.Fireport.position, bonepos);
                //   float traveltime = distance / (Globals.LocalPlayerWeapon.SpeedFactor * Globals.LocalPlayerWeapon.CurrentAmmoTemplate.InitialSpeed);
                //  float bulletDrop = 0.5f * 9.81f * (traveltime * traveltime) * CalculateDragCoefficient(traveltime);

            }


        public static bool IsBoneOnScreen(int bone, Player player)
        {
            switch (bone)
            {
                case 0:
                    return Globals.IsScreenPointVisible(Updating.WorldPointToScreenPoint(player.PlayerBones.Head.position + new Vector3(0, 0.07246377f, 0)));
                case 1:
                    return Globals.IsScreenPointVisible(Updating.WorldPointToScreenPoint(player.PlayerBones.Neck.position));
                case 2:
                    return Globals.IsScreenPointVisible(Updating.WorldPointToScreenPoint(player.PlayerBones.Spine1.position));
                case 3:
                    return Globals.IsScreenPointVisible(Updating.WorldPointToScreenPoint(player.PlayerBones.Spine3.position));
                case 4:
                    return Globals.IsScreenPointVisible(Updating.WorldPointToScreenPoint(player.PlayerBones.Pelvis.position));
            }

            return false;
        }
    }
}
