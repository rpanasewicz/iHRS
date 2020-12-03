using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;

namespace iHRS.Domain.DomainEvents
{
    public class EmployeeCreated : IDomainEvent
    {
        public Employee Employee { get; }

        public EmployeeCreated(Employee employee)
        {
            Employee = employee;
        }
    }
}
