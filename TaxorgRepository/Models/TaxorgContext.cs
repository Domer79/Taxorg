using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using DataRepository;
using SystemTools;
using System.Data.Entity;

namespace TaxorgRepository.Models
{

    public sealed class TaxorgContext : RepositoryDataContext
    {
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

        public DbSet<FsFile> FsFiles { get; set; }
        public DbSet<FileSystem> FileSystems { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<TaxSummary> TaxSummaries { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<SliceTax> SliceTaxes { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionTaxType> SessionTaxTypes { get; set; }

        [DebuggerStepThrough]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region FileSystem

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

            #endregion

            #region Organization

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

            #endregion

            #region Tax

            modelBuilder.Entity<Tax>()
                .Property(e => e.TaxSum)
                .HasPrecision(19, 4);

            #endregion

            #region TaxType

            modelBuilder.Entity<TaxType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<TaxType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TaxType>()
                .HasMany(e => e.Taxes)
                .WithRequired(e => e.TaxType)
                .WillCascadeOnDelete(false);

            #endregion

            #region TaxSummary

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

            #endregion

            #region Error

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

            #endregion

            #region Bug

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
                .Property(e => e.Accept)
                .IsRequired();

            #endregion

            #region Settings

            modelBuilder.Entity<Settings>()
                .HasKey(e => e.IdSettings)
                .ToTable("Settings");

            modelBuilder.Entity<Settings>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Settings>()
                .Property(e => e.Value)
                .IsUnicode(false);

            modelBuilder.Entity<Settings>()
                .Property(e => e.Description)
                .IsUnicode(false);

            #endregion

            #region Sessions

//            modelBuilder.Entity<Session>().Property(e => e.Created).HasColumnType("smalldatetime");
//            modelBuilder.Entity<Session>().Property(e => e.Expires).HasColumnType("smalldatetime");
//            modelBuilder.Entity<Session>().Property(e => e.LockDate).HasColumnType("smalldatetime");

            #endregion

        }
    }
}
