namespace GDMService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GDMServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.GDMServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // GDMServiceProcessInstaller
            // 
            this.GDMServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.GDMServiceProcessInstaller.Password = null;
            this.GDMServiceProcessInstaller.Username = null;
            // 
            // GDMServiceInstaller
            // 
            this.GDMServiceInstaller.DisplayName = "GDM Service";
            this.GDMServiceInstaller.ServiceName = "GDMService";
            this.GDMServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.GDMServiceInstaller.Committed += new System.Configuration.Install.InstallEventHandler(this.GDMServiceInstaller_Committed);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.GDMServiceProcessInstaller,
            this.GDMServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller GDMServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller GDMServiceInstaller;
    }
}