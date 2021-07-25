This repository contains my mods for MelonLoader. Join the [MelonLoader discord](https://discord.gg/2Wn3N2P) for support and more mods!  
Looking for my VRChat mods? [Look here!](https://github.com/knah/ML-UniversalMods)

## Known limitations
All of these mods were compiled against IL2CPP generated assemblies for Unity 2018.4.20. This means they might have compatibility issues with other Unity versions and non-IL2CPP games.

## CoreLimiter
A mod to automatically limit your game to a certain amount of CPU cores. This can be used to boost performance on some Ryzen CPUs by limiting the game to a single CCX.
Naturally, limiting the game may reduce maximum possible performance under heavy load, and results are highly dependent on how well the game is multithreaded.

You should experiment with settings in a CPU-heavy world or scene to measure performance on your specific system. For CPUs with less than 8 cores it might be worth it to reduce used core count or allow hyperthreads.

This mod is Windows-only. It likely won't do anything on Intel CPUs, but you're free to experiment with it.

Settings:
* Max Cores (default half the physical cores you have) - the maximum amount of cores that the game may use. 4 is the sweet spot on a 2700X/3700X.
* Use both hyperthreads - will use both hyperthreads on cores if enabled (otherwise just one)
* Use first X cores (instead of last) - will use CPU cores starting from first one instead of last one
* Use specific cores - allows you to specify a comma-separated list of cores (including hyperthreads). Cores are numbered starting from zero, and each hyperthread gets a number, so for a 8-core CPU valid values here would range from 0 to 15. 


## HWIDPatch
This mod allows you to fake your Hardware ID. This mod creates a new ID on launch and saves it for future launches. The ID can be changed in `modprefs.ini` afterwards. Set it to empty string to generate a new one.  
Privacy first!


## LocalPlayerPrefs
This mod moves game settings storage from Windows registry to UserData folder.  
This can make using multiple game installs easier by having them not share the same space in the registry.  
Do note that some settings will stay in registry (the ones that Unity itself uses as opposed to game code).  
There's also no import from registry, so any data/settings stored there will need to be configured anew after installing this mod. 
 
## NoSteamAtAll
Makes the game unable to access Steam. At all.  
Naturally, this will break all game features dependent on steam (for example logins/user accounts, multiplayer, leaderboards, achievements).  
Intended for games that don't really need steam, but might abuse it for data collection/targeting/other suspicious activities.

## RuntimeGraphicsSettings
A mod to allow tweaking some graphics settings at runtime to get those extra few frames.  
Works best when your game has an in-game UI for mod settings (as another mod, naturally).

Settings description:
 * -1 on integer settings means "don't change the default value"
 * MSAALevel - multi-sampled anti-aliasing level. Valid values are 2, 4 and 8
 * AllowMSAA - toggle MSAA at runtime
 * AnisotropicFiltering - texture anisotropic filtering
 * RealtimeShadows - allow realtime shadows
 * SoftShadows - use soft shadows if shadows are enabled. Soft shadows are more expensive.
 * PixelLights - maximum amount of pixel lights that can affect an object
 * Texture decimation - Reduces texture resolution by 2^(this setting). A value of 0 means full-resolution textures, a value of 1 means half-res, 2 would be quarter res, and so on.
 * GraphicsTier - Unity Graphics Hardware Tier. Valid values are 1, 2 and 3. Only affects shaders loaded after it was changed. 
 
## Installation
To install these mods, you will need to install [MelonLoader](https://discord.gg/2Wn3N2P) (discord link, see \#how-to-install).  
Then, you will have to put mod .dll files in the `Mods` folder of your game directory

## Building
To build these, drop required libraries (found in `<game dir>/MelonLoader/Managed` after melonloader installation, list found in `Directory.Build.props`) into Libs folder, then use your IDE of choice to build.
 * Libs folder is intended for newest libraries (MelonLoader 0.3.0)

## License
All mods here are provided under the terms of [GNU GPLv3 license](LICENSE)
