using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Win32;

namespace GDMService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            
        }

       
        [STAThread]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            if (!EventLog.SourceExists("GDM Service"))
                System.Diagnostics.EventLog.CreateEventSource("GDM Service", "Application");
            if (!EventLog.SourceExists("GDMService"))
                System.Diagnostics.EventLog.CreateEventSource("GDMService", "Application");

            Thread worker = new Thread(Show);
            worker.SetApartmentState(ApartmentState.STA);

            worker.Start();
            worker.Join(TimeSpan.FromMinutes(5));
            
        }

        private  void Show()
        {
           
            
                try
                {

                    var fd = new ChooseFolder();


                    fd.ShowDialog();
                    
                    using (var rk = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\GDM"))
                    {
                        if (rk == null)
                        { 
                            MessageBox.Show("Install failed. Repair installation!");
                            EventLog.WriteEntry("GDMService","Reg not work");
                        }
                        else
                            rk.SetValue("Configurations", fd.Path);
                    }
                   
                }
                catch (Exception e)
                {
                    MessageBox.Show("Install failed. Repair installation!");
                    EventLog.WriteEntry("GDMService", e.Message);
                }
            }
        

        public override void Uninstall(IDictionary savedState)
        {
            try
            {
                base.Uninstall(savedState);
            
                if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\GDM") != null)
                    Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\GDM");
            }
            catch { }
        }

        private void GDMServiceInstaller_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                ServiceController sc = new ServiceController();
                sc.ServiceName = "GDMService";




                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));




            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}
