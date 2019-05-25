using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GDMCore;
using GDMInterfaces;

namespace GDMTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            //  AppDomainSetup info = new AppDomainSetup();
            //  info.PrivateBinPath = "Plugin";
            //  AppDomain d = AppDomain.CreateDomain("MyDomain", AppDomain.CurrentDomain.Evidence, info);
            //AppDomain.
            for (int index = 0; index < args.Length; index++)
            {
                args[index] = args[index].ToLowerInvariant();
            }


            var path = Directory.GetCurrentDirectory();
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\GDM";
            if (args.Length > 0)
            {

                for (int i = 0; i < args.Length - 1; i++)
                {
                    if (args[i].ToLowerInvariant() == "-path")
                    {
                        if (Directory.Exists(args[i + 1]))
                        {
                            configPath = args[i + 1];
                        }
                    }
                }


            }
            var controller = new Controller(path, configPath);
            if (args.Contains("-all"))
            {
                foreach (Config c in controller.ConfigManager.Configurations)
                {

                    controller.ConfigManager.Save(c, c.FileName);
                    controller.ResetData();
                    controller.BatchRun(c);

                }
                controller.ResetEvent.WaitOne();
            }
            
            else
            {
                PluginSettings.IsInUIMode = true;
                
                //AppDomain.CurrentDomain.AppendPrivatePath();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main(controller));
            }
        }

    }
}
