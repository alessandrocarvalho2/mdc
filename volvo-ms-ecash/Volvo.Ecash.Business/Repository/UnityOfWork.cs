using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class UnitOfWork : BaseUnitOfWork
    {

        #region attributes

        private UserRepository _userRepository;

        #endregion

        #region Repositories
        public UserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        #endregion

        public UnitOfWork(DbContext context) : base(context)
        {
            _context = context;
            _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        protected override void _ResetRepositories()
        {
            _userRepository = null;
        }

        /// <summary>
        /// Class distructor.
        /// </summary>
        ~UnitOfWork()
        {
            _Dispose(false);
        }
    }
}
