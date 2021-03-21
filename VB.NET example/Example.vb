' NativeUI IV VB.NET example script for version 0.7.

Option Strict On
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports GTA
Imports NativeUI

Public Class Example : Inherits Script

#Region "Other"
    Private testGif As Image
#End Region

#Region "Native UI"
    Private uiMenu1 As UIMenu
    Private uiMenu2 As UIMenu

    ' Items
    Private uiMenuItem1 As UI.UIMenuItem
    Private uiMenuItem2 As UI.UIMenuItem
    Private uiMenuItem3 As UI.UIMenuItem
    Private uiMenuItemWithImage4 As UI.UIMenuItem
    Private uiMenuItem5 As UI.UIMenuItem

    Private uIMenuCheckBoxItem1 As UI.UIMenuCheckboxItem
    Private uIMenuCheckBoxItem2 As UI.UIMenuCheckboxItem
    Private uIMenuCheckBoxItem3 As UI.UIMenuCheckboxItem
    Private uIMenuCheckBoxItem4 As UI.UIMenuCheckboxItem

    Private uiListItem1 As UI.UIMenuListItem
#End Region

    Public Sub Example()
        ' Load test gif - Replace path with your gif file.
        testGif = Image.FromFile(Game.InstallFolder + "\\scripts\\NativeUITest\\testGif.gif")

        ' Set settings for all menus
        UIMenu.Options.UpKey = Keys.Up
        UIMenu.Options.DownKey = Keys.Down
        UIMenu.Options.LeftKey = Keys.Left
        UIMenu.Options.RightKey = Keys.Right
        UIMenu.Options.AcceptKey = Keys.Enter
        UIMenu.Options.disablePhoneWhenMenuIsOpened = True
        UIMenu.Options.enableControllerSupport = True
        UIMenu.Options.enableMenuSounds = True
        UIMenu.Options.AnimatedBannerFrameRate = 120

#Region "Menu1"
        ' Create a New menu
        uiMenu1 = New UIMenu("NativeUI IV", "NATIVEUI IV SHOWCASE", testGif)
        uiMenu2 = New UIMenu("NestedMenu", "Only a test menu.")

        ' Create default items
        uiMenuItem1 = New UI.UIMenuItem("TestItem1", "I'm a button", "You can click on me!", True)
        AddHandler uiMenuItem1.OnClick, AddressOf uiMenuItem1_Click ' If item gets pressed.
        uiMenuItem2 = New UI.UIMenuItem("TestItem2", "I'm a disabled button", "You can't click on me... :(", False)
        AddHandler uiMenuItem2.OnClick, AddressOf uiMenuItem2_Click ' If item gets pressed.
        uiMenuItem3 = New UI.UIMenuItem("TestItem3", "Showcase button", "This is a long text. But wait, it can get even longer! It doesn't stop getting longer! Send help please! Quick! ...", True)

        ' Create default item with an icon
        uiMenuItemWithImage4 = New UI.UIMenuItem("TestItem4", "More information...", "This is a default menu item just with an icon!", True)
        uiMenuItemWithImage4.DefaultIcon = New Texture(My.Resources.InfoSymbolDefault) ' The icon that shows If the item Is Not selected.
        uiMenuItemWithImage4.SelectedIcon = New Texture(My.Resources.InfoSymbolSelected) ' The icon that shows If the item Is selected.
        uiMenuItemWithImage4.DisabledIcon = New Texture(My.Resources.InfoSymbolDisabled) ' The icon that shows If the item Is disabled.
        uiMenuItemWithImage4.IconLocation = UI.UIMenuItem.iconLocations.Right ' Location Of your item (Left Is Default)
        uiMenuItemWithImage4.IconSize = New SizeF(32.0F, 32.0F) ' The size Of your icon. This Is very important! (Maximum recommended size For your icon(s): 32x32)
        ' uiMenuItemWithImage4.IconOffset(0f, 0f) ' If your icon size Is Not 32x32 then you might have to adjust the offset a bit.
        uiMenuItemWithImage4.DrawIcon = True ' Allows the item To draw your icon

        ' Create default item with a nested menu.
        uiMenuItem5 = New UI.UIMenuItem("TestItem5", "Show another menu...", "This shows the nested menu set for this item.", True)
        uiMenuItem5.NestedMenu = uiMenu2

        ' Create checkbox items
        uIMenuCheckBoxItem1 = New UI.UIMenuCheckboxItem("TestCheckbox1", "I'm a checked checkbox", "You can also uncheck me if you want.", True, True)
        AddHandler uIMenuCheckBoxItem1.OnCheckedChanged, AddressOf uIMenuCheckBoxItem1_OnCheckedChanged ' If checkbox check state changes.
        uIMenuCheckBoxItem2 = New UI.UIMenuCheckboxItem("TestCheckbox2", "I'm a unchecked checkbox", "You can also check me if you want.", True, False)
        uIMenuCheckBoxItem3 = New UI.UIMenuCheckboxItem("TestCheckbox3", "I'm a checked disabled checkbox", "You can't uncheck me!", False, True)
        uIMenuCheckBoxItem4 = New UI.UIMenuCheckboxItem("TestCheckbox3", "I'm a unchecked disabled checkbox", "You can't check me!", False, False)

        ' Create list item
        uiListItem1 = New UI.UIMenuListItem("TestListItem1", "I'm a item with an list!", "Hit left, right to navigate through the list. Hit enter to show get the current selected item.", True)
        AddHandler uiListItem1.OnSelectedIndexChanged, AddressOf uiListItem1_OnSelectedIndexChanged
        AddHandler uiListItem1.OnClick, AddressOf uiListItem1_OnClick
        ' Add items to list
        uiListItem1.AddItem("-")
        uiListItem1.AddItem("Item1")
        uiListItem1.AddItem("Item2")
        uiListItem1.AddItem("And this is item 3")

        ' Add items to the menu
        uiMenu1.AddItem(uiMenuItem1)
        uiMenu1.AddItem(uiMenuItem2)
        uiMenu1.AddItem(uIMenuCheckBoxItem1)
        uiMenu1.AddItem(uIMenuCheckBoxItem2)
        uiMenu1.AddItem(uIMenuCheckBoxItem3)
        uiMenu1.AddItem(uIMenuCheckBoxItem4)
        uiMenu1.AddItem(uiMenuItem3)
        uiMenu1.AddItem(uiListItem1)
        uiMenu1.AddItem(uiMenuItemWithImage4)
        uiMenu1.AddItem(uiMenuItem5)
