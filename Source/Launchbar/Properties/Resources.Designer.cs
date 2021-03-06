﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Launchbar.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Launchbar.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # Launchbar {0}
        ///## Based on RUNit by [Andreas Hoeller](http://www.magister-lex.at)
        ///* Note: Nearly all icons used in this application are made by [Arvid Axelsson](http://arvidaxelsson.se) released under the [Creative Commons Attribution-Share Alike License](http://creativecommons.org/licenses/by-sa/3.0).
        /// </summary>
        public static string AboutTextFormat {
            get {
                return ResourceManager.GetString("AboutTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Launchbar.
        /// </summary>
        public static string ApplicationName {
            get {
                return ResourceManager.GetString("ApplicationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # Changelog
        ///All notable changes to this project will be documented in this file.
        ///
        ///The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
        ///and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
        ///
        ///## [Unreleased]
        ///### Changed
        ///- Upgrade to .NET Core 3.1, built with Visual Studio 2019
        ///- Changelog is now rendered with Markdown
        ///
        ///## [4.6.0] - 2019-08-28
        ///### Changed
        ///- Upgrade to .NET 4.8, built with Visual Studio 2019
        ///- Disable DPI awareness, as  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Changelog {
            get {
                return ResourceManager.GetString("Changelog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This seems to be the first time this application is launched. With this application it is possible to launch your favorite applications very quickly. By default the activation zone is on the right side of your screen. Just move your mouse to the right end of the screen and press the right mouse button. You are now ready to use the application. Add your favorite applications to the menu by using the configuration dialog. Have fun!.
        /// </summary>
        public static string InformationForNewUser {
            get {
                return ResourceManager.GetString("InformationForNewUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Program.
        /// </summary>
        public static string MenuEntryProgram {
            get {
                return ResourceManager.GetString("MenuEntryProgram", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Separator.
        /// </summary>
        public static string MenuEntrySeparator {
            get {
                return ResourceManager.GetString("MenuEntrySeparator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Submenu.
        /// </summary>
        public static string MenuEntrySubmenu {
            get {
                return ResourceManager.GetString("MenuEntrySubmenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application could not find a configuration file for this version! Do you want to import the configuration from an older version (1.x) of this application?.
        /// </summary>
        public static string QuestionUpgradeConfig {
            get {
                return ResourceManager.GetString("QuestionUpgradeConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Launchbar is already running! Please shutdown any existing instances of this application before starting a new one..
        /// </summary>
        public static string SingleInstanceWarning {
            get {
                return ResourceManager.GetString("SingleInstanceWarning", resourceCulture);
            }
        }
    }
}
