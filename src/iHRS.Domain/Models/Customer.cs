using iHRS.Domain.Common;
using iHRS.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Domain.Models
{
    public class Customer : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; }
        public string EmailAddress { get; }
        public string PhoneNumber { get; }
        public Guid HotelId { get; }
        public Hotel Hotel { get; }

        public IEnumerable<Reservation> Reservations
        {
            get => _reservations.AsEnumerable();
            private set => _reservations = new HashSet<Reservation>(value);
        }
        public IEnumerable<ValidationLink> ValidationLinks
        {
            get => _validationLinks.AsEnumerable();
            private set => _validationLinks = new HashSet<ValidationLink>(value);
        }

        private ISet<Reservation> _reservations;
        private ISet<ValidationLink> _validationLinks;

        private Customer() { }

        private Customer(Guid customerId, string firstName, string lastName, string emailAddress, string phoneNumber, Hotel hotel,
            IEnumerable<Reservation> reservations = null,
            IEnumerable<ValidationLink> validationLinks = null)
        {

            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(emailAddress)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(emailAddress));
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(phoneNumber));
            if (hotel is null) throw new ArgumentNullException(nameof(hotel));
            if (customerId == default) throw new ArgumentNullException(nameof(customerId));

            this.Id = customerId;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            EmailAddress = emailAddress.ToLower().Trim();
            PhoneNumber = phoneNumber.Trim();
            Hotel = hotel;
            HotelId = hotel.Id;
            Reservations = reservations ?? Enumerable.Empty<Reservation>();
            ValidationLinks = validationLinks ?? Enumerable.Empty<ValidationLink>();
        }

        public static Customer CreateNew(string firstName, string lastName, string emailAddress, string phoneNumber, Hotel hotel)
        {
            return new Customer(Guid.NewGuid(), firstName, lastName, emailAddress, phoneNumber, hotel);
        }

        public ValidationLink ValidateCustomer(string firstName, string lastName, int linkExpirationTimeMinutes)
        {
            if (_validationLinks is null) throw new PropertyNotInitializedException(nameof(ValidationLinks));

            if (FirstName.ToLower() != firstName.ToLower().Trim() || LastName.ToLower() != lastName.ToLower().Trim())
                throw new CustomerValidationException();

            var validationLink = ValidationLink.CreateNew(this, DateTime.UtcNow.AddMinutes(linkExpirationTimeMinutes));

            _validationLinks.Add(validationLink);

            return validationLink;
        }
    }
}
