using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHRS.Domain.Common;

namespace iHRS.Domain.Models
{
    public class ReservationStatus : Enumeration
    {
        public static readonly ReservationStatus New = new ReservationStatus(1, "New");
        public static readonly ReservationStatus Confirmed = new ReservationStatus(2, "Confirmed");

        public ReservationStatus(int id, string name) : base(id, name)
        {
        }
    }
}
