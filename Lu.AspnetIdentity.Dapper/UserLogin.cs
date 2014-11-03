using Lu.Dapper.Extensions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lu.AspnetIdentity.Dapper
{
    public class IdentityUserLogin<TKey>
    {
        public IdentityUserLogin()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public TKey UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
