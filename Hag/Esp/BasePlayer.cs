using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;
using EFT.Interactive;
using EFT.InventoryLogic;
using System.IO;
namespace Hag.Esp
{
    class BasePlayer : IDisposable
    {
        public BasePlayer(Player player)
        {
            Player = player;

        }
        public Weapon Weapon;
        public Player Player;
        public int Distance;
        public Vector3 W2S;
        public Vector3 HeadW2S;
        public string Flag;
        public int Value;
        public List<Item> Items;

        public Color32 Colour;
        public Color32 VisibleColour;
        public Color32 InvisibleColour;
        public Color32 BoxColour;
        public Color32 FilledBoxColour;

        public Vector3[] Bones = new Vector3[20];

        public void Dispose()
        {

            GC.SuppressFinalize(this);
      
        }
       
        public float GetPlayerValue()
        {
            float Invcost = 0;



            foreach (Item item in this.Player.Profile.Inventory.Equipment.GetAllItems())
            {
                try
                {
                    OurItem ouritem = OurItems.list[item.Template._id];



                    Invcost += ouritem.price;


                    if (Globals.Config.LocalPlayerWorld.LootPockets)
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

                    }


                    if (item.Template._parent == "5448bf274bdc2dfc2f8b456a") // container, loop the items in it and remove the value and then remove the value of the container itself
                    {
                        foreach (Item otem in item.GetAllItems())
                        {
                            try
                            {
                                OurItem ouritem2 = OurItems.list[otem.Template._id];
                                Invcost -= ouritem2.price;

                            }
                            catch { }
                        }


                    }
                }
                catch { }
            }
            return Invcost;

        }
               

        
        
    /*    public float GetPlayerValue(out List<Item>Items)
        {
            float Invcost = 0;
            List<Item> RetItems = new List<Item>();
            //  this.Player.Profile.Inventory.Equipment.GetSlot
        //    this.Player.Profile.Inventory.Equipment.GetSlot("").
            foreach (Item item in this.Player.Profile.Inventory.Equipment.GetAllItems())
            {
                if (!OurItems.list.ContainsKey(item.Template._id))
                    continue;
                try
                {
                    OurItem ouritem = OurItems.list[item.Template._id];

                    /*   if (item.Template._parent == "5448bf274bdc2dfc2f8b456a" // container
                       || item.Template._parent == "5447e1d04bdc2dff2f8b4567" // knife
                       || item.Template._id == "5f4f9eb969cdc30ff33f09db" // compass
                       || item.Template._id == "557ffd194bdc2d28148b457f" // pockets
                       || item.Template._id == "55d7217a4bdc2d86028b456d") // default inventory
                           continue;
                    if (item.Template._parent == "5447e1d04bdc2dff2f8b4567" // knife
                        || item.Template._id == "5f4f9eb969cdc30ff33f09db" // compass
                        || item.Template._id == "557ffd194bdc2d28148b457f" // pockets
                         || ouritem.name == "Pockets 1x3" // pockets 1x3
                        || item.Template._id == "55d7217a4bdc2d86028b456d" // default inventory
                        || ouritem.subtype == item_subtype.ArmBand // arm bands
                         || ouritem.subtype == item_subtype.MobContainer
                      || ouritem.name.Contains("Boss container"))
                         
                        continue;
                    item.Template.ExamineTime = 0;
                    Invcost += ouritem.price;
                    RetItems.Add(item);
                    if (item.Template._parent == "5448bf274bdc2dfc2f8b456a")
                    {
                        //  RetItems.Add(item);
                        foreach (Item otem in item.GetAllItems())
                        {
                            try
                            {
                                OurItem ouritem2 = OurItems.list[otem.Template._id];
                                Invcost -= ouritem2.price;
                                RetItems.Remove(otem);
                            }
                            catch { }
                        }

                    }

                    //    File.WriteAllText("itm.txt", $"{ouritem.name}, {ouritem.price}, {item.Template._id}");
                    /*   foreach (Item sub_item in item.GetAllItems(false))
                       {
                           OurItem oursubitem = OurItems.list[sub_item.Template._id];
                           Invcost += ouritem.price;
                       }
                }
                catch { }
            }
            Items = RetItems;
            return Invcost;
        }*/
        public void SetOneTimeVars()
        {
            Value = (int)GetPlayerValue();
        }
    }
     
}
