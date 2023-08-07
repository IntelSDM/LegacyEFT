using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EFT.Interactive;
using EFT.InventoryLogic;
namespace Hag.Esp
{
    class BaseContainer : IDisposable
    {
        public BaseContainer(LootableContainer container)
        {
            Container = container;
        
        }
        public LootableContainer Container;
        public Vector3 W2S;
        public int Distance;
        public Color32 Colour;
        public int Value;
        public List<Item> Items;
        public float ContainerValue(out List<Item> Items)
        {
           
            float Invcost = 0;
            List<Item> RetItems = new List<Item>();
            try
            {
                foreach (Item item in this.Container.ItemOwner.RootItem.GetAllItems())
                {
                    if(Globals.Config.LocalPlayerWorld.InstantExamine)
                        item.Template.ExamineTime = 0;
                    if (!OurItems.list.ContainsKey(item.Template._id))
                        continue;
                    try
                    {
                        OurItem ouritem = OurItems.list[item.Template._id];

                        if (item.Template._parent == "5448bf274bdc2dfc2f8b456a" // container
                        || item.Template._parent == "5447e1d04bdc2dff2f8b4567" // knife
                        || item.Template._id == "5f4f9eb969cdc30ff33f09db" // compass
                        || item.Template._id == "557ffd194bdc2d28148b457f" // pockets
                        || item.Template._id == "55d7217a4bdc2d86028b456d" // default inventory
                        || ouritem.subtype == item_subtype.LootContainer
                        || ouritem.subtype == item_subtype.MobContainer
                        || ouritem.name == Container.Template.LocalizedShortName()) // for some reason everything has a 61k rub value of its own name in the items. weird.
                            continue;

                       
                        Invcost += ouritem.price;
                        RetItems.Add(item);
                        //    File.WriteAllText("itm.txt", $"{ouritem.name}, {ouritem.price}, {item.Template._id}");
                        /*   foreach (Item sub_item in item.GetAllItems(false))
                           {
                               OurItem oursubitem = OurItems.list[sub_item.Template._id];
                               Invcost += ouritem.price;
                           }*/
                    }
                    catch { }
                }
            }
            catch { }
            Items = RetItems;
            return Invcost;
        }
        public void Dispose()
        { }
        public void SetOneTimeVars()
        { Value = (int)ContainerValue(out Items); }
    }
}
