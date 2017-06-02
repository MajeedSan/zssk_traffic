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
            TrafficSimulation simulation = new TrafficSimulation(1);
            simulation.StartSimulation();
            Console.Read();
        }
    }
}
