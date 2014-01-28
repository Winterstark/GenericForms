GenericForms
============

A .NET library that handles a few common application tasks: updates, tutorials, and preferences.
Pretty much all of my projects need to have this functionality, so I created this library to easily reuse the code.

Also included here is a demo project that demonstrates how to use the modules.


Updater module
----------------

![Screenshot: New update available](http://i.imgur.com/B7gzQLk.png)

This module makes updating your projects very easy, automatic, and silent (if the users sets those options). An update can contain more than one file, including the main .exe, and after downloading the program can automatically close itself, install the new version, and run it.

Use the following code when you want to check for a new version (typically when the program starts):

```
Updater.Update(double currVersion, string updateURL, bool[] askPermissions, bool showChangelog);
```

* currVersion is the current program version (e.g. 1.03). If the Updater discovers a greater version it will download the update.
* updateURL is the URL link to a raw text file online which contains information about the update.
  The text file does not have to be exclusively information about the update, but it needs to have an update information section that is properly formatted.
  The basic formatting looks like this:

   ```
   [UPDATE]
   [VERSION]updateVersion[/VERSION]
   [FILE]fileURL->filePath[/FILE]
   [CHANGELOG]updateDetails[/CHANGELOG]
   [/UPDATE]
   ```
   * updateVersion is the update version number (e.g. 1.01).
   * fileURL is the URL link to a file that needs to be downloaded; file Path is the destination path where the file needs to be located (to specify the main application directory use the keyword "root" - for example: "root\subdir1\subdir2\file.exe"). This line can be repeated indefinitely (to specify multiple files to be updated).
   * updateDetails is the text description of this update that will be shown to the user.

* askPermissions is an array of three bool values, each of which tells the Updater that it needs to ask the user's permission to perform an action.
   The actions are:
   1. Check for updates
   2. Download update
   3. Install update
   If a value is set to False then that action will be performed automatically; otherwise, the user will have to give manual permission in a message box.

* If showChangelog is set to True, after installing the update the changelog will be displayed to the user.

Note: If you performed an update and are wondering where this file UpdInst.exe came from and what its purpose is, read this:
One of the problems with the update process in general is that after you download the new version you need to replace your .exe file with the new one. However, you can't replace the program while it is being run, and if you close the program then who or what will perform the file replace operation? The solution is to run another program (UpdInst), close the main program and wait for UpdInst to perform the following algorithm:
1. Wait until the main program closes
2. Move all update files from the download temp. directory to their destinations
3. Run the newly updated program


Tutorial module
-----------------

Another common feature programs need is a guide or help system to inform new users how to use the program. The Tutorial module displays a series of popup "balloons" that give information about a particular control on the window.

Use the following code to give the user the option of seeing the tutorial for the current window:

```
Tutorial tutorial = new Tutorial(string path, Form targetWnd);
```

* path is the path to a text file containing the tutorial steps. To describe a basic tutorial popup use the following syntax:

   ```
   "info"-targetControl
   ```
   * info is the text displayed to the user in a "balloon".
   * targetControl is the name of the control the popup will point to.
   The hyphen (-) indicates a simple line will connect the popup and the target control.
   To draw a pointed triangle mark at the end of the line use write a greater-than character as well: ->.
   To draw a rectangle around the target enclose its name within square brackets: [targetControl].
   To draw an ellipse around the target enclose its name within parentheses: (targetControl).

   A popup can have an extra button titled "Advanced" which, when clicked, displays another string of text. To use this functionality just add an asterisk (*) followed by the advanced text at the end of the line, like this:

   ```
   "info"-targetControl*advancedInfo
   ```
   
   The DEMO application uses the following tutorial:
   
   ```
   "The Tutorial module displays a series of popups.\nEvery popup points to a specific control on the form, or the form itself (like this one)."->this
   "You can remove the triangle at the end of the line."-this
   "You can also display a selection rectangle around the target control."->[buttPrefs]
   "The selection rectangle can be replaced with an ellipse."->(buttPrefs)
   "Every popup can have a button to display more information."->[buttPrefs]*Stuff that's good to know but not of the utmost importance to the average user.
   "That's pretty much it about the Tutorial module. Click on this button to see the Preferences module."->(buttPrefs)
   "Lastly, click on this button to check for a new update, download it, and automatically install it."->(buttUpdate)
   ```
   
   ![Screenshot: Tutorial module 1](http://i.imgur.com/RETpBXB.png)
   
   ![Screenshot: Tutorial module 2](http://i.imgur.com/OT9OLrR.png)
   
   Note: after the user views the tutorial it will be disabled. This is indicated by the first line in the tutorial file, which will be: "SKIP".
   
* targetWnd lets the Tutorial know what window it needs to point to.


Preferences module
---------------------

Lastly, the Preferences module implements a simple options window. It supports multiple tabs and several types of controls; however, I've come to realize that my programs' options usually require a greater deal of customization and flexibility, so I've abandoned development on this module. It still works and could be viable for very simple programs that don't require anything fancy.

Similarly to the Tutorial module, the Preferences module is defined by a text file which is loaded with the following command:

```
Preferences prefs = new Preferences(string configPath, string savePath);
```

* configPath is the path to the text file describing how to construct the Preferences window.
   The module supports five types of options (controls):
   1. String (TextBox)
   
   ```
   optionName=textbox:optionPrompt=defaultValue
   ```

   2. Int (NumericUpDown)
   
   ```
   optionName=numbox(minValue--maxValue):optionPrompt=defaultValue
   ```

   3. Bool (CheckBox)
   
   ```
   optionName=checkbox:optionPrompt=defaultValue
   ```
   Note: defaultValue can be True or False.

   4. Multiple choice (ComboBox)
   
   ```
   optionName=combobox:optionPrompt=value1/value2/.../valueN=defaultValue
   ```

   *or*

   ```
   optionName=combobox(list):optionPrompt=value1/value2/.../valueN=defaultValue
   ```
   The difference in these two definitions is that "combobox(list)" accepts only predefined values while "combobox" can accept manual text input from the user.


   5. Multiple choice (RadioButton set)
   
   ```
   optionName=radiobuttons:optionPrompt=value1/value2/.../valueN=defaultValue
   ```

   *or*

   ```   
   optionName=radiobuttons(vert):optionPrompt=value1/value2/.../valueN=defaultValue
   ```
   "radiobuttons" organizes the buttons in a single row; "radiobuttons(vert)" organizes them in a single column.

   If the option prompt or its default value contain characters that would mess up the preferences format (such as ":" or "="), enclosing it within double quotes (") will fix any ambiguity.
   
   You can organize options into tabs by writing this:

   ```
   [tabName]
   ```
   All options that follow (until the next tab definition) will belong in this tab.

   
* savePath is the path to the text file that saves the actual option values. The file does not need to exist; after the user saves his options it'll be created.

To show the options window use the following:

```
prefs.Show();
```

*or*

```
prefs.Show(FormClosedEventHandler endReport);
```
The second way sets an event that fires when the options window closes.

The DEMO application uses the following options configuration to simulate the configuration of a rectangle class:

```
[General]
name=textbox:"Object name:"=My rectangle
caption=textbox:"Display label:"=Rectangle 1
effect=radiobuttons(vert):"Visual effect:"=Glow/Radiance/Bevel/Shadow=Shadow

[Size & Position]
x=numbox(0--1600):"X ="=5
y=numbox(0--900):"Y ="=5
w=numbox(0--100):"Width ="=50
h=numbox(0--100):"Height ="=50

[Appearance]
fill=combobox(list):"Fill style:"=Transparent/Semi-transparent/Opaque=Opaque
border=checkbox:"Draw border"=True
border_width=numbox(1--32):"Border width ="=1
border_style=radiobuttons:"Border style:"=Solid/Dashed/Dotted=Dashed
```

![Screenshot: Preferences - General tab ](http://i.imgur.com/0xmapQI.png)
![Screenshot: Preferences - Size & Position tab ](http://i.imgur.com/W7pAtua.png)
![Screenshot: Preferences - Appearance tab ](http://i.imgur.com/huWf6xr.png)
