using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCarrier.ApplicationModels.Auth.Response
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string FullName { get; set; }
    }
}
