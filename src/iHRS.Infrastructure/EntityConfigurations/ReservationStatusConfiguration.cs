using iHRS.Domain.Models;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ReservationStatusConfiguration : EnumerationBaseConfiguration<ReservationStatus>
    {
        public override string TableName => "ReservationStatuses";
        public override string PrimaryKeyColumnName => "ReservationStatusId";
    }
}
