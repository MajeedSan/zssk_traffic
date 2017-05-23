using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficLightsControlSystem;

namespace TraffiCProject
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrafficSim());
            // TrafficSimulation simulation = new TrafficSimulation(4);
            // simulation.StartSimulation();
            // Console.Read();
        }
    }
}
