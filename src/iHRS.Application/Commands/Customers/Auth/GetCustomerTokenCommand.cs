using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;

namespace iHRS.Application.Commands.Customers.Auth
{
    public class GetCustomerTokenCommand : ICommand<JsonWebToken>
    {
        public string LinkReference { get; }

        public GetCustomerTokenCommand(string linkReference)
        {
            LinkReference = linkReference;
        }
    }

    public class GetCustomerTokenCommandHandler : ICommandHandler<GetCustomerTokenCommand, JsonWebToken>
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IRepository<ValidationLink> _linkRepository;

        public GetCustomerTokenCommandHandler(IRepository<ValidationLink> linkRepository, IJwtHandler jwtHandler)
        {
            _linkRepository = linkRepository;
            _jwtHandler = jwtHandler;
        }

        public async Task<JsonWebToken> Handle(GetCustomerTokenCommand cmd)
        {
            var linkId = new Guid(Convert.FromBase64String(cmd.LinkReference));

            var link = await _linkRepository.GetAsync(linkId, l => l.Customer, c => c.Hotel);

            if(link is null)
                throw new NotFoundException(nameof(ValidationLink), cmd.LinkReference);

            return  _jwtHandler.CreateToken(link.CustomerId.ToString(), "customer");
        }
    }
}