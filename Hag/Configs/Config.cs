using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Config
    {
        public Colours Colours = new Colours();
        public Menu Menu = new Menu();
        public Item Item = new Item();
        public Player Player = new Player();
        public Scav ScavPlayer = new Scav();
        public Scav Scav = new Scav();
        public Exfil Exfil = new Exfil();
        public Corpse Corpse = new Corpse();
        public LocalPlayerInfo LocalPlayerInfo = new LocalPlayerInfo();
        public Container Container = new Container();
        public Weapons Weapon = new Weapons();
        public Movement Movement = new Movement();
        public LocalPlayerVisuals Visuals = new LocalPlayerVisuals();
        public Aimbot Aimbot = new Aimbot();
        public EntityAimbot PlayerAimbot = new EntityAimbot();
        public EntityAimbot ScavAimbot = new EntityAimbot();
        public EntityAimbot ScavPlayerAimbot = new EntityAimbot();
        public LocalPlayerWorld LocalPlayerWorld = new LocalPlayerWorld();
        public PlayerInformation PlayerInformation = new PlayerInformation();
        #region ItemFilters
        public Fuel ItemFuel = new Fuel();
        public BarterItem ItemBarterItem = new BarterItem();
        public SpecialItem ItemSpecialItem = new SpecialItem();
        public Cases ItemCases = new Cases();
        public Backpacks ItemBackpacks = new Backpacks();
        public Weapon ItemWeapon = new Weapon();
        public Mod ItemMods = new Mod();
        public Clothing ItemClothing = new Clothing();
        public Armour ItemArmour = new Armour();
        public FoodAndDrink ItemFoodAndDrink = new FoodAndDrink();
        public Key ItemKey = new Key();
        public Meds ItemMeds = new Meds();
        public Ammo ItemAmmo = new Ammo();
        public RepairKits ItemRepairKits = new RepairKits();
        #endregion
    }
}
