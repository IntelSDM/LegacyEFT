using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using EFT;
using EFT.Interactive;
using Comfort.Common;
using EFT.InventoryLogic;
using System.Collections;
using System.IO;
using UnityEngine.Scripting;

namespace Hag.Esp
{
    class Caching : MonoBehaviour
    {

        private static float ExfilCacheTime;
        private static float ContainerCacheTime;
        private static float GrenadeCacheTime;

        void Start()
        {
          
            Globals.MainCamera = Camera.main;
            StartCoroutine(CachePlayer());
            StartCoroutine(CacheItems());
            StartCoroutine(CacheExfil());
            StartCoroutine(CacheContainers());
   //         StartCoroutine(CacheAssets());
            StartCoroutine(CacheGrenade());
            
        }
        IEnumerator CacheAssets()
        {
            for (; ; )
            {
              //  Helpers.ShaderHelper.RefreshList();
                yield return new WaitForSeconds(10f);
            }
        }
        IEnumerator CachePlayer()
        {
            for (; ; )
            {
                GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
                Globals.GameWorld = Singleton<GameWorld>.Instance;
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(3f);
                if (!Globals.EndedFrame)
                    continue;
                Globals.PlayerList.Clear();

                if (Globals.GameWorld != null)
                {

                    foreach (Player player in Globals.GameWorld.RegisteredPlayers)
                    {

                        if (player == null)
                            continue;
                        if (player.IsYourPlayer)
                        {
                            Globals.LocalPlayer = player;
                            continue;
                        }
                          if (player.HealthController.IsAlive)
                           {
                        BasePlayer player1 = new BasePlayer(player);
                        player1.SetOneTimeVars();
                        Globals.PlayerList.Add(player1);
                         }
                    

                    }
                }
                yield return new WaitForSeconds(3f);
            }
        
        }
        IEnumerator CacheItems()
        {
            for (; ; )
            {
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(5f);
                if (!Globals.EndedFrame)
                    continue;
                Globals.CorpseList.Clear();
                Globals.LootList.Clear();
                if (!Globals.Config.Item.Enabled) // add corpse check too
                    yield return new WaitForSeconds(5f);


                if (Globals.GameWorld != null)
                {
                    for (int i = 0; i < Globals.GameWorld.LootItems.Count; i++)
                    {
                        LootItem item = Globals.GameWorld.LootItems.GetByIndex(i);
                        if (item == null)
                            continue;

                        if (!(item.Item.TemplateId == "55d7217a4bdc2d86028b456d".Localized()))
                        {
                            if (!Globals.Config.Item.Enabled)
                                continue;
                            BaseItem item1 = new BaseItem(item);
                    //        Helpers.ShaderHelper.LoadSprites(item.Item);
                            item1.SetOneTimeVars();
                            Globals.LootList.Add(item1);
                        }

                        if (item.Item.TemplateId == "55d7217a4bdc2d86028b456d".Localized() && Globals.Config.Corpse.Enable)
                        {
                            
                            BaseCorpse corpse1 = new BaseCorpse(item);
                            corpse1.SetOneTimeVars();
                            Globals.CorpseList.Add(corpse1);

                        }

                    }
                }

                yield return new WaitForSeconds(5f);
                // continue;

            }
        }
        IEnumerator CacheExfil()
        {
            for (; ; )
            {
                Globals.ExfilList.Clear();
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(10f);
                if (!Globals.EndedFrame)
                    continue;
             
                //     if (Globals.GameWorld.ExfiltrationController.ExfiltrationPoints == null)
                //       yield return new WaitForSeconds(10f);
                foreach (ExfiltrationPoint exfilpoint
                in LocationScene.GetAllObjects<ExfiltrationPoint>(false))
                {
                    if (exfilpoint == null)
                        continue;
                    BaseExfil exfilpoint1 = new BaseExfil(exfilpoint);
                
                    Globals.ExfilList.Add(exfilpoint1);
                    exfilpoint1.SetOneTimeVars();

                }
                yield return new WaitForSeconds(10f);
            }
        }
        IEnumerator CacheContainers()
        {
            for (; ; )
            {
                Globals.ContainerList.Clear();
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(5f);
                if (!Globals.EndedFrame)
                    continue;
            
                        if (!Globals.Config.Container.Enable) 
                           yield return new WaitForSeconds(5f);


                foreach (LootableContainer container
                in LocationScene.GetAllObjects<LootableContainer>(false))
                {
                    if (container == null)
                        continue;
                    BaseContainer container1 = new BaseContainer(container);
             //       
                    Globals.ContainerList.Add(container1);
                    container1.SetOneTimeVars();
                }

                    yield return new WaitForSeconds(5f);
            }
        }

