using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents;
using System;

namespace iHRS.Domain.Models
{
    public class ValidationLink : Entity
    {
        public Guid CustomerId { get; }
        public Customer Customer { get; }

        private ValidationLink()
        {

        }

        private ValidationLink(Customer customer)
        {
            Customer = customer;
            CustomerId = customer.Id;
        }

        internal static ValidationLink CreateNew(Customer customer, DateTime expirationDate)
        {
            var validationLink = new ValidationLink(customer);
            validationLink.ExpirationDate = expirationDate;

            validationLink.AddEvent(new ValidationLinkCreatedDomainEvent(validationLink, customer));

            return validationLink;
        }
    }
}
