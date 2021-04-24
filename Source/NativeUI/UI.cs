// NativeUI for Grand Theft Auto IV
// Made by ItsClonkAndre
// Version 0.8

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using GTA;
using static NativeUI.NativeUIEventHandler;

namespace NativeUI {
    /// <summary>
    /// All menu items.
    /// </summary>
    public class UI {

        /// <summary>
        /// All control types.
        /// </summary>
        public enum ControlType
        {
            /// <summary>
            /// Default control type (This is used by the BaseElement)
            /// </summary>
            Default,
            /// <summary>
            /// UIMenuItem: Normal item with just text.
            /// </summary>
            UIMenuItem,
            /// <summary>
            /// UIMenuCheckboxItem: Item with text and a checkbox.
            /// </summary>
            UIMenuCheckboxItem,
            /// <summary>
            /// UIMenuListItem: Item with text and a listbox with items in it.
            /// </summary>
            UIMenuListItem
        }

        /// <summary>
        /// Base class for all UI Elements.
        /// </summary>
        public class BaseElement
        {
            // Default properties
            // String
            /// <summary>
            /// The name of the element.
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// The text of the element.
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// The description for that element.
            /// </summary>
            public string Description { get; set; }

            // Object
            /// <summary>
            /// The tag of the element.
            /// This is used to store informations in it.
            /// </summary>
            public object[] Tag { get; set; }

            // Boolean
            /// <summary>
            /// Gets or sets if the element is enabled or not.
            /// </summary>
            public bool IsEnabled { get; set; }
            /// <summary>
            /// Gets if the current item is selected or not.
            /// </summary>
            public bool IsSelected { get; set; }

            // Color
            /// <summary>
            /// The backcolor of the non-selected item. (Default ARGB: A:170, R:10, G:10, B:10)
            /// </summary>
            public Color defaultItemBackColor { get; set; }
            /// <summary>
            /// The textcolor of the non-selected item. (Default ARGB: A:255, R:255, G:255, B:255)
            /// </summary>
            public Color defaultItemTextColor { get; set; }
            /// <summary>
            /// The backcolor of the selected item. (Default ARGB: A:255, R:245, G:245, B:245)
            /// </summary>
            public Color selectedItemBackColor { get; set; }
            /// <summary>
            /// The textcolor of the selected item. (Default ARGB: A:255, R:5, G:5, B:5)
            /// </summary>
            public Color selectedItemTextColor { get; set; }
            /// <summary>
            /// The textcolor of the disabled item. (Default ARGB: A:255, R:150, G:150, B:150)
            /// </summary>
            public Color disabledItemTextColor { get; set; }

            // Other
            /// <summary>
            /// The type of the item.
            /// </summary>
            public ControlType Type { get; protected set; } = ControlType.Default;
        }

        /// <summary>
        /// Default menu item with only text.
        /// </summary>
        public class UIMenuItem : BaseElement
        {
            /// <summary>
            /// Icon locations.
            /// </summary>
            public enum iconLocations
            {
                /// <summary>
                /// Icon will be placed on the left side.
                /// </summary>
                Left = 0,
                /// <summary>
                /// Icon will be placed on the right side.
                /// </summary>
                Right = 1
            }

            #region Constructor
            /// <summary>
            /// Creates a new menu item with the default colors.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            public UIMenuItem(string name, string text, string description, bool enabled) {
                this.Type = ControlType.UIMenuItem;
                this.IconLocation = iconLocations.Left;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;

