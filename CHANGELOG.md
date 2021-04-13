<a name="2020.1.3f1"></a>
# [2020.1.3f1](https://github.com/akveo/nebular/compare/v2020.1.0b1...v2020.1.3f1) (2021-01-18)

### Features
* Dialogues are now handled through the node editor in StoryTime.
* `JsonTableSync` - This class enables the user to enable localization. 
* Inventory manager has been added to the package.
* Cooking/Crafting manager has been added to the package.
* Dialogue supports multiple languages.
* Example UI added for inventory.
* Example UI added for crafting/cooking

### Bug Fixes
* `DatabaseSyncWindow` wasn't showing selected audio clips.

### Code Refactoring
* Tables can now be fetched from the top bar instead of the `DatabaseSyncWindow` Settings window.
* Editor for tables is now able to show linked ids. i.e you want to show the craft ables but the
  item names are in a different table. see [TableBehaviour]() for mor info
  ```c#
  ItemRecipeSO() : base("shopCraftables", "name", "childId", UInt32.MaxValue, "items") { }
  ```

### BREAKING CHANGES
* `TableBinary` - method `GetValue` is now `GetField`.
* Several ScriptableObjects consist now of `localized string` instead of normal `string`.
