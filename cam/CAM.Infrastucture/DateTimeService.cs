using CAM.Contracts;
using System;

namespace CAM.Infrastucture
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
