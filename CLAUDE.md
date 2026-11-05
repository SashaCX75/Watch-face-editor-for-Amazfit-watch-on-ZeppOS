# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A Windows Forms desktop editor (C#, .NET Framework 4.7.2) for designing Amazfit watch faces that run on ZeppOS. It edits a JSON project model, renders a live preview, generates the ZeppOS JavaScript (`index.js`) for the face, packs `.zpk`/`.zab` packages, and can also unpack and parse existing ZeppOS watch faces back into the model.

## Build

Classic (non-SDK-style) MSBuild projects with `packages.config`. There's no `dotnet build` and no test project. This matches CI (`.github/workflows/release.yml`):

```bash
nuget restore "AmazFit_Editor_(Watch_Face_Type_GT3).sln"
```

```bash
msbuild "AmazFit_Editor_(Watch_Face_Type_GT3).sln" /p:Configuration=Release
```

- MSBuild is installed at `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe`. The `packages/` folder is gitignored, so a fresh worktree needs the restore step first.
- Output goes to `Watch_Face_Editor/bin/<Config>/`. A **post-build event** (cmd syntax) moves all `*.dll` and `SplashScreen.*` into `Libs\` and the satellite resource folders (`de`, `en`, `cs`, …) into `Language\`. `App.config` sets `<probing privatePath="Libs;Language" />` so they still load. New dependencies and new languages must fit that layout.
- **CI:** every push to `master` (and every `v *.*` tag) builds and publishes a GitHub Release. The release/tag name comes from the last commit message. Pushing to `master` therefore ships a release.

## Solution layout

- `Watch_Face_Editor/`: the main WinExe (`Watch_Face_Editor_(ZeppOS).csproj`). Holds almost all the logic.
- `ControlLibrary/`: WinForms user controls used by the editor UI (see below).
- `SplashScreen/`: small splash-window exe, referenced by the main project.
- `ZeppOS_Converter(VB)/`: VB.NET library that decodes ZeppOS-format images back to PNG (`ZeppOSConverter_VB.MyMethod`). Used when unpacking existing faces.

Because the projects are old-style, **every new `.cs` file, resx, or content file must be added to the `.csproj` by hand**. Data files that ship next to the exe (`Skin\*.json`/`*.png`, `Mask\*.png`, `model_config\configurations.json`, `File_templates\*`) need `CopyToOutputDirectory=PreserveNewest`.

## Architecture

### `Form1` is one giant partial class

Nearly all application logic lives in `partial class Form1`, split across several very large files. Read these by searching, not end to end:

| File | Responsibility |
|---|---|
| `Form1.cs` (~30k lines) | UI event handlers, project open/save (`LoadJson`, `button_SaveJson_Click`), `Add<Element>()` methods, `ShowElemetsWatchFace()` (rebuilds the layer list from the model), ZPK creation/unpacking (`CreateZPK`, `Unpack_Zpk`), settings |
| `ReadWriteJson.cs` (~48k lines) | **Model → JS:** `JsonToJS` / `AddElementToJS` / `AddEditableElementToJS` emit ZeppOS `hmUI` widget code. **JS → model:** `JSToJson` / `ParametrsToObject` / `Object_*` parse an existing `index.js` back into model objects |
| `Preview_main_screen.cs` (~18k lines) | GDI+ rendering of the watch-face preview from the model |
| `Options_ReadWrite.cs` | `TextToJson` / `ObjectsToElements` turn the saved project JSON into typed element objects. Also loads and saves the options panels |
| `EditableElemets.cs` | Editable-elements (user-customizable widget slots) UI logic |
| `JSON_Scale.cs` | Rescales a whole project between device resolutions |
| `ImageConversion.cs`, `ImgConvert.cs`, `Classes/ConvertingImages/` | PNG ↔ ZeppOS image format conversion |
| `WatchFace_Testing.cs` | Integration with the Zepp simulator / ZeppPlayer (window capture via BitBlt/DXGI, UI Automation), GitHub upload helpers |

### Data model (`Classes/WatchFace/`)

- `WATCH_FACE` is the root of the project file: `WatchFace_Info`, `ScreenNormal`, `ScreenAOD`, `Editable_Elements`, `ElementEditablePointers`, plus top-level alerts, shortcuts, buttons, and background switches.
- `ScreenNormal.Elements` / `ScreenAOD.Elements` are `List<object>`. Each element class (`ElementDigitalTime`, `ElementSteps`, …) has a string field `elementName`. On load, `ObjectsToElements` switches on `elementName` to deserialize each item into its concrete class.
- Throughout the code, elements are dispatched with `switch (element.GetType().Name)` (`case "ElementSteps":` …). That happens in `Form1.cs`, `Options_ReadWrite.cs`, `Preview_main_screen.cs`, and `ReadWriteJson.cs`.
- Elements are composed from widget classes that mirror the ZeppOS `hmUI` API (`hmUI_widget_IMG`, `hmUI_widget_IMG_NUMBER`, `hmUI_widget_TEXT`, `Circle_Scale`, `Linear_Scale`, …). Each element keeps a `position` for every sub-option to control draw and emit order.
- Serialization uses Newtonsoft.Json with `NullValueHandling.Ignore`. Classes implement `ICloneable` with hand-written `Clone()`, so **new fields must also be added to `Clone()`**.

**Adding or changing an element type** usually touches all of these:
1. The model class in `WATCH_FACE.cs`.
2. `ObjectsToElements` in `Options_ReadWrite.cs`.
3. `Add<Element>()` and `ShowElemetsWatchFace()` in `Form1.cs`, plus the matching `uCtrl_<Name>_Elm_*` event handlers.
4. Preview rendering in `Preview_main_screen.cs`.
5. JS generation in `AddElementToJS` and JS parsing in `ParametrsToObject` / `Object_*`, both in `ReadWriteJson.cs`.
6. A `ControlLibrary/Elements/UCtrl_<Name>_Elm` control.
7. Localized strings.

### Generated JS round-trip

The editor reads back JS that it generated itself. It does not parse arbitrary JavaScript. `JSToJson` relies on the exact shape of the emitted code and on marker comments: it strips `//start of ignored block` … `//end of ignored block` and `//#region` … `//#endregion` before parsing, and it extracts user-script sections. **When you change the JS that `AddElementToJS` emits, update the matching parser path too**, or faces saved by the new version won't re-open. `File_templates/` holds the `app.js`/`app.json`/`index.js`/`.zpk`/`.zab` skeletons used when packing.

