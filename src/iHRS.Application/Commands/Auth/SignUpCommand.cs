using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using Microsoft.Extensions.Logging;

namespace iHRS.Application.Commands.Auth
{
    public class SignUpCommand : ICommand
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime DateOfBirth { get; }

        public SignUpCommand(string firstName, string lastName, string email, string password, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
        }
    }

    public class SignUpCommandHandler : ICommandHandler<SignUpCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<SignUpCommandHandler> _logger;

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public SignUpCommandHandler(IRepository<User> userRepository, IPasswordService passwordService, ILogger<SignUpCommandHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task Handle(SignUpCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Email) || !EmailRegex.IsMatch(cmd.Email))
            {
                _logger.LogError($"Invalid email: {cmd.Email}");
                throw new InvalidEmailException(cmd.Email);
            }

            var user = await _userRepository.GetAsync(u => u.EmailAddress == cmd.Email);
            if (user is { })
            {
                _logger.LogError($"Email already in use: {cmd.Email}");
                throw new EmailInUseException(cmd.Email);
            }

            var password = _passwordService.Hash(cmd.Password);

            user = User.CreateNew(cmd.FirstName, cmd.LastName, cmd.Email, password, cmd.DateOfBirth);
            await _userRepository.AddAsync(user);

            _logger.LogInformation($"Created an account for the user with id: {user.Id}.");
        }
    }
}
