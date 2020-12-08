// NativeUI IV Example script for version 0.4.

using System;
using System.Drawing;
using System.Windows.Forms;

using GTA;
using NativeUI;

namespace NativeUITest
{
    public class Example : Script
    {

        private NativeUI.Menu uiMenu1; // Main menu

        // Items
        private NativeUI.UI.UIMenuItem uiMenuItem1;
        private NativeUI.UI.UIMenuItem uiMenuItem2;
        private NativeUI.UI.UIMenuItem uiMenuItem3;
        private NativeUI.UI.UIMenuItem uiMenuItemWithImage4;

        private NativeUI.UI.UIMenuCheckboxItem uIMenuCheckboxItem1;
        private NativeUI.UI.UIMenuCheckboxItem uIMenuCheckboxItem2;
        private NativeUI.UI.UIMenuCheckboxItem uIMenuCheckboxItem3;
        private NativeUI.UI.UIMenuCheckboxItem uIMenuCheckboxItem4;

        private NativeUI.UI.UIMenuListItem uiListItem1;

        public Example()
        {
            // Set settings for all menus
            NativeUI.Menu.Options.UpKey = Keys.Up;
            NativeUI.Menu.Options.DownKey = Keys.Down;
            NativeUI.Menu.Options.LeftKey = Keys.Left;
            NativeUI.Menu.Options.RightKey = Keys.Right;
            NativeUI.Menu.Options.AcceptKey = Keys.Enter;
            NativeUI.Menu.Options.disablePhoneWhenMenuIsOpened = true;
            NativeUI.Menu.Options.enableControllerSupport = true;
            NativeUI.Menu.Options.enableMenuSounds = true;

            #region Menu1
            // Create a new menu
            uiMenu1 = new NativeUI.Menu("NativeUI IV", "NATIVEUI IV SHOWCASE");

            // Create default items
            uiMenuItem1 = new UI.UIMenuItem("TestItem1", "I'm a button", "You can click on me!", true);
            uiMenuItem1.OnClick += UiMenuItem1_OnClick; // If item gets pressed.
            uiMenuItem2 = new UI.UIMenuItem("TestItem2", "I'm a disabled button", "You can't click on me... :(", false);
            uiMenuItem2.OnClick += UiMenuItem2_OnClick; // If item gets pressed.
            uiMenuItem3 = new UI.UIMenuItem("TestItem3", "Showcase button", "This is a long text. But wait, it can get even longer! It doesn't stop getting longer! Send help please! Quick! ...", true);

            // Create default button with an icon
            uiMenuItemWithImage4 = new UI.UIMenuItem("TestItem4", "More information...", "This is a default menu item just with an icon!", true);
            uiMenuItemWithImage4.DefaultIcon = new Texture(Properties.Resources.InfoSymbolDefault); // The icon that shows if the item is not selected.
            uiMenuItemWithImage4.SelectedIcon = new Texture(Properties.Resources.InfoSymbolSelected); // The icon that shows if the item is selected.
            uiMenuItemWithImage4.DisabledIcon = new Texture(Properties.Resources.InfoSymbolDisabled); // The icon that shows if the item is disabled.
            uiMenuItemWithImage4.IconLocation = UI.UIMenuItem.iconLocations.Right; // Location of your item (Left is default)
            uiMenuItemWithImage4.IconSize = new SizeF(32f, 32f); // The size of your icon. This is very important! (Maximum recommended size for your icon(s): 32x32)
            // uiMenuItemWithImage4.IconOffset(0f, 0f); // If your icon size is not 32x32 then you might have to adjust the offset a bit.
            uiMenuItemWithImage4.DrawIcon = true; // Allows the item to draw your icon

            // Create checkbox items
            uIMenuCheckboxItem1 = new UI.UIMenuCheckboxItem("TestCheckbox1", "I'm a checked checkbox", "You can also uncheck me if you want.", true, true);
            uIMenuCheckboxItem1.OnCheckedChanged += UIMenuCheckboxItem1_OnCheckedChanged; // If checkbox check state changes.
            uIMenuCheckboxItem2 = new UI.UIMenuCheckboxItem("TestCheckbox2", "I'm a unchecked checkbox", "You can also check me if you want.", true, false);
            uIMenuCheckboxItem3 = new UI.UIMenuCheckboxItem("TestCheckbox3", "I'm a checked disabled checkbox", "You can't uncheck me!", false, true);
            uIMenuCheckboxItem4 = new UI.UIMenuCheckboxItem("TestCheckbox3", "I'm a unchecked disabled checkbox", "You can't check me!", false, false);

            // Create list item
            uiListItem1 = new UI.UIMenuListItem("TestListItem1", "I'm a item with an list!", "Hit left, right to navigate through the list. Hit enter to show get the current selected item.", true);
            uiListItem1.OnSelectedIndexChanged += UiListItem1_OnSelectedIndexChanged;
            uiListItem1.OnClick += UiListItem1_OnClick;
            // Add items to list
            uiListItem1.AddItem("-");
            uiListItem1.AddItem("Item1");
            uiListItem1.AddItem("Item2");
            uiListItem1.AddItem("And this is item 3");

            // Add items to the menu
            uiMenu1.AddItem(uiMenuItem1);
            uiMenu1.AddItem(uiMenuItem2);
            uiMenu1.AddItem(uIMenuCheckboxItem1);
            uiMenu1.AddItem(uIMenuCheckboxItem2);
            uiMenu1.AddItem(uIMenuCheckboxItem3);
            uiMenu1.AddItem(uIMenuCheckboxItem4);
            uiMenu1.AddItem(uiMenuItem3);
            uiMenu1.AddItem(uiListItem1);
            uiMenu1.AddItem(uiMenuItemWithImage4);
            #endregion

            // Scripts stuff...
            this.Interval = 100;
            this.Tick += Class1_Tick;
            this.PerFrameDrawing += Class1_PerFrameDrawing;
            this.KeyDown += Class1_KeyDown;
        }

        private void UiListItem1_OnSelectedIndexChanged(UI.BaseElement sender, int newIndex)
        {
            Game.DisplayText("The new selected index is: " + newIndex.ToString());
        }
        private void UiListItem1_OnClick(UI.BaseElement sender)
        {
            Game.DisplayText("You clicked on item: " + sender.Name + " the selected item is: " + uiListItem1.SelectedText, 1500);
        }

        private void UIMenuCheckboxItem1_OnCheckedChanged(UI.BaseElement sender, bool newValue)
        {
            Game.DisplayText("CheckboxItem1 checked changed. New value = " + newValue.ToString(), 1500);
        }

        private void UiMenuItem2_OnClick(UI.BaseElement sender)
        {
            Game.DisplayText("'uiMenuItem2' is disabled so this should not show up ingame.", 1500);
        }
        private void UiMenuItem1_OnClick(UI.BaseElement sender)
        {
            Game.DisplayText("You clicked on button: " + sender.Name, 1500);
        }

 
        private void Class1_Tick(object sender, EventArgs e)
        {
            NativeUI.Menu.ProcessController(); // Allows you to use a controller to navigate through the menu. (Make sure that 'enableControllerSupport' is enabled)
        }
        private void Class1_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            NativeUI.Menu.ProcessDrawing(e); // Very important! This is used to draw the hole menu.
        }
        private void Class1_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            NativeUI.Menu.ProcessKeyPress(e); // Also very important! This is used to navigate through the menu with a keyboard.
            if (e.Key == Keys.G) // If Key 'G' is pressed, open or close the menu.
            {
                if (NativeUI.Menu.IsAnyMenuOpen()) // Check if any menu is opened right now.
                {
                    NativeUI.Menu.HideAllMenus(); // If true then hide all menus.
                }
                else
                {
                    NativeUI.Menu.Show(uiMenu1); // If false then show main menu.
                }
            }
        }

    }
}