### Devices

`model_config/configurations.json` defines each supported watch: `background` size, `designWidth`, `deviceSource_ids`, `screenType`, `scaling` presets, `maskImage`, `watchSkin`, `versionOS`, and more. It's loaded into `AvailableConfigurations`, and the active one is `SelectedModel`. Adding a watch model means adding an entry there, plus optional `Skin/WatchSkin_<Model>.json` + `Skin_<Model>.png` and a `Mask/mask_*.png`, all registered in the csproj. `JsonToJS` forces background sizes to `SelectedModel.background`.

### ControlLibrary

- `Elements/UCtrl_<Name>_Elm`: a row in the layer list for one element. It raises `SelectChanged`, `OptionsMoved`, `VisibleElementChanged`, and `VisibleOptionsChanged`, which `Form1` handles.
- `UCtrl_<Kind>_Opt` (`UCtrl_Text_Opt`, `UCtrl_Pointer_Opt`, `UCtrl_Circle_Scale_Opt`, …): property editors shared across many element types.

### Localization

- The neutral resources (`*.resx` with no culture) are **Russian**. Translations exist for `cs`, `de`, `en`, `es`, `fr`, `it`, `pt-BR`, `uk`, and `zh`.
- String tables: `Watch_Face_Editor/Properties/FormStrings*.resx` and `ElementsString*.resx`, and in ControlLibrary `Properties/Strings*.resx`, `Buttons*.resx`, and `ButtonFunctions*.resx`. Each culture has its own checked-in `.Designer.cs`.
- Forms and controls also have per-culture `.resx` files. `Resources/tips_<lang>.rtf` holds the tips text.
- A new user-facing string should go into the neutral resx and every culture resx.
- Code comments and many identifiers are in Russian, and some names are misspelled (`EditableElemets`, `Parametrs`, `Prewiev`). Keep the existing names when referencing them.

### Runtime files next to a project

- Autosave writes `<project>.json.temp`. On open, the editor offers to restore it.
- `Preview.States` (in the project folder) stores the preview's simulated sensor values (time, steps, weather, …).
- Image assets live in an `assets\` folder next to the project JSON.
