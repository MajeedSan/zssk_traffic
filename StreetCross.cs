using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightsControlSystem
{
    class StreetCross
    {
        private int _ratioTreshold = 5;
        public double NRatio { get; private set; }
        public int DecideTime { get; private set; }
        public List<TrafficLight> TrafficLights = new List<TrafficLight>();

        public StreetCross(int dTime)
        {
            DecideTime = dTime;
        }
        public void CalculateNRatio()
        {
            int waitingN = 0, waitingE = 0;
            Console.WriteLine($"kalkulejtuje");

            foreach (var trafficLight in TrafficLights)
            {
                if (trafficLight.Direction == "north")
                {
                    waitingN = trafficLight.CarsInLookingDistance.Count;
                }
                else
                {
                    waitingE = trafficLight.CarsInLookingDistance.Count;
                }
            }
            if (waitingN > 0)
            {
                NRatio = (waitingN - waitingE) / waitingN;
                Console.WriteLine($"NRatio: {NRatio}");
            }
        }

        public int CheckRatio()
        {
            CalculateNRatio();
            if (NRatio > 0 && NRatio > _ratioTreshold)
            {
                return 1;
            }
            else if (NRatio < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

    }
}
