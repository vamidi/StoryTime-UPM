<a name="2020.1.7f1"></a>
## [2020.1.6f3](https://github.com/vamidi/storytime/compare/v2020.1.0b1...v2020.1.6f3) (2021-05-11)

### Breaking changes
- DatabaseSyncModule now only fetches the data from the database. Whereas the TableData now process the data.
- TableSO added to generate overview of the json data.

### Features
- First version of the Day and night cycle added.
- Addressables added.
- 


<a name="2020.1.6f3"></a>
## [2020.1.6f3](https://github.com/vamidi/storytime/compare/v2020.1.0b1...v2020.1.6f3) (2021-05-11)

### Breaking Changes
- Namespaces changed from `DatabaseSync` to `StoryTime`.

<a name="2020.1.6f1"></a>
## [2020.1.6f1](https://github.com/vamidi/storytime/compare/v2020.1.0b1...v2020.1.6f1) (2021-22-10)

### Breaking Changes
- Minimal required `com.unity.localization` version is 1.0.9.

### Features

- Stats have been added to the StoryTime package.
  - You are now able to load character class data.
  - You are now also able to load skills, equipments and stats.
- Stat manager has been added to keep track of the players/enemies stats.

<a name="2020.1.3f1"></a>
# [2020.1.3f1](https://github.com/akveo/nebular/compare/v2020.1.0b1...v2020.1.3f1) (2021-18-01)

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