                // Default colors
                this.defaultItemBackColor = Color.FromArgb(170, 10, 10, 10);
                this.defaultItemTextColor = Color.FromArgb(255, 255, 255, 255);
                this.selectedItemBackColor = Color.FromArgb(255, 245, 245, 245);
                this.selectedItemTextColor = Color.FromArgb(255, 5, 5, 5);
                this.disabledItemTextColor = Color.FromArgb(255, 150, 150, 150);
            }
            /// <summary>
            /// Creates a new menu item with custom colors.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            /// <param name="_defaultItemBackColor">The backcolor of the non-selected item. (Default ARGB: A:170, R:10, G:10, B:10)</param>
            /// <param name="_defaultItemTextColor">The textcolor of the non-selected item. (Default ARGB: A:255, R:255, G:255, B:255)</param>
            /// <param name="_selectedItemBackColor">The backcolor of the selected item. (Default ARGB: A:255, R:245, G:245, B:245)</param>
            /// <param name="_selectedItemTextColor">The textcolor of the selected item. (Default ARGB: A:255, R:5, G:5, B:5)</param>
            /// <param name="_disabledItemTextColor">The textcolor of the disabled item. (Default ARGB: A:255, R:150, G:150, B:150)</param>
            public UIMenuItem(string name, string text, string description, bool enabled, Color _defaultItemBackColor, Color _defaultItemTextColor, Color _selectedItemBackColor, Color _selectedItemTextColor, Color _disabledItemTextColor) {
                this.Type = ControlType.UIMenuItem;
                this.IconLocation = iconLocations.Left;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;

                // Default colors
                this.defaultItemBackColor = _defaultItemBackColor;
                this.defaultItemTextColor = _defaultItemTextColor;
                this.selectedItemBackColor = _selectedItemBackColor;
                this.selectedItemTextColor = _selectedItemTextColor;
                this.disabledItemTextColor = _disabledItemTextColor;
            }
            #endregion

