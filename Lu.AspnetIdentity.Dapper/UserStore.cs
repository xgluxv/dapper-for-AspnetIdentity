using Dapper;
using Lu.Dapper.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lu.AspnetIdentity.Dapper
{

    public class UserStore<TUser,TRole>:UserStore<TUser,TRole,string,string>,IUserStore<TUser>
        where TUser : IdentityUser where TRole:IdentityRole
    {
        public UserStore(DapperIdentityDbContext<TUser,TRole> dbContext)
            : base(dbContext)
        {

        }
        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await DbContent.UserLoginRepository.FindAsync(c => c.UserId == user.Id);
            IList<UserLoginInfo> rs = new List<UserLoginInfo>();
            if (result != null && result.Any())
            {
                foreach (var login in result)
                {
                    rs.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }
            }

            return rs;
        }
        public override async Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await DbContent.UserClaimRepository.FindAsync(c => c.UserId == user.Id);
            IList<System.Security.Claims.Claim> rs = new List<System.Security.Claims.Claim>();
            if (result != null && result.Any())
            {
                foreach (var login in result)
                {
                    rs.Add(new System.Security.Claims.Claim(login.ClaimType, login.ClaimValue));
                }
            }

            return rs;
        }

        public override async Task RemoveClaimAsync(TUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            await DbContent.UserClaimRepository.RemoveAsync(c =>c.UserId == user.Id 
                && c.ClaimValue == claim.Value 
                && c.ClaimType == claim.ValueType);
        }
        public override async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            var roles = await DbContent.RoleRepository.FindAsync(c => c.Name == roleName);
            if (roles != null && roles.Any())
            {
                var role = roles.First();
                await DbContent.UserRoleRepository.RemoveAsync(c => c.RoleId == role.Id && c.UserId == user.Id);
            }
        }
        public override async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            await DbContent.UserLoginRepository.RemoveAsync(c => c.UserId == user.Id 
                && c.ProviderKey == login.ProviderKey 
                && c.LoginProvider == login.LoginProvider);
        }
    }

    public class UserStore<TUser,TRole,TKey,TRoleKey> : IUserLoginStore<TUser,TKey>,
        IUserClaimStore<TUser, TKey>,
        IUserRoleStore<TUser, TKey>,
        IUserPasswordStore<TUser, TKey>,
        IUserSecurityStampStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>,
        IUserEmailStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>,
        IUserLockoutStore<TUser, TKey>,
        IUserStore<TUser,TKey>
        where TUser : IdentityUser<TKey> where TRole:IdentityRole<TRoleKey>
    {

        private IdentityDbContext<TUser, TRole, TKey, TRoleKey> _dbContent;
        public IdentityDbContext<TUser, TRole, TKey, TRoleKey> DbContent
        {
            get { return _dbContent; }
        }
        public UserStore(IdentityDbContext<TUser, TRole, TKey, TRoleKey> dbContent)
        {
            if(dbContent==null)
                throw new ArgumentNullException("dbContent is null");
            _dbContent = dbContent;
        }
        public virtual async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var userLogin = new IdentityUserLogin<TKey>()
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            };
            await DbContent.UserLoginRepository.InsertAsync(userLogin);

        }

        public virtual async Task<TUser> FindAsync(UserLoginInfo login)
        {
            var task = DbContent.UserLoginRepository.FindAsync(c => c.LoginProvider == login.LoginProvider && c.ProviderKey == login.ProviderKey);
            task.Wait();
            var logines = task.Result;
            if(logines!=null && logines.Any())
            {
                var userId = logines.First().UserId;
                return await DbContent.UserRepository.GetAsync(userId);
            }
            return null;
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result= await DbContent.UserLoginRepository.FindAsync(c => (object)c.UserId == (object)user.Id);
            IList<UserLoginInfo> rs = new List<UserLoginInfo>();
            if (result != null && result.Any())
            {
                foreach (var login in result)
                {
                    rs.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }
            }

            return rs;


        }

        public virtual async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            await DbContent.UserLoginRepository.RemoveAsync(c=>(object)c.UserId==(object)user.Id && c.ProviderKey==login.ProviderKey && c.LoginProvider==login.LoginProvider);
        }

        public virtual async Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            await DbContent.UserRepository.InsertAsync(user);
        }

        public virtual async Task DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            await DbContent.UserRepository.RemoveAsync(user);
        }

        public virtual async Task<TUser> FindByIdAsync(TKey userId)
        {
            if(userId==null)
            {
                throw new ArgumentNullException("userId");
            }
            return await DbContent.UserRepository.GetAsync(userId);
        }

        public virtual async Task<TUser> FindByNameAsync(string userName)
        {
            if(string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            var users = await DbContent.UserRepository.FindAsync(c => c.UserName == userName);
            return users.FirstOrDefault();
        }

        public virtual async Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual void Dispose()
        {
            //if (DbContent.UserRepository != null && DbContent.UserRepository is DapperRepository<TUser>)
            //{
            //    var rr = DbContent.UserRepository as DapperRepository<TUser>;
            //    rr.Dispose();
            //}
        }

        public virtual async Task AddClaimAsync(TUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            var userClaim = new IdentityUserClaim<TKey>()
            {
                UserId = user.Id,
                ClaimType = claim.ValueType,
                ClaimValue = claim.Value
            };
            await DbContent.UserClaimRepository.InsertAsync(userClaim);
        }

        public virtual async Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await DbContent.UserClaimRepository.FindAsync(c => (object)c.UserId == (object)user.Id);
            IList<System.Security.Claims.Claim> rs = new List<System.Security.Claims.Claim>();
            if (result != null && result.Any())
            {
                foreach (var login in result)
                {
                    rs.Add(new System.Security.Claims.Claim(login.ClaimType, login.ClaimValue));
                }
            }

            return rs;
        }

        public virtual async Task RemoveClaimAsync(TUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            await DbContent.UserClaimRepository.RemoveAsync(c=>(object)c.UserId==(object)user.Id && c.ClaimValue==claim.Value && c.ClaimType==claim.ValueType);
        }

        public virtual async Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            var roles = await DbContent.RoleRepository.FindAsync(c => c.Name == roleName);
            if(roles!=null && roles.Any())
            {
                var roleId = roles.First().Id;
                var userRole = new IdentityUserRole<TKey,TRoleKey>()
                {
                    RoleId = roleId,
                    UserId = user.Id,
                };
                await DbContent.UserRoleRepository.InsertAsync(userRole);
            }
        }

        public virtual async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var sql = GetRoleNamesSqlString();
            var roles = await DbContent.RoleRepository.QueryAsync<string>(sql, user);
            IList<string> rs = new List<string>();
            if(roles!=null)
            {
                foreach(var role in roles)
                {
                    rs.Add(role);
                }
            }
            return rs;

        }

        private string GetRoleNamesSqlString()
        {
            var roletableName=DbContent.RoleRepository.GetTableName<TRole>();
            var userRoleTableName=DbContent.UserRoleRepository.GetTableName<IdentityUserRole<TKey,TRoleKey>>();
            return string.Format("select name from {0} a inner join {1} b on a.Id=b.RoleId where b.UserId=@Id", roletableName, userRoleTableName);
        }
        private string GetIsInRoleSqlString()
        {
            var roletableName = DbContent.RoleRepository.GetTableName<TRole>();
            var userRoleTableName = DbContent.UserRoleRepository.GetTableName<IdentityUserRole<TKey, TRoleKey>>();
            return string.Format("select name from {0} a inner join {1} b on a.Id=b.RoleId where b.UserId=@UserId and a.Name=@Name", roletableName, userRoleTableName);

        }

        public virtual async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            var sql = GetIsInRoleSqlString();
            var param = new DynamicParameters();
            param.Add("UserId", user.Id);
            param.Add("Name", roleName);
            var roles = await DbContent.RoleRepository.QueryAsync<string>(sql, param);
            if (roles != null && roles.Any())
            {
                return true;
            }
            return false;
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if(string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            var roles=await DbContent.RoleRepository.FindAsync(c => c.Name == roleName);
            if (roles != null && roles.Any())
            {
                var role = roles.First();

                await DbContent.UserRoleRepository.RemoveAsync(c => (object)c.RoleId == (object)role.Id && (object)c.UserId == (object)user.Id);
            }
        }

        public virtual async Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PasswordHash);
            //var u = await DbContent.UserRepository.GetAsync(user.Id);
            //if (u != null)
            //    return u.PasswordHash;
            //return string.Empty;
        }

        public virtual async Task<bool> HasPasswordAsync(TUser user)
        {
            var passwordHash =await  GetPasswordHashAsync(user);
            return string.IsNullOrEmpty(passwordHash);
        }

        public virtual async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.SecurityStamp);
        }

        public virtual async Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual  IQueryable<TUser> Users
        {
            get {
                var users = GetAllUsers();
                //task.Start();
                //task.Wait();
                //var users=task.Result;
                return users.AsQueryable<TUser>();
            }
        }
        private IEnumerable<TUser> GetAllUsers()
        {
            var users = DbContent.UserRepository.Find(c => c.UserName != " ");
            return users;
        }
        //private async Task<IEnumerable<TUser>> GetAllUsers()
        //{
        //    var users=await DbContent.UserRepository.FindAsync(c => c.UserName != " ");
        //    return users;
        //}

        public virtual async Task<TUser> FindByEmailAsync(string email)
        {
            var users = await DbContent.UserRepository.FindAsync(c => c.Email == email);
            if (users != null && users.Any())
                return users.First();
            return null;
        }

        public virtual async Task<string> GetEmailAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.Email);
        }

        public virtual async Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.EmailConfirmed);
        }

        public virtual async Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.PhoneNumber);
        }

        public virtual async Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public virtual async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.AccessFailedCount);
        }

        public virtual async Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.LockoutEnabled);
        }

        public virtual async Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public virtual async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            await DbContent.UserRepository.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        public virtual async Task ResetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            await DbContent.UserRepository.UpdateAsync(user);
        }

        public virtual async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            await DbContent.UserRepository.UpdateAsync(user);
        }
    }
}
