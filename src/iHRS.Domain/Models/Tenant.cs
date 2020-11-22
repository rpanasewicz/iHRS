using System;

namespace iHRS.Domain.Models
{
    public class Tenant
    {
        public Guid Id { get; }
        public string Name { get; }

        private Tenant()
        {

        }

        public Tenant(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