        IEnumerator CacheGrenade()
        {
            for (; ; )
            {
                Globals.GrenadeList.Clear();
                if (Globals.GameWorld == null)
                    yield return new WaitForSeconds(0.2f);
                if (!Globals.EndedFrame)
                    continue;
               

                //    var e = Globals.GameWorld.Grenades.GetValuesEnumerator().GetEnumerator();
                foreach (Throwable grenade
                  in LocationScene.GetAllObjects<Throwable>(false))
                {
                    if (grenade == null)
                        continue;

                    BaseGrenade grenade1 = new BaseGrenade(grenade);
                    grenade1.SetOneTimeVars();
                    Globals.GrenadeList.Add(grenade1);
                }
                  

                  
                

                yield return new WaitForSeconds(0.2f);
            }
        }
        void Cache()
        {

            
            if (Globals.LocalPlayer != null)
                    if (Globals.LocalPlayer?.HandsController?.Item is Weapon)
                        Globals.LocalPlayerWeapon = (Weapon)Globals.LocalPlayer?.HandsController?.Item;
            
            if (Globals.GameWorld == null)
                    return;
              
            
            

            // Globals.GameWorld = Singleton<GameWorld>.Instance;
            //    if (Globals.LocalPlayer != null)
            //        if (Globals.LocalPlayer?.HandsController?.Item is Weapon)
            //            Globals.LocalPlayerWeapon = (Weapon)Globals.LocalPlayer?.HandsController?.Item;
           

             /*   #region Player
                  if (Time.time > PlayerCacheTime)
                  {
             
                Globals.PlayerList.Clear();

                      foreach (Player player in Globals.GameWorld.RegisteredPlayers)
                      {
                          if (player != null)
                          {
                              if (player.IsYourPlayer)
                              {
                                  Globals.LocalPlayer = player;
                                  continue;
                              }
                              if (player.HealthController.IsAlive)
                              {
                                  Globals.PlayerList.Add(player);

                              }
                          }

                      }

                      PlayerCacheTime = Time.time + 2;
                  }
                  #endregion */ 
             /*   #region Loot
                if (Time.time > ItemCacheTime)
                        {
                            Globals.CorpseList.Clear();
                            Globals.LootList.Clear();
                            for (int i = 0; i < Globals.GameWorld.LootItems.Count; i++)
                            {
                                LootItem item = Globals.GameWorld.LootItems.GetByIndex(i);
                                if (!(item.Item.ShortName.Localized() == "Default Inventory"))
                                {
                                    if (item != null)
                                    {

                                        Globals.LootList.Add(item);
                                    }

                                }
                                if (item.Item.ShortName.Localized() == "Default Inventory")
                                {
                                    if (item != null)
                                    {
                                        Globals.CorpseList.Add(item);
                                    }

                                }

                            }
                            ItemCacheTime = Time.time + 4;
                        }
                        #endregion*/
                 /*   #region Container 
                    if (Time.time > ContainerCacheTime)
                    {
                        Globals.ContainerList.Clear();
                        foreach (LootableContainer container
                            in LocationScene.GetAllObjects<LootableContainer>(false))
                        {
                            if (container != null)
                            {
                                Globals.ContainerList.Add(container);
                            }
                        }
                        ContainerCacheTime = Time.time + 3;
                    }
                    #endregion*/
             /*     #region Exfil
                   if (Time.time >= ExfilCacheTime)
                   {
                       Globals.ExfilList.Clear();
                       if (Globals.GameWorld.ExfiltrationController.ExfiltrationPoints != null)
                       {
                           foreach (ExfiltrationPoint exfilpoint
                               in Globals.GameWorld.ExfiltrationController.ExfiltrationPoints)
                           {


                               if (exfilpoint != null)
                               {

                                   Globals.ExfilList.Add(exfilpoint);

                               }
                           }

                       }


                       ExfilCacheTime = Time.time + 5;

                   }
                   #endregion*/
           /*    #region Grenade
               if (Time.time > GrenadeCacheTime)
               {
                   Globals.GrenadeList.Clear();
                   var e = Globals.GameWorld.Grenades.GetValuesEnumerator().GetEnumerator();
                   while (e.MoveNext())
                   {
                       var grenade = e.Current;

                       if (grenade == null)
                           continue;


                       Globals.GrenadeList.Add(grenade);
                   }
                   GrenadeCacheTime = Time.time + 1;
               }
               #endregion*/
           

        }
    }
}