#End Region

        ' Scripts stuff...
        Me.Interval = 100
        AddHandler Me.Tick, AddressOf Example_Tick
        AddHandler Me.PerFrameDrawing, AddressOf Example_PerFrameDrawing
        AddHandler Me.KeyDown, AddressOf Example_KeyDown
    End Sub

    Private Sub uiListItem1_OnSelectedIndexChanged(sender As UI.BaseElement, newIndex As Integer)
        Game.DisplayText("The new selected index is: " + newIndex.ToString())
    End Sub
    Private Sub uiListItem1_OnClick(sender As UI.BaseElement)
        Game.DisplayText("You clicked on item: " + sender.Name + " the selected item is: " + uiListItem1.SelectedText, 1500)
    End Sub

    Private Sub uIMenuCheckBoxItem1_OnCheckedChanged(sender As UI.BaseElement, newValue As Boolean)
        Game.DisplayText("CheckboxItem1 checked changed. New value = " + newValue.ToString(), 1500)
    End Sub

    Private Sub uiMenuItem2_Click(sender As UI.BaseElement)
        Game.DisplayText("'uiMenuItem2' is disabled so this should not show up ingame.", 1500)
    End Sub
    Private Sub uiMenuItem1_Click(sender As UI.BaseElement)
        Game.DisplayText("You clicked on button: " + sender.Name, 1500)
    End Sub


    Private Sub Example_Tick(sender As Object, e As EventArgs)
        UIMenu.ProcessController() ' Allows you to use a controller to navigate through the menu. (Make sure that 'enableControllerSupport' is enabled)
    End Sub
    Private Sub Example_PerFrameDrawing(sender As Object, e As GraphicsEventArgs)
        UIMenu.ProcessDrawing(e) ' Very important! This is used to draw the hole menu.
    End Sub
    Private Sub Example_KeyDown(sender As Object, e As GTA.KeyEventArgs)
        UIMenu.ProcessKeyPress(e) ' Also very important! This Is used To navigate through the menu With a keyboard.
        If e.Key = Keys.G Then ' If Key 'G' is pressed, open or close the menu.
            If UIMenu.IsAnyMenuOpen() Then ' Check Then If any Then Menu Is opened right now.
                UIMenu.HideAllMenus() ' If True Then hide all menus.
            Else
                UIMenu.Show(uiMenu1) ' If False Then show main menu.
            End If
        End If
    End Sub

End Class
