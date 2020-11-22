using iHRS.Domain.Common;

namespace iHRS.Domain.Models
{
    public class Role : Enumeration
    {
        public static readonly Role TenantOwner = new Role(1, "TenantOwner");
        public static readonly Role Receptionist = new Role(2, "Receptionist");

        private Role(int id, string name) : base(id, name)
        {
        }
    }
}
