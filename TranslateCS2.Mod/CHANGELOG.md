# Version 2.1.0.5
* compatibility with Version 1.2.0f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
# Version 2.1.0.4
* the previous version gathered/obtained a new mod id!
* fix a serious issue, where updating an existing version of this mod made the game crash
* developer-changes: exports
    * due to the fix, localizations
        * arent collected at startup
        * have to be collected manually
            * export is enabled afterwards
# Version 2.1.0.3
* this version gathers/obtains a new mod id
* within the next few minutes, there will be another release!
* fix a serious issue, where updating an existing version of this mod made the game crash
* developer-changes: exports
    * due to the fix, localizations
        * arent collected at startup
        * have to be collected manually
            * export is enabled afterwards
# Version 2.1.0.2
* developer-changes: exports
    * hotfix for "Mod Version 2.1.0.1" to not collect assets, each time the game returns to main menu, since assets are precollected and can be refreshed
# Version 2.1.0.1
* developer-changes: exports
    * language(s) to export is now ordered by locale-id
    * the generated .json while exporting now contains the mods technical displayname instead of the mods id
    * now assets and code-mods localizations can be exported
    * most information is gathered while the game itself starts
    * some information, especially for assets that are loaded by 'Extra Assets Importer (EAI)'/'Asset Packs Manager (APM)', cannot be gathered at startup
    * therefore a button called 'refresh type(s) to export' is introduced
        * if mods/assets are missing, it can be used to refresh the dropdown
        * depending on the amount of activated mods/assets within the current playset, this action may take a few seconds and the User Interface is freezed
        * if the desired asset/mod does not appear within the dropdown after a refresh, its localizations can not be gathered
        * please take ZZZ_Uncategorized into account
    * due to technical limitations,
        * some asstes/code-mods localizations can not be gathered, those assets/code-mods do not appear within the respective drop-down
        * some localizations cannot be clearly assigned, these are summarized under "ZZZ_Uncategorized"
# Version 2.1.0.0
* developer-changes
    * fixed exporting errors
    * developers
        * now have the ability to select a specific export type
            * All: Base-Game and active Mods
            * Game: Base-Game
            * [Region-Name] Pack: Only for the selected Region Pack
            * Mod-Specific
        * Warning:
            * If a Mod does not provide Asset-Names/Translations for a certain selected Language, the generated .json does not contain Key-Value-Pairs the specific Mod!
            * It may result in a json-file that only contains an empty json-object.
        * The generated .json-files-names contain the locale-id and the Type (All, Game, the respective ModID or, in case of Local-Mods, the technical name).
            * Examples:
                * en-US_All.json
                * en-US_Game.json
                * en-US_91930.json (in case of the French Pack)
                * en-US_MyNewAssetMod.json (in case local mods)
# Version 2.0.9.6
- checked compatibility with Version 1.1.12f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
# Version 2.0.9.5
- added Security Section to this Mods README.md
- updated [GUIDE.md](https://github.com/mettliebe/CS2_TranslateCS2/blob/master/TranslateCS2.Mod/GUIDE.md)
    - Section 1.3
        - added where and how other mods are taken into account
    - Section 1.3.1
        - added link to ModExample
- DistributedBinaries
    - is a new directory and can be found over there
        - https://github.com/mettliebe/CS2_TranslateCS2/tree/master/TranslateCS2.Mod/DistributedBinaries
    - from now on, for the current ModVersion, this directory contains a
        - a general [README.md](https://github.com/mettliebe/CS2_TranslateCS2/blob/master/TranslateCS2.Mod/DistributedBinaries/README.md)
        - a .zip-file related to the current ModVersion
            - with the binaries, that are build and distributed
            - with the respective .hashes-file, that is NOT distributed
        - a .hash-file related to the current ModVersion
            - with the SHA512-Hash for the .zip-file itself
# Version 2.0.9.4
- Hashes
    - TranslateCS2.Mod.hashes
        - contains the FileNames with Extension and the respective SHA512-Hash of all Files that are published with this mod
        - can be found over there: https://github.com/mettliebe/CS2_TranslateCS2/blob/master/TranslateCS2.Mod/TranslateCS2.Mod.hashes
    - checkIntegrity.ps1
        - a powershell-script to check integrity
        - it
            - counts and checks file-counts
            - compares hashes
        - has to be executed manually
        - can be found over there: https://github.com/mettliebe/CS2_TranslateCS2/blob/master/TranslateCS2.Mod/checkIntegrity.ps1
# Version 2.0.9.3
- checked compatibility with Version 1.1.11f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
# Version 2.0.9.2
- checked compatibility with Version 1.1.10f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
- changed logging in cases the entire mod fails to load to be able to use the messages copy-button
# Version 2.0.9.1
- Long Description got reworked (maximum of 10000 characters)
- checked compatibility with Version 1.1.8f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
# Version 2.0.9.0
- developer-changes
    - destination to where this mods Key-Value-Pairs are generated can be chosen
    - added a confirmation-dialog to the generate-button
- split settings into tabs
    - Settings
    - Developers
# Version 2.0.8.0
- descriptions for this mods settings got formatted
- developers are able to export the vanilla languages to a directory of their choice
- avoid an TaskManager-Error that occured under certain circumstances on exiting the game
# Version 2.0.7.1
- full compatibility with Version 1.1.7f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
# Version 2.0.7.0
- full compatibility with Version 1.1.6f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
- changes that affect users:
    - the current selected language from the default interface settings is now displayed within this mods settings
        - due to internal/code changes
- internal/code changes:
    - there is only one Flavor-DropDown left
        - its DropDownItems are changed whenever the selected language within the default interface settings change
    - unnessesary languages, those that are not built in without flavor-sources, are removed from the dictionary
- warning:
    - for those who translate/translated this mod
    - due to internal/code changes, the Key-Value-Pairs have changed
# Version 2.0.6.0
- compatibility with Version 1.1.6f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
- minor code changes
# Version 2.0.5.0
- ability to sideload translations that are provided via others mods
    - inspired by
        - [yenyang](https://github.com/yenyang)
        - Morgan · Toverux
    - this feature/behavior is enabled by default and can be turned off within this mods settings
        - uncheck "Load from other mods"
    - this mod checks for valid mods that are local or enabled within the active playset
    - this mod checks the presence of a certain directory within the found mods
    - if a mod provides unoffical locales/translations, those translations are also taken into account
    - what does that mean?
        - if you use this mod pro-actively and provide a translation or translations as described within the readme
            - your provided translation(s) rule(s) and is/are not overwritten.
            - if a translation provided by another mod contains more/other/additional translations, those additional translations are also used in-game
    - if errors occur while loading the translationfiles
        - dedicated error-messages are shown
            - for this mod, TranslateCS2.Mod, the message starts with
                - the following provided translationfiles within this mods data directory are corrupt
            - for each mod, a separate message is displayed
                - the mod "TechnicalNameOfTheMod" provided the following corrupt translationfiles"
                - or for local mods
                - the local mod "NameOfTheMod" provided the following corrupt translationfiles"
- now, an error message is displayed, if the entire mod fails to load
# Version 2.0.4.1
- Hotfix for an issue
# Version 2.0.4.0
- changes related to Version 1.1.5f1 of [Cities: Skylines II](https://www.paradoxinteractive.com/games/cities-skylines-ii)
- access to filesystem reduced
- logging reduced
# Version 2.0.3.0
- try to take additional indices into account
- some code changes
# Version 2.0.2.0
- read and add index counts
    - to roll the dice for city names correctly
    - for other random stuff (?)
# Version 2.0.1.0
- developers have the ability to generate log entries for supported languages
- code changes (use autogenerated properties and setting-items for flavor-drop-downs)
- added support for the following languages that are grouped as 'other languages' within the interface settings language drop down:
- full list can be found over there: https://github.com/mettliebe/CS2_TranslateCS2/blob/master/TranslateCS2.Mod/LANGUAGES.SUPPORTED.md
# Version 2.0
- display languages english name within this mods settings for labels and descriptions
- button to reload languages is only visible for developers
- developers have the ability to generate a .json-file with this mods Keys and Values/Texts/Translations
- updated supported languages
- added supported languages:
    - no - Norwegian
    - ChineseSimplified
        - zh-CN - Chinese (Simplified, China)
        - zh-SG - Chinese (Simplified, Singapore)
        - zh-CHS - Chinese (Simplified) Legacy
    - ChineseTraditional
        - zh-HK - Chinese (Traditional, Hong Kong SAR)
        - zh-MO - Chinese (Traditional, Macao SAR)
        - zh-TW - Chinese (Traditional, Taiwan)
        - zh-CHT - Chinese (Traditional) Legacy
# ChangeLog Version 1.5.2

* added appendix '(Latin)' or '(Cyrillic)' for Serbian languages within the flavor-drop-down to be able to differentiate them
* the language drop down within the interface settings now lists 'српски/hrvatski' instead of 'SerboCroatian'
* if provided json-files are corrupt on load, an error message is shown (an example screenshot is available within the gallery and within this mods examples directory)
* if provided json-files are corrupt on reload, an error message is shown (an example screenshot is available within the gallery and within this mods examples directory)
* performance: just as an info and at least on my machine: it takes amongst 3.5 seconds to load all 167 (non-corrupt) language-/translation-files with a total of 2,942,874 entries (17,622 entries per language-/translation-file)
* added Disclaimer to Readme/LongDescription
* tried to explain it a bit more


# ChangeLog Version 1.5.1

* native names are displayed instead of english names
* reduced examples to mappable examples
* reduced LocalesSupported to mappable language-region-codes


# ChangeLog Version 1.5

* long language names (more than 31 characters) are cut
* filenames are read case-insensitive
* some more handling between built-in locale-ids and read locale-ids
* update github link to mods readme.md


# ChangeLog Version 1.4

* fix missing pre-init for non built-in languages
* fix upper-lower-cases (for example: zh-Hans versus zh-HANS)
* example jsons for each supported language-region-code added
* images and examples wont be embedded anymore to reduce the dlls size


# ChangeLog Version 1.3

## fix on load error
* error on loading the mod and prevented saved flavor selections from beeing restored


# ChangeLog Version 1.2

## display name
* correct stepping to display at least the correct english name

## reload language
* add to flavormapping on load, otherwise language source got removed, but not added


# ChangeLog Version 1.1

## File-System/Locations

Translations/Localizations now have to be placed into the following directory:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsData\TranslateCS2.Mod\


Settings can now be found over there:
%AppData%\..\LocalLow\Colossal Order\Cities Skylines II\ModsSettings\TranslateCS2.Mod\


## Mod-Settings within the game

* removed behaviour -&gt; add as source
* removed clear overwritten
* added drop down to select a flavor
* if a certain language has no flavors, the drop down is disabled
* if a flavor for a built in language is added, the drop down has an entry 'none'; this entry replaces the removed 'clear overwritten'
* if a 'new' language is added, the drop down misses the entry 'none'
