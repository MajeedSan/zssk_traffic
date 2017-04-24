using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightsControlSystem
{
    class Car
    {
        public int Length { get; private set; }

        public Car(int length)
        {
            Length = length;
        }
    }
}
