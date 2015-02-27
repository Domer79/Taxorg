using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using TaxorgRepository.Repositories;
using TaxorgRepository.Tools;
using WebTools;

namespace TaxorgRepository.Models
{
    using System;
    using System.Data.Entity;

    public sealed class TaxorgContext : DbContext
    {
        private static TaxorgContext _context;

        public static TaxorgContext Context
        {
            get { return _context ?? (_context = new TaxorgContext(ApplicationSettings.ConnectionString)); }
//            get { return _context ?? (_context = new TaxorgContext("Data Source=.;Initial Catalog=Taxorg;Integrated Security=true")); }
        }

        public TaxorgContext()
            : this(ApplicationSettings.ConnectionString)
        {
        }

        public TaxorgContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
#if !DEBUG
            if (ApplicationCustomizer.LoggingDbContext)
                Database.Log = SystemLogs.SaveLog;
#endif
        }

        public DbSet<FsFile> FsFile { get; set; }
        public DbSet<FileSystem> FileSystem { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Tax> Tax { get; set; }
        public DbSet<TaxType> TaxType { get; set; }
        public DbSet<TaxSummary> TaxSummary { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<SliceTax> SliceTax { get; set; }

        [DebuggerStepThrough]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileSystem>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<FileSystem>()
                .Property(e => e.RemoteHostName)
                .IsUnicode(false);

            modelBuilder.Entity<FileSystem>()
                .Property(e => e.RemoteHostFileName)
                .IsUnicode(false);

            modelBuilder.Entity<FileSystem>()
                .Property(e => e.RemoteHostIpv4)
                .IsUnicode(false);

            modelBuilder.Entity<FileSystem>()
                .Property(e => e.RemoteHostIpv6)
                .IsUnicode(false);

            modelBuilder.Entity<FileSystem>()
                .Property(e => e.RemoteHostMac)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .Property(e => e.Inn)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .HasMany(e => e.Tax)
                .WithOptional(e => e.Organization)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Organization>().MapToStoredProcedures(c =>
            {
                c.Insert(i => i.HasName("AddOrganization").Result(r => r.IdOrganization, "generated_blog_identity"));
                c.Update(u => u.HasName("ModifyOrganization"));
                c.Delete(d => d.HasName("DeleteOrganization"));
            });

            modelBuilder.Entity<Tax>()
                .Property(e => e.TaxSum)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TaxType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<TaxType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TaxType>()
                .HasMany(e => e.Tax)
                .WithRequired(e => e.TaxType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaxSummary>()
                .HasKey(e => e.IdOrganization)
                .ToTable("TaxSummary");

            modelBuilder.Entity<TaxSummary>()
                .Property(e => e.Inn)
                .IsUnicode(false);

            modelBuilder.Entity<TaxSummary>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TaxSummary>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<TaxSummary>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<TaxSummary>()
                .Property(e => e.Tax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TaxSummary>()
                .Property(e => e.PrevTax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Error>()
                .Property(e => e.TypeError)
                .IsRequired()
                .IsUnicode(false);

            modelBuilder.Entity<Error>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Error>()
                .Property(e => e.StackTrace)
                .IsUnicode(false);

            modelBuilder.Entity<Error>()
                .Property(e => e.TimeLabel)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Error>()
                .HasKey(e => e.IdError)
                .ToTable("Error");

            modelBuilder.Entity<Bug>()
                .HasKey(e => e.IdBug)
                .ToTable("Bug");

            modelBuilder.Entity<Bug>()
                .Property(e => e.ErrorData)
                .IsUnicode(false);

            modelBuilder.Entity<Bug>()
                .Property(e => e.TimeLabel)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Bug>()
                .Property(e => e.IsNotLoaded)
                .IsRequired();
//                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }

        public string GetKeyName<TEntity>() where TEntity : class
        {
            if (_entityInfos.Keys.Contains(typeof (TEntity)))
                return ((EntityInfo<TEntity>)_entityInfos[typeof (TEntity)]).KeyName;

            var entityInfo = new EntityInfo<TEntity>(this);
            _entityInfos.Add(typeof(TEntity), entityInfo);

            return entityInfo.KeyName;
        }

        public Expression<Func<TEntity, TKey>> GetExpression<TEntity, TKey>() where TEntity : class
        {
            return GetExpression<TEntity, TKey>(null);
        }

        public Expression<Func<TEntity, TKey>> GetExpression<TEntity, TKey>(string columnName) where TEntity : class
        {
            if (_entityInfos.Keys.Contains(typeof(TEntity)))
                return ((EntityInfo<TEntity>)_entityInfos[typeof(TEntity)]).GetMemberAccess<TKey>(columnName);

            var entityInfo = new EntityInfo<TEntity>(this);
            _entityInfos.Add(typeof(TEntity), entityInfo);

            return entityInfo.GetMemberAccess<TKey>(columnName);
        }

        public Expression<Func<TEntity, object>> GetExpression<TEntity>(string columnName) where TEntity : class
        {
            if (_entityInfos.Keys.Contains(typeof(TEntity)))
                return ((EntityInfo<TEntity>)_entityInfos[typeof(TEntity)]).GetMemberAccess(columnName);

            var entityInfo = new EntityInfo<TEntity>(this);
            _entityInfos.Add(typeof(TEntity), entityInfo);

            return entityInfo.GetMemberAccess(columnName);
        }

        public EntityInfo<TEntity> GetEntityInfo<TEntity>() where TEntity : class
        {
            if (_entityInfos.Keys.Contains(typeof(TEntity)))
                return ((EntityInfo<TEntity>)_entityInfos[typeof(TEntity)]);

            var entityInfo = new EntityInfo<TEntity>(this);
            _entityInfos.Add(typeof(TEntity), entityInfo);

            return entityInfo;
        }

        private readonly Dictionary<Type, object> _entityInfos = new Dictionary<Type, object>();
    }
}
