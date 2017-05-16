using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightsControlSystem
{
    class TrafficLight
    {
        private StreetCross _streetCross;
        private int _carLength = 3;
        private int _spaceBetweenCars = 1;
        private int _elapsedTime = 0;

        public bool IsRedOn { get; private set; }
        public bool IsGreenOn { get; private set; }
        public String Direction { get; private set; }
        public int RedTime { get; private set; }
        public int GreenTime { get; private set; }
        public int CycleTime { get; private set; }
        public int LookingDistance { get; private set; }
        public List<Car> CarsInLookingDistance { get; private set; }
        public List<Car> TotalWaitingCars { get; set; }

        public TrafficLight(StreetCross streetCross, String direction, int redTime, int greenTime, int lookingDistance, bool isGreenOn)
        {
            ChangeLights(isGreenOn);
            _streetCross = streetCross;
            Direction = direction;
            RedTime = redTime;
            GreenTime = greenTime;
            CycleTime = redTime + greenTime;
            LookingDistance = lookingDistance;
            CarsInLookingDistance = new List<Car>();
            TotalWaitingCars = new List<Car>();
        }

        public void ChangeLights(bool isGreenOn)
        {
            if (isGreenOn)
            {

                IsGreenOn = true;
                IsRedOn = false;
            }
            else
            {
                IsGreenOn = false;
                IsRedOn = true;
            }
            _elapsedTime = 0;
        }

        public void CheckLightChange()
        {
            _elapsedTime++;
            if (IsGreenOn && (_elapsedTime==GreenTime))
            {
                ChangeLights(false);
            }
            else if (IsRedOn && (_elapsedTime==RedTime))
            {
                ChangeLights(true);
            }
        }

        public void IncreaseGreenTime()
        {
            GreenTime += 1;
            RedTime -= 1;
        }

        public void IncreaseRedTime()
        {
            RedTime += 1;
            GreenTime -= 1;
        }

        public void AddCar()
        {
            TotalWaitingCars.Add(new Car(_carLength));
        }

        public void RemoveCar()
        {
            if (TotalWaitingCars.Count > 0)
            {
                TotalWaitingCars.RemoveAt(0);
                switch (Direction)
                {
                    case "north":
                        if (_streetCross.NorthernStreetCross != null)
                        {
                            _streetCross.NorthernStreetCross.NorthernTrafficLight.AddCar();
                        }
                        break;
                    case "east":
                        if (_streetCross.EasternStreetCross != null)
                        {
                            _streetCross.EasternStreetCross.EasternTrafficLight.AddCar();
                        }
                        break;
                    default:
                        Console.WriteLine("Diffrent direction?");
                        break;
                }
            }
        }


        public void CheckCarsInLookingDistance()
        {
            int distance = 0;
            CarsInLookingDistance.Clear();
            foreach (var waitingCar in TotalWaitingCars)
            {
                if (distance + _spaceBetweenCars + waitingCar.Length <= LookingDistance)
                {
                    CarsInLookingDistance.Add(waitingCar);
                    distance += _spaceBetweenCars + waitingCar.Length;
                }
                else
                {
                    break;
                }
            }
        }
        
    }
}
