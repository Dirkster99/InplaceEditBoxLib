# InplaceEditBoxLib
WPF/MVVM control to implement a textbox on top of other elements like a
TreeViewItem or ListViewItem (use case: perform in place edit of a displayed item)

<p><img src="https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/screenshot.png" align="right" width="500" ></p>
<br/><br/><br/>

[![Build status](https://ci.appveyor.com/api/projects/status/7g6bx6uku9e1qow8?svg=true)](https://ci.appveyor.com/project/Dirkster99/inplaceeditboxlib)

# Use Case: Edit-In-Place

The edit-in-place text control contained in this project can be used as a base for developing applications where users would like to edit text strings as overlay over the normally displayed string.

The best and well known example of an edit-in-place text control is the textbox overlay that is used for renaming renaming a file or folder in Windows Explorer. The user typically selects an item in a list (listbox, listview, grid) or structure of items (treeview) and renames the item using a textbox overlay (without an additional dialog).

Change of focus (activation of a different window), pressing escapee leads to canceling of the rename process and pressing enter leads to confirmation of the new string.

# Editing with Text Overlay
Here is a sequence of screenshots that shows the normal steps when renaming an item with an overlay TextBox control:
![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/EditOverlay/Step1.png)

Press F2 to start renaming
![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/EditOverlay/Step2.png)

Type a different sequence of characters
![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/EditOverlay/Step3.png)

Press enter to confirm the new name
![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/EditOverlay/Step4_Result.png)

## Features ##

This edit-in-place control in this project can be used in the collection of any **ItemsControl** (Treeview, ListBox, ListView etc).

Find more details in CodeProject:
https://www.codeproject.com/Articles/802385/A-WPF-MVVM-In-Place-Edit-TextBox-Control

# Demo in this Repository
The demo program shows how the control can be used in a treeview with:

* keybinding - Press F2 to rename - Press ESC to cancel renaming

* Context Menu - Click Rename in context Menu to rename an item
* Double Click - Double click the text portion to start renaming

and Handling Errors, such as:

* Renaming with an invalid character (Press ? in Edit Mode to see a pop-up message)
* Attempting to name 2 items with the same name (Name 2 items 'a' should invoke a pop-up message on the 2nd items rename)
* Minimum and Maximum length of a name should between 1 - 254 Characters (naming item with empty string '' should invoke a pop-up message)

### Editing text with Text and DisplayText properties ###

The edit-in-place control has 2 string properties, one is for display (**DisplayText**) and the other (**Text**) string represents the value that should be edited.

This setup enables application developers to show more than just a name in each item. Each item can, for example, display a name and a number by using the **DisplayText** property, while the **Text** property should contain the string that is to be edit.

The confirmation of editing does not change either of the above dependency properties. The edit-in-place control executes instead the command that is bound to the **RenameCommand** dependency property to let the viewmodel adjust all relevant strings.

The view invokes the bound **RenameCommand** and passes the **RenameCommandParameter** as parameter along. 

```XML
<EditInPlace:EditBox Text="{Binding Path=DisplayName, Mode=OneWay, UpdateSourceTrigger=PropertyChanged}"
  DisplayText="{Binding Path=DisplayName,StringFormat={}{0} (File), Mode=OneWay, UpdateSourceTrigger=PropertyChanged}"
 ToolTip="{Binding Description, Mode=OneWay, UpdateSourceTrigger=PropertyChanged}"
  Focusable="True"

  VerticalAlignment="Stretch"
  HorizontalAlignment="Left"
  IsReadOnly="{Binding IsItemReadOnly}"
  RenameCommand="{Binding Path=Data.RenameCommand, Source={StaticResource DataContextProxy}}"
  RenameCommandParameter="{Binding}"
  ToolTipService.ShowOnDisabled="True"
      
  Margin="2,0" />
```
The actual renaming (changing the data structure and checking for quality issues, such as, minimal length of string, is then performed by the code invoked in the viewmodel. The viewmodel can then choose to show an error notification and refuse the renaming or perform the renaming and close the process (see Demo in [SolutionViewModel.cs](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/source/SolutionLib/ViewModels/Browser/SolutionViewModel.cs)).

```C++
// Do we already know this item?
if (string.IsNullOrEmpty(newName) == true ||
  newName.Length < 1 || newName.Length > 254)
{
    solutionItem.RequestEditMode(RequestEditEvent.StartEditMode);
    solutionItem.ShowNotification("Invalid legth of name",
        "A name must be between 1 and 254 characters long.");
    return;
}

var parent = solutionItem.Parent;

if (parent != null)
{
    // Do we already know this item?
    var existingItem = parent.FindChild(newName);
    if (existingItem != null)
    {
        solutionItem.RequestEditMode(RequestEditEvent.StartEditMode);
        solutionItem.ShowNotification("Item Already Exists",
            "An item with this name exists already. All names must be unique.");
        return;
    }

    parent.RenameChild(solutionItem, newName);

    // This parent selection + sort + child selection
    // scrolls the renamed item into view...
    parent.IsItemSelected = true;
    parent.IsItemExpanded = true;   // Ensure parent is expanded
    parent.SortChildren();
    solutionItem.IsItemSelected = true;
...
}
```

### Initiate Editing Text from the ViewModel ###

The edit-in-place control expects the viewmodel to implement the **InplaceEditBoxLib.Interfaces.IEditBox** interface which contains a **RequestEdit** event. This event can be fired by the viewmodel to start editing of a given item.

### Initiate Editing Text from the View ###

Editing text from the view can be done directly by 'double click' on the text or via command binding on the viewmodel which invokres the **RequestEdit** event mentioned above.

See demo project with:
* Rename context menu item or
* F2 Key binding

### Usage of Limited Space ###

The EditBox in-place overlay control should not exceed the view port area of the parent scrollviewer of the items control. That is, the EditBox should not exceed the visible area of a treeview if it was used within a treeview. This rule ensure that users do not end up typing in an invisible area (off-screen) when typing long string in small areas.

The following sequence of images shows the application behavior when the user enters the string 'The quick fox jumps over the river' in a limited space scenario:

![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/SpaceLimits/Step1.png) ![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/SpaceLimits/Step2.png)
![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/SpaceLimits/Step1.png) ![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/SpaceLimits/Step4.png)
 ![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/SpaceLimits/Step5_Result.png)

### Cancel and Confirm ###

Editing text with the edit-in-place control can be canceled by pressing the 'Esc' key or changing the input focus to another windows or control. The application shows the text as it was before the editing started.

Editing text can be confirmed pressing the enter key. The application shows the entered text instead of the text before the editing started.

### IsReadOnly property ###

The edit-in-place control supports a Boolean **IsReadonly** dependency property to lock individual items from being renamed. Default is **false** meaning every item is editable unless binding defines somtheing else.

### IsEditableOnDoubleClick ###

Editing the string that is displayed with the edit-in-place control can be triggered with a time 'double click'.
This double click can be configured to occur in a certain time frame. There are 2 double dependency properties that can be setup to consume only those double clicks with a time frame that is larger than **MinimumClickTime** but smaller than **MaximumClickTime**.

Default values for **MinimumClickTime** and **MaximumClickTime** are 300 ms and 700 ms, respectively.

The **IsEditableOnDoubleClick** boolean dependency property can be setup to dermine whether double clicks are evaluated for editing or not. Default is true.

### IsEditing property ###

The edit-in-place control supports a **one way** Boolean **IsEditing** dependency property to enable viewmodels to determine whether an item is currently edited or not. This property cannot be used by the viewmodel to force the view into editable mode (since it is a get only property in the view). Use the **RequestEdit** event defined in **InplaceEditBoxLib.Interfaces.IEditBox** to request an edit mode that is initialized by the viewmodel.

### Key Filter and Error Handling ###

The EditBox control contains properties that can be used to define a blacklist of characters that should not be input by the user. See properties:

- **InvalidInputCharacters**
- **InvalidInputCharactersMessage**
- **InvalidInputCharactersTitle**

The control implements a pop-up message element to show hints to the user if he types invalid characters.

![](https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/00_Docu/ErrorHandling/PopUpMessage.png?raw=true)

## Known Limitations ##

- Clicking on the background of the ItemsControl (TreeView, ListView etc) does not cancel the edit mode (I would like to implement this but do not have a solution).

- Key definitions entered in the in-place textbox cannot be defined through a white-list. The textbox does not support input masks.

- Restyling TextBox with Hyperlink does not work since a Hyperlink is stored in the InlineCollection of a TextBox. But an InlineCollection cannot be set via dependency property and I cannot seem to work around this with a custom dependency property.

# Credits #

- Thanks to Joseph Leung for coaching me along the way

- This code uses part of ATC Avalon Team's work:
http://blogs.msdn.com/atc_avalon_team/archive/2006/03/14/550934.aspx

- CodeProject Article "Editable TextBlock in WPF for In-place Editing"
http://www.codeproject.com/Articles/31592/Editable-TextBlock-in-WPF-for-In-place-Editing?fid=1532208&df=90&mpp=25&noise=3&prof=False&sort=Position&view=Normal&spc=Relaxed&fr=26#xx0xx

## Theming

Check theming here:
- [FileExplorer in Edi](https://github.com/Dirkster99/Edi/wiki/File-Explorer-Tool-Window)
- [MLib](https://github.com/Dirkster99/MLib)

Load *Light* or *Dark* brush resources in you resource dictionary to take advantage of existing definitions.

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/InplaceEditBoxLib;component/Themes/DarkBrushes.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

```XAML
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/InplaceEditBoxLib;component/Themes/LightBrushes.xaml" />
    </ResourceDictionary.MergedDictionaries>
```

These definitions do not theme all controls used within this library. You should use a standard theming library, such as:
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro),
- [MLib](https://github.com/Dirkster99/MLib), or
- [MUI](https://github.com/firstfloorsoftware/mui)

to also theme standard elements, such as, button and textblock etc.
