using iHRS.Domain.Common;

namespace iHRS.Domain.Models
{
    public class CommunicationMethod : Enumeration
    {
        public static readonly CommunicationMethod Email = new CommunicationMethod(1, "Email");
        public static readonly CommunicationMethod Sms = new CommunicationMethod(2, "Sms");

        public CommunicationMethod(int id, string name) : base(id, name)
        {
        }
    }
}
