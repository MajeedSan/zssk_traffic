using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficLightsControlSystem
{
    class TrafficSimulation
    {
        //-----------Wykresy-----------------
        private int _decideTime = 10;
        private int _lookingDistance = 100;
        private int _timeRedGreen = 10;
        private int _numberOfCarsNorth = 40;
        private int _numberOfCarsEast = 80;
        //-----------------------------------
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
            CreateStreetCrossWeb();
            ConnectStreetCrossWeb();
            SaveDataHeadersToFiles();
            
            while (!IsStreetCrossWebEmpty())
            {
                SaveDataToFiles();
                System.Threading.Thread.Sleep(500);
                _timeInSeconds++;
                int i = 0;
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

            if (_timeInSeconds % (streetCross.DecideTime) == 0)
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

        public void AddCars(TrafficLight trafficLight, int numberOfCars)
        {
            for (int i = 0; i < numberOfCars; i++)
            {
                trafficLight.AddCar();
            }
        }

        public void CreateStreetCrossWeb()
        {
            _streetCrossWeb = new StreetCross[_numberOfColumns, _numberOfColumns];
            for (int i = 0; i < _numberOfColumns; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    string northLabel = $"{i}_{j}_north";
                    string eastLabel = $"{i}_{j}_east";
                    _streetCrossWeb[i, j] = new StreetCross(_decideTime);
                    _streetCrossWeb[i, j].NorthernTrafficLight = new TrafficLight(northLabel, _streetCrossWeb[i, j], "north", _timeRedGreen, _timeRedGreen, _lookingDistance, true);
                    _streetCrossWeb[i, j].EasternTrafficLight = new TrafficLight(eastLabel, _streetCrossWeb[i, j], "east", _timeRedGreen, _timeRedGreen, _lookingDistance, false);
                    _streetCrossWeb[i, j].RefreshTrafficLights();
                    AddCars(_streetCrossWeb[i, j].NorthernTrafficLight, _numberOfCarsNorth);
                    AddCars(_streetCrossWeb[i, j].EasternTrafficLight, _numberOfCarsEast);
                }
            }   
        }

        public void ConnectStreetCrossWeb()
        {
            for (int i = 0; i < _numberOfColumns; i++)
            {
                for (int j = 0; j < _numberOfColumns - 1; j++)
                {
                    _streetCrossWeb[j, i].EasternStreetCross = _streetCrossWeb[j + 1, i];
                }
            }
            for (int i = 1; i < _numberOfColumns; i++)
            {
                for (int j = 0; j < _numberOfColumns - 1; j++)
                {
                    _streetCrossWeb[j, i].NorthernStreetCross = _streetCrossWeb[j, i-1];
                }
            }
        }

        public void SaveDataToFiles()
        {
            foreach (var streetCross in _streetCrossWeb)
            {
                foreach (var trafficLight in streetCross.TrafficLights)
                {
                    switch (trafficLight.Direction)
                    {
                        case "north":
                            StreamWriter northOutputFile = new StreamWriter($"{trafficLight.Label}.csv", true);
                            northOutputFile.WriteLine($"{_timeInSeconds};{trafficLight.TotalWaitingCars.Count};{trafficLight.GreenTime};{trafficLight.RedTime}");
                            northOutputFile.Close();
                            break;
                        case "east":
                            StreamWriter eastOutputFile = new StreamWriter($"{trafficLight.Label}.csv", true);
                            eastOutputFile.WriteLine($"{_timeInSeconds};{trafficLight.TotalWaitingCars.Count};{trafficLight.GreenTime};{trafficLight.RedTime}");
                            eastOutputFile.Close();
                            break;
                    }
                }
            }
        }

        public void SaveDataHeadersToFiles()
        {
            foreach (var streetCross in _streetCrossWeb)
            {
                foreach (var trafficLight in streetCross.TrafficLights)
                {
                    StreamWriter outputFile = new StreamWriter($"{trafficLight.Label}.csv", true);
                    outputFile.WriteLine($"Cars North:{_numberOfCarsNorth};");
                    outputFile.WriteLine($"Cars East:{_numberOfCarsEast};");
                    outputFile.WriteLine($"Decide Time:{_decideTime};");
                    outputFile.WriteLine($"Looking Dist:{_lookingDistance};");
                    outputFile.WriteLine($"RedGreenTime:{_timeRedGreen}");
                    outputFile.WriteLine($"Time;WaitingCars;GreenTime;RedTime");
                    outputFile.Close();
                }
            }
        }

        private bool IsStreetCrossWebEmpty()
        {
            foreach (var streetCross in _streetCrossWeb)
            {
                if (!streetCross.IsStreetCrossEmpty())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
