using iHRS.Application.Common;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Customers.Auth
{
    public class ValidateCustomerCommand : ICommand
    {
        public string EmailAddress { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public ValidateCustomerCommand(string emailAddress, string firstName, string lastName)
        {
            EmailAddress = emailAddress;
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class ValidateCustomerCommandHandler : ICommandHandler<ValidateCustomerCommand>
    {
        private readonly IRepository<Customer> _customerRepository;

        public ValidateCustomerCommandHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(ValidateCustomerCommand cmd)
        {
            var customer = await _customerRepository
                .GetAsync(c => c.EmailAddress == cmd.EmailAddress.ToLower().Trim(), c => c.ValidationLinks, c => c.Hotel);

            customer.ValidateCustomer(cmd.FirstName, cmd.LastName, 30);

            return Unit.Value;
        }
    }
}