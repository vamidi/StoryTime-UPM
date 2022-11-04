---
description: How to install StoryTime for Unreal engine 4/5.
---

# Unreal 4/5 Installation

### Prerequisites

* You must have git installed on your machine.

### Getting Started

> Make sure if you use an (existing) project in Unreal engine 5 and generated the visual studio files.

#### Adding the plugin

* If you don't have one, create a folder called "Plugins" in your project directory (where your content folder is located).
* If you have a Plugins folder checkout the plugin (or download the release) and copy the plugin to the Plugins folder.

#### Setting up the config

* When you copied everything over to the plugins' folders, go to `StoryTimeEditor` > `Resources` > `Config` and copy/rename `config.json.example` to `config.json`
* Put in all your info and credentials.

| Variable    | Default Value                                               | Description                                    |
| ----------- | ----------------------------------------------------------- | ---------------------------------------------- |
| databaseURL | YOUR\_WEB\_SERVER\_URL                                      | Your web server to retrieve the data from      |
| projectID   | YOUR\_PROJECT\_ID                                           | Your project id you want to use.               |
| email       | YOUR\_EMAIL\_LOGIN                                          | Your email credential to log into StoryTime.   |
| password    | YOUR\_PASSWORD\_LOGIN                                       | Your password credential to log into StoryTime |
| dataPath    | /ABSOLUTE/PATH/TO/YOUR/STORYTIME/PLUGIN/FOLDER/Content/Data | The location to store the JSON data in.        |

#### Adding and building all modules

* Rebuild your project/game
* Start your project in Visual Studio via "Launch via Local Debugger".
* If you get asked to build some modules, do it.
* Once opened go to `Edit` > `Plugins` and search for StoryTime.
* Make sure they are both enabled.
* \[OPTIONAL] Close Unreal Editor.
* \[OPTIONAL] You can now open the game via the Launcher or the `.uproject` file.
* After filling all the necessary fields you can start syncing all the data.



\