            #region Variables
            private float IconOffsetX;
            private float IconOffsetY;
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets if the selected icon should be displayed.
            /// </summary>
            public bool DrawIcon { get; set; }
            /// <summary>
            /// The default icon for this item. (When the item is unselected)
            /// <para>Maximum icon size: 32x32.</para>
            /// <para>Note: If the size of the icon is less then 32x32, then you need to manually adjust the offset.</para>
            /// </summary>
            public Texture DefaultIcon { get; set; }
            /// <summary>
            /// The selected icon for this item. (When the item is selected)
            /// <para>Maximum icon size: 32x32.</para>
            /// <para>Note: If the size of the icon is less then 32x32, then you need to manually adjust the offset.</para>
            /// </summary>
            public Texture SelectedIcon { get; set; }
            /// <summary>
            /// The disabled icon for this item. (When the item is disabled)
            /// <para>Maximum icon size: 32x32.</para>
            /// <para>Note: If the size of the icon is less then 32x32, then you need to manually adjust the offset.</para>
            /// </summary>
            public Texture DisabledIcon { get; set; }
            /// <summary>
            /// The size of your icon(s).
            /// <para>Maximum icon size: 32x32.</para>
            /// </summary>
            public SizeF IconSize { get; set; }
            /// <summary>
            /// The location of the icon. (Default location: Left)
            /// </summary>
            public iconLocations IconLocation { get; set; }
            /// <summary>
            /// Gets or sets the nested menu for this item.
            /// <para>If the user clicks on this item, the nested menu will be displayed.</para>
            /// <para>The nested menu will not display if this item is disabled.</para>
            /// </summary>
            public UIMenu NestedMenu { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Shifts the X and Y coordinates of the icon by specified values.
            /// </summary>
            /// <param name="X">The amount to offset the icon's X coordinate.</param>
            /// <param name="Y">The amount to offset the icon's Y coordinate.</param>
            public void IconOffset(float X, float Y) {
                IconOffsetX = X;
                IconOffsetY = Y;
            }
            #endregion

            #region Events
            /// <summary>
            /// Raises when the user clicks on this item. It doesn't raise when the item is not enabled.
            /// </summary>
            public event IClickEventHandler OnClick;
            #endregion

            #region EventRaisers
            /// <summary>
            /// Generates a OnClick event.
            /// </summary>
            public void PerformClick()
            {
                IClickEventHandler handler = OnClick;
                if (handler != null) { handler(this); }
            }
            #endregion

            #region Drawing
            protected internal void DrawItem(GraphicsEventArgs args, GTA.Font menuFont, int i)
            {
                Size itemTextSize = TextRenderer.MeasureText(Text, menuFont.WindowsFont); // Get text size
                if (IsEnabled) { // Item enabled
                    if (IsSelected) {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);

                        if (DrawIcon) {
                            if (SelectedIcon != null) {
                                switch (IconLocation) {
                                    case iconLocations.Left:
                                        args.Graphics.DrawText(Text, new RectangleF(75f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(SelectedIcon, new RectangleF(35f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                    case iconLocations.Right:
                                        args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(SelectedIcon, new RectangleF(424f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                }
                            }
                            else {
                                args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);
                            }
                        }
                        else {
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);
                        }
                    }
                    else {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);

                        if (DrawIcon) {
                            if (DefaultIcon != null) {
                                switch (IconLocation) {
                                    case iconLocations.Left:
                                        args.Graphics.DrawText(Text, new RectangleF(75f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(DefaultIcon, new RectangleF(35f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                    case iconLocations.Right:
                                        args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(DefaultIcon, new RectangleF(424f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                }
                            }
                            else {
                                args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);
                            }
                        }
                        else {
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);
                        }
                    }
                }
                else { // Item disabled
                    if (IsSelected) {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);

                        if (DrawIcon) {
                            if (DisabledIcon != null) {
                                switch (IconLocation) {
                                    case iconLocations.Left:
                                        args.Graphics.DrawText(Text, new RectangleF(75f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(DisabledIcon, new RectangleF(35f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                    case iconLocations.Right:
                                        args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(DisabledIcon, new RectangleF(424f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                }
                            }
                            else {
                                args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                            }
                        }
                        else {
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                        }
                    }
                    else {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);

                        if (DrawIcon) {
                            if (DisabledIcon != null) {
                                switch (IconLocation) {
                                    case iconLocations.Left:
                                        args.Graphics.DrawText(Text, new RectangleF(75f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(DisabledIcon, new RectangleF(35f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                    case iconLocations.Right:
                                        args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 385f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                                        args.Graphics.DrawSprite(DisabledIcon, new RectangleF(424f + IconOffsetX, 155f + IconOffsetY + i * 38, IconSize.Width, IconSize.Height));
                                        break;
                                }
                            }
                            else {
                                args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                            }
                        }
                        else {
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Default menu item with text and a checkbox.
        /// </summary>
        public class UIMenuCheckboxItem : BaseElement
        {
            #region Constructor
            /// <summary>
            /// Creates a new checkbox item with the default colors.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            /// <param name="_isChecked">Sets if the checkbox should be checked from the beginning.</param>
            public UIMenuCheckboxItem(string name, string text, string description, bool enabled, bool _isChecked)
            {
                this.Type = ControlType.UIMenuCheckboxItem;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;
                this.IsChecked = _isChecked;

                // Default colors
                this.defaultItemBackColor = Color.FromArgb(170, 10, 10, 10);
                this.defaultItemTextColor = Color.FromArgb(255, 255, 255, 255);
                this.selectedItemBackColor = Color.FromArgb(255, 245, 245, 245);
                this.selectedItemTextColor = Color.FromArgb(255, 5, 5, 5);
                this.disabledItemTextColor = Color.FromArgb(255, 150, 150, 150);
            }
            /// <summary>
            /// Creates a new checkbox item wth custom colors and custom sprites.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            /// <param name="_isChecked">Sets if the checkbox should be checked from the beginning.</param>
            /// <param name="_defaultItemBackColor">The backcolor of the non-selected item. (Default ARGB: A:170, R:10, G:10, B:10)</param>
            /// <param name="_defaultItemTextColor">The textcolor of the non-selected item. (Default ARGB: A:255, R:255, G:255, B:255)</param>
            /// <param name="_selectedItemBackColor">The backcolor of the selected item. (Default ARGB: A:255, R:245, G:245, B:245)</param>
            /// <param name="_selectedItemTextColor">The textcolor of the selected item. (Default ARGB: A:255, R:5, G:5, B:5)</param>
            /// <param name="_disabledItemTextColor">The textcolor of the disabled item. (Default ARGB: A:255, R:150, G:150, B:150)</param>
            public UIMenuCheckboxItem(string name, string text, string description, bool enabled, bool _isChecked, Color _defaultItemBackColor, Color _defaultItemTextColor, Color _selectedItemBackColor, Color _selectedItemTextColor, Color _disabledItemTextColor)
            {
                this.Type = ControlType.UIMenuCheckboxItem;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;
                this.IsChecked = _isChecked;

                // Default colors
                this.defaultItemBackColor = _defaultItemBackColor;
                this.defaultItemTextColor = _defaultItemTextColor;
                this.selectedItemBackColor = _selectedItemBackColor;
                this.selectedItemTextColor = _selectedItemTextColor;
                this.disabledItemTextColor = _disabledItemTextColor;
            }
            #endregion

            #region Properties
            private bool _isChecked;
            /// <summary>
            /// Gets or sets if the item is checked.
            /// </summary>
            public bool IsChecked
            {
                get { return _isChecked; }
                set {
                    _isChecked = value;

                    ICheckedChangedEventHandler handler = OnCheckedChanged;
                    if (handler != null) { handler(this, _isChecked); }
                }
            }
            #endregion

            #region Events
            /// <summary>
            /// Raises when the check state changes. It doesn't raise when the item is not enabled.
            /// </summary>
            public event ICheckedChangedEventHandler OnCheckedChanged;
            #endregion

            #region Drawing
            protected internal void DrawItem(GraphicsEventArgs args, GTA.Font menuFont, int i, Texture checkboxCheckedSelected, Texture checkboxUncheckedSelected, Texture checkboxCheckedUnselected, Texture checkboxUncheckedUnselected, Texture checkboxCheckedDisabled, Texture checkboxUncheckedDisabled)
            {
                Size chkItemTextSize = TextRenderer.MeasureText(Text, menuFont.WindowsFont); // Get text size
                if (IsEnabled) { // Item enabled
                    if (IsSelected) {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);
                        args.Graphics.DrawText(Text, new RectangleF(40f, 157 + i * 38, 380f, chkItemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);

                        if (IsChecked) {
                            args.Graphics.DrawSprite(checkboxCheckedSelected, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                        else {
                            args.Graphics.DrawSprite(checkboxUncheckedSelected, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                    }
                    else {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);
                        args.Graphics.DrawText(Text, new RectangleF(40f, 157 + i * 38, 380f, chkItemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);

                        if (IsChecked) {
                            args.Graphics.DrawSprite(checkboxCheckedUnselected, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                        else {
                            args.Graphics.DrawSprite(checkboxUncheckedUnselected, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                    }
                }
                else { // Item disabled
                    if (IsSelected) {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);
                        args.Graphics.DrawText(Text, new RectangleF(40f, 157 + i * 38, 380f, chkItemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);

                        if (IsChecked) {
                            args.Graphics.DrawSprite(checkboxCheckedDisabled, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                        else {
                            args.Graphics.DrawSprite(checkboxUncheckedDisabled, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                    }
                    else {
                        args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);
                        args.Graphics.DrawText(Text, new RectangleF(40f, 157 + i * 38, 380f, chkItemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);

                        if (IsChecked) {
                            args.Graphics.DrawSprite(checkboxCheckedDisabled, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                        else {
                            args.Graphics.DrawSprite(checkboxUncheckedDisabled, new RectangleF(431f, 161.5f + i * 38, 16f, 16f));
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Item with text and a listbox with items in it.
        /// </summary>
        public class UIMenuListItem : BaseElement
        {
            #region Constructor
            /// <summary>
            /// Creates a new list item with the default colors.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            public UIMenuListItem(string name, string text, string description, bool enabled)
            {
                this.Type = ControlType.UIMenuListItem;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;

                // Default colors
                this.defaultItemBackColor = Color.FromArgb(170, 10, 10, 10);
                this.defaultItemTextColor = Color.FromArgb(255, 255, 255, 255);
                this.selectedItemBackColor = Color.FromArgb(255, 245, 245, 245);
                this.selectedItemTextColor = Color.FromArgb(255, 5, 5, 5);
                this.disabledItemTextColor = Color.FromArgb(255, 150, 150, 150);
            }
            /// <summary>
            /// Creates a new list item with the default colors and a custom list of items.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            /// <param name="items">Your custom list of items.</param>
            public UIMenuListItem(string name, string text, string description, bool enabled, List<string> items)
            {
                this.Type = ControlType.UIMenuListItem;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;
                this.itemList = items;

                // Default colors
                this.defaultItemBackColor = Color.FromArgb(170, 10, 10, 10);
                this.defaultItemTextColor = Color.FromArgb(255, 255, 255, 255);
                this.selectedItemBackColor = Color.FromArgb(255, 245, 245, 245);
                this.selectedItemTextColor = Color.FromArgb(255, 5, 5, 5);
                this.disabledItemTextColor = Color.FromArgb(255, 150, 150, 150);
            }

            /// <summary>
            /// Creates a new list item with the custom colors.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            /// <param name="_defaultItemBackColor">The backcolor of the non-selected item. (Default ARGB: A:170, R:10, G:10, B:10)</param>
            /// <param name="_defaultItemTextColor">The textcolor of the non-selected item. (Default ARGB: A:255, R:255, G:255, B:255)</param>
            /// <param name="_selectedItemBackColor">The backcolor of the selected item. (Default ARGB: A:255, R:245, G:245, B:245)</param>
            /// <param name="_selectedItemTextColor">The textcolor of the selected item. (Default ARGB: A:255, R:5, G:5, B:5)</param>
            /// <param name="_disabledItemTextColor">The textcolor of the disabled item. (Default ARGB: A:255, R:150, G:150, B:150)</param>
            public UIMenuListItem(string name, string text, string description, bool enabled, Color _defaultItemBackColor, Color _defaultItemTextColor, Color _selectedItemBackColor, Color _selectedItemTextColor, Color _disabledItemTextColor)
            {
                this.Type = ControlType.UIMenuListItem;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;

                // Default colors
                this.defaultItemBackColor = _defaultItemBackColor;
                this.defaultItemTextColor = _defaultItemTextColor;
                this.selectedItemBackColor = _selectedItemBackColor;
                this.selectedItemTextColor = _selectedItemTextColor;
                this.disabledItemTextColor = _disabledItemTextColor;
            }
            /// <summary>
            /// Creates a new list item with the custom colors and a custom list of items.
            /// </summary>
            /// <param name="name">The name of this item.</param>
            /// <param name="text">The text of this item.</param>
            /// <param name="description">The description of this item.</param>
            /// <param name="enabled">Sets if the item should be enabled from the beginning, or not.</param>
            /// <param name="items">Your custom list of items.</param>
            /// <param name="_defaultItemBackColor">The backcolor of the non-selected item. (Default ARGB: A:170, R:10, G:10, B:10)</param>
            /// <param name="_defaultItemTextColor">The textcolor of the non-selected item. (Default ARGB: A:255, R:255, G:255, B:255)</param>
            /// <param name="_selectedItemBackColor">The backcolor of the selected item. (Default ARGB: A:255, R:245, G:245, B:245)</param>
            /// <param name="_selectedItemTextColor">The textcolor of the selected item. (Default ARGB: A:255, R:5, G:5, B:5)</param>
            /// <param name="_disabledItemTextColor">The textcolor of the disabled item. (Default ARGB: A:255, R:150, G:150, B:150)</param>
            public UIMenuListItem(string name, string text, string description, bool enabled, List<string> items, Color _defaultItemBackColor, Color _defaultItemTextColor, Color _selectedItemBackColor, Color _selectedItemTextColor, Color _disabledItemTextColor)
            {
                this.Type = ControlType.UIMenuListItem;
                this.Name = name;
                this.Text = text;
                this.Description = description;
                this.IsEnabled = enabled;
                this.itemList = items;

                // Default colors
                this.defaultItemBackColor = _defaultItemBackColor;
                this.defaultItemTextColor = _defaultItemTextColor;
                this.selectedItemBackColor = _selectedItemBackColor;
                this.selectedItemTextColor = _selectedItemTextColor;
                this.disabledItemTextColor = _disabledItemTextColor;
            }
            #endregion

            #region Variables
            protected internal IList<string> itemList = new List<string>();
            protected internal int currentIndex;
            private float _maximumListBoxWidth = 200f;
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets the selected index of the list.
            /// </summary>
            public int SelectedIndex {
                get { return currentIndex; }
                set { currentIndex = value; }
            }
            /// <summary>
            /// Gets the selected text from the current index.
            /// </summary>
            public string SelectedText {
                get { return itemList[currentIndex]; }
            }
            /// <summary>
            /// Gets the amount of items in the list.
            /// </summary>
            public int ItemAmount {
                get { return itemList.Count; }
            }

            /// <summary>
            /// Gets or sets the maximum width of the listbox. (Default value: 200)
            /// </summary>
            public float MaximumListBoxWidth {
                get { return _maximumListBoxWidth; }
                set { _maximumListBoxWidth = value; }
            }
            #endregion

            #region Collection Methods
            // Add
            /// <summary>
            /// Adds an items to the list of items.
            /// </summary>
            /// <param name="item">The item that should be added to the list of items.</param>
            public void AddItem(string item)
            {
                itemList.Add(item);
            }
            /// <summary>
            /// Adds an array of items to the list of items..
            /// </summary>
            /// <param name="items">The array of items that should be added to the list of items.</param>
            public void AddItems(string[] items)
            {
                try {
                    for (int i = 0; i < items.Length; i++) {
                        itemList.Add(items[i]);
                    }
                }
                catch (Exception ex) {
                    Game.Console.Print("NativeUI - Error while adding items to the list of items. Details: " + ex.Message);
                }
            }

            // Remove
            /// <summary>
            /// This removes the specific item from the list of items through it's name.
            /// </summary>
            /// <param name="item">The item that should be removed</param>
            /// <returns>Returns true if the item was deleted, otherwise false.</returns>
            public bool RemoveItem(string item)
            {
                try {
                    for (int i = 0; i < itemList.Count; i++) {
                        if (itemList[i] == item) {
                            itemList.RemoveAt(i);
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex) {
                    Game.Console.Print("NativeUI - Error while removing an item from the list through it's name. Details: " + ex.Message);
                    return false;
                }
            }
            /// <summary>
            /// This removes the specific item from the list of items through it's index.
            /// </summary>
            /// <param name="index">The index where the item is at</param>
            /// <returns>Returns true if the item was deleted, otherwise false.</returns>
            public bool RemoveItem(int index)
            {
                try {
                    for (int i = 0; i < itemList.Count; i++) {
                        if (i == index) {
                            itemList.RemoveAt(i);
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
            /// This removes all items from the list of items.
            /// </summary>
            public void RemoveAllItems()
            {
                itemList.Clear();
            }
            #endregion

            #region Drawing
            protected internal void DrawItem(GraphicsEventArgs args, GTA.Font menuFont, int i, Texture arrowLeft, Texture arrowRight)
            {
                Size itemTextSize = TextRenderer.MeasureText(Text, menuFont.WindowsFont); // Get text size
                if (IsEnabled) { // Item enabled
                    if (IsSelected) {
                        if (itemList.Count != 0) {
                            Size itemSize = TextRenderer.MeasureText(itemList[SelectedIndex], menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (SelectedText.Length * 2) - (SelectedText.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 365f - itemWidth, itemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);

                            args.Graphics.DrawText(itemList[SelectedIndex], new RectangleF(433f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Center, selectedItemTextColor, menuFont);
                            args.Graphics.DrawSprite(arrowLeft, new RectangleF(420f - (itemWidth + 4), 162.5f + i * 38, 11f, 16f));
                            args.Graphics.DrawSprite(arrowRight, new RectangleF(436f, 162.5f + i * 38, 11f, 16f));
                        }
                        else {
                            string minus = "-";
                            Size itemSize = TextRenderer.MeasureText(minus, menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (minus.Length * 2) - (minus.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 365f - itemWidth, itemTextSize.Height), TextAlignment.Left, selectedItemTextColor, menuFont);

                            args.Graphics.DrawText(minus, new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, selectedItemTextColor, menuFont);
                        }
                    }
                    else {
                        if (itemList.Count != 0) {
                            Size itemSize = TextRenderer.MeasureText(itemList[SelectedIndex], menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (SelectedText.Length * 2) - (SelectedText.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 365f - itemWidth, itemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);

                            args.Graphics.DrawText(itemList[SelectedIndex], new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, defaultItemTextColor, menuFont);
                        }
                        else {
                            string minus = "-";
                            Size itemSize = TextRenderer.MeasureText(minus, menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (minus.Length * 2) - (minus.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 365f - itemWidth, itemTextSize.Height), TextAlignment.Left, defaultItemTextColor, menuFont);

                            args.Graphics.DrawText(minus, new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, defaultItemTextColor, menuFont);
                        }
                    }
                }
                else { // Item disabled
                    if (IsSelected) {
                        if (itemList.Count != 0) {
                            Size itemSize = TextRenderer.MeasureText(itemList[SelectedIndex], menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (SelectedText.Length * 2) - (SelectedText.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);

                            args.Graphics.DrawText(itemList[SelectedIndex], new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, disabledItemTextColor, menuFont);
                        }
                        else {
                            string minus = "-";
                            Size itemSize = TextRenderer.MeasureText(minus, menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (minus.Length * 2) - (minus.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, selectedItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 365f - itemWidth, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);

                            args.Graphics.DrawText(minus, new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, disabledItemTextColor, menuFont);
                        }
                    }
                    else {
                        if (itemList.Count != 0)  {
                            Size itemSize = TextRenderer.MeasureText(itemList[SelectedIndex], menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (SelectedText.Length * 2) - (SelectedText.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 420f, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);

                            args.Graphics.DrawText(itemList[SelectedIndex], new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, disabledItemTextColor, menuFont);
                        }
                        else {
                            string minus = "-";
                            Size itemSize = TextRenderer.MeasureText(minus, menuFont.WindowsFont);
                            float itemWidth = Math.Min(_maximumListBoxWidth, itemSize.Width - (minus.Length * 2) - (minus.Length == 0 ? 0 : 10f));

                            args.Graphics.DrawRectangle(245f, 171f + i * 38, 432f, 38f, defaultItemBackColor);
                            args.Graphics.DrawText(Text, new RectangleF(40f, 157f + i * 38, 365f - itemWidth, itemTextSize.Height), TextAlignment.Left, disabledItemTextColor, menuFont);

                            args.Graphics.DrawText(minus, new RectangleF(445f - itemWidth, 157.5f + i * 38, itemWidth, itemSize.Height), TextAlignment.Right, disabledItemTextColor, menuFont);
                        }
                    }
                }
            }
            #endregion

            #region Events
            /// <summary>
            /// Raises when the user clicks on this item. It doesn't raise when the item is not enabled.
            /// </summary>
            public event IClickEventHandler OnClick;
            /// <summary>
            /// Raises when the selected index of the list changes.
            /// </summary>
            public event ISelectedIndexChangedEventHandler OnSelectedIndexChanged;
            /// <summary>
            /// Raises when the selected text of this list changes.
            /// </summary>
            public event ISelectedTextChangedEventHandler OnSelectedTextChanged;
            #endregion

            #region EventRaisers
            /// <summary>
            /// Generates a OnClick event.
            /// </summary>
            public void PerformClick()
            {
                IClickEventHandler handler = OnClick;
                if (handler != null) { handler(this); }
            }

            protected internal void SelectedTextChangedRaiser(string s)
            {
                ISelectedTextChangedEventHandler handler = OnSelectedTextChanged;
                if (handler != null) { handler(this, s); } // RaiseEvent
            }
            protected internal void SelectedIndexChangedRaiser(int i)
            {
                ISelectedIndexChangedEventHandler handler = OnSelectedIndexChanged;
                if (handler != null) { handler(this, i); } // RaiseEvent
            }
            #endregion
        }

        // <summary>
        // Item with text and a slider.
        // </summary>
        //public class UIMenuSliderItem : BaseElement
        //{
        //    #region Constructor

        //    #endregion

        //    #region Properties

        //    /// <summary>
        //    /// The maximum value of this slider.
        //    /// </summary>
        //    public int Maximum { get; set; }

        //    /// <summary>
        //    /// The minimum value of this slider.
        //    /// </summary>
        //    public int Minimum { get; set; }

        //    /// <summary>
        //    /// The current value of this slider.
        //    /// </summary>
        //    public int Value { get; set; }

        //    #endregion
        //}

    }
}
