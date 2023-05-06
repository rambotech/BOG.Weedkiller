# BOG.Weedkiller

Written in C#. Migrated from SourceForge. Updated to run with .NET Framework 4.8

## Example screenshots:

## Manager
![alt text](https://github.com/rambotech/BOG.Weedkiller/blob/master/Assets/screenshot_manaqger.jpg)

## Tester 
![alt text](https://github.com/rambotech/BOG.Weedkiller/blob/master/Assets/screenshot_tester.jpg)

## Overview 
Weed Killer is a .NET solution to prune aged files from folders, working equally well on a single workstation, 
or multiple data centers. It consists of 

- An MDI form to build / edit / test / execute configurations
- A class for direct use in an application.
- A console worker for delegated removal (i.e run on a server as timed event)

Feature include:
- Enhanced multi-layer wildcard support for folder-trees.
- Choice of regular expressions or traditional wildcards for file or folder patterns.
- Ability to specify not only inclusion, but exclusion patterns as well.
- Multiple configurations per file.
- Simple MDI GUI for editing and testing.
- Configuration extension registered as a file type for opening directly from Windows Explorer.
- Multiple configuration file editing, with copy/paste support.
- Server restrictions on a configuration file, to limit what systems may execute the file when automated.
- Automatic assignment/removal of delete permissions for an NT credential on a targeted root folder.

