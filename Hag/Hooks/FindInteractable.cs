﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hag.Helpers;
using EFT.Interactive;
using EFT;
namespace Hag.Hooks
{
    class FindInteractable : MonoBehaviour
    {
        private static DumbHook InteractHook;
        void Start()
        {
            InteractHook = new DumbHook();
            InteractHook.Init(typeof(GameWorld).GetMethod("FindInteractable"), typeof(FindInteractable).GetMethod("FindInteractableHook"));
            InteractHook.Hook();
        }
        public static int int_3;
        private static int int_1 = LayerMask.GetMask(new string[]
        {
                "Interactive",
                "Deadbody",
                "Player",
                "Loot"
        });
        private static LayerMask intmask = 1 << 12 | 1 << 18 | 1 << 13 | 1 << 9 | 1 << 22;

        public static bool Raycast1(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
        {
            bool result = Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
            return result;
        }
        public static GameObject FindInteractableHook(Ray ray, out RaycastHit hit)
        {
            if (Globals.Config.LocalPlayerWorld.LootThroughWalls)
            {
                EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE = 4;
                EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE = 4;
                       EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE = 4;

                Hag.Hooks.FindInteractable.int_3 = LayerMask.GetMask(new string[]
                {

                 });
            }
            else
            {
                   Hag.Hooks.FindInteractable.int_3 = LayerMask.GetMask(new string[] {
                   "HighPolyCollider",
                   "TransparentCollider"
                   });
            }
            GameObject gameObject = Raycast1(ray, out hit, Mathf.Max(EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE, EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE + EFTHardSettings.Instance.BEHIND_CAST), int_1) ? hit.collider.gameObject : null;
            if (gameObject && !Physics.Linecast(ray.origin, hit.point, int_3))
            {
                return gameObject;
            }
            return null;
        }
    }
}
