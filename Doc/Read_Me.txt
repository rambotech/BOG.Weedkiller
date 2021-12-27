Weed Killer
===========

Version History: -------------------------------------------------------

v1.1.0.1 -- Dec 27, 2021
  - Update projects to .NET Framework 4.8

v1.1.0.0 -- May 1, 2018
  - Migrated to Github
  - Update projects to .NET Framework 4.6.1

v1.0.6.5 -- Dec 4, 2015
  - Reversioned releases after 1.0.6 to 1.0.6.*, since they were all maintenance releases.

v1.0.6.4
  - The worker required a path portion on the configuration file argument, so configuration file(s) in the
    current directory have to be specified with ".\" or some form of path to the current directory to prevent
	an invalid path error from being thrown.  Note to self: Tell Microsoft that a path of string.Empty should
	be treated as ".\" by default.

v1.0.6.3 - Maintenance release
  - The event sequence was not reporting End Folder events.  This has been corrected. 
  - Adjusted the initial size and anchors of the result textbox in the tester.  It was underlapping the 
    status bar.

v1.0.6.2 - Maintenance release
  - Adjusts installation package so that all needed assemblies are copied to both the Manager and Worker subfolders.

v1.0.6.1 - Maintenance release
  Manager:
  - Fixed a null object bug where the tester would throw an error if the first click of the Launch button was 
    for a LIVE (vice Test) launch.

v1.0.6.0
  **NOTE: As of this version, all desired features (at least from my initial design intent) are in place.
  Manager:
  - Allow event selections and color selections in tester to be saved and recalled as a named set.
  - If usersettings.xml does not exist, creates four separate initial templates: DEFAULT, Remove Only, Spared Only, No Match
  - Fixed the broken hyperlink in the About form.
  - Changed the About form to have three tabs: license, technical and ReadMe.txt.
  - Added MRU under a new entry in the File menu (Recent)
  - Add an options dialog under the Tools menu.
    - Added MRU maximum size setting.
	- Added button to clear MRU entries.
  - Substantial improvement to the tester behavior.
    - Background worker now processes the actual Weed Killer actions.
	- Result grid population and adjustment time reduced to approximately 10% of time previously used.
  Other:
  - Changed release license to Microsoft Public License, starting with this version.
  - One installation file (WeedKiller_Installer.msi) for both the Manager and the Worker.

v1.0.5.0
  Manager and Worker:
  - Corrected an issue where applying an NT permission was setting the Propagation flags to Inherit Only, 
    instead of None.  This caused the permission to not work on the root folder itself, and the files
	within the root folder itself.
  Misc: 
  - Added a section the help document describing how to add/remove an NT permission via the Manager.

v1.0.4.0
  Core Class:
  - Fixed a logic bug where an enumeration-resolved root directory was not properly reported when the 
    directory name contained wildcards.
  Manager:
  - Added a new menu option and form (Ajust NT Permissions) to apply a given NT credential to all resolved root
    folders within a configuration.  Allows applying the NT permission running the Weed Killer worker to the 
	folders targeted in a configuration, or removing them when no longer needed.
  - Disabled the main menu in the manager MDI form, when a configuration is being edited.  This prevents a save
    operation from occurring before the accept/cancel button is clicked to end the edit.  This appeared to be
	the source of a potentional to corrupt a configuration entry.

v1.0.3.0
  Core Class:
  - Fixed a logic bug where excluding the root with subfolders enabled would actually prevent
    any processing of files.
  - Added new enumerations for BeginFolder / EndFolder
  - Added new enumeration for Add ACL entry, only used by the tester and the console worker.
  Manager:
  - Changed help to an internal browser in a modal form, rather than lauching the html in the default browser.
  - Adjusted the tester to use SuspendLayout/ResumeLayout for faster re-rendering when changing the selections
    and colors.
  - Change all framework targets and dependencies from 2.0 to 3.5
  - Cosmetic corrections in number formatting in tester to drop decimal points and add group
    separators (comma).
  - Added method calls BeginUpdate() and EndUpdate() to the datagrid object on the
    tester.  Allows much faster reloading when filters/colors are changed.
  - In addition to Ctrl+A to select all configuraton items in the configuration set
    - Ctrl+D now deselects all items
    - Ctrl+I inverts selections
  - Cleanup of numeric summary display in the set tester form:
    - Commas for thousands / elminiation of decimal points previously in integers.
  Worker:
  - TO DO: ACL Management. Finish the /permission+/- options.

v1.0.2.0   -- non-released, internal-only version
  Manager:
  - Cosmetic corrections in number formatting in tester to drop decimal points and add group
    separators (comma).
  - Added method calls BeginUpdate() and EndUpdate() to the datagrid object on the
    tester.  Allows much faster reloading when filters/colors are changed.
  - Unfinished: added ACL Management options "/permission+/-:".
  - In addition to Ctrl+A to select all configuraton items in the configuration set
    - Ctrl+D now deselects all items
    - Ctrl+I inverts selections

v1.0.1.0
  Worker:
  - New switch added (/c for counts) which attempts to summarize number of files removed 
    and estimate space recovered.  
  - Added a new switch to specify an NT account to add to the ACL for delete privelege.
    Allows admins to run the worker under their credentials with this switch, and apply the
    permissions for the credential which normally would run the scheduled task.

v1.0.0.5 - Alpha
  Manager:
  - Use ".wkconf" as the extension for the Weed Killer configuration files.
  - Associate wkconf to open with WeedKillerManager in an explorer extension (Open).
  - default_appsettings.xml installed, and used as a template when no settings exist for user.
  - Use group markings on long numbers in event counts (tester).  E.g.  2,751,648 vice 2751648
  - Ensure single instance launch of the Manager.
  - Added Execution Server list to restrict execution by Worker to specific server name(s).

  Worker:
  - Make a terse output (datetime and path of deleted only).  "/s" for short form.
  - Add support for wildcards in the configuration files parameter.
  - Validated/corrected error level returns.
  - Honor Execution Server list property.

  Other:
  - Add a readme file explaining that Setup.exe is only needed, if the MSI reports an error.  Systems 
    with .NET 2.0 or later should not need the setup.exe file.
  - Documentation update.
  
v1.0.0.4 Alpha
  -  Change the Help window to load in the default web browser outside of the application.

v1.0.0.3-Alpha
  -  Added Weeks to the age metric enumerations in WeedKillerClass (was missing)
  -  Change the Help window to a non-modal, child window.

v1.0.0.2-Alpha
  - Added support for Wildcards in addition to Regular Expressions
  - minor bug fixes

v1.0.0.1-Alpha
  - Adjusted the Start Menu installation location for the tools

v1.0.0.0-Alpha
  - First release
