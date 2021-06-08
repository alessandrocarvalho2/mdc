//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Volvo.Ecash.Dto.Model;

//namespace Volvo.Ecash.Domain.Repository
//{
//    public class ECashContext : DbContext
//    {
//        public DbSet<UserVR> Users { get; set; }

//        public ECashContext() : base()
//        {
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder options)
//        {
//            if (!options.IsConfigured)
//            {
//                //options.UseSqlServer(Setting.ConnectionString);
//                //switch (Setting.DatabaseType)
//                //{
//                //    case EnumCommon.DatabaseTypeEnum.SqlServer:
//                //        options.UseSqlServer(Setting.ConnectionString);
//                //        break;
//                //    case EnumCommon.DatabaseTypeEnum.PostgreSql:
//                //        options.UseNpgsql(Setting.ConnectionString);
//                //        break;
//                //    default:
//                //        options.UseSqlServer(Setting.ConnectionString);
//                //        break;
//                //}

//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {

//            modelBuilder.Entity<UserVR>().HasQueryFilter(x => !x.IsDeleted);

//            base.OnModelCreating(modelBuilder);
//        }


//        public override int SaveChanges()
//        {
//            return base.SaveChanges();
//        }

//        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//        {
//            return (await base.SaveChangesAsync(true, cancellationToken));
//        }

//        protected virtual long GetPrimaryKeyValue<T>(T entity)
//        {
//            string keyName = Model.FindEntityType(entity.GetType()).FindPrimaryKey().Properties.Select(x => x.Name).Single();
//            long result = (long)entity.GetType().GetProperty(keyName).GetValue(entity, null);
//            if (result < 0)
//                return -1;

//            return result;
//        }

//    }
//}
