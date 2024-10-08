# Changelog
[releases](https://github.com/mettliebe/CS2_TranslateCS2/releases)

## Version 2.0.4.0
- [i286-1](https://github.com/i286-1) thankfully shared some information about XBoxGames
    - the App is not limited to the [STEAM](https://store.steampowered.com)-Version anymore
    - at startup this app tries to detect [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installation location
        - [STEAM](https://store.steampowered.com) location
        - if a manual location already exists within 'TranslateCS2.dll.config', it is also taken into account
    - if [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installation location could not be detected automatically,
        - an information gets displayed
        - afterwards a dialog to manually navigate to [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii) installation location and to select 'Cities2_Data'-directory gets displayed
    - the selected location is going to be saved within 'TranslateCS2.dll.config'
    - and this App gets restarted
- display translation from translator again
    - got set, but not displayed

## Version 2.0.3.0
- use LocName instead of Session-Name
- dont duplicate entries
- validate edit inputs on open
- overwriting .loc-files directly is no longer supported

## Version 2.0.1.0
- export merge-values
- languages
- monospace font for mod-readme and mod-changelog

## Version 2.0
- sessions view
    - edit/create new session
        - the inputs are separated with a separator for better readability
        - localename in english:
            - now has a drop down to select an available locales english name
            - drop down is searchable
            - selecting/changing the english name changes the localename localized respectively
            - below the drop down, the current english name is shown to provide backward-compatibility
        - localename localized
            - now has a drop down to select an available locales native name
            - it is searchable
            - as long as no english name is selected, selecting/changing the native name changes the localename in english respectively
            - the textbox below the drop down shows the current localename localized
                - it also allows you to specify your own name to use it while exporting via the add Key checkbox
                - especially for those who would like to create a slang/dialect
                    - 'Colognian'/'Kölsch' as an example for german
- current session info in ribbon bar
    - added localename in english
    - added localename localized
- export
    - TranslateCS2.Mod-group within ribbon bar
        - now contains two buttons
        - one to display the mods readme
        - one to display the mods changelog
- import
    - now, complete new key-value-pairs can be imported
- moved changelog from start view to a new tab
- ribbon bar is disabled under certain circumstances
- Open/Save-File-Dialogs follow Links instead of treating them as selected
- fixed an error for multiple read file while importing (enable/disable buttons)
- fixed an error where the current translation session isnt reset on cancel editig it within the session management
- fixed an error where manually added entries didnt got exported to json-files
- if it was an error, its fixed: dont replicate translations into entries with empty Values in certain circumstances

## Version 1.5.2
- ModBridge is not needed for translators
- create/edit translation sessions
    - localename in english is selectable
    - localename localized is selectable and customizable
    - tooltips adjusted/improved
    - separators added

## Version 1.5
- minor code changes
- fixed an error where existing entries could be deleted by adding a new entry
- tooltips improved
- export:
    - moved additionl information into separate dialog-window that can be opened by a click on a button in the ribbonbar
    - as JSON
        - filename proposals based on the english name of the translation/translation-session

## Version 1.1
- dispose AppHttpClient gracefully
- separate costum filters from autogenerated filters
- current session info
    - available in Ribbon-Tabs as RibbonGroup
- unified ex-/import to JSON only with two checkboxes that are described within the app itself
    - ex-/import translation for [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod
        - please see also: https://forum.paradoxplaza.com/forum/threads/i18n-everywhere.1646597/
        - export
            - to use with [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod
            - feel free to share the translation with him for his Mod:
                - https://github.com/baka-gourd/I18NEverywhere.Localization
        - import
            - from [baka-gourd](https://github.com/baka-gourd)'s [I18NEverywhere](https://github.com/baka-gourd/I18NEveryWhere)-Mod
    - ex-/import translation for the mod provided by the author of this app
    - export
        - gather JSON-export-filename as a proposal
        - try to preselect at least 'mods_subscribed' folder
    - import
        - 'old' exported JSON-files will be read/imported as expected (backward-compatibility)
- ability to add/remove custom entries
    - to add translations for known keys used by mods


## Version 0.4
- configure/define custom filters via App.config/TranslateCS2.dll.config
    - TranslateCS2.dll.config contains an explanation and two examples next to the three 'built-in' filters
- edit entry window
    - cancel is interrupted by a question if the translation changed
- modularization
- translator module
    - ability to implement and realize an external translator module that calls a translator-api (depends on modularization)
    - see also: [TranslateCS2.TranslatorsExample readme.md](https://github.com/mettliebe/CS2_TranslateCS2/blob/master/TranslateCS2.TranslatorsExample/readme.md)
    - translator module(s) is/are only hooked into the 'large edit entry window/dialog'

## Version 0.3.1 (Patch for texts with many rows)
- edit entry window
    1. is now resizeable
    2. now has a scrollviewer
    3. now has copy to clipboard buttons for each value (english, merge, translation)
    4. example key: Tutorials.DESCRIPTION[CargoAirlineTutorialConnectingRoute]

## Version 0.3
- Editing: added the ability to edit the current entry in a new window by opening the context-menu of a row
    1. a right click on a row opens a context menu that provides the ability to edit the current entry in a new window


- Ex- and Import-Views display information about the affected session
- Import-View:
    1. compare existing and read translations in a new window
    2. warning: the text-search-filter does not exclude translations to import. It's just to ease comparison!


- one internationalization-file per view
- some tooltips added
- readonly-configs
- only configurable configs within TranslateCS2.ddl.config
    1. DatabaseMaxBackUpCount


- workaround for the following localization files, cause they have more content than expected
    1. pl-PL.loc
    2. zh-HANS.loc
    3. zh-HANT.loc


- added ability to delete sessions (includes Database-BackUp before session is deleted)

## Version 0.2
- ex-/import as/from JSON to share with others
- Database-BackUps (next to the working database "Translations.sqlite", a maximum of twenty backups is created and held)
    1. at every start of the tool
    2. before translations are imported
- Edit- and Edit by Occurances Views display the number of rows shown

## Version 0.1.1
- Translation-Cells accept return for multi-line-editing
- save-performance improved
- minor other changes
- Credits updated

## Version 0.1
- initial release
