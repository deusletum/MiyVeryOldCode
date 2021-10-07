//-------------------------------------------------------------------
///

///

///
/// <summary>
///    Runs Application Center Command Line tests (CLT)
/// </summary>
/// 
/// <types>
/// </types>
/// 
/// <history>
///     <record date="10-Mar-03" who="deangj">
///     First Creation
///     </record>     
/// </history>
///
//-------------------------------------------------------------------
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace CLTtests.Test.Smx.Microsoft
{
    /// <summary>
    /// 
    /// </summary>
    class DeployVars
    {
        private string controller;
        private string member;

        public DeployVars(string Controller, string Member)
        {
            this.controller = Controller;
            this.member = Member;
        }
        /// <summary>
        /// Variation 3.2.1:    Disable automatic replication to the cluster controller–from controller
        /// </summary>
        public void Var321()
        {
            string var = "Var 3.2.1";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var321";

            string setup = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:COMPLUSAPP /LOADBALANCING:CLB /MANAGEMENTNIC:" + managenic;

            string acargs = "DEPLOY /DISABLESYNC /SOURCE:" + controller + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
    
        //*********************************************
        //Var 3.2.2 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.3 - 4 not implemented
        //These variations are no longer valid, due to changes in CLT functionality.
        //*********************************************
        /// <summary>
        /// Variation 3.2.5:    Disable automatic replication to the cluster 
        /// controller–default values, on the controller
        /// </summary>
        public void Var325()
        {
            string var = "Var 3.2.5";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;

            string acargs = "DEPLOY /DISABLESYNC /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.6 not implemented
        //*********************************************
        /// <summary>
        /// Variation 3.2.7:    Disable automatic replication to a cluster 
        /// member–from the controller, specify explicit credentials
        /// </summary>
        public void Var327()
        {
            string var = "Var 3.2.7";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /MANAGEMENTNIC:" + membermnic;

            string acargs = "DEPLOY /DISABLESYNC /SOURCE:" + member + " /SOURCEUSER:" + user 
                + " /SOURCEPASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.8 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************
        /// <summary>
        /// Variation 3.2.9:    Enable automatic replication to the cluster controller–from controller
        /// </summary>
        public void Var329()
        {
            string var = "Var 3.2.9";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;

            string acargs = "DEPLOY /ENABLESYNC /SOURCE:" + controller + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }  
        //*********************************************
        //Var 3.2.10 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.11 - 12 not implemented
        //These variations are no longer valid, due to changes in CLT functionality.
        //*********************************************
        /// <summary>
        /// Variation 3.2.13:    Enable automatic replication to the cluster 
        /// controller–default values, on the controller
        /// </summary>
        public void Var3213()
        {
            string var = "Var 3.2.13";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;

            string acargs = "DEPLOY /ENABLESYNC";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.14 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************
        /// <summary>
        /// Variation 3.2.15:    Enable automatic replication to a cluster 
        /// member–from the controller, specify explicit credentials
        /// </summary>
        public void Var3215()
        {
            string var = "Var 3.2.15";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            //string cluster = "Var3215";
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /ENABLESYNC /SOURCE:" + member + " /SOURCEUSER:" + user 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.16 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************       
        /// <summary>
        /// Variation 3.2.17:    Display deployment status for all members of a 
        /// cluster to which a given member belongs
        /// </summary>
        public void Var3217()
        {
            string var = "Var 3.2.17";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /MANAGEMENTNIC:" + membermnic;

            string acargs = "DEPLOY /STATUS:ALL /SOURCE:" + member + " /SOURCEUSER:" + user 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }  
        /// <summary>
        /// Variation 3.2.18:    Display deployment status for all members–on the 
        /// controller, source is the controller, all members have identical credentials, omit /SOURCE
        /// </summary>
        public void Var3218()
        {
            string var = "Var 3.2.18";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /STATUS:ALL";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.19 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************
        //*********************************************
        //Var 3.2.20 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************
        /// <summary>
        /// Variation 3.2.21:    Display deployment status for all members–on the 
        /// controller, source is a member, all members have identical credentials
        /// </summary>
        public void Var3221()
        {
            string var = "Var 3.2.21";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /STATUS:ALL /SOURCE:" + member + " /SOURCEUSER:" + user 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }    
        /// <summary>
        /// Variation 3.2.22:    Display deployment status for all members–on the controller, 
        /// source is a member, all members have identical credentials, omit /SOURCEUSER
        /// </summary>
        public void Var3222()
        {
            string var = "Var 3.2.22";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /STATUS:ALL /SOURCE:" + member 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }     
        /// <summary>
        /// Variation 3.2.23:    Display deployment status for all members–on the controller
        /// , source is a member, all members have identical credentials, omit /SOURCEPASSWORD
        /// </summary>
        public void Var3223()
        {
            string var = "Var 3.2.23";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /STATUS:ALL /SOURCE:" + member 
                + " /SOURCEUSER:" + user;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.24 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.25 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.26 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.27 - 35 not implemented
        //*********************************************
        /// <summary>
        /// Variation 3.2.36:    LIST DEPLOYMENTS for all members of a 
        /// cluster to which a given member belongs
        /// </summary>
        public void Var3236()
        {
            string var = "Var 3.2.36";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /MANAGEMENTNIC:" + membermnic;

            string acargs = "DEPLOY /LISTDEPLOYMENTS /SOURCE:" + member + " /SOURCEUSER:" + user 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.37:    LIST DEPLOYMENTS for all members–on the 
        /// controller, source is the controller, all members have identical credentials, omit /SOURCE
        /// </summary>
        public void Var3237()
        {
            string var = "Var 3.2.37";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string acargs = "DEPLOY /LISTDEPLOYMENTS";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }

        //*********************************************
        //Var 3.2.38 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************
        //*********************************************
        //Var 3.2.39 not implemented
        //This variation is no longer valid, due to changes in CLT functionality.  
        //*********************************************
        /// <summary>
        /// Variation 3.2.40:    LIST DEPLOYMENTS for all members–on the 
        /// controller, source is a member, all members have identical credentials
        /// </summary>
        public void Var3240()
        {
            string var = "Var 3.2.40";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /LISTDEPLOYMENTS /SOURCE:" + member + " /SOURCEUSER:" + user 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.41:    LIST DEPLOYMENTS for all members–on the controller, 
        /// source is a member, all members have identical credentials, omit /SOURCEUSER
        /// </summary>
        public void Var3241()
        {
            string var = "Var 3.2.41";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /LISTDEPLOYMENTS /SOURCE:" + member 
                + " /SOURCEPASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.42:    LIST DEPLOYMENTS for all members–on the controller
        /// , source is a member, all members have identical credentials, omit /SOURCEPASSWORD
        /// </summary>
        public void Var3242()
        {
            string var = "Var 3.2.42";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string acargs = "DEPLOY /LISTDEPLOYMENTS /SOURCE:" + member 
                + " /SOURCEUSER:" + user;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }

        //*********************************************
        //Var 3.2.43 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.43 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.43 not implemented
        //*********************************************
        //*********************************************
        //Var 3.2.44 - 54 not implemented
        //*********************************************  
        /// <summary>
        /// Variation 3.2.55: Deploy applications to specific targets
        /// </summary>
        public void Var3255()
        {
            string var = "Var 3.2.55";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3255";
            string dep = "test1";

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller 
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.56: Deploy applications–invalid deployment name
        /// </summary>
        public void Var3256()
        {
            string var = "Var 3.2.56";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3256";
            //string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + invalid + " /SOURCE:" + controller 
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.57: Deploy applications–omit /DEPNAME
        /// </summary>
        public void Var3257()
        {
            string var = "Var 3.2.57";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3257";
            //string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /SOURCE:" + controller 
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }

        /// <summary>
        /// Variation 3.2.58: Deploy applications–omit the deployment name
        /// </summary>
        public void Var3258()
        {
            string var = "Var 3.2.58";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3258";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /SOURCE:" + controller + " /DEPNAME:"
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.59: Deploy applications–invalid source name
        /// </summary>
        public void Var3259()
        {
            string var = "Var 3.2.59";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3259";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /SOURCE:" + invalid + " /DEPNAME:" + dep
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.60: Deploy applications–omit /SOURCE.
        /// </summary>
        public void Var3260()
        {
            string var = "Var 3.2.60";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3260";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION    /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY    /START /DEPNAME:" + dep
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.61: Deploy applications–omit the source name
        /// </summary>
        public void Var3261()
        {
            string var = "Var 3.2.61";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3261";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:"
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        //*********************************************
        //Var 3.2.62 - 67 not implemented
        //*********************************************  
        /// <summary>
        /// Variation 3.2.68: Deploy applications–invalid target
        /// </summary>
        public void Var3268()
        {
            string var = "Var 3.2.68";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3268";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + invalid + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.69: Deploy applications–omit /TARGETS
        /// </summary>
        public void Var3269()
        {
            string var = "Var 3.2.69";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3269";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.70: Deploy applications–omit the target list
        /// </summary>
        public void Var3270()
        {
            string var = "Var 3.2.70";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3270";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.71: Deploy applications–invalid target username
        /// </summary>
        public void Var3271()
        {
            string var = "Var 3.2.71";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3271";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + invalid + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.72: Deploy applications–omit /TARGETUSER
        /// </summary>
        public void Var3272()
        {
            string var = "Var 3.2.72";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3272";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.73: Deploy applications–omit the target username
        /// </summary>
        public void Var3273()
        {
            string var = "Var 3.2.73";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3273";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.74 Deploy applications–invalid target password
        /// </summary>
        public void Var3274()
        {
            string var = "Var 3.2.74";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3274";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + invalid + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.75: Deploy applications–omit /TARGETPASSWORD
        /// </summary>
        public void Var3275()
        {
            string var = "Var 3.2.75";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3275";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user
                + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.76: Deploy applications–omit the target password
        /// </summary>
        public void Var3276()
        {
            string var = "Var 3.2.76";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3276";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.77: Deploy applications–invalid application list
        /// </summary>
        public void Var3277()
        {
            string var = "Var 3.2.77";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3277";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + invalid + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.78: Deploy applications–omit /APPNAME
        /// </summary>
        public void Var3278()
        {
            string var = "Var 3.2.78";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3278";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.79: Deploy applications–omit the application list
        /// </summary>
        public void Var3279()
        {
            string var = "Var 3.2.79";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3279";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.80: Deploy applications–all targets invalid
        /// </summary>
        public void Var3280()
        {
            string var = "Var 3.2.80";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3280";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + invalid + "," + invalid + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + app + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.81: Deploy applications–one invalid target in list
        /// </summary>
        public void Var3281()
        {
            string var = "Var 3.2.81";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3281";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + "," + invalid + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + invalid + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 3.2.82: Deploy applications–all application names invalid
        /// </summary>
        public void Var3282()
        {
            string var = "Var 3.2.82";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string membermnic = U.MemberManagementNIC;
            string app = "var3282";
            string dep = "test1";
            string invalid = U.Invalid;

            string setup = "APPLICATION /CREATE /MEMBER:" + controller + " /NAME:" + app;

            string acargs = "DEPLOY /START /DEPNAME:" + dep + " /SOURCE:" + controller
                + " /TARGETS:" + member + " /TARGETUSER:" + user + " /TARGETPASSWORD:"
                + password + " /APPNAME:" + invalid + " /WAIT";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }

        /// <summary>
        /// This class contains all implemented Variation of Application Center Command Line tests
        /// </summary>
    } 

    /// <summary>
    /// 
    /// </summary>
    class ClusterVars
    {
        private string controller;
        private string member;
        /// <summary>
        /// Constructor for class Vars
        /// </summary>
        /// <param name="Controller">AC Controller computer name</param>
        /// <param name="Member">AC Member computer name</param>
        public ClusterVars(string Controller, string Member)
        {
            this.controller = Controller;
            this.member = Member;
        }

        /// <summary>
        /// Variation 1.2.1:    Create a COM+ cluster loadbalanced by CLB
        /// </summary>
        public void Var121()
        {
            string var = "Var 1.2.1";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var121";

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:COMPlusApp /LOADBALANCING:CLB /MANAGEMENTNIC:" + managenic;
            string cleanup = "CLUSTER /CLEAN /KEEPIPS /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.2:    Create an COM+ cluster loadbalanced by NLB
        /// </summary>
        public void Var122()
        {
            string var = "Var 1.2.2";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var122";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:COMPlusApp /LOADBALANCING:NLB /CLUSTERIP:" + clusterip
                + " /CLUSTERIPSUBNETMASK:" + clustersubnet + " /LBNIC:" + clusterlbnic
                + " /MANAGEMENTNIC:" + managenic;
            string cleanup = "CLUSTER /CLEAN /KEEPIPS /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.3:    Create a web cluster with no loadbalancing
        /// </summary>
        public void Var123()
        {
            string var = "Var 1.2.3";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var123";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:NONE /MANAGEMENTNIC:" + managenic;
            string cleanup = "CLUSTER /CLEAN /KEEPIPS /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.4:    Create a web cluster with Other (3rd-party) loadbalancing
        /// </summary>
        public void Var124()
        {
            string var = "Var 1.2.4";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var124";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:OTHER /LBNIC:" + clusterlbnic + " /MANAGEMENTNIC:" + managenic;
            string cleanup = "CLUSTER /CLEAN /KEEPIPS /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.5:    Add a member to a cluster–specified controller is actually a member
        /// </summary>
        public void Var125()
        {
            string var = "var 1.2.5";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var125";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;

            string setup = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:NLB /CLUSTERIP:" + clusterip
                + " /CLUSTERIPSUBNETMASK:" + clustersubnet + " /LBNIC:" + clusterlbnic
                + " /MANAGEMENTNIC:" + managenic;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + member + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + managenic;

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.7:    Add a member to a cluster–controller is unreachable
        /// </summary>
        public void Var127()
        {
            string var = "var 1.2.7";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + invalid + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.8:    Add a member to a cluster–bad member name
        /// </summary>
        public void Var128()
        {
            string var = "var 1.2.8";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + invalid +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.9:    Add a member to a cluster–member is not reachable
        /// </summary>
        public void Var129()
        {
            string var = "var 1.2.9";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + invalid +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.10:    Add a member to a cluster–bad username and password
        /// </summary>
        public void Var1210()
        {
            string var = "var 1.2.10";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + invalid + " /PASSWORD:" + invalid + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.11:    Add a member to a cluster–bad username
        /// </summary>
        public void Var1211()
        {
            string var = "var 1.2.11";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + invalid + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.12:    Add a member to a cluster–bad password
        /// </summary>
        public void Var1212()
        {
            string var = "var 1.2.12";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + invalid + " /DEDICATEDIP:" + invalid
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.13:    Add a member to a cluster–omit domain from username
        /// </summary>
        public void Var1213()
        {
            string var = "var 1.2.13";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.AccountNODomain;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.14:    Add a member to a cluster–invalid dedicated IP address
        /// </summary>
        public void Var1214()
        {
            string var = "var 1.2.14";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + invalid
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.17:    Add a member to a cluster–invalid subnet mask
        /// </summary>
        public void Var1217()
        {
            string var = "var 1.2.17";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.InvalidIP;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.18:    Add a member to a cluster–valid subnet mask, but not on the member
        /// </summary>
        public void Var1218()
        {
            string var = "var 1.2.18";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.SubNetNotOnComputer;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.19:    Add a member to a cluster–subnet mask is 0.0.0.0
        /// </summary>
        public void Var1219()
        {
            string var = "var 1.2.19";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.ZeroIP;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.20:    Add a member to a cluster–invalid LB NIC ID
        /// </summary>
        public void Var1220()
        {
            string var = "var 1.2.20";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = "300";
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }

        /// <summary>
        /// Variation 1.2.21:    Add a member to a cluster–invalide management NIC ID
        /// </summary>
        public void Var1221()
        {
            string var = "var 1.2.21";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = "300";
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.22:    Add a member to a cluster–LB NIC ID and management NIC ID are the same
        /// , a valid LB NIC (i.e., can be used as an LB NIC).  
        /// </summary>
        public void Var1222()
        {
            string var = "var 1.2.22";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberLBNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.23:    Add a member to a cluster–LB NIC ID and management NIC ID are the same
        /// , an invalid LB NIC (i.e., cannot be used as an LB NIC)
        /// </summary>
        public void Var1223()
        {
            string var = "var 1.2.23";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = "300";
            string membermannic = "300";
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.24:    Add a member to a cluster–omit /CONTROLLER, execute on the controller
        /// </summary>
        public void Var1224()
        {
            string var = "var 1.2.24";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.26:    Add a member to a cluster–omit /MEMBER, execute on the controller
        /// </summary>
        public void Var1226()
        {
            string var = "var 1.2.26";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.29:    Add a member to a cluster–omit /USER, 
        /// all members require the same credentials
        /// </summary>
        public void Var1229()
        {
            string var = "var 1.2.29";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.30:    Add a member to a cluster–omit /PASSWORD,
        /// all members require the same credentials
        /// </summary>
        public void Var1230()
        {
            string var = "var 1.2.30";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.33:    Add a member to a cluster–omit /DEDICATEDIP 
        /// and provide /DEDICATEDIPSUBNETMASK
        /// </summary>
        public void Var1233()
        {
            string var = "var 1.2.33";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIPSUBNETMASK:" + membersubnet
                + " /LBNIC:" + memberlbnic + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.34:    Add a member to a cluster–omit /DEDICATEDIPSUBNETMASK
        /// </summary>
        public void Var1234()
        {
            string var = "var 1.2.34";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /LBNIC:" + memberlbnic + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.35:    Add a member to a cluster–omit /LBNIC
        /// </summary>
        public void Var1235()
        {
            string var = "var 1.2.35";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.36:    Add a member to a cluster–omit /MANAGEMENTNIC
        /// </summary>
        public void Var1236()
        {
            string var = "var 1.2.36";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member +
                " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.37:    Add a member to a cluster–no synchronization, member is offline
        /// </summary>
        public void Var1237()
        {
            string var = "var 1.2.37";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic + " /NOSYNCONADD /DISABLELOADBALANCING";

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user +
                " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.38:    Add a member to a cluster–omit value for /CONTROLLER
        /// </summary>
        public void Var1238()
        {
            string var = "var 1.2.38";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER: /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.39:    Add a member to a cluster–omit value for /MEMBER
        /// </summary>
        public void Var1239()
        {
            string var = "var 1.2.39";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:"
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.40:    Add a member to a cluster–omit value for /USER
        /// </summary>
        public void Var1240()
        {
            string var = "var 1.2.40";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER: /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.41:    Add a member to a cluster–omit value for /PASSWORD
        /// </summary>
        public void Var1241()
        {
            string var = "var 1.2.41";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD: /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.42:    Add a member to a cluster–omit value for /DEDICATEDIP
        /// </summary>
        public void Var1242()
        {
            string var = "var 1.2.42";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:"
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.43:    Add a member to a cluster–omit value for /DEDICATEDIPSUBNETMASK
        /// </summary>
        public void Var1243()
        {
            string var = "var 1.2.43";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK: /LBNIC:" + memberlbnic
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.44:    Add a member to a cluster–omit value for /LBNIC
        /// </summary>
        public void Var1244()
        {
            string var = "var 1.2.44";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.45:    Add a member to a cluster–omit value for /MANAGEMENTNIC
        /// </summary>
        public void Var1245()
        {
            string var = "var 1.2.45";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.46:    Remove a member from a cluster–invalid computer name
        /// </summary>
        public void Var1246()
        {
            string var = "var 1.2.46";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + invalid + " /USER:" + user
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.47:    Remove a member from a cluster–specify controller as member name
        /// </summary>
        public void Var1247()
        {
            string var = "var 1.2.47";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + controller + " /USER:" + user
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.49:    Remove a member from a cluster–bad username and password
        /// </summary>
        public void Var1249()
        {
            string var = "var 1.2.49";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + invalid
                + " /PASSWORD:" + invalid;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }

        /// <summary>
        /// Variation 1.2.50:    Remove a member from a cluster–bad username
        /// </summary>
        public void Var1250()
        {
            string var = "var 1.2.50";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + invalid
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.51:    Remove a member from a cluster–bad password
        /// </summary>
        public void Var1251()
        {
            string var = "var 1.2.51";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + invalid;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.52:    Remove a member from a cluster–omit value for /MEMBER
        /// </summary>
        public void Var1252()
        {
            string var = "var 1.2.52";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER: /USER:" + user
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.53:    Remove a member from a cluster–omit value for /USER
        /// </summary>
        public void Var1253()
        {
            string var = "var 1.2.53";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:"
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.54:    Remove a member from a cluster–omit value for /PASSWORD
        /// </summary>
        public void Var1254()
        {
            string var = "var 1.2.54";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:";

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.55:    Remove a member from a cluster–omit /MEMBER, execute on controller
        /// </summary>
        public void Var1255()
        {
            string var = "var 1.2.55";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /USER:" + user
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.62:    Remove a member from a cluster–forced removal,
        /// execute on member, controller is available
        /// </summary>
        public void Var1262()
        {
            string var = "var 1.2.62";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /FORCE" + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.64:    List the members of a cluster, from a member
        /// </summary>
        public void Var1264()
        {
            string var = "var 1.2.64";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /LISTMEMBERS";

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup,acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.68:    Set the cluster controller–bad username
        /// </summary>
        public void Var1268()
        {
            string var = "var 1.2.68";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /SETCONTROLLER /MEMBER:" + member + " /USER:" + invalid
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.69:    Set the cluster controller–bad password
        /// </summary>
        public void Var1269()
        {
            string var = "var 1.2.69";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /SETCONTROLLER /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + invalid;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.70:    Set the cluster controller–omit value for /USER
        /// </summary>
        public void Var1270()
        {
            string var = "var 1.2.70";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /SETCONTROLLER /MEMBER:" + member + " /USER:"
                + " /PASSWORD:" + password;

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.71:    Set the cluster controller–omit value for /PASSWORD
        /// </summary>
        public void Var1271()
        {
            string var = "var 1.2.71";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string setup = "CLUSTER /ADD /CONTROLLER:" + controller + " /MEMBER:" + member
                + " /USER:" + user + " /PASSWORD:" + password + " /DEDICATEDIP:" + memberip
                + " /DEDICATEDIPSUBNETMASK:" + membersubnet + " /LBNIC:"
                + " /MANAGEMENTNIC:" + membermannic;

            string acargs = "CLUSTER /SETCONTROLLER /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:";

            string cleanup = "CLUSTER /REMOVE /MEMBER:" + member + " /USER:" + user
                + " /PASSWORD:" + password + " /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.72:    Set the cluster controller–specify existing controller as new controller
        /// </summary>
        public void Var1272()
        {
            string var = "var 1.2.72";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = true;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /SETCONTROLLER /MEMBER:" + controller + " /USER:" + user
                + " /PASSWORD:" + password;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.73:    Set the cluster controller–specify existing controller as new controller
        /// , use bad credentials
        /// </summary>
        public void Var1273()
        {
            string var = "var 1.2.73";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string memberip = U.MemberIPAddress;
            string membersubnet = U.MemberSubnetMask;
            string memberlbnic = U.MemberLBNIC;
            string membermannic = U.MemberManagementNIC;
            string user = U.TestAccount;
            string password = U.TestPassword;
            string invalid = U.Invalid;

            string acargs = "CLUSTER /SETCONTROLLER /MEMBER:" + controller + " /USER:" + invalid
                + " /PASSWORD:" + invalid;

            string cleanup = "CLUSTER /CLEAN /KEEPIPS /Y";

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn, cleanup);
        }
        /// <summary>
        /// Variation 1.2.74:    Create a web cluster loadbalanced by NLB–specified 
        /// controller name is already a controller
        /// </summary>
        public void Var1274()
        {
            string var = "Var 1.2.74";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var122";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;

            string setup = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:NLB /CLUSTERIP:" + clusterip
                + " /CLUSTERIPSUBNETMASK:" + clustersubnet + " /LBNIC:" + clusterlbnic
                + " /MANAGEMENTNIC:" + managenic;

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:NLB /CLUSTERIP:" + clusterip
                + " /CLUSTERIPSUBNETMASK:" + clustersubnet + " /LBNIC:" + clusterlbnic
                + " /MANAGEMENTNIC:" + managenic;

            string cleanup = "CLUSTER /CLEAN /KEEPIPS /Y";

            ExecVars E = new ExecVars();
            E.Run(var, setup, acargs, expectedreturn, cleanup);
        }

        //*********************************************
        //Var 1.2.75 not implemented
        //*********************************************

        //*********************************************
        //Var 1.2.76 not implemented
        //*********************************************

        //*********************************************
        //Var 1.2.77 not implemented
        //*********************************************

        /// <summary>
        /// Create a web cluster loadbalanced by NLB–invalid cluster IP address
        /// </summary>
        public void Var1278()
        {
            string var = "Var 1.2.78";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var122";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;
            string badip = U.InvalidIP;

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:NLB /CLUSTERIP:" + badip
                + " /CLUSTERIPSUBNETMASK:" + clustersubnet + " /LBNIC:" + clusterlbnic
                + " /MANAGEMENTNIC:" + managenic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
        /// <summary>
        /// Variation 1.2.79:    Create a web cluster loadbalanced by NLB–invalid cluster IP subnet mask
        /// </summary>
        public void Var1279()
        {
            string var = "Var 1.2.79";
            string controller = this.controller;
            string member = this.member;
            bool expectedreturn = false;

            Utilities U = new Utilities(controller, member);
            string managenic = U.ControllerManagementNIC;
            string cluster = "Var122";
            string clusterip = U.ControllerIPAddress;
            string clustersubnet = U.ControllerSubnetMask;
            string clusterlbnic = U.ControllerLBNIC;
            string badip = U.InvalidIP;

            string acargs = "CLUSTER /CREATE /CONTROLLER:" + controller + " /NAME:" + cluster +
                " /TYPE:WEB /LOADBALANCING:NLB /CLUSTERIP:" + clusterip
                + " /CLUSTERIPSUBNETMASK:" + badip + " /LBNIC:" + clusterlbnic
                + " /MANAGEMENTNIC:" + managenic;

            ExecVars E = new ExecVars();
            E.Run(var, acargs, expectedreturn);
        }
    }
    /// <summary>
    /// Class to execute CLT vars
    /// </summary>
    class ExecVars
    {
        /// <summary>
        /// Runs a CLT var
        /// </summary>
        /// <param name="VarNumber">var number</param>
        /// <param name="VarCMD">AC.EXE command parameters</param>
        /// <param name="ExpectedReturn">Expected return code from the command.
        /// This value is true if the command should Succeed
        /// , and false if the command should fail</param>
        public void Run(string VarNumber, string VarCMD, bool ExpectedReturn)
        {
            string varnumber = VarNumber;
            string varcmd = VarCMD;
            bool expectedretrun = ExpectedReturn;
            int ReturnCode;

            Utilities U = new Utilities();
        
            if (expectedretrun)
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode != 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            else
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode == 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            U.Log("", false);
        }
        /// <summary>
        /// Runs a CLT var
        /// </summary>
        /// <param name="VarNumber">var number</param>
        /// <param name="SetupCMD">AC.EXE commmand parameters to setup the test</param>
        /// <param name="VarCMD">AC.EXE command parameters to run the test</param>
        /// <param name="ExpectedReturn">Expected return code from the command.
        /// This value is true if the command should Succeed
        /// , and false if the command should fail</param>
        public void Run(string VarNumber, string SetupCMD, string VarCMD,bool ExpectedReturn)
        {
            string varnumber = VarNumber;
            string varcmd = VarCMD;
            bool expectedretrun = ExpectedReturn;
            string setup = SetupCMD;
            int ReturnCode;

            Utilities U = new Utilities();

            ReturnCode = U.Exec("AC", setup);
            if (ReturnCode != 0)
            {
                U.Log("Setup for " + VarNumber + " Failed");
            }
            else
            {
                U.Log("Setup Up for " + VarNumber + " Passed");
            }
        
            if (expectedretrun)
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode != 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            else
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode == 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            U.Log("", false);
        }
        /// <summary>
        /// Runs a CLT var
        /// </summary>
        /// <param name="VarNumber">var number</param>
        /// <param name="VarCMD">AC.EXE command parameters to run the test</param>
        /// <param name="ExpectedReturn">Expected return code from the command.
        /// This value is true if the command should Succeed
        /// , and false if the command should fail</param>
        /// <param name="CleanupCMD">AC.EXE commmand parameters to clean up the test</param>
        public void Run(string VarNumber,string VarCMD,bool ExpectedReturn, string CleanupCMD)
        {
            string varnumber = VarNumber;
            string varcmd = VarCMD;
            bool expectedretrun = ExpectedReturn;
            string cleanup = CleanupCMD;
            int ReturnCode;

            Utilities U = new Utilities();
        
            if (expectedretrun)
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode != 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            else
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode == 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }

            ReturnCode = U.Exec("AC", cleanup);
            if (ReturnCode != 0)
            {
                U.Log("Clean Up for " + VarNumber + " Failed");
            }
            else
            {
                U.Log("Clean Up for " + VarNumber + " Passed");
            }

            U.Log("", false);
        }
        /// <summary>
        /// Runs a CLT var
        /// </summary>
        /// <param name="VarNumber">var number</param>
        /// <param name="SetupCMD">AC.EXE commmand parameters to setup the test</param>
        /// <param name="VarCMD">AC.EXE command parameters to run the test</param>
        /// <param name="ExpectedReturn">Expected return code from the command.
        /// This value is true if the command should Succeed
        /// , and false if the command should fail</param>
        /// <param name="CleanupCMD">AC.EXE commmand parameters to clean up the test</param>
        public void Run(string VarNumber, string SetupCMD,string VarCMD, bool ExpectedReturn,
            string CleanupCMD)
        {
            string varnumber = VarNumber;
            string varcmd = VarCMD;
            bool expectedretrun = ExpectedReturn;
            string cleanup = CleanupCMD;
            string setup = SetupCMD;
            int ReturnCode;

            Utilities U = new Utilities();
        
            //setup code
            U.Log("Executing setup command");
            ReturnCode = U.Exec("AC", setup);
            if (ReturnCode != 0)
            {
                U.Log("Setup for " + VarNumber + " Failed");
            }
            else
            {
                U.Log("Setup for " + VarNumber + " Passed");
            }

            //run var
            if (expectedretrun)
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode != 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            else
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode == 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
        
            //cleanup code
            ReturnCode = U.Exec("AC", cleanup);
            if (ReturnCode != 0)
            {
                U.Log("Clean Up for " + VarNumber + " Failed");
            }
            else
            {
                U.Log("Clean Up for " + VarNumber + " Passed");
            }
            U.Log("", false);
        }

        /// <summary>
        /// Runs a CLT var
        /// </summary>
        /// <param name="VarNumber">var number</param>
        /// <param name="SetupCMD">AC.EXE commmand parameters to setup the test</param>
        /// <param name="VarCMD">AC.EXE command parameters to run the test</param>
        /// <param name="ExpectedReturn">Expected return code from the command.
        /// This value is true if the command should Succeed
        /// , and false if the command should fail</param>
        /// <param name="CleanupCMD">AC.EXE commmand parameters to clean up the test</param>
        public void Run(string VarNumber, string[] SetupCMD,string VarCMD, bool ExpectedReturn,
            string[] CleanupCMD)
        {
            string varnumber = VarNumber;
            string varcmd = VarCMD;
            bool expectedretrun = ExpectedReturn;
            string[] cleanup = CleanupCMD;
            string[] setup = SetupCMD;
            int ReturnCode;

            Utilities U = new Utilities();
        
            //setup code
            U.Log("Executing setup commands");
            foreach (string s in setup)
            {
                ReturnCode = U.Exec("AC", s);
                if (ReturnCode != 0)
                {
                    U.Log("Setup command " + s + " for " + VarNumber + " Failed");
                }
                else
                {
                    U.Log("Setup command " + s + " for " + VarNumber + " Passed");
                }
            }

            //run var
            if (expectedretrun)
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode != 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
            else
            {
                U.Log(varnumber);
                ReturnCode = U.Exec("AC", varcmd);
                if (ReturnCode == 0)
                {
                    U.Log(VarNumber + " - VAR_FAIL");
                }
                else
                {
                    U.Log(VarNumber + " - VAR_PASS");
                }
            }
        
            //cleanup code
            foreach (string c in cleanup)
            {
                ReturnCode = U.Exec("AC", c);
                if (ReturnCode != 0)
                {
                    U.Log("Clean Up command " + c + " for " + VarNumber + " Failed");
                }
                else
                {
                    U.Log("Clean Up command " + c + " for " + VarNumber + " Passed");
                }
            }
            U.Log("", false);
        }

    }
    /// <summary>
    /// logically group tests together and executes them
    /// </summary>
    class TestGroups
    {
        /// <summary>
        /// Runs AC Cluster tests on the cluster controller
        /// </summary>
        /// <param name="Controller">AC Controller computer name</param>
        /// <param name="Member">AC Member computer name</param>
        public void ClusterTests(string Controller, string Member)
        {
            Console.WriteLine("Starting AC CLT Cluster tests");
            string controller = Controller;
            string member = Member;
        
            //check for the log file and delete if it exists
            Utilities Util = new Utilities();
            string logfile = Util.LogFile;
            if(File.Exists(logfile))
            {
                File.Delete(logfile);
            }
        
            //start log
            Util.Log("*LOG_START*-CLTtests", false);

            //run the tests
            ClusterVars CLT = new ClusterVars(controller, member);
            CLT.Var121();
            CLT.Var122();
            CLT.Var123();
            CLT.Var124();
            CLT.Var125();
            CLT.Var127();
            CLT.Var128();
            CLT.Var129();
            CLT.Var1210();
            CLT.Var1211();
            CLT.Var1212();
            CLT.Var1213();
            CLT.Var1214();
            CLT.Var1217();
            CLT.Var1218();
            CLT.Var1219();
            CLT.Var1220();
            CLT.Var1221();
            CLT.Var1222();
            CLT.Var1223();
            CLT.Var1224();
            CLT.Var1226();
            CLT.Var1229();
            CLT.Var1230();
            CLT.Var1233();
            CLT.Var1234();
            CLT.Var1235();
            CLT.Var1236();
            CLT.Var1237();
            CLT.Var1238();
            CLT.Var1239();
            CLT.Var1240();
            CLT.Var1241();
            CLT.Var1242();
            CLT.Var1243();
            CLT.Var1244();
            CLT.Var1245();
            CLT.Var1246();
            CLT.Var1247();
            CLT.Var1249();
            CLT.Var1250();
            CLT.Var1251();
            CLT.Var1252();
            CLT.Var1253();
            CLT.Var1254();
            CLT.Var1255();
            CLT.Var1262();
            CLT.Var1264();
            CLT.Var1268();
            CLT.Var1269();
            CLT.Var1270();
            CLT.Var1271();
            CLT.Var1272();
            CLT.Var1273();


            //end log
            Util.Log("*LOG_DONE*", false);
        }
        /// <summary>
        /// Runs AC Cluster tests on the cluster member
        /// </summary>
        /// <param name="Controller">AC Controller computer name</param>
        /// <param name="Member">AC Member computer name</param>
        public void ClusterMemberTests(string Controller, string Member)
        {
            Console.WriteLine("Starting AC CLT Cluster tests");
            string controller = Controller;
            string member = Member;
        
            //check for the log file and delete if it exists
            Utilities Util = new Utilities();
            string logfile = Util.LogFile;
            if(File.Exists(logfile))
            {
                File.Delete(logfile);
            }
        
            //start log
            Util.Log("*LOG_START*-CLTtests", false);

            //run the tests
            ClusterVars CLT = new ClusterVars(controller, member);
            //CLT.Var121();

            //end log
            Util.Log("*LOG_DONE*", false);
        }

        /// <summary>
        /// Runs AC Deploy tests on the cluster controller
        /// </summary>
        /// <param name="Controller">AC Controller computer name</param>
        /// <param name="Member">AC Member computer name</param>
        public void DeployTests(string Controller, string Member)
        {
            Console.WriteLine("Starting AC CLT Cluster tests");
            string controller = Controller;
            string member = Member;
        
            //check for the log file and delete if it exists
            Utilities Util = new Utilities();
            string logfile = Util.LogFile;
            if(File.Exists(logfile))
            {
                File.Delete(logfile);
            }
        
            //start log
            Util.Log("*LOG_START*-CLTtests", false);

            //run the tests
            DeployVars Deploy = new DeployVars(controller, member);
            Deploy.Var321();
            Deploy.Var325();
            Deploy.Var327();
            Deploy.Var329();
            Deploy.Var3213();
            Deploy.Var3215();
            Deploy.Var3217();
            Deploy.Var3218();
            Deploy.Var3221();
            Deploy.Var3222();
            Deploy.Var3223();
            Deploy.Var3236();
            Deploy.Var3237();
            Deploy.Var3240();
            Deploy.Var3241();
            Deploy.Var3242();
            Deploy.Var3255();
            Deploy.Var3256();
            Deploy.Var3257();
            Deploy.Var3258();
            Deploy.Var3259();
            Deploy.Var3260();
            Deploy.Var3261();
            Deploy.Var3268();
            Deploy.Var3269();
            Deploy.Var3270();
            Deploy.Var3271();
            Deploy.Var3272();
            Deploy.Var3273();
            Deploy.Var3274();
            Deploy.Var3275();
            Deploy.Var3276();
            Deploy.Var3277();
            Deploy.Var3278();
            Deploy.Var3279();
            Deploy.Var3280();
            Deploy.Var3281();
            Deploy.Var3282();

            //end log
            Util.Log("*LOG_DONE*", false);
        }
    }
    /// <summary>
    /// The Utilities class provide standard methods and properties that are
    /// required to run CLT
    /// </summary>
    class Utilities
    {
        //private variables
        private string invalid = @"!@#$%^&*()";
        private string member;
        private string controller;
        private string controllerMNIC;
        private string controllerLNIC;
        private string controllerIP;
        private string controllerSubnet;
        private string memberMNIC;
        private string memberLNIC;
        private string memberIP;
        private string memberSubnet;
        private string testaccount;
        private string testpassword;
        private string accountnodomain;
        private string logfile;
        private string zeroip;
        private string invalidip;
        private string ipnoton;
        private string subnetnoton;

        /// <summary>
        /// Utilities overloaded constructor 
        /// </summary>
        /// <param name="Controller">AC Controller computer name</param>
        /// <param name="Member">AC Member computer name</param>
        public Utilities(string Controller, string Member)
        {
            this.controller = Controller;
            this.member = Member;

            try
            {
                //Get IP info for controller NIC
                NetworkAdaptor ConNIC = new NetworkAdaptor();
                Hashtable ConNicInfo = new Hashtable();
                ConNicInfo = ConNIC.GetNICinfo(this.controller);
                this.controllerMNIC = ConNicInfo["ManagementNIC"].ToString();
                this.controllerLNIC = ConNicInfo["LBNIC"].ToString();
                this.controllerIP = ConNicInfo["STATIC_IP0"].ToString();
                this.controllerSubnet = ConNicInfo["STATIC_SUBNET0"].ToString();

                //Get IP info for member NIC
                NetworkAdaptor MemNIC = new NetworkAdaptor();
                Hashtable MemNicInfo = new Hashtable();
                MemNicInfo = MemNIC.GetNICinfo(this.member);
                this.memberMNIC = MemNicInfo["ManagementNIC"].ToString();
                this.memberLNIC = MemNicInfo["LBNIC"].ToString();
                this.memberIP = MemNicInfo["STATIC_IP0"].ToString();
                this.memberSubnet = MemNicInfo["STATIC_SUBNET0"].ToString();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("One or more values were not found in WMI");
                Console.WriteLine("Please makse sure that both computers have to static IP Address");
                Console.WriteLine("If unsure run ipset release and ipset get");
                Environment.Exit(1);
            }
            //Get CDS User info
            try
            {
                CDSComNET.Users CDS = new CDSComNET.UsersClass();
                this.testaccount = @"smx\asttest";
                object account = (object)this.testaccount;
                this.testpassword = (string)CDS.GetPassword(ref account);
            }
            catch(Exception)
            {
                Console.WriteLine("There was an error creating or connecting to CDS");
                Console.WriteLine("Please make sure that CDScom.dll is registered");
                Environment.Exit(1);
            }
            
            this.accountnodomain = "asttest";
            this.zeroip = "0.0.0.0";
            this.invalidip = "300.300.300.300";
            this.ipnoton = "192.168.1.1";
            this.subnetnoton = "255.0.0.0";
            this.logfile = "AC_CLT.log";
        }
        /// <summary>
        /// Utilities constructor that take on parameters
        /// </summary>
        public Utilities()
        {
            this.logfile = "AC_CLT.log";
        }
        /// <summary>
        /// Provide a string of invalid chars
        /// </summary>
        public string Invalid
        {
            get
            {
                return invalid;
            }
        }
        /// <summary>
        /// Controller Management NIC index ID
        /// </summary>
        public string ControllerManagementNIC
        {
            get
            {
                return controllerMNIC;
            }
        }
        /// <summary>
        /// Controller LoadBalancing NIC index ID
        /// </summary>
        public string ControllerLBNIC
        {
            get
            {
                return controllerLNIC;
            }
        }
        /// <summary>
        /// Controller static IP Address
        /// </summary>
        public string ControllerIPAddress
        {
            get
            {
                return controllerIP;
            }
        }
        /// <summary>
        /// Controller static Subnet Mask
        /// </summary>
        public string ControllerSubnetMask
        {
            get
            {
                return controllerSubnet;
            }
        }

        /// <summary>
        /// Member Management NIC index ID
        /// </summary>
        public string MemberManagementNIC
        {
            get
            {
                return memberMNIC;
            }
        }
        /// <summary>
        /// Member LoadBalancing NIC index ID
        /// </summary>
        public string MemberLBNIC
        {
            get
            {
                return memberLNIC;
            }
        }
        /// <summary>
        /// Member static IP Address
        /// </summary>
        public string MemberIPAddress
        {
            get
            {
                return memberIP;
            }
        }
        /// <summary>
        /// Member static Subnet Mask
        /// </summary>
        public string MemberSubnetMask
        {
            get
            {
                return memberSubnet;
            }
        }
        /// <summary>
        /// Default test account on smx.net domain
        /// smx\asttest
        /// </summary>
        public string TestAccount
        {
            get
            {
                return testaccount;
            }
        }
        /// <summary>
        /// Password for smx\asttest which is obtained through CDS
        /// </summary>
        public string TestPassword
        {
            get
            {
                return testpassword;
            }
        }
        /// <summary>
        /// asttest
        /// </summary>
        public string AccountNODomain
        {
            get
            {
                return accountnodomain;
            }
        }
        /// <summary>
        /// An invalid IP Address
        /// </summary>
        public string InvalidIP
        {
            get
            {
                return invalidip;
            }
        }
        /// <summary>
        /// An all zeros IP Address
        /// </summary>
        public string ZeroIP
        {
            get
            {
                return zeroip;
            }
        }
        /// <summary>
        /// A valid IP Address not on the computer
        /// </summary>
        public string IpNotOnComputer
        {
            get
            {
                return ipnoton;
            }
        }
        /// <summary>
        /// A valid Subnet Mask not on the computer
        /// </summary>
        public string SubNetNotOnComputer
        {
            get
            {
                return subnetnoton;
            }
        }
        /// <summary>
        /// Name of the log file
        /// </summary>
        public string LogFile
        {
            get
            {
                return logfile;
            }
        }
        /// <summary>
        /// Writes strings to a log file
        /// </summary>
        /// <param name="LogEntry">Text string to be written to the log file</param>
        public void Log(string LogEntry)
        {
            try
            {
                StreamWriter LogStream = new StreamWriter(LogFile,true);
                LogStream.WriteLine(System.DateTime.Now + " " + LogEntry);
                Console.WriteLine(System.DateTime.Now + " " +LogEntry);
                LogStream.Close();
            }
            catch(FileLoadException e)
            {
                Console.WriteLine("File " + logfile + " did not load.");
                Console.WriteLine("An Exception occurred" + e.Message);
            }
        }
        /// <summary>
        /// Writes strings to a log file
        /// </summary>
        /// <param name="LogEntry">Text string to be written to the log file</param>
        /// <param name="Time">Log time</param>
        public void Log(string LogEntry, bool Time)
        {
            try
            {
                bool time = Time;
                if(time)
                {
                    StreamWriter LogStream = new StreamWriter(LogFile,true);
                    LogStream.WriteLine(System.DateTime.Now + " " + LogEntry);
                    Console.WriteLine(System.DateTime.Now + " " + LogEntry);
                    LogStream.Close();
                }
                else
                {
                    StreamWriter LogStream = new StreamWriter(LogFile,true);
                    LogStream.WriteLine(LogEntry);
                    Console.WriteLine(LogEntry);
                    LogStream.Close();
                }
            }
            catch(FileLoadException e)
            {
                Console.WriteLine("File " + logfile + " did not load.");
                Console.WriteLine("An Exception occurred" + e.Message);
            }
        }
        /// <summary>
        /// Executes an command or application in a command shell.
        /// </summary>
        /// <param name="Command">Name of the command or application</param>
        /// <param name="Parms">Parameters to be passed to the command or application</param>
        /// <returns></returns>
        public int Exec(string Command, string Parms)
        {
            int Code;
            Process CMD = new Process();
            CMD.StartInfo.FileName = Command;
            CMD.StartInfo.Arguments = Parms;
            CMD.StartInfo.UseShellExecute = false;
            CMD.StartInfo.RedirectStandardOutput = true;
            Log("Executing " + Command + " " + Parms);
            CMD.Start();
            CMD.WaitForExit();
            string output = CMD.StandardOutput.ReadToEnd();
            Log(output, false);
            Code = CMD.ExitCode;
            Log("Command return code is:" + Code);
            return Code;
        }
    }
    /// <summary>
    /// This class provide network adaptor information
    /// such ass ip address, subnet mask, and nic index ID
    /// </summary>
    class NetworkAdaptor
    {
        /// <summary>
        /// Get NIC info and returns it in a hastable
        /// </summary>
        /// <param name="ComputerName">Name of the computer you want nic info for</param>
        /// <returns>hastable with nic info</returns>
        public Hashtable GetNICinfo(string ComputerName)
        {
            char Quote = (char)34;
            string WMIQuery = "Select * From Win32_NetworkAdapterConfiguration Where IPEnabled=" 
                + Quote + "True" + Quote;
            string WMIScope = @"\\" + ComputerName + @"\root\CimV2";

            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
                (WMIScope,WMIQuery);
        
            Hashtable MyHash = new Hashtable();
            foreach (ManagementObject NetCard in WMISearcher.Get())
            {
                if (NetCard["DHCPEnabled"].ToString() != "True")
                {
                    MyHash.Add("LBNIC", NetCard["Index"]);
                    string[] IPS = (string[])NetCard["IPAddress"];
                    string[] Subnets = (string[])NetCard["IPSubnet"];
                
                    int i=0;
                    while (i < IPS.Length - 1)
                    {
                        MyHash.Add("STATIC_IP" + i, IPS[i]);
                        MyHash.Add("STATIC_SUBNET" + i, Subnets[i]);
                        i++;
                    }
                }
                else
                {
                    MyHash.Add("ManagementNIC", NetCard["Index"]);
                }
            }
            return MyHash;
        }
        /// <summary>
        /// Get NIC info and returns it in a hastable
        /// </summary>
        /// <returns>hastable with nic info</returns>
        public Hashtable GetNICinfo()
        {
            char Quote = (char)34;
            string WMIQuery = "Select * From Win32_NetworkAdapterConfiguration Where IPEnabled=" 
                + Quote + "True" + Quote;

            ManagementObjectSearcher WMISearcher = new ManagementObjectSearcher
                (WMIQuery);

            Hashtable MyHash = new Hashtable();
            foreach (ManagementObject NetCard in WMISearcher.Get())
            {
                if (NetCard["DHCPEnabled"].ToString() != "True")
                {
                    MyHash.Add("LBNIC", NetCard["Index"]);
                    string[] IPS = (string[])NetCard["IPAddress"];
                    string[] Subnets = (string[])NetCard["IPSubnet"];
                
                    int i=0;
                    while (i < IPS.Length - 1)
                    {
                        MyHash.Add("STATIC_IP" + i, IPS[i]);
                        MyHash.Add("STATIC_SUBNET" + i, Subnets[i]);
                        i++;
                    }
                }
                else
                {
                    MyHash.Add("ManagementNIC", NetCard["Index"]);
                }
            }
            return MyHash;
        }
    }

    /// <summary>
    /// Summary description for Start.
    /// </summary>
    class Start
    {
        /// <summary>
        /// Displays usage
        /// </summary>
        static void Usage()
        {
            Console.WriteLine("AC Command Line testing");
            Console.WriteLine("This test asumes:");
            Console.WriteLine("that you have two computers");
            Console.WriteLine("that both computers are on the SMX.NET network and domain");
            Console.WriteLine("that both computers have two NIC cards,");
            Console.WriteLine("that both computers Have run ipset get to get static IP Addresses");
            Console.WriteLine("that both computers Have been built using Oasis");
            Console.WriteLine("that AC10 or above is installed on both computers");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("CLTtests.exe Computer1 Computer2 ExecuteOn Test");
            Console.WriteLine("Valid entries for ExecuteOn");
            Console.WriteLine("CONTROLLER - to run test on the controller(Computer1)");
            Console.WriteLine("MEMBER - to run test on the member(Computer2)");
            Console.WriteLine("Valid entries for Test");
            Console.WriteLine("CLUSTER - for AC Cluster tests");
            Console.WriteLine("DEPLOY - for AC Deploy tests");
            Console.WriteLine("CLB - for AC CLB tests");
            Console.WriteLine("APPLICATION - for AC Application tests");
            Console.WriteLine("Example:");
            Console.WriteLine("CLTtests.exe deangj02e deangj03e CONTROLLER CLUSTER");
            Environment.Exit(0);
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Usage();
            }
            if (args[0] == "/?")
            {
                Usage();
            }
            if (args[0] == "?")
            {
                Usage();
            }
            string Controller = args[0];
            string Member = args[1];
            string ExecOn = args[2];
            string Tests = args[3];

            if(ExecOn.ToUpper() == "CONTROLLER" && Tests.ToUpper() == "CLUSTER")
            {
                TestGroups t = new TestGroups();
                t.ClusterTests(Controller, Member);
            }
            if(ExecOn.ToUpper() == "MEMBER" && Tests.ToUpper() == "CLUSTER")
            {
                TestGroups t = new TestGroups();
                t.ClusterMemberTests(Controller, Member);
            }
            if(ExecOn.ToUpper() == "CONTROLLER" && Tests.ToUpper() == "DEPLOY")
            {
                TestGroups t = new TestGroups();
                t.DeployTests(Controller, Member);
            }
//            if(ExecOn == "MEMBER" | Tests == "DEPLOY")
//            {
//                TestGroups t = new TestGroups();
//                t.ClusterMemberTests(Controller, Member);
//            }
        }
    }
}