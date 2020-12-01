using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;

namespace iHRS.Domain.DomainEvents
{
    public class EmployeePasswordReseted : IDomainEvent
    {
        public Employee Employee { get; }

        public EmployeePasswordReseted(Employee employee)
        {
            Employee = employee;
        }
    }
}
