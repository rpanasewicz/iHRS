using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Auth
{
    public class SignInCommand : ICommand<JsonWebToken>
    {
        public string Email { get; }
        public string Password { get; }

        public SignInCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class SignInCommandHandler : ICommandHandler<SignInCommand, JsonWebToken>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtHandler _jwtHandler;
        private readonly ILogger<SignInCommandHandler> _logger;

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public SignInCommandHandler(IRepository<User> userRepository, IPasswordService passwordService,
            IJwtHandler jwtHandler, ILogger<SignInCommandHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        public async Task<JsonWebToken> Handle(SignInCommand cmd)
        {
            if (!EmailRegex.IsMatch(cmd.Email))
            {
                _logger.LogError($"Invalid email: {cmd.Email}");
                throw new InvalidEmailException(cmd.Email);
            }

            var user = await _userRepository.FindFromAllAsync(u => u.EmailAddress == cmd.Email);
            if (user is null || !_passwordService.IsValid(user.Password, cmd.Password))
            {
                _logger.LogError($"User with email: {cmd.Email} was not found.");
                throw new InvalidCredentialsException();
            }

            if (!_passwordService.IsValid(user.Password, cmd.Password))
            {
                _logger.LogError($"Invalid password for user with id: {user.Id}");
                throw new InvalidCredentialsException();
            }

            var claims = new Dictionary<string, IEnumerable<string>>
            {
                ["tenantId"] = new string[] { user.TenantId.ToString() },
                ["firstName"] = new string[] { user.FirstName },
                ["lsatName"] = new string[] { user.LastName },
            };

            var token = _jwtHandler.CreateToken(user.Id.ToString(), user.Role.Name, claims: claims);

            _logger.LogInformation($"User with id: {user.Id} has been authenticated.");

            return token;
        }
    }
}