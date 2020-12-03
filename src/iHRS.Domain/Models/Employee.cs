using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents;
using System;

namespace iHRS.Domain.Models
{
    public class Employee : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public bool PasswordChanged { get; private set; }

        public int RoleId { get; private set; }
        public Role Role
        {
            get => Enumeration.FromValue<Role>(RoleId);
            private set => RoleId = value.Id;
        }

        private Employee() { } // For EF

        private Employee(Guid id, string firstName, string lastName, string emailAddress, string password, DateTime dateOfBirth, Role role, bool passwordChanged)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
            DateOfBirth = dateOfBirth;
            Role = role;
            PasswordChanged = passwordChanged;
        }

        public static Employee CreateNew(string firstName, string lastName, string emailAddress, string password, DateTime dateOfBirth, Role role)
        {
            var emp = new Employee(Guid.NewGuid(), firstName, lastName, emailAddress, password, dateOfBirth, role, false);
            emp.AddEvent(new EmployeeCreated(emp));
            return emp;
        }

        public void ResetPassword(string password)
        {
            PasswordChanged = false;
            Password = password;

            this.AddEvent(new EmployeePasswordReseted(this));
        }

        public void ChangePassword(string password)
        {
            PasswordChanged = true;
            Password = password;
        }
    }
}
