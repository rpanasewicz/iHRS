using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Employees
{
    public class CreateEmployeeCommand : ICommand
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime DateOfBirth { get; }
        public string Role { get; }

        public CreateEmployeeCommand(string firstName, string lastName, string email, DateTime dateOfBirth, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            Role = role;
        }
    }

    public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand>
    {
        private readonly IRepository<Employee> _emplpoyeeRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger;

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public CreateEmployeeCommandHandler(IRepository<Employee> emplpoyeeRepository, IPasswordService passwordService, ILogger<CreateEmployeeCommandHandler> logger)
        {
            _emplpoyeeRepository = emplpoyeeRepository;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateEmployeeCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Email) || !EmailRegex.IsMatch(cmd.Email))
            {
                _logger.LogError($"Invalid email: {cmd.Email}");
                throw new InvalidEmailException(cmd.Email);
            }

            var user = await _emplpoyeeRepository.GetAsync(u => u.EmailAddress == cmd.Email);
            if (user is { })
            {
                _logger.LogError($"Email already in use: {cmd.Email}");
                throw new EmailInUseException(cmd.Email);
            }

            var passwordHash = _passwordService.Hash(Guid.NewGuid().ToString("N"));

            user = Employee.CreateNew(cmd.FirstName, cmd.LastName, cmd.Email, passwordHash, cmd.DateOfBirth, Enumeration.FromDisplayName<Role>(cmd.Role));
            await _emplpoyeeRepository.AddAsync(user);

            _logger.LogInformation($"Created an account for the user with id: {user.Id}.");

            return Unit.Value;
        }
    }
}
