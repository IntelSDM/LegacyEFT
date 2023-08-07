using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Configs
{
    class Item
    {
        public bool Enabled = true;
        public int MaxDistance = 600;
        public bool DrawName = true;
        public bool DrawDistance = true;
        public bool DrawValue = true;
        public bool Type = true;
        public int MinValue = 40000;

        public bool DrawRare = true;
        public bool DrawSuperRare = true;
        public bool SuperRareBypassMinValue = true;
        public bool DrawAll = false;
        public bool DrawQuestItems = false;
        public int BypassFilterOverValue = 75000;
        public bool WhitelistedItems = true;
        public bool EnableInaccurateEsp = false;
        public bool EnableAccurateEsp = true;
    }
    class Mod
    {
        public bool Enable = false;
        public bool BypassMinValue = false;
        public int MinValue = 10000;
        /*
                 FunctionalMod,
            Muzzle,
            Sights,
            SpecialScope,
            Mod,
            GearMod,
        Magazine,
         MasterMod,
         */
    }
    class Fuel
    {
        public bool Enable = false;
        public int MinValue = 10000;
        //Lubricant,

    }
    class BarterItem
    {
    
        public bool Enable = true;
        public int MinValue = 40000;
        //BarterItem,
    }
    class SpecialItem
    {
        public bool Enable = true;
        public int MinValue = 70000;
        //Item,
    }
    class Cases
    {
        public bool Enable = true;
        public int MinValue = 1000;
        // compounditem
    }
    class Backpacks
    {
        public bool Enable = true;
        public int MinValue = 30000;
        //SearchableItem // also has some crates
    }
    class Weapon
    {
        public bool Enable = true;
        public int MinValue = 43000;
        //Weapon
    }
    class Clothing
    {
        public bool Enable = true;
        public int MinValue = 40000;
        //Equipment,
    }
    class Armour
    {
        public bool Enable = true;
        public int MinValue = 60000;
        //ArmoredEquipment,
    }
    class FoodAndDrink 
    {
        public bool Enable = false;
        public int MinValue = 1000;
    }
    class Key
    {
        public bool Enable = true;
        public int MinValue = 15000;
        // check the keycards and make them a unique colourr
        //Key,
    }
    class Meds
    {
        public bool Enable = true;
        public int MinValue = 40000;
        //Meds,
    }
    class Ammo
    {
        public bool Enable = true;
        public int MinValue = 40000;
        //StackableItem,
    }
    class RepairKits
    {
        public bool Enable = true;
        public int MinValue = 60000;
    }
}
