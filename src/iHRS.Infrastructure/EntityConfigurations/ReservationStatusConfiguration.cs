using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ReservationStatusConfiguration : EnumerationBaseConfiguration<ReservationStatus>
    {
        public override string TableName => "ReservationStatuses";
        public override string PrimaryKeyColumnName => "ReservationStatusId";
    }
}
