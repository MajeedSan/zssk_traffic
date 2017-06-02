using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using System.Windows.Forms;
=======
using System.Timers;
>>>>>>> 5e85db72f10760f68afe9ae308cdab6d0180c062
using TrafficLightsControlSystem;

namespace TraffiCProject
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
<<<<<<< HEAD
            TrafficSimulation simulation = new TrafficSimulation(1);
=======
           
            TrafficSimulation simulation = new TrafficSimulation(1);
            var watch = System.Diagnostics.Stopwatch.StartNew();
>>>>>>> 5e85db72f10760f68afe9ae308cdab6d0180c062
            simulation.StartSimulation();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Time: {elapsedMs}");
            Console.Read();
        }
    }
}
