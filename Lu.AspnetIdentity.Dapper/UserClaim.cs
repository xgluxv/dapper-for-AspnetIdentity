using Lu.Dapper.Extensions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lu.AspnetIdentity.Dapper
{
    public class IdentityUserClaim<TKey>
    {
        public IdentityUserClaim()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public TKey UserId { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimType { get; set; }
    }
}
