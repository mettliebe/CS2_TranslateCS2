# Version 2.1.0.1
* developer-changes: exports
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
