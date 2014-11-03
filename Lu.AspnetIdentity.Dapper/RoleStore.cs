using Lu.Dapper.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lu.AspnetIdentity.Dapper
{

    public class RoleStore<TUser,TRole> : RoleStore<TUser,TRole, string,string> 
        where TUser :IdentityUser
        where TRole : IdentityRole
    {
        public RoleStore(DapperIdentityDbContext<TUser, TRole> dbContext)
            : base(dbContext)
        {

        }
    }

    public class RoleStore<TUser,TRole, TKey,TRoleKey> : IQueryableRoleStore<TRole, TRoleKey>, IRoleStore<TRole, TRoleKey>, IDisposable
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TRoleKey>
    {
        private IdentityDbContext<TUser, TRole, TKey, TRoleKey> _dbContent;
        public IdentityDbContext<TUser, TRole, TKey, TRoleKey> DbContent
        {
            get { return _dbContent; }
        }
        public RoleStore(IdentityDbContext<TUser, TRole, TKey, TRoleKey> dbContent)
        {
            if(dbContent==null)
                throw new ArgumentNullException("dbContent is null");
            _dbContent = dbContent;
        }

        public virtual IQueryable<TRole> Roles
        {
            get {
                var roles = GetAllRoles();
                //task.Start();
                //task.Wait();
                //var roles = task.Result;
                return roles.AsQueryable<TRole>();
            }
        }
        private IEnumerable<TRole> GetAllRoles()
        {
            try
            {
                var tableName = DbContent.RoleRepository.GetTableName<TRole>();
                var sql = string.Format("select * from {0}", tableName);
                var roles = DbContent.RoleRepository.Query<TRole>(sql, null);
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //private async Task<IEnumerable<TRole>> GetAllRoles()
        //{
        //    try
        //    {
        //        var tableName = DbContent.RoleRepository.GetTableName<TRole>();
        //        var sql = string.Format("select * from {0}", tableName);
        //        var roles = await DbContent.RoleRepository.QueryAsync<TRole>(sql, null);
        //        return roles;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public virtual async Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            await DbContent.RoleRepository.InsertAsync(role);
            //return Task.FromResult<object>(null);
        }

        public virtual async Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            await DbContent.RoleRepository.RemoveAsync(role);
            //return Task.FromResult<object>(null);
        }

        public virtual async Task<TRole> FindByIdAsync(TRoleKey roleId)
        {
            return await DbContent.RoleRepository.GetAsync(roleId);
        }

        public virtual async Task<TRole> FindByNameAsync(string roleName)
        {
            var roles= await DbContent.RoleRepository.FindAsync(c => c.Name == roleName);
            return roles.FirstOrDefault();
        }

        public virtual async Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            await DbContent.RoleRepository.UpdateAsync(role);
        }

        public virtual void Dispose()
        {
            //if (DbContent.RoleRepository != null && DbContent.RoleRepository is DapperRepository<TRole>)
            //{
            //    var rr = DbContent.RoleRepository as DapperRepository<TRole>;
            //    rr.Dispose();
            //}
        }
    }
}
