﻿using iHRS.Domain.Common;
using System;

namespace iHRS.Domain.Models
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public int RoleId { get; private set; }
        public Role Role
        {
            get => Enumeration.FromValue<Role>(RoleId);
            private set => RoleId = value.Id;
        }

        private User() { } // For EF

        private User(Guid id, string firstName, string lastName, string emailAddress, string password, DateTime dateOfBirth, Role role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
            DateOfBirth = dateOfBirth;
            Role = role;
        }

        public static User CreateNew(string firstName, string lastName, string emailAddress, string password, DateTime dateOfBirth, Role role)
        {
            return new User(Guid.NewGuid(), firstName, lastName, emailAddress, password, dateOfBirth, role);
        }
    }
}
