using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataProcessor.Configuration;

namespace DataProcessor.Models
{
    public class DataContext : DbContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public DataContext(DatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public DataContext(DbContextOptions<DataContext> options, DatabaseSettings databaseSettings)
            : base(options)
        {
            _databaseSettings = databaseSettings;
        }

        public virtual DbSet<UserRecord> UserRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle(_databaseSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("APP_SCHEMA");

            modelBuilder.Entity<UserRecord>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("USER_LOAD_TABLE");

                entity.Property(e => e.Email)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.ExternalId)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("EXTERNAL_ID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.LastName)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.ProcessedIndicator)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("PROCESSED_IND");
            });
        }
    }
} 