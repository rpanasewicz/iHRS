using System.Collections.Generic;

namespace iHRS.Application.Auth
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public long Expires { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public IDictionary<string, IEnumerable<string>> Claims { get; set; }
    }
}