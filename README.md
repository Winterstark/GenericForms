GenericForms
============

A .NET library that handles a few common application tasks: updates, tutorials, and preferences.
Pretty much all of my projects need to have this functionality, so I created this library to easily reuse the code.

Also included here is a demo project that demonstrates how to use the modules.


Updater module
----------------
This module makes updating your projects very easy, automatic, and silent (if the users sets those options). An update can contain more than one file, including the main .exe, and after downloading the program can automatically close itself, install the new version, and run it.

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
   updateVersion is the update version number (e.g. 1.01).
   fileURL is the URL link to a file that needs to be downloaded; file Path is the destination path where the file needs to be located (to specify the main application directory use the keyword "root" - for example: "root\subdir1\subdir2\file.exe"). This line can be repeated indefinitely (to specify multiple files to be updated).
   updateDetails is the text description of this update that will be shown to the user.

* askPermissions is an array of three bool values, each of which tells the Updater that it needs to ask the user's permission to perform an action.
   The actions are:
   1. Check for updates
   2. Download update
   3. Install update
   If a value is set to False then that action will be performed automatically; otherwise, the user will have to give manual permission in a message box.

* If showChangelog is set to True, after installing the update the changelog will be displayed to the user.


Tutorial module
-----------------
