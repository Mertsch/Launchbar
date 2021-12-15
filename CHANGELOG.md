# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Changed
- Upgrade to .NET 6.0
- Changelog is now rendered with Markdown
### Removed
- Assembly Strong Name

## [4.6.0] - 2019-08-28
### Changed
- Upgrade to .NET 4.8
- Disable DPI awareness, as it breaks right-click behaviour.

## [4.5.1] - 2019-03-15
### Changed
- Upgrade to .NET 4.7.2
- Version number is now aligned with the changelog

## [4.5.0] - 2016-01-17
### Changed
- Process priority will not be set when 'Normal' (setting the process priority may not be allowed by the OS)
- Error message when starting a process provides more details

## [4.4.1] - 2015-08-02
### Changed
- Upgrade to .NET 4.6

## [4.3.0] - 2013-10-05
### Changed
- New Splash Screen
- Upgrade to .NET 4.5.1

## [4.2.2] - 2012-11-02
### Changed
- Improved handling of left clicks

## [4.1.0] - 2012-10-17
### Changed
- Assembly renamed and signed with strong name
### Removed
- Removed tray icon

## [4.0.0] - 2012-09-16
### Added
- Added click through support (left clicks are no longer blocken when menu is closed)
### Changed
- Now based on .NET 4.5

## [3.0.23] - 2009-08-16
### Added
- Added support for environment variables

## [3.0.13] - 2009-08-16
### Added
- Added a little splash screen

## [3.0.12] - 2009-08-16
### Added
- Browse functionality
- Move functionality
- Colorize path functionality

## [3.0.9] - 2009-06-06
### Added
- Support for multiple activation zones on the working area.
### Changed
- Keyboard shortcuts are now accepted from any level, not only the main level.

## [3.0.6] - 2009-06-04
### Added
- Application has been fully ported to WPF (.NET 3.5 SP1).
- Every setting is applied immediately for instant feedback.
- Making use of vector based icons for much better looks.
### Changed
- Icons have been updated to the latest Kempelton 3.1 (2009-05-21).
- Context menu scaling instead of icon scaling.
- Compiled with Visual Studio 2008 SP1.
### Fixed
- Memory leak on icon extraction
- Bug where the application would pop up in the taskbar

## [2.2.0] - 2009-05-31
### Added
- Show the location of the profile file on the disk.
### Changed
- Activation zone can be placed on any side of the screen.
- Activation zone change is now applied instantly.

## [2.1.0] - 2009-02-15
### Changed
- Hiding the application icon from the alt-tab-manager is now reliable.
- Some more refactoring for improved application performance and stability.

## [2.0.2] - 2008-01-03
### Fixed
- Incorrect resizing of the context menu entries.

## [2.0.1] - 2007-12-29
### Added
- Now detecting desktop resolution changes.
### Changed
- Changed icons to Kempelton 2.0 theme.
- Dynamically applying properties to bar (to preview changes in the configuration).

## [2.0.0] - 2007-12-01
### Added
- Programs can now be started with a different priority.
### Changed
- Complete redesign of the underlying code to make the program more efficient, structured and much more stable.
- Compiled with Visual Studio 2008.
- The application does now save its settings in the "Application Data" folder under the current user. This makes the application multi-user friendly.
- The Launchbar can still be accessed with an open configuration dialog.
- The configuration dialog is now fully resizable.

## [1.92] - 2007-03-24
### Changed
- Internal structure (less code, same function)
- Due to user input: If a folder is selected, a new entry will be added into the folder (not below the folder).

## [1.76] - 2007-03-18
### Fixed
- Multiple instance protection did not work

## [1.7] - 2007-03-18
### Added
- The downloader supports compression (much smaller update)
### Changed
- Redesigned the downloader - its fully stable and gives more feedback

## [1.6] - 2007-03-11
### Added
+ You can now choose an individual icon for any entry
### Changed
- Internal restructuring to make this possible
- A few GUI changes

## [1.5] - 2007-03-04
### Added
- Automatic update is now possible (see advanced options)
- Prevents multiple instances on the same executable
### Changed
- A few internal changes

## [1.4] - 2007-03-03
### Added
- An update-in-progress window
### Changed
- Checking for updates does no longer block the application
- You can now retry updating when it fails
- A bit of GUI tweaking
- Reduced the size of the application
## [
1.32] - 2007-03-03
### Changed
- Tweaked the updater (much more reliable and gives feedback)

## [1.31] - 2007-02-25
### Changed
- Deletes the updater after update

## [1.3] - 2007-02-25
### Added
- The application can now update itself

## [1.21] - 2007-02-24
### Changed
- OK-Apply-Abort work correctly now
- A minor GUI fix (for users who do not use XP-style)

## [1.2] - 2007-02-24
### Added
- The program does no longer show up in alt-tab manager
- You can now close the application from the options window
### Changed
- Improved the ? warning icon
- A very minor code change

## [1.1] - 2007-02-13
### Added
- Turn tray icon on-off
- Define size of context menu-icons
- Icon preview in context menu-configuration
- Ability to open a Folder instead of an executable
### Changed
- Tweaked icons for "new ..." buttons
- A few minor GUI improvements
### Fixed
- Forgot to save 2 settings

## [1.0] - 2007-02-11
Initial release