﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3615
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3DR_Testing.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("psadmin@problemsolutions.net")]
        public string _3DRUserName {
            get {
                return ((string)(this["_3DRUserName"]));
            }
            set {
                this["_3DRUserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("password")]
        public string _3DRPassword {
            get {
                return ((string)(this["_3DRPassword"]));
            }
            set {
                this["_3DRPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("fedoraAdmin")]
        public string FedoraAdminName {
            get {
                return ((string)(this["FedoraAdminName"]));
            }
            set {
                this["FedoraAdminName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("fedoraAdmin")]
        public string FedoraAdminPassword {
            get {
                return ((string)(this["FedoraAdminPassword"]));
            }
            set {
                this["FedoraAdminPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Documents and Settings\\montoyaa\\My Documents\\3dr trunk UPLOAD\\3dr\\testing\\Test" +
            "Files\\")]
        public string ContentPath {
            get {
                return ((string)(this["ContentPath"]));
            }
            set {
                this["ContentPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8080/fedora")]
        public string FedoraAccessURL {
            get {
                return ((string)(this["FedoraAccessURL"]));
            }
            set {
                this["FedoraAccessURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Documents and Settings\\montoyaa\\My Documents\\3dr trunk UPLOAD\\3dr\\testing\\sele" +
            "nium-remote-control-1.0.3\\selenium-server-1.0.3\\selenium-server.jar")]
        public string SeleniumLocation {
            get {
                return ((string)(this["SeleniumLocation"]));
            }
            set {
                this["SeleniumLocation"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.100.10.89/fedora/services/access")]
        public string _3DR_Testing_FedoraA_Fedora_API_A_Service {
            get {
                return ((string)(this["_3DR_Testing_FedoraA_Fedora_API_A_Service"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://l0.100.10.89/fedora/services/management")]
        public string _3DR_Testing_fedoraM_Fedora_API_M_Service {
            get {
                return ((string)(this["_3DR_Testing_fedoraM_Fedora_API_M_Service"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:1996")]
        public string _3DRURL {
            get {
                return ((string)(this["_3DRURL"]));
            }
            set {
                this["_3DRURL"] = value;
            }
        }
    }
}
