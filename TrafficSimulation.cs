using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficLightsControlSystem
{
    class TrafficSimulation
    {
        private int _nRatio;
        private int _carCrossingTime = 2;
        private int _timeInSeconds = 0;

        public void StartSimulation()
        {
            StreetCross streetCross = new StreetCross(60);
            streetCross.TrafficLights.Add(new TrafficLight("north", 10, 10, 50, true));
            streetCross.TrafficLights.Add(new TrafficLight("east", 10, 10, 50, false));
            AddCars(streetCross.TrafficLights, 20);
            while (_timeInSeconds < 60)
            {
                System.Threading.Thread.Sleep(500);
                _timeInSeconds++;
               
                foreach (var trafficLight in streetCross.TrafficLights)
                {
                    trafficLight.CheckCarsInLookingDistance();
                    Console.WriteLine($"[{trafficLight.Direction}] Number of total waiting cars: {trafficLight.TotalWaitingCars.Count}");
                    Console.WriteLine($"[{trafficLight.Direction}] Number of cars in looking distance: {trafficLight.CarsInLookingDistance.Count}");
                    Console.WriteLine($"[{trafficLight.Direction}] Green time: {trafficLight.GreenTime}");
                    Console.WriteLine($"[{trafficLight.Direction}] Red Time: {trafficLight.RedTime}");

              
                    trafficLight.CheckLightChange();
                    if (trafficLight.IsGreenOn)
                    {
                        trafficLight.RemoveCar();
                    }
                }

                if ((_timeInSeconds % (streetCross.DecideTime / 6)) == 0)
                {
                    switch (streetCross.CheckRatio())
                    {
                        case 1:
                            Console.WriteLine($"Zwiekszamy zielone w {streetCross.TrafficLights[0].Direction}");
                            streetCross.TrafficLights[0].IncreaseGreenTime();
                            streetCross.TrafficLights[1].IncreaseRedTime();
                            break;
                        case -1:
                            Console.WriteLine($"Zwiekszamy zielone w {streetCross.TrafficLights[1].Direction}");
                            streetCross.TrafficLights[1].IncreaseGreenTime();
                            streetCross.TrafficLights[0].IncreaseRedTime();
                            break;
                        default:
                            Console.WriteLine("Nic sie nie zmienilo.");
                            break;
                    }
                }

            }
        }



        public void AddCars(List<TrafficLight> trafficLights, int numberOfCars)
        {
            for (int i = 0; i < numberOfCars; i++)
            {
                foreach (var trafficLight in trafficLights)
                {
                    trafficLight.AddCar();
                }
            }
        }
    }
}
