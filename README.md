# Terminal Commander Mod for Lethal Company

![banner](https://github.com/Tronald/TerminalCommander/blob/main/banner.jpg?raw=true)

## Overview

Terminal Commander is a mod for Lethal Company designed to enhance your terminal operation experience by introducing convenient hotkeys. This mod simplifies various tasks, allowing you to manage turrets, landmines, player views, and communication more efficiently.

*This mod is vanilla compatible*

## Hotkey Features

*All hotkeys may be reconfigured via the configuration file. It is recommended that you use a configuration editor mod such as LethalConfig to edit keybinds, but users may edit the ...\Lethal Company\BepInEx\config\Tronald.TerminalCommander.cfg file directly.**

| Feature                          | Hotkey Command                 | Description                                                                                            |
|----------------------------------|----------------------|--------------------------------------------------------------------------------------------------------|
| Jam All Turrets and Landmines    | <kbd>Ctrl</kbd>+<kbd>J</kbd> | Temporarily deactivate all active turrets and landmines to create a safe path for your team.           |
| Open / Close All Facility Doors   | <kbd>Ctrl</kbd>+<kbd>D</kbd> | Open / Close all powered doors within the facility.                                                    |
| Quickly View Monitor              | <kbd>Ctrl</kbd>+<kbd>M</kbd> | Quickly view or close the monitor within the terminal.                                                  |
| Fast Player View Switch           | <kbd>Ctrl</kbd>+<kbd>S</kbd> | Swiftly switch between different players on the monitor, providing you with a dynamic perspective.     |
| Quick Transmit                    | <kbd>Ctrl</kbd>+<kbd>T</kbd> | **Requirements:** Signal Translator <br> Simplify communication by automatically entering the "transmit" command. You only need to type in your message. |
| Teleport                    | <kbd>Ctrl</kbd>+<kbd>W</kbd> | **Requirements:** Teleporter <br> Triggers the teleporter. |
| Inverse Teleport                    | <kbd>Ctrl</kbd>+<kbd>I</kbd> | **Requirements:** Inverse Teleporter <br> Starts the inverse teleporter. |
| Emergency Teleport                    | <kbd>Ctrl</kbd>+<kbd>E</kbd> | **Requirements:** Teleporter <br> Teleports all players back to the ship(experimental, use with caution). |

## Command Features

| Command                         | Description                                                                                            |
|----------------------------------|--------------------------------------------------------------------------------------------------------|
| "help" -> "commander"    | Displays a list of hot key commands within the terminal for reference.           |
|"tp"| Teleport player currently viewed on the terminal monitor. |
| "itp" | Activates the inverse teleporter.|
| "etp"  | Emergency teleport all players back to the ship (experimental, use with caution). |

## Gameplay Settings

*Gameplay settings may be adjusted via the configuration file. It is recommended that you use a configuration editor mod such as LethalConfig to edit these settings, but users may edit the ...\Lethal Company\BepInEx\config\Tronald.TerminalCommander.cfg file directly.*

| Setting                         | Description                                                                                            |
|----------------------------------|--------------------------------------------------------------------------------------------------------|
| "Sync host"	| Sync gameplay settings to clients. Ensures clients use hosts gameplay settings for Terminal Commander.	|
| "Allow jamming"    | Is player allowed to use the turrent/mine jammer command.           |
| "Allow door control"| Is player allowed to use the open/close all facility doors command. |
| "Allow emergency teleport"| Is player allowed to execute an emergency teleport to bring everyone back to ship (experimental, use with caution). |
| "Jamming cool down" | Cool down time in seconds that the player must wait before reusing the turrent/mine jammer command.|
| "Door control cooldown" | Cool down time in seconds that the player must wait before reusing the open/close all facility doors command.|
| "Max emergency teleports" | Maximum number of emergency teleports that may be executed per round.|

## Installation

[Available on Thunderstore](https://thunderstore.io/c/lethal-company/p/Tronald/TerminalCommander/)
