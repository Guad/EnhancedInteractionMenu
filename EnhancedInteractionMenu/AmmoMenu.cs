using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;
using NativeUI;

namespace EnhancedInteractionMenu
{
    public class AmmoMenu : UIMenu
    {
        private Dictionary<string, WeaponHash[]> _database = new Dictionary<string, WeaponHash[]>
        {
            {"Pistols", new WeaponHash[]
            {
                WeaponHash.APPistol,
                WeaponHash.Pistol, 
                WeaponHash.Pistol50, 
                WeaponHash.CombatPistol, 
                WeaponHash.HeavyPistol, 
                WeaponHash.MarksmanPistol, 
                WeaponHash.SNSPistol, 
                WeaponHash.VintagePistol,
            } },
            { "Submachine", new WeaponHash[]
            {
                WeaponHash.SMG, 
                WeaponHash.AssaultSMG, 
                WeaponHash.MicroSMG, 
                WeaponHash.CombatPDW, 
                WeaponHash.MG, 
                WeaponHash.CombatMG, 
                WeaponHash.Gusenberg, 
            }},
            { "Assault Rifles", new WeaponHash[]
            {
                WeaponHash.AssaultRifle, 
                WeaponHash.AdvancedRifle, 
                WeaponHash.BullpupRifle, 
                WeaponHash.CarbineRifle, 
                WeaponHash.SpecialCarbine,
            }},
            {"Sniper Rifles", new WeaponHash[]
            {
                WeaponHash.SniperRifle, 
                WeaponHash.HeavySniper, 
                WeaponHash.MarksmanRifle, 
            } },
            { "Shotguns", new WeaponHash[]
            {
                WeaponHash.AssaultShotgun, 
                WeaponHash.BullpupShotgun, 
                WeaponHash.HeavyShotgun, 
                WeaponHash.PumpShotgun, 
                WeaponHash.SawnOffShotgun, 
                WeaponHash.Musket, 
            }},
            {"Heavy", new WeaponHash[]
            {
                WeaponHash.GrenadeLauncher, 
                WeaponHash.HomingLauncher, 
                WeaponHash.RPG, 
                WeaponHash.Railgun, 
                WeaponHash.Minigun, 
            }},
            {"Explosives", new WeaponHash[]
            {
                WeaponHash.Grenade, 
                WeaponHash.BZGas, 
                WeaponHash.SmokeGrenade, 
                WeaponHash.Molotov, 
                WeaponHash.StickyBomb, 
                WeaponHash.ProximityMine, 
            } }
        };

        public AmmoMenu() : base("", "~b~AMMO")
        {
            Title.Caption = Game.Player.Name;
            SetBannerType(PIMenu.GlobalMenuBanner);
            Title.Font = GTA.Font.ChaletComprimeCologne;
            Title.Scale = 0.86f;
            Subtitle.Color = PIMenu.MenuColor;
            RedrawLists("Pistols");
            RefreshIndex();
        }

        private void RedrawLists(string type)
        {
            Clear();
            var typeList = _database.Select(pair => pair.Key).Cast<dynamic>().ToList();
            var typeItem = new UIMenuListItem("Ammo Type", typeList, typeList.FindIndex(n => n.ToString() == type), "Select an ammo type to purchase.");
            AddItem(typeItem);

            typeItem.OnListChanged += (list, newindex) =>
            {
                string newType = ((UIMenuListItem)MenuItems[0]).IndexToItem(((UIMenuListItem)MenuItems[0]).Index).ToString();
                RedrawLists(newType);
            };

            var weaponItem = new UIMenuListItem("Weapon", GetListForType(type), 0);
            AddItem(weaponItem);

            var buyRounds = new UIMenuItem("Rounds x 24");
            buyRounds.SetRightLabel("113$");
            var buyAllRounds = new UIMenuItem("Full Ammo");
            buyRounds.SetRightLabel("113$");

            buyRounds.Activated += (menu, item) =>
            {
                Game.Player.Character.Weapons.Give((WeaponHash)Enum.Parse(typeof(WeaponHash), ((UIMenuListItem)MenuItems[1]).IndexToItem(((UIMenuListItem)MenuItems[1]).Index).ToString()), 24, false, false);
            };

            buyAllRounds.Activated += (menu, item) =>
            {
                Game.Player.Character.Weapons.Give((WeaponHash)Enum.Parse(typeof(WeaponHash), ((UIMenuListItem)MenuItems[1]).IndexToItem(((UIMenuListItem)MenuItems[1]).Index).ToString()), 9999, false, false);
            };

            AddItem(buyRounds);
            AddItem(buyAllRounds);
        }

        private void RecalculatePrice()
        {
            
        }

        public List<dynamic> GetListForType(string type)
        {
            return _database[type].Select(hash => (dynamic) hash).ToList();
        }
    }
}