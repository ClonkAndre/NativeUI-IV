// NativeUI for Grand Theft Auto IV
// Made by ItsClonkAndre
// Version 0.8.1

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using static NativeUI.UI;
using static NativeUI.NativeUIEventHandler;

namespace NativeUI {
    /// <summary>
    /// Create a new instance of this class in order to create a menu.
    /// </summary>
    public class UIMenu {

        /// <summary>
        /// Options for all menus
        /// </summary>
        public class Options {
            #region Properties
            /// <summary>
            /// This will disable the movement of the player when the menu is opened and re-enables it when the menu closes.
            /// <para>It will also disable the phone.</para>
            /// <para>WARNING: If the player drives a vehicle, the vehicle WILL stop instantly!</para>
            /// </summary>
            public static bool disablePlayerMovementWhenMenuIsOpened { get; set; }
            /// <summary>
            /// Disables the phone when the menu is opened and re-enables it when the menu closes.
            /// <para>This allows you to use the following keys: UP, DOWN, LEFT, RIGHT and ENTER</para>
            /// </summary>
            public static bool disablePhoneWhenMenuIsOpened { get; set; }
            /// <summary>
            /// This enables the support for navigating through the menu with a controller.
            /// </summary>
            public static bool enableControllerSupport { get; set; }
            /// <summary>
            /// This will enable or disable all menu sounds.
            /// </summary>
            public static bool enableMenuSounds { get; set; }

            /// <summary>
            /// Gets or sets the animated banner frame rate.
            /// <para>If set to zero, 120 will be used instead.</para>
            /// </summary>
            public static int AnimatedBannerFrameRate { get; set; }

            /// <summary>
            /// Key for navigating up in the menu.
            /// </summary>
            public static Keys UpKey { get; set; }
            /// <summary>
            /// Key for navigating down in the menu.
            /// </summary>
            public static Keys DownKey { get; set; }
            /// <summary>
            /// Key for navigating left in the menu.
            /// </summary>
            public static Keys LeftKey { get; set; }
            /// <summary>
            /// Key for navigating right in the menu.
            /// </summary>
            public static Keys RightKey { get; set; }
            /// <summary>
            /// Key to confirm the selection in the menu.
            /// </summary>
            public static Keys AcceptKey { get; set; }
            #endregion
        }

        private void AnimatonHelper_AnimatedTextureReturner(Texture texture)
        {
            if (texture != null) menuImage = texture;
        }

        #region Static

        #region Variables
        /// <summary>
        /// This stores the currently opened menu in it.
        /// </summary>
        private static UIMenu currentlyOpenedMenu;
        /// <summary>
        /// A collection of all menus.
        /// </summary>
        private static Collection<UIMenu> allMenus = new Collection<UIMenu>();
        #endregion

        #region Methods
        /// <summary>
        /// Starts drawing the menu on screen.
        /// </summary>
        /// <param name="targetMenu">The menu that you want to be drawn</param>
        public static void Show(UIMenu targetMenu) {
            if (targetMenu != null) {
                if (currentlyOpenedMenu != null) {
                    currentlyOpenedMenu.isMenuOpened = false; // Close old menu
                    currentlyOpenedMenu = targetMenu; // Replace old menu with target menu
                    currentlyOpenedMenu.selectedIndex = 0;
                    currentlyOpenedMenu.viewRangeStart = 0;
                    currentlyOpenedMenu.viewRangeEnd = currentlyOpenedMenu.MaxItemsVisibleAtOnce;
                    currentlyOpenedMenu.isMenuOpened = true; // Open target menu
                    if (currentlyOpenedMenu.animatonHelper != null) currentlyOpenedMenu.animatonHelper.StartLoopingGIF();
                    if (Options.disablePlayerMovementWhenMenuIsOpened) Game.LocalPlayer.CanControlCharacter = false;
                    if (Options.disablePhoneWhenMenuIsOpened) Function.Call("TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME", "spcellphone");
                }
                else {
                    currentlyOpenedMenu = targetMenu; // Set target menu
                    currentlyOpenedMenu.selectedIndex = 0;
                    currentlyOpenedMenu.viewRangeStart = 0;
                    currentlyOpenedMenu.viewRangeEnd = currentlyOpenedMenu.MaxItemsVisibleAtOnce;
                    currentlyOpenedMenu.isMenuOpened = true; // Open target menu
                    if (currentlyOpenedMenu.animatonHelper != null) currentlyOpenedMenu.animatonHelper.StartLoopingGIF();
                    if (Options.disablePlayerMovementWhenMenuIsOpened) Game.LocalPlayer.CanControlCharacter = false;
                    if (Options.disablePhoneWhenMenuIsOpened) Function.Call("TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME", "spcellphone");
                }
            }
        }
        /// <summary>
        /// Stops drawing the menu on screen.
        /// </summary>
        /// <param name="targetMenu">The menu that you want to hide</param>
        public static void Hide(UIMenu targetMenu) {
            if (targetMenu != null) {
                if (currentlyOpenedMenu != null) {
                    if (currentlyOpenedMenu == targetMenu) {
                        currentlyOpenedMenu.isMenuOpened = false;
                        if (currentlyOpenedMenu.animatonHelper != null) currentlyOpenedMenu.animatonHelper.StopLoopingGIF();
                        if (Options.disablePlayerMovementWhenMenuIsOpened) Game.LocalPlayer.CanControlCharacter = true;
                        if (Options.disablePhoneWhenMenuIsOpened) Function.Call("START_NEW_SCRIPT", "spcellphone", 512);
                        return;
                    }
                }

                for (int i = 0; i < allMenus.Count; i++) {
                    if (allMenus[i] == targetMenu) {
                        allMenus[i].isMenuOpened = false;
                        if (currentlyOpenedMenu.animatonHelper != null) currentlyOpenedMenu.animatonHelper.StopLoopingGIF();
                        if (Options.disablePlayerMovementWhenMenuIsOpened) Game.LocalPlayer.CanControlCharacter = true;
                        if (Options.disablePhoneWhenMenuIsOpened) Function.Call("START_NEW_SCRIPT", "spcellphone", 512);
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// Hides all of your menus.
        /// </summary>
        public static void HideAllMenus() {
            if (allMenus.Count != 0) {
                for (int i = 0; i < allMenus.Count; i++) {
                    if (allMenus[i].isMenuOpened) {
                        allMenus[i].isMenuOpened = false;
                        if (allMenus[i].animatonHelper != null) currentlyOpenedMenu.animatonHelper.StopLoopingGIF();
                    }
                }

                if (Options.disablePlayerMovementWhenMenuIsOpened) Game.LocalPlayer.CanControlCharacter = true;
                if (Options.disablePhoneWhenMenuIsOpened) Function.Call("START_NEW_SCRIPT", "spcellphone", 512);
            }
        }
        /// <summary>
        /// Checks if any menu is opened right now.
        /// </summary>
        /// <returns>Returns true if there is a menu opened, otherwise false.</returns>
        public static bool IsAnyMenuOpen() {
            if (allMenus.Count != 0) {
                for (int i = 0; i < allMenus.Count; i++) {
                    if (allMenus[i].isMenuOpened) {
                        return true;
                    }
                }
                return false;
            }
            else {
                return false;
            }
        }
        #endregion

        #region Processing
        /// <summary>
        /// This method needs to be called everytime from your PerFrameDrawing method in order for the menu to be drawn.
        /// </summary>
        /// <param name="args">The GraphicsEventArgs from your PerFrameDrawing method.</param>
        public static void ProcessDrawing(GraphicsEventArgs args) {
            if (allMenus.Count != 0) {
                for (int i = 0; i < allMenus.Count; i++) {
                    allMenus[i].DrawMenu(args);
                }
            }
        }
        /// <summary>
        /// This method needs to be called everytime you press a key from your KeyDown method in order for the menu to work.
        /// </summary>
        /// <param name="args">The GTA.KeyEventArgs from your KeyDown method.</param>
        public static void ProcessKeyPress(GTA.KeyEventArgs args)
        {
            if (allMenus.Count != 0) {
                for (int i = 0; i < allMenus.Count; i++) {
                    allMenus[i].KeyPress(args);
                }
            }
        }
        /// <summary>
        /// This method needs to be called everytime from your Tick method in order for the controller navigation to work.
        /// <para>Note: The higher your Tick interval, the higher the input lag from your controller is.</para>
        /// </summary>
        public static void ProcessController()
        {
            if (allMenus.Count != 0) {
                for (int i = 0; i < allMenus.Count; i++) {
                    allMenus[i].GetControllerInput();
                }
            }
        }
        #endregion

        #endregion

        #region EventHandler
        /// <summary>
        /// Raises when the selected index of this menu changes.
        /// </summary>
        public event MSelectedIndexChangedEventHandler OnSelectedIndexChanged;
        #endregion

        #region Variables and Properties
        // Variables
        private Texture menuImage;
        private Texture menuArrowsUpDown;
        private Texture checkboxCheckedSelected;
        private Texture checkboxUncheckedSelected;
        private Texture checkboxCheckedUnselected;
        private Texture checkboxUncheckedUnselected;
        private Texture checkboxCheckedDisabled;
        private Texture checkboxUncheckedDisabled;
        private Texture listboxArrowLeft;
        private Texture listboxArrowRight;

        private AnimationHelper animatonHelper;

        private GTA.Font menuImageFont;
        private GTA.Font menuFont;

        private string noItemText = "There are no items in this list.";
        private Size noItemTextSize;
        private Size menuTitleSize;
        private Size menuDescriptionSize;

        protected internal int viewRangeStart = 0;
        protected internal int viewRangeEnd = 6;
        private int maxItemsVisibleAtOnce = 6;

        // Collections
        /// <summary>
        /// A collection of all items currently available in this menu.
        /// </summary>
        public Collection<BaseElement> menuItems;

        // Properties
        /// <summary>
        /// The title of the menu.
        /// </summary>
        public string menuTitle { get; private set; }
        /// <summary>
        /// The description of the menu.
        /// </summary>
        public string menuDescription { get; private set; }
        /// <summary>
        /// Gets the visibility state of this menu.
        /// </summary>
        public bool isMenuOpened { get; private set; }
        /// <summary>
        /// Gets the currently selected item index of this menu.
        /// </summary>
        public int selectedIndex { get; private set; }
        /// <summary>
        /// Gets or sets how much items the menu can display at once.
        /// <para>Default value: 6</para>
        /// <para>Minimum value: 2</para>
        /// </summary>
        public int MaxItemsVisibleAtOnce
        {
            get { return maxItemsVisibleAtOnce; }
            set {
                if (value < 2) {
                    maxItemsVisibleAtOnce = 6;
                    viewRangeEnd = 6;
                }
                else {
                    maxItemsVisibleAtOnce = value;
                    viewRangeEnd = value;
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of the Menu class.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="description">The description of the menu. (Text under the image)</param>
        public UIMenu(string title, string description) 
        {
            try {
                menuImage = new Texture(Properties.Resources.MenuImage);
                menuArrowsUpDown = new Texture(Properties.Resources.ArrowsUpDown);
                checkboxCheckedSelected = new Texture(Properties.Resources.CheckboxCheckedSelected);
                checkboxUncheckedSelected = new Texture(Properties.Resources.CheckboxUncheckedSelected);
                checkboxCheckedUnselected = new Texture(Properties.Resources.CheckboxCheckedUnselected);
                checkboxUncheckedUnselected = new Texture(Properties.Resources.CheckboxUncheckedUnselected);
                checkboxCheckedDisabled = new Texture(Properties.Resources.CheckboxCheckedDisabled);
                checkboxUncheckedDisabled = new Texture(Properties.Resources.CheckboxUncheckedDisabled);
                listboxArrowLeft = new Texture(Properties.Resources.ArrowLeft);
                listboxArrowRight = new Texture(Properties.Resources.ArrowRight);
                menuItems = new Collection<BaseElement>();
                menuFont = new GTA.Font("Calibri", 26f, FontScaling.Pixel, true, false);
                menuFont.Effect = FontEffect.None;
                menuImageFont = new GTA.Font("Calibri", 42f, FontScaling.Pixel, true, false);
                menuImageFont.Effect = FontEffect.None;
                
                menuTitle = title;
                menuTitleSize = TextRenderer.MeasureText(menuTitle, menuFont.WindowsFont);
                menuDescription = description;
                menuDescriptionSize = TextRenderer.MeasureText(menuDescription, menuFont.WindowsFont);
                noItemTextSize = TextRenderer.MeasureText(noItemText, menuFont.WindowsFont);

                allMenus.Add(this);
            }
            catch (Exception ex) {
                Game.Console.Print("NativeUI - Error while creating menu. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Constructor of the Menu class.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="description">The description of the menu (Text under the image).</param>
        /// <param name="image">The menu image. Not GIF friendly. Only static images. Recommended size: 432x97.</param>
        public UIMenu(string title, string description, Texture image)
        {
            try {
                if (image != null) { menuImage = image; } else { menuImage = new Texture(Properties.Resources.MenuImage); }
                menuArrowsUpDown = new Texture(Properties.Resources.ArrowsUpDown);
                checkboxCheckedSelected = new Texture(Properties.Resources.CheckboxCheckedSelected);
                checkboxUncheckedSelected = new Texture(Properties.Resources.CheckboxUncheckedSelected);
                checkboxCheckedUnselected = new Texture(Properties.Resources.CheckboxCheckedUnselected);
                checkboxUncheckedUnselected = new Texture(Properties.Resources.CheckboxUncheckedUnselected);
                checkboxCheckedDisabled = new Texture(Properties.Resources.CheckboxCheckedDisabled);
                checkboxUncheckedDisabled = new Texture(Properties.Resources.CheckboxUncheckedDisabled);
                listboxArrowLeft = new Texture(Properties.Resources.ArrowLeft);
                listboxArrowRight = new Texture(Properties.Resources.ArrowRight);
                menuItems = new Collection<BaseElement>();
                menuFont = new GTA.Font("Calibri", 26f, FontScaling.Pixel, true, false);
                menuFont.Effect = FontEffect.None;
                menuImageFont = new GTA.Font("Calibri", 42f, FontScaling.Pixel, true, false);
                menuImageFont.Effect = FontEffect.None;

                menuTitle = title;
                menuTitleSize = TextRenderer.MeasureText(menuTitle, menuFont.WindowsFont);
                menuDescription = description;
                menuDescriptionSize = TextRenderer.MeasureText(menuDescription, menuFont.WindowsFont);
                noItemTextSize = TextRenderer.MeasureText(noItemText, menuFont.WindowsFont);

                allMenus.Add(this);
            }
            catch (Exception ex) {
                Game.Console.Print("NativeUI - Error while creating menu. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Constructor of the Menu class.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="description">The description of the menu (Text under the image).</param>
        /// <param name="image">The menu image. Supports GIFs. Recommended size: 432x97.</param>
        public UIMenu(string title, string description, Image image)
        {
            try {
                if (image == null) {
                    menuImage = new Texture(Properties.Resources.MenuImage);
                }
                else {
                    animatonHelper = new AnimationHelper(image);
                    animatonHelper.AnimatedTextureReturner += AnimatonHelper_AnimatedTextureReturner;
                    menuImage = animatonHelper.GetFirstFrameOfImage();
                }

                menuArrowsUpDown = new Texture(Properties.Resources.ArrowsUpDown);
                checkboxCheckedSelected = new Texture(Properties.Resources.CheckboxCheckedSelected);
                checkboxUncheckedSelected = new Texture(Properties.Resources.CheckboxUncheckedSelected);
                checkboxCheckedUnselected = new Texture(Properties.Resources.CheckboxCheckedUnselected);
                checkboxUncheckedUnselected = new Texture(Properties.Resources.CheckboxUncheckedUnselected);
                checkboxCheckedDisabled = new Texture(Properties.Resources.CheckboxCheckedDisabled);
                checkboxUncheckedDisabled = new Texture(Properties.Resources.CheckboxUncheckedDisabled);
                listboxArrowLeft = new Texture(Properties.Resources.ArrowLeft);
                listboxArrowRight = new Texture(Properties.Resources.ArrowRight);
                menuItems = new Collection<BaseElement>();
                menuFont = new GTA.Font("Calibri", 26f, FontScaling.Pixel, true, false);
                menuFont.Effect = FontEffect.None;
                menuImageFont = new GTA.Font("Calibri", 42f, FontScaling.Pixel, true, false);
                menuImageFont.Effect = FontEffect.None;

                menuTitle = title;
                menuTitleSize = TextRenderer.MeasureText(menuTitle, menuFont.WindowsFont);
                menuDescription = description;
                menuDescriptionSize = TextRenderer.MeasureText(menuDescription, menuFont.WindowsFont);
                noItemTextSize = TextRenderer.MeasureText(noItemText, menuFont.WindowsFont);

                allMenus.Add(this);
            }
            catch (Exception ex)
            {
                Game.Console.Print("NativeUI - Error while creating menu. Details: " + ex.Message);
            }
        }
        #endregion

        #region Collection Methods
        // Add
        /// <summary>
        /// Add an item to the menu.
        /// </summary>
        /// <param name="item">Adds the item to the menu</param>
        public void AddItem(BaseElement item) {
            menuItems.Add(item);
        }
        /// <summary>
        /// Adds an array of items to the menu.
        /// </summary>
        /// <param name="items">The array with items</param>
        public void AddItems(BaseElement[] items) {
            try {
                for (int i = 0; i < items.Length; i++) {
                    menuItems.Add(items[i]);
                }
            }
            catch (Exception ex) {
                Game.Console.Print("NativeUI - Error while adding items to the menu. Details: " + ex.Message);
            }
        }

        // Remove
        /// <summary>
        /// This removes the specific item from the list.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns>Returns true if the item was deleted, otherwise false.</returns>
        public bool RemoveItem(BaseElement item) {
            try {
                for (int i = 0; i < menuItems.Count; i++) {
                    if (menuItems[i] == item) {
                        menuItems.Remove(menuItems[i]);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex) {
                Game.Console.Print("NativeUI - Error while removing an item from the list. Details: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// This removes the specific item from the list through it's index.
        /// </summary>
        /// <param name="index">The index where the item is at</param>
        /// <returns>Returns true if the item was deleted, otherwise false.</returns>
        public bool RemoveItem(int index) {
            try {
                for (int i = 0; i < menuItems.Count; i++) {
                    if (i == index) {
                        menuItems.RemoveAt(i);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex) {
                Game.Console.Print("NativeUI - Error while removing an item from the list through it's index. Details: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// This removes all items from the menu.
        /// </summary>
        public void RemoveAllItems() {
            menuItems.Clear();
        }
        #endregion

        #region Controller
        private void GetControllerInput() {
            try {
                if (Options.enableControllerSupport) {
                    if (isMenuOpened) {
                        if (Function.Call<bool>("IS_USING_CONTROLLER")) {
                            if (Function.Call<bool>("IS_BUTTON_PRESSED", 0, 8)) // DPAD_UP
                            {
                                ActualKeyPress(Options.UpKey);
                            }
                            else if (Function.Call<bool>("IS_BUTTON_PRESSED", 0, 9)) // DPAD_DOWN
                            {
                                ActualKeyPress(Options.DownKey);
                            }
                            else if (Function.Call<bool>("IS_BUTTON_PRESSED", 0, 10)) // DPAD_LEFT
                            {
                                ActualKeyPress(Options.LeftKey);
                            }
                            else if (Function.Call<bool>("IS_BUTTON_PRESSED", 0, 11)) // DPAD_RIGHT
                            {
                                ActualKeyPress(Options.RightKey);
                            }
                            else if (Function.Call<bool>("IS_BUTTON_PRESSED", 0, 16)) // A
                            {
                                ActualKeyPress(Options.AcceptKey);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Game.Console.Print("NativeUI ProcessController unhandled exception. Details: " + ex.ToString());
                Options.enableControllerSupport = false;
                Game.Console.Print("NativeUI Warning: 'enableControllerSupport' was disabled to prevent any more errors.");
            }
        }
        #endregion

        #region Drawing
        private void DrawMenu(GraphicsEventArgs args) {
            if (isMenuOpened) {
                // Draw image
                args.Graphics.DrawSprite(menuImage, new RectangleF(29f, 17f, 432f, 97f));
                args.Graphics.DrawText(menuTitle, new RectangleF(40f, 45f, 432f, menuTitleSize.Height + 10), TextAlignment.Center, Color.White, menuImageFont);

                // Draw Description
                args.Graphics.DrawRectangle(245f, 133f, 432f, 38f, Color.Black);
                args.Graphics.DrawText(menuDescription, new RectangleF(38f, 120f, menuDescriptionSize.Width, menuDescriptionSize.Height), TextAlignment.Left, Color.FromArgb(255, 45, 110, 184), menuFont);

                // Draw selected index text
                Size selectedIndexTextSize;
                if (menuItems.Count == 0) {
                    string zeroText = "0 / 0";
                    selectedIndexTextSize = TextRenderer.MeasureText(zeroText, menuFont.WindowsFont);
                    float xValue = selectedIndexTextSize.Width - (zeroText.Length * 2) - (zeroText.Length == 0 ? 0 : 40f);

                    args.Graphics.DrawText(zeroText, new RectangleF(410f - xValue, 120f, selectedIndexTextSize.Width, selectedIndexTextSize.Height), TextAlignment.Center, Color.FromArgb(255, 45, 110, 184), menuFont);
                }
                else {
                    string selectedIndexText = string.Format("{0} / {1}", (selectedIndex + 1).ToString(), menuItems.Count.ToString());
                    selectedIndexTextSize = TextRenderer.MeasureText(selectedIndexText, menuFont.WindowsFont);
                    float xValue = selectedIndexTextSize.Width - (selectedIndexText.Length * 2) - (selectedIndexText.Length == 0 ? 0 : 40f);

                    args.Graphics.DrawText(selectedIndexText, new RectangleF(410f - xValue, 120f, selectedIndexTextSize.Width, selectedIndexTextSize.Height), TextAlignment.Center, Color.FromArgb(255, 45, 110, 184), menuFont);
                }

                // Distance between items and text: 38
                // Draw items
                if (menuItems.Count != 0) {

                    if (menuItems.Count < maxItemsVisibleAtOnce) {
                        for (int i = 0; i < menuItems.Count; i++) {
                            if (selectedIndex == i) {
                                menuItems[i].IsSelected = true;
                            }
                            else {
                                menuItems[i].IsSelected = false;
                            }

                            switch (menuItems[i].Type) { // Draw items

                                case ControlType.UIMenuItem:
                                    UIMenuItem defaultItem = (UIMenuItem)menuItems[i];
                                    defaultItem.DrawItem(args, menuFont, i);
                                    break;

                                case ControlType.UIMenuCheckboxItem:
                                    UIMenuCheckboxItem chkItem = (UIMenuCheckboxItem)menuItems[i];
                                    chkItem.DrawItem(args, menuFont, i, checkboxCheckedSelected, checkboxUncheckedSelected, checkboxCheckedUnselected, checkboxUncheckedUnselected, checkboxCheckedDisabled, checkboxUncheckedDisabled);
                                    break;

                                case ControlType.UIMenuListItem:
                                    UIMenuListItem listItem = (UIMenuListItem)menuItems[i];
                                    listItem.DrawItem(args, menuFont, i, listboxArrowLeft, listboxArrowRight);
                                    break;

                            }
                        }
                    }
                    else {
                        for (int i = viewRangeStart; i < viewRangeEnd; i++) {
                            if (selectedIndex == i) {
                                menuItems[i].IsSelected = true;
                            }
                            else {
                                menuItems[i].IsSelected = false;
                            }

                            switch (menuItems[i].Type) { // Draw items

                                case ControlType.UIMenuItem:
                                    UIMenuItem defaultItem = (UIMenuItem)menuItems[i];
                                    defaultItem.DrawItem(args, menuFont, i - viewRangeStart);
                                    break;

                                case ControlType.UIMenuCheckboxItem:
                                    UIMenuCheckboxItem chkItem = (UIMenuCheckboxItem)menuItems[i];
                                    chkItem.DrawItem(args, menuFont, i - viewRangeStart, checkboxCheckedSelected, checkboxUncheckedSelected, checkboxCheckedUnselected, checkboxUncheckedUnselected, checkboxCheckedDisabled, checkboxUncheckedDisabled);
                                    break;

                                case ControlType.UIMenuListItem:
                                    UIMenuListItem listItem = (UIMenuListItem)menuItems[i];
                                    listItem.DrawItem(args, menuFont, i - viewRangeStart, listboxArrowLeft, listboxArrowRight);
                                    break;

                            }
                        }
                    }

                }
                else {
                    args.Graphics.DrawRectangle(245f, 171f, 432f, 38f, Color.FromArgb(170, 10, 10, 10));
                    args.Graphics.DrawText(noItemText, new RectangleF(40f, 157f, noItemTextSize.Width, noItemTextSize.Height), TextAlignment.Left, Color.White, menuFont);
                }

                if (menuItems.Count != 0) {
                    if (menuItems.Count < maxItemsVisibleAtOnce) {
                        // Draw arrows
                        args.Graphics.DrawRectangle(245f, 210f + (menuItems.Count - 1) * 38, 432f, 38f, Color.FromArgb(225, 0, 0, 0));
                        args.Graphics.DrawSprite(menuArrowsUpDown, new RectangleF(235.5f, 195f + (menuItems.Count - 1) * 38, 18f, 29f));

                        // Draw help text
                        args.Graphics.DrawRectangle(245f, 235f + (menuItems.Count - 1) * 38, 432f, 2f, Color.Black); // Black line

                        Size DescriptionTextSize = TextRenderer.MeasureText(menuItems[selectedIndex].Description, menuFont.WindowsFont, new Size(424, 35), TextFormatFlags.WordBreak);
                        args.Graphics.DrawRectangle(245f, 255f + (menuItems.Count - 1) * 38, 432f, 38f, Color.FromArgb(100, 10, 10, 10));
                        args.Graphics.DrawText(menuItems[selectedIndex].Description, new RectangleF(38f, 241f + (menuItems.Count - 1) * 38, 424f, DescriptionTextSize.Height), TextAlignment.WordBreak, Color.White, menuFont);
                    }
                    else {
                        // Draw arrows
                        args.Graphics.DrawRectangle(245f, 210f + (maxItemsVisibleAtOnce - 1) * 38, 432f, 38f, Color.FromArgb(225, 0, 0, 0));
                        args.Graphics.DrawSprite(menuArrowsUpDown, new RectangleF(235.5f, 195f + (maxItemsVisibleAtOnce - 1) * 38, 18f, 29f));

                        // Draw help text
                        args.Graphics.DrawRectangle(245f, 235f + (maxItemsVisibleAtOnce - 1) * 38, 432f, 2f, Color.Black); // Black line

                        Size DescriptionTextSize = TextRenderer.MeasureText(menuItems[selectedIndex].Description, menuFont.WindowsFont, new Size(424, 35), TextFormatFlags.WordBreak);
                        args.Graphics.DrawRectangle(245f, 255f + (maxItemsVisibleAtOnce - 1) * 38, 432f, 38f, Color.FromArgb(100, 10, 10, 10));
                        args.Graphics.DrawText(menuItems[selectedIndex].Description, new RectangleF(38f, 241f + (maxItemsVisibleAtOnce - 1) * 38, 424f, DescriptionTextSize.Height), TextAlignment.WordBreak, Color.White, menuFont);
                    }
                }
            }
        }
        #endregion

        #region KeyPress
        private void KeyPress(GTA.KeyEventArgs args) {
            ActualKeyPress(args.Key);
        }
        private void ActualKeyPress(Keys pressedKey) {
            if (isMenuOpened) {
                if (pressedKey == Options.UpKey) { // UP
                    if (menuItems.Count != 0) {
                        if (selectedIndex == 0) {
                            selectedIndex = (menuItems.Count - 1);
                            viewRangeStart = (menuItems.Count - 1) - (maxItemsVisibleAtOnce - 1);
                            viewRangeEnd = menuItems.Count;
                        }
                        else {
                            selectedIndex--;
                            if (selectedIndex < viewRangeStart) {
                                viewRangeStart--;
                                viewRangeEnd--;
                            }
                        }

                        if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_HIGHLIGHT_DOWN_UP"); } // Play sound
                        MSelectedIndexChangedEventHandler handler = OnSelectedIndexChanged;
                        if (handler != null) { handler(this, selectedIndex); } // RaiseEvent
                    }
                }
                else if (pressedKey == Options.DownKey) { // DOWN
                    if (menuItems.Count != 0) {
                        if (selectedIndex == (menuItems.Count - 1)) {
                            selectedIndex = 0;
                            viewRangeStart = 0;
                            viewRangeEnd = maxItemsVisibleAtOnce;
                        }
                        else {
                            selectedIndex++;
                            if (selectedIndex >= viewRangeEnd) {
                                viewRangeStart++;
                                viewRangeEnd++;
                            }
                        }

                        if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_HIGHLIGHT_DOWN_UP"); } // Play sound
                        MSelectedIndexChangedEventHandler handler = OnSelectedIndexChanged;
                        if (handler != null) { handler(this, selectedIndex); } // RaiseEvent
                    }
                }
                else if (pressedKey == Options.LeftKey) { // LEFT
                    if (menuItems.Count != 0) {
                        switch (menuItems[selectedIndex].Type) {

                            case ControlType.UIMenuListItem:
                                UIMenuListItem listItem = (UIMenuListItem)menuItems[selectedIndex];
                                if (listItem.IsEnabled && listItem.itemList.Count != 0) {
                                    if (listItem.SelectedIndex == 0) { listItem.SelectedIndex = (listItem.ItemAmount - 1); } else { listItem.SelectedIndex--; }
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_SLIDER_DOWN"); } // Play sound

                                    listItem.SelectedIndexChangedRaiser(listItem.SelectedIndex);
                                    listItem.SelectedTextChangedRaiser(listItem.SelectedText);
                                }
                                break;

                        }
                    }
                }
                else if (pressedKey == Options.RightKey) { // RIGHT
                    if (menuItems.Count != 0) {
                        switch (menuItems[selectedIndex].Type) {

                            case ControlType.UIMenuListItem:
                                UIMenuListItem listItem = (UIMenuListItem)menuItems[selectedIndex];
                                if (listItem.IsEnabled && listItem.itemList.Count != 0) {
                                    if (listItem.SelectedIndex == (listItem.ItemAmount - 1)) { listItem.SelectedIndex = 0; } else { listItem.SelectedIndex++; }
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_SLIDER_UP"); } // Play sound

                                    listItem.SelectedIndexChangedRaiser(listItem.SelectedIndex);
                                    listItem.SelectedTextChangedRaiser(listItem.SelectedText);
                                }
                                break;

                        }
                    }
                }
                else if (pressedKey == Options.AcceptKey) { // ACCEPT
                    if (menuItems.Count != 0) {
                        switch (menuItems[selectedIndex].Type) {

                            case ControlType.UIMenuItem:
                                UIMenuItem item = (UIMenuItem)menuItems[selectedIndex];
                                if (item.IsEnabled) {
                                    item.PerformClick();

                                    if (item.NestedMenu != null) { Show(item.NestedMenu); } // Open nested menu
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_SELECT"); } // Play sound
                                }
                                else {
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_ERROR"); } // Play error sound
                                }
                                break;

                            case ControlType.UIMenuCheckboxItem:
                                UIMenuCheckboxItem chkItem = (UIMenuCheckboxItem)menuItems[selectedIndex];
                                if (chkItem.IsEnabled) {
                                    if (chkItem.IsChecked) {
                                        chkItem.IsChecked = false;
                                        if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_TOGGLE_OFF"); } // Play uncheck sound
                                    }
                                    else {
                                        chkItem.IsChecked = true;
                                        if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_TOGGLE_ON"); } // Play check sound
                                    }
                                }
                                else {
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_ERROR"); } // Play error sound
                                }
                                break;

                            case ControlType.UIMenuListItem:
                                UIMenuListItem listItem = (UIMenuListItem)menuItems[selectedIndex];
                                if (listItem.IsEnabled && listItem.itemList.Count != 0) {
                                    listItem.PerformClick();
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_SELECT"); } // Play sound
                                }
                                else {
                                    if (Options.enableMenuSounds) { Function.Call("PLAY_SOUND_FRONTEND", -1, "FRONTEND_MENU_ERROR"); } // Play error sound
                                }
                                break;

                        }
                    }
                }
            }
        }
        #endregion

    }
}
