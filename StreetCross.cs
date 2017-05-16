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
        public StreetCross NorthernStreetCross { get; set; }
        //public StreetCross SouthStreetCross { get; private set; }
        //public StreetCross WestStreetCross { get; private set; }
        public StreetCross EasternStreetCross { get; set; }
        public double NRatio { get; private set; }
        public int DecideTime { get; private set; }
        public List<TrafficLight> TrafficLights = new List<TrafficLight>();
        public TrafficLight NorthernTrafficLight { get; set; }
        public TrafficLight EasternTrafficLight { get; set; }

        public StreetCross(int decideTime)
        {
            DecideTime = decideTime;
        }
        public void CalculateNRatio()
        {
            int waitingN = 0, waitingE = 0;
            Console.WriteLine($"kalkulejtuje");
            waitingN = NorthernTrafficLight.CarsInLookingDistance.Count;
            waitingE = EasternTrafficLight.CarsInLookingDistance.Count;

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

        public void RefreshTrafficLights()
        {
            TrafficLights.Clear();
            TrafficLights.Add(NorthernTrafficLight);
            TrafficLights.Add(EasternTrafficLight);
        }

    }
}
