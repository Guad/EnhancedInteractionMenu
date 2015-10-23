using GTA;
using GTA.Native;
using GTA.Math;
using GTA.NaturalMotion;
using NativeUI;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using EnhancedInteractionMenu;

public class PIMenu : Script
{
    MenuPool MenuPool = new MenuPool();
    public static UIResRectangle GlobalMenuBanner;

    UIMenu MainMenu;
    UIMenuListItem QuickGPSItem;
    UIMenuListItem PlayerMoodItem;
    UIMenuListItem WalkStyleItem;
    UIMenuListItem AimingStyleItem;
    UIMenuItem PassiveModeItem;
    UIMenuListItem MenuColorItem;

    bool IsPassiveMode = false;

    UIMenu InventoryMenu;

    public static Color MenuColor;

    public PIMenu()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;

        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@brave");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@confident");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@drunk");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@drunk@verydrunk");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@fat@a");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@shadyped@a");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@hurry@a");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@injured");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@intimidation@1h");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@quick");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@sad@a");
        Function.Call(Hash.REQUEST_CLIP_SET, "move_m@tool_belt@a");

        MenuColor = Color.Blue;

        MainMenu = new UIMenu(Game.Player.Name.ToString(), "INTERACTION MENU", new Point(0, 0));
        MenuPool.Add(MainMenu);
        GlobalMenuBanner = new UIResRectangle();
        GlobalMenuBanner.Color = MenuColor;
        MainMenu.SetBannerType(GlobalMenuBanner);
        MainMenu.Title.Font = GTA.Font.ChaletComprimeCologne;
        MainMenu.Title.Scale = 0.86f;
        MainMenu.Subtitle.Color = MenuColor;

        var QuickGPSList = new List<dynamic>
        {
            "None",
            "Ammu-Nation",
            "Convenience Store",
            "Mod Shop",
            "Clothes Store",
        };

        MainMenu.AddItem(QuickGPSItem = new UIMenuListItem("Quick GPS", QuickGPSList, 0, "Select to place your waypoint at a set location."));

        QuickGPSItem.Activated += (UIMenu sender, UIMenuItem selecteditem) =>
        {
            var tmpList = (UIMenuListItem)selecteditem;
            switch (tmpList.Index)
            {
                case 0:
                    Function.Call(Hash.SET_WAYPOINT_OFF);
                    break;
                case 1:
                    Vector3 NearestAmmunation = PointsOfInterest.GetClosestPoi(Game.Player.Character.Position, PointsOfInterest.Type.AmmuNation);
                    Function.Call(Hash.SET_NEW_WAYPOINT, NearestAmmunation.X, NearestAmmunation.Y);
                    break;
            }
        };

        InventoryMenu = new UIMenu(Game.Player.Name.ToString(), "INVENTORY", new Point(0, 0));
        MenuPool.Add(InventoryMenu);
        InventoryMenu.SetBannerType(GlobalMenuBanner);
        InventoryMenu.Title.Font = GTA.Font.ChaletComprimeCologne;
        InventoryMenu.Title.Scale = 0.86f;
        InventoryMenu.Subtitle.Color = MenuColor;

        var InventoryMenuItem = new UIMenuItem("Inventory", "Your inventory which contains clothing, ammo, and much more.");

        MainMenu.AddItem(InventoryMenuItem);

        MainMenu.BindMenuToItem(InventoryMenu, InventoryMenuItem);

        var PlayerMoodsList = new List<dynamic>
        {
            "None",
            "Normal",
            "Happy",
            "Angry",
            "Injured",
            "Stressed",
        };

        MainMenu.AddItem(PlayerMoodItem = new UIMenuListItem("Player Mood", PlayerMoodsList, 0, "Sets your character's facial expression."));

        var WalkStyleList = new List<dynamic>
        {
            "Normal",
            "Brave",
            "Confident",
            "Drunk",
            "Fat",
            "Gangster",
            "Hurry",
            "Injured",
            "Intimidated",
            "Quick ",
            "Sad",
            "Tough Guy"
        };

        MainMenu.AddItem(WalkStyleItem = new UIMenuListItem("Walk Style", WalkStyleList, 0, "Sets your Character's walking style."));

        var AimingStyleList = new List<dynamic>
        {
            "None",
            "Gangster",
            "Cowboy",
        };

        MainMenu.AddItem(AimingStyleItem = new UIMenuListItem("Aiming Style", AimingStyleList, 0, "Sets your Character's pistol aiming style."));

        PassiveModeItem = new UIMenuItem("Enable Passive Mode", "Passive Mode will prevent damage and wanted levels from police.");

        MainMenu.AddItem(PassiveModeItem);

        var MenuColorList = new List<dynamic>
        {
            "Blue",
            "Red",
            "Green",
            "Orange",
            "Purple",
            "Yellow",
        };

        MainMenu.AddItem(MenuColorItem = new UIMenuListItem("Color", MenuColorList, 0, "Select interaction menu's color theme."));

        MainMenu.OnItemSelect += (UIMenu sender, UIMenuItem selectedItem, int index) =>
        {
            switch (index)
            {
                case 5:
                    if (!IsPassiveMode)
                    {
                        Game.Player.Character.IsInvincible = true;
                        Game.Player.Character.Alpha = 200;
                        Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, true);
                        Function.Call(Hash.SET_WANTED_LEVEL_MULTIPLIER, 0.0f);
                        PassiveModeItem.Text = "Disable Passive Mode";
                        IsPassiveMode = true;
                    }

                    else if (IsPassiveMode)
                    {
                        Game.Player.Character.ResetAlpha();
                        Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, false);
                        Function.Call(Hash.SET_WANTED_LEVEL_MULTIPLIER, 1.0f);
                        PassiveModeItem.Text = "Enable Passive Mode";
                        IsPassiveMode = false;
                    }
                    break;
            }
        };

        MainMenu.OnListChange += (UIMenu sender, UIMenuListItem listItem, int newIndex) =>
        {
            if (listItem == MenuColorItem)
            {
                switch (newIndex)
                {
                    case 0:
                        MenuColor = Color.Blue;
                        break;
                    case 1:
                        MenuColor = Color.Red;
                        break;
                    case 2:
                        MenuColor = Color.Green;
                        break;
                    case 3:
                        MenuColor = Color.Orange;
                        break;
                    case 4:
                        MenuColor = Color.Purple;
                        break;
                    case 5:
                        MenuColor = Color.Yellow;
                        break;
                }
            }

            if (listItem == WalkStyleItem)
            {
                switch (newIndex)
                {
                    case 0:
                        Game.Player.Character.Task.ClearAll();
                        Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Game.Player.Character, 0x3E800000);
                        break;
                    case 1:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@brave", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 2:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@confident", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 3:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@drunk@verydrunk", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 4:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@fat@a", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 5:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@shadyped@a", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 6:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@hurry@a", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 7:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@injured", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 8:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@intimidation@1h", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 9:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@quick", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 10:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@sad@a", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                    case 11:
                        Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "move_m@tool_belt@a", 0x3E800000);
                        Game.Player.Character.Task.ClearAll();
                        break;
                }
            }

            if (listItem == PlayerMoodItem)
            {
                switch (newIndex)
                {
                    case 0:
                        Function.Call(Hash.CLEAR_FACIAL_IDLE_ANIM_OVERRIDE, Game.Player.Character);
                        break;
                    case 1:
                        Function.Call(Hash.SET_FACIAL_IDLE_ANIM_OVERRIDE, Game.Player.Character, "mood_normal_1", 0);
                        break;
                    case 2:
                        Function.Call(Hash.SET_FACIAL_IDLE_ANIM_OVERRIDE, Game.Player.Character, "mood_happy_1", 0);
                        break;
                    case 3:
                        Function.Call(Hash.SET_FACIAL_IDLE_ANIM_OVERRIDE, Game.Player.Character, "Mood_Angry_1", 0);
                        break;
                    case 4:
                        Function.Call(Hash.SET_FACIAL_IDLE_ANIM_OVERRIDE, Game.Player.Character, "Mood_Injured_1", 0);
                        break;
                    case 5:
                        Function.Call(Hash.SET_FACIAL_IDLE_ANIM_OVERRIDE, Game.Player.Character, "Mood_Stressed_1", 0);
                        break;
                }
            }

            if (listItem == AimingStyleItem)
            {
                switch (newIndex)
                {
                    case 0:
                        Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "default"));
                        break;
                    case 1:
                        Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "Gang1H"));
                        break;
                    case 2:
                        Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, Game.Player.Character, Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly"));
                        break;
                }
            }
        };

        var menu = new AmmoMenu();
        var ammoItem = new UIMenuItem("Ammo");
        InventoryMenu.AddItem(ammoItem);
        InventoryMenu.BindMenuToItem(menu, ammoItem);
        MenuPool.Add(menu);

        MainMenu.RefreshIndex();
        InventoryMenu.RefreshIndex();

        
    }
    
    public static void PrettifyMenu(ref UIMenu menu)
    {
        menu.Title.Caption = Game.Player.Name;
        menu.SetBannerType(GlobalMenuBanner);
        menu.Title.Font = GTA.Font.ChaletComprimeCologne;
        menu.Title.Scale = 0.86f;
        menu.Subtitle.Color = MenuColor;
    }

    void OnTick(object sender, EventArgs e)
    {
        Game.DisableControl(2, GTA.Control.InteractionMenu);
        MenuPool.ProcessMenus();

        if (MenuPool.IsAnyMenuOpen())
        {
            GlobalMenuBanner.Color = MenuColor;
            MainMenu.Subtitle.Color = MenuColor;
            InventoryMenu.Subtitle.Color = MenuColor;
        }

        if (IsPassiveMode)
        {
            Game.Player.WantedLevel = 0;
        }
    }

    void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (Game.IsControlJustPressed(2, GTA.Control.InteractionMenu) && !MenuPool.IsAnyMenuOpen())
        {
            Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            MainMenu.Visible = true;
        }

        else if (Game.IsControlJustPressed(2, GTA.Control.InteractionMenu) && MenuPool.IsAnyMenuOpen())
        {
            Game.PlaySound("BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            MenuPool.CloseAllMenus();
        }
    }
}