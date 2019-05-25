using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GDMCore;
using GDMInterfaces;


namespace GDMService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = "Plugin";
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RunGDMBatch()
                
            };
            GDMCore.Log.EventLog = true;
            ServiceBase.Run(ServicesToRun);
        }
    }
}
