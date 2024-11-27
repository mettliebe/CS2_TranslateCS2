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
