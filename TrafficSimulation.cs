using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightsControlSystem
{
    class TrafficSimulation
    {
        public void startSimulation()
        {
            StreetCross streetCross = new StreetCross();
            streetCross.TrafficLightsContainer.Add(new TrafficLight("south", 50, 50, 300, 50));
            streetCross.TrafficLightsContainer.Add(new TrafficLight("west", 50, 50, 300, 50));
            AddCars(streetCross.TrafficLightsContainer, 20);
            foreach (var trafficLight in streetCross.TrafficLightsContainer)
            {
                trafficLight.CheckCarsInLookingDistance();
                Console.WriteLine($"Number of total waiting cars: {trafficLight.TotalWaitingCars.Count}");
                Console.WriteLine($"Number of cars in looking distance: {trafficLight.CarsInLookingDistance.Count}");
            }
        }

        public void AddCars(List<TrafficLight> trafficLights, int numberOfCars)
        {
            for (int i = 0; i < numberOfCars; i++)
            {
                Car car = new Car(3);
                foreach (var trafficLight in trafficLights)
                {
                    trafficLight.TotalWaitingCars.Add(car);
                }
            }
        }
    }
}
