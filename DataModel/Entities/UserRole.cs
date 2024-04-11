using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Entities
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public long UserRoleId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
