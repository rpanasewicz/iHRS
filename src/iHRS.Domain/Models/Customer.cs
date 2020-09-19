using iHRS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Domain.Models
{
    public class Customer : Entity
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string EmailAddress { get; }
        public string PhoneNumber { get; }

        public IEnumerable<Reservation> Reservations
        {
            get => _reservations.AsEnumerable();
            private set => _reservations = new HashSet<Reservation>(value);
        }

        private ISet<Reservation> _reservations;

        private Customer()
        {

        }

        private Customer(Guid customerId, string firstName, string lastName, string emailAddress, string phoneNumber, IEnumerable<Reservation> reservations = null)
        {
            this.Id = customerId;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Reservations = reservations ?? Enumerable.Empty<Reservation>();
        }

        public static Customer CreateNew(string firstName, string lastName, string emailAddress, string phoneNumber)
        {
            return new Customer(Guid.NewGuid(), firstName, lastName, emailAddress, phoneNumber);
        }

    }
}
