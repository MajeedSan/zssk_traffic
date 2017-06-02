using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using TrafficLightsControlSystem;

namespace TraffiCProject
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            TrafficSimulation simulation = new TrafficSimulation(4);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            simulation.StartSimulation();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Time: {elapsedMs}");
            Console.Read();
        }
    }
}
