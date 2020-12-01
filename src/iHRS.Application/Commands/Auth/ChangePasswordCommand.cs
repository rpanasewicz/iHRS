using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Auth
{
    public class ChangePasswordCommand : ICommand
    {
        public string OldPassword { get; }
        public string NewPassword { get; }

        public ChangePasswordCommand(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }

    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        private IRepository<Employee> _repository;
        private readonly IPasswordService _passwordService;
        private readonly IAuthProvider _authProvider;

        public ChangePasswordCommandHandler(IRepository<Employee> repository, IPasswordService passwordService)
        {
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand cmd)
        {
            var user = await _repository.GetAsync(_authProvider.UserId);

            if (user == null)
                throw new NotFoundException(nameof(Employee), _authProvider.UserId);

            if (_passwordService.IsValid(user.Password, cmd.OldPassword))
                throw new InvalidCredentialsException();

            var passwordHash = _passwordService.Hash(cmd.NewPassword);

            user.ChangePassword(passwordHash);

            return Unit.Value;
        }
    }
}
