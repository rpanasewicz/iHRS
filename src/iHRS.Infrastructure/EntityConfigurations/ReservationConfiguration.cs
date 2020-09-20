using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ReservationConfiguration : EntityBaseConfiguration<Reservation>
    {
        public override void ConfigureFields(EntityTypeBuilder<Reservation> entity)
        {
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Reservation> entity)
        {
        }

        public override string TableName => "Reservations";
        public override string PrimaryKeyColumnName => "ReservationId";
    }
}
