using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficLightsControlSystem;

namespace TraffiCProject
{
    class Program
    {
        static void Main(string[] args)
        {
            TrafficSimulation simulation = new TrafficSimulation();
            simulation.startSimulation();
            Console.Read();
        }
    }
}
