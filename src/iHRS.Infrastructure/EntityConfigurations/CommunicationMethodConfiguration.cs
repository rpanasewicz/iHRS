using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHRS.Domain.Models;

namespace iHRS.Infrastructure.EntityConfigurations
{
    class CommunicationMethodConfiguration : EnumerationBaseConfiguration<CommunicationMethod>
    {
        public override string TableName => "CommunicationMethods";
        public override string PrimaryKeyColumnName => "CommunicationMethodId";
    }
}
