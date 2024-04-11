using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.Common
{
    public class RequestUserDetails
    {
        // Properties from User entity
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsUserActive { get; set; }

        // Properties from Role entity
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsRoleActive { get; set; }

        //Properties from ConfirmationToken
        public string? Token { get; set; }
        public DateTime? ExpiryDateTime { get; set; }
    }
}
