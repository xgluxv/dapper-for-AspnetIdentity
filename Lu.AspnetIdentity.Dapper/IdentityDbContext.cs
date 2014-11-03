using Lu.Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lu.AspnetIdentity.Dapper
{

    public class DapperIdentityDbContext<TUser,TRole>:IdentityDbContext<TUser, TRole, string, string>
        where TUser : IdentityUser<string>
        where TRole : IdentityRole<string>
    {
        public DapperIdentityDbContext(IDbConnection connection):base(connection,null)
        {

        }
        public DapperIdentityDbContext(IDbConnection connection, IDbTransaction transaction):base(connection,transaction)
        {
        }
    }

    public class IdentityDbContext<TUser, TRole, TKey, TRoleKey>:IDisposable
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TRoleKey>
    {
        private IDbConnection _conn;
        private IDbTransaction _transaction;


        private IRepository<TUser, TKey> _userRepository;
        private IRepository<IdentityUserLogin<TKey>> _userLoginRepository;
        private IRepository<IdentityUserClaim<TKey>> _userClaimRepository;
        private IRepository<TRole, TRoleKey> _roleRepository;
        private IRepository<IdentityUserRole<TKey, TRoleKey>> _userRoleRepository;

        public IRepository<TUser, TKey> UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new DapperRepository<TUser, TKey>(DbConnection, DbTransaction));
            }
        }
        public IRepository<TRole, TRoleKey> RoleRepository
        {
            get
            {
                return _roleRepository ?? (_roleRepository = new DapperRepository<TRole, TRoleKey>(DbConnection, DbTransaction));
            }
        }
        public IRepository<IdentityUserRole<TKey, TRoleKey>> UserRoleRepository
        {
            get
            {
                return _userRoleRepository ?? (_userRoleRepository = new DapperRepository<IdentityUserRole<TKey, TRoleKey>>(DbConnection, DbTransaction));
            }
        }
        public IRepository<IdentityUserLogin<TKey>> UserLoginRepository
        {
            get
            {
                return _userLoginRepository ?? (_userLoginRepository = new DapperRepository<IdentityUserLogin<TKey>>(DbConnection, DbTransaction));
            }
        }
        public IRepository<IdentityUserClaim<TKey>> UserClaimRepository
        {
            get
            {
                return _userClaimRepository ?? (_userClaimRepository = new DapperRepository<IdentityUserClaim<TKey>>(DbConnection, DbTransaction));
            }
        }

        public IDbConnection DbConnection
        {
            get
            {
                return _conn;
            }
        }
        public IDbTransaction DbTransaction
        {
            get
            {
                return _transaction;
            }
        }

        public IdentityDbContext(IDbConnection connection):this(connection,null)
        {

        }
        public IdentityDbContext(IDbConnection connection,IDbTransaction transaction)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            _conn = connection;
            _transaction = transaction;
        }

        public void Dispose()
        {
            if(_conn.State!=ConnectionState.Closed)
            {
                _conn.Close();
            }
        }
    }
}
