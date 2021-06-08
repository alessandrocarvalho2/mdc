using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Business
{
    public abstract class BaseBusiness<T> : IDisposable where T : IUnitOfWork
    {

        //protected Logger logger { get; private set; }

        protected IConfiguration Configuration { get; set; }

        /// <summary>
        /// The parents business object.
        /// </summary>
        protected BaseBusiness<T> Parent { get; }

        /// <summary>
        /// Unit of Work
        /// </summary>
        protected T UnitOfWork { get; set; }

        /// <summary>
        /// Flag that handles the destruction of this instance.
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// Configuration that helps the unit of Work to connect to the databases.
        /// </summary>
        protected IConfiguration GetConfiguration() { return Configuration; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="Configuration">Configuration used for connecting to the database.</param>
        public BaseBusiness(DbContext context)
        {
            UnitOfWork = (T)Activator.CreateInstance(typeof(T), new object[] { context });
            //logger = LogManager.GetLogger(GetType().ToString());
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="Configuration">Configuration used for connecting to the database.</param>
        public BaseBusiness(IConfiguration configuration, DbContext context)
        {
            this.Configuration = configuration;
            UnitOfWork = (T)Activator.CreateInstance(typeof(T), new object[] { context });
            //logger = LogManager.GetLogger(GetType().ToString());
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="_parent"><see BusinessBase used as a parent object.</param>
        public BaseBusiness(BaseBusiness<T> _parent)
        {
            this.Parent = _parent;
            this.Configuration = _parent.GetConfiguration();
            this.UnitOfWork = this.Parent.UnitOfWork;
        }

        /// <summary>
        /// Verify if the object has a BusinessBase object as parent.
        /// </summary>
        /// <returns>True if it has parent. False otherwise</returns>
        public bool HasParent()
        {
            return this.Parent != null;
        }

        /// <summary>
        /// Distructor of the class.
        /// </summary>
        ~BaseBusiness()
        {
            _Dispose(false);
        }

        /// <summary>
        /// Performs the distruction of this class.
        /// </summary>
        /// <param name="disposing"></param>
        protected void _Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && !HasParent())
                {
                    UnitOfWork.Dispose();
                    UnitOfWork = default(T);
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Implemented method for destructor from <see cref="System.IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            _Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Commits any operation made in database from Unity of Work.
        /// </summary>
        protected void Commit()
        {
            if (!HasParent())
            {
                UnitOfWork.Commit();
            }
        }
    }
}
