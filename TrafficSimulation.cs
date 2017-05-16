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
        private int _numberOfStreetCrosses;
        private int _numberOfColumns;
        private StreetCross[,] _streetCrossWeb;

        public TrafficSimulation(int numberOfStreetCrosses)
        {
            double columns = Math.Sqrt(numberOfStreetCrosses);
            if (columns % 1 != 0)
            {
                throw new Exception($"Number of Street Crosses is invalid. = {numberOfStreetCrosses}");
            }
            _numberOfStreetCrosses = numberOfStreetCrosses;
            _numberOfColumns = (int)columns;
        }

        public void StartSimulation()
        {
            createStreetCrossWeb();
            connectStreetCrossWeb();
            while (_timeInSeconds < 60)
            {
                System.Threading.Thread.Sleep(500);
                _timeInSeconds++;
                foreach (var streetCross in _streetCrossWeb)
                {
                        HandleStreetCross(streetCross);
                }
            }
        }

        public void HandleStreetCross(StreetCross streetCross)
        {
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

            if (_timeInSeconds % (streetCross.DecideTime / 6) == 0)
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

        public void createStreetCrossWeb()
        {
            _streetCrossWeb = new StreetCross[_numberOfColumns, _numberOfColumns];
            for (int i = 0; i < _numberOfColumns; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    _streetCrossWeb[i, j] = new StreetCross(60);
                    _streetCrossWeb[i, j].NorthernTrafficLight = new TrafficLight(_streetCrossWeb[i, j], "north", 10, 10, 50, true);
                    _streetCrossWeb[i, j].EasternTrafficLight = new TrafficLight(_streetCrossWeb[i, j], "east", 10, 10, 50, false);
                    _streetCrossWeb[i, j].RefreshTrafficLights();
                    AddCars(_streetCrossWeb[i, j].TrafficLights, 20);
                }
            }   
        }

        public void connectStreetCrossWeb()
        {
            for (int i = 0; i < _numberOfColumns-1; i++)
            {
                for (int j = 0; j < _numberOfColumns - 2; j++)
                {
                    _streetCrossWeb[j, i].EasternStreetCross = _streetCrossWeb[j + 1, i];
                }
            }
            for (int i = 1; i < _numberOfColumns - 1; i++)
            {
                for (int j = 0; j < _numberOfColumns - 2; j++)
                {
                    _streetCrossWeb[j, i].NorthernStreetCross = _streetCrossWeb[j, i-1];
                }
            }
        }
    }
}
