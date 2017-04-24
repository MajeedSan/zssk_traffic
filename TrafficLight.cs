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
        private int _spaceBetweenCars = 1;
        public String Direction { get; private set; }
        public int RedTime { get; private set; }
        public int GreenTime { get; private set; }
        public int CycleTime { get; private set; }
        public int DecideTime { get; private set; }
        public int LookingDistance { get; private set; }
        public List<Car> CarsInLookingDistance { get; private set; }
        public List<Car> TotalWaitingCars { get; set; }

        public TrafficLight(String direction, int redTime, int greenTime, int decideTime, int lookingDistance)
        {
            Direction = direction;
            RedTime = redTime;
            GreenTime = greenTime;
            CycleTime = redTime + greenTime;
            DecideTime = decideTime;
            LookingDistance = lookingDistance;
            CarsInLookingDistance = new List<Car>();
            TotalWaitingCars = new List<Car>();
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
