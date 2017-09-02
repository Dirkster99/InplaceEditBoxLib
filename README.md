# InplaceEditBoxLib
WPF/MVVM control to implement a textbox on top of other elements like a
TreeViewItem or ListViewItem (use case: perform in place edit of a displayed item)

<img src"https://github.com/Dirkster99/InplaceEditBoxLib/blob/master/00_Docu/screenshot.png" width="600"/>

Check thiming here:
- [FileExplorer in Edi](https://github.com/Dirkster99/Edi/wiki/File-Explorer-Tool-Window)

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
* Minimum and Maximum length of a name should between 1 - 254 Characters (nameing item with empty string '' should invoke a pop-up message)



## Theming

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
