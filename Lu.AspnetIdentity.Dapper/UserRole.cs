using Lu.Dapper.Extensions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lu.AspnetIdentity.Dapper
{
    public class IdentityUserRole<TKey,TRoleKey>
    {
        public IdentityUserRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public TKey UserId { get; set; }
        public TRoleKey RoleId { get; set; }
    }
}
