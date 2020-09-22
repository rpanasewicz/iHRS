using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;

namespace iHRS.Domain.DomainEvents
{
    public class ValidationLinkCreatedDomainEvent : IDomainEvent
    {
        public ValidationLink ValidationLink { get; }
        public Customer Customer { get; }

        public ValidationLinkCreatedDomainEvent(ValidationLink validationLink, Customer customer)
        {
            ValidationLink = validationLink;
            Customer = customer;
        }
    }
}
