using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;
using EFT.Interactive;
using EFT.InventoryLogic;
using Hag.Helpers;
using System.Reflection;

namespace Hag.Esp
{
    class BaseCorpse
    {
        public BaseCorpse(LootItem corpse)
        {
            Corpse = corpse;
        }
        public LootItem Corpse;
        public Vector3 W2S;
        public int Distance;
        public Color32 Colour;
        public int Value;
        public List<string> Items;

        public void SetOneTimeVars()
        { Value = (int)CorpseValue(out Items); 
        }
        public float CorpseValue(out List<string> Items)
        {
            float Invcost = 0;
            List<string> RetItems = new List<string>();

            foreach (Item item in this.Corpse.ItemOwner.RootItem.GetAllItems().ToList())
            {
                
                    try
                    {
                        OurItem ouritem = OurItems.list[item.Template._id];


                   
                        Invcost += ouritem.price;
                        RetItems.Add(item.Template._id);
                    if(Globals.Config.LocalPlayerWorld.LootPockets)
                        item.Template.NotShownInSlot = false;
                    if (Globals.Config.LocalPlayerWorld.InstantExamine)
                        item.Template.ExamineTime = 0;
                    // Exclude Items That Are Parented By These Slots
                    if (item.Template._parent == "5447e1d04bdc2dff2f8b4567"// knife
                         ||
                         item.Template._parent == "55d7217a4bdc2d86028b456d" // Default Inventory
                         ||
                         item.Template._id == "55d7217a4bdc2d86028b456d" // Default Inventory
                            ||
                         item.Template._id == "5f4f9eb969cdc30ff33f09db" // compass
                         ||
                         item.Template._id == "557ffd194bdc2d28148b457f" // pockets
                         ||
                         ouritem.subtype == item_subtype.ArmBand
                         )
                         {
                             Invcost -= ouritem.price;
                             RetItems.Remove(item.Template._id);
                         }


                        if (item.Template._parent == "5448bf274bdc2dfc2f8b456a" || ouritem.subtype == item_subtype.MobContainer) // container, loop the items in it and remove the value and then remove the value of the container itself
                        {
                            foreach (Item otem in item.GetAllItems())
                            {
                                try
                                {
                                    OurItem ouritem2 = OurItems.list[otem.Template._id];
                                           Invcost -= ouritem2.price;
                                          RetItems.Remove(item.Template._id);
                                }
                                catch { }
                            }


                        }

                    }
                    catch
                    { }



                }
                Items = RetItems;
            return Invcost;
        }
    }
}
