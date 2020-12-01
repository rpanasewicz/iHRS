using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Auth
{
    public class ResetPasswordCommand : ICommand
    {
        public string Email { get; }

        public ResetPasswordCommand(string email)
        {
            Email = email;
        }
    }

    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private IRepository<Employee> _repository;
        private readonly IPasswordService _passwordService;

        public ResetPasswordCommandHandler(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(ResetPasswordCommand cmd)
        {
            var user = await _repository.GetAsync(u => u.EmailAddress == cmd.Email);

            if (user == null)
                throw new NotFoundException(nameof(Employee), cmd.Email);

            var passwordHash = _passwordService.Hash(Guid.NewGuid().ToString("N"));

            user.ResetPassword(passwordHash);

            return Unit.Value;
        }
    }
}
