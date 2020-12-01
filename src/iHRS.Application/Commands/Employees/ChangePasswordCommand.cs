using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Employees
{
    public class ChangePasswordCommand : ICommand
    {
        public string EmailAddress { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }

        public ChangePasswordCommand(string emailAddress, string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
            EmailAddress = emailAddress.Trim().ToLower();
        }
    }

    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        private IRepository<Employee> _repository;
        private readonly IPasswordService _passwordService;

        public ChangePasswordCommandHandler(IRepository<Employee> repository, IPasswordService passwordService)
        {
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand cmd)
        {
            var user = await _repository.GetAsync(u => u.EmailAddress == cmd.EmailAddress);

            if (user == null)
                throw new NotFoundException(nameof(Employee), cmd.EmailAddress);

            if(user.PasswordChanged)
            {
                if (_passwordService.IsValid(user.Password, cmd.OldPassword))
                    throw new InvalidCredentialsException();
            }
            else
            {
                if (string.Equals(user.Password, cmd.OldPassword))
                    throw new InvalidCredentialsException();
            }

            var passwordHash = _passwordService.Hash(cmd.NewPassword);

            user.ChangePassword(passwordHash);

            return Unit.Value;
        }
    }
}
