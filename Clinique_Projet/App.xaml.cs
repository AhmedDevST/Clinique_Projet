using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Clinique_Projet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // application doit run a un seul instance car on doit avoir un seul listener
        // si application run a plus intance on a plus listener=>erreur
         protected override void OnStartup(StartupEventArgs e)
        {
           // if (AlreadyRunning())
             //   App.Current.Shutdown(); // Just shutdown the current application,if any instance found.  
            base.OnStartup(e);
        }

        public static bool AlreadyRunning()
        {
            bool running = false;
            try
            {
                // Getting collection of process  
                Process currentProcess = Process.GetCurrentProcess();

                // Check with other process already running   
                foreach (var p in Process.GetProcesses())
                {
                    if (p.Id != currentProcess.Id) // Check running process   
                    {
                        if (p.ProcessName.Equals(currentProcess.ProcessName) == true)
                        {
                            running = true;
                            return running;
                        }
                    }
                }
            }
            catch { }
            return running;
        }
    }
}
