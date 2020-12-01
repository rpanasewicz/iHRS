using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Auth
{
    public class SetupPasswordCommand : ICommand
    {
        public string EmailAddress { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }

        public SetupPasswordCommand(string emailAddress, string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
            EmailAddress = emailAddress.Trim().ToLower();
        }
    }

    public class SetupPasswordCommandHandler : ICommandHandler<SetupPasswordCommand>
    {
        private IRepository<Employee> _repository;
        private readonly IPasswordService _passwordService;

        public SetupPasswordCommandHandler(IRepository<Employee> repository, IPasswordService passwordService)
        {
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(SetupPasswordCommand cmd)
        {
            var user = await _repository.GetAsync(u => u.EmailAddress == cmd.EmailAddress);

            if (user == null || user.PasswordChanged || !string.Equals(user.Password, cmd.OldPassword))
                throw new InvalidCredentialsException();

            var passwordHash = _passwordService.Hash(cmd.NewPassword);

            user.ChangePassword(passwordHash);

            return Unit.Value;
        }
    }
}
