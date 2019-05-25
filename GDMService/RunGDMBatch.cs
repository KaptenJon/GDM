using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMCore;
using Microsoft.Win32;
using Timer = System.Threading.Timer;

namespace GDMService
{
    public partial class RunGDMBatch : ServiceBase
    {
        private static string _configurations;
        private static System.Threading.Timer timer;
        private static bool _started =false;
        public RunGDMBatch()
        {
            InitializeComponent();
      
            
            
        }

        private static void Timer1_Tick()
        {
            lock(timer)
                if (_started)
                    try
                    {
                        var domain  = new AppDomainSetup(); 
                           domain.PrivateBinPath = "Plugin";
                        
                        var controller = new Controller(AppDomain.CurrentDomain.BaseDirectory, _configurations);

                            foreach (Config c in controller.ConfigManager.Configurations)
                            {
                                try
                                {
                                    controller.ResetData();
                                    controller.BatchRun(c);
                                }
                                catch (Exception ee)
                                {
                                    EventLog.WriteEntry("GDM Service", "Config file:" + c.FileName+"\n"+  ee.Message, EventLogEntryType.Error);
                                }
                        }
                        controller.ResetEvent.WaitOne();
                    }
                    catch (Exception ee)
                    {
                        EventLog.WriteEntry("GDM Service", ee.Message, EventLogEntryType.Error);
                    }
        }

        protected override void OnStart(string[] args)
        {
            
            try
            {
                using (var rk = Registry.LocalMachine.OpenSubKey(@"Software\GDM"))
                {
                    _configurations = (string) rk.GetValue("Configurations");

                }
            }
            catch (Exception ee)
            {
                EventLog.WriteEntry("GDM Service", ee.Message, EventLogEntryType.Error);
            }
            _started = true;
            timer = new Timer(state => Timer1_Tick(),null,TimeSpan.FromMinutes(1), TimeSpan.FromHours(1) );
            

        }

        protected override void OnStop()
        {
            timer.Dispose();
            _started = false;
        }
    }
}
