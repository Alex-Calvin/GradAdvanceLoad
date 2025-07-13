using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GradAdvanceLoad.Models
{
    public partial class AdvanceContext : DbContext
    {
        public AdvanceContext()
        {
        }

        public AdvanceContext(DbContextOptions<AdvanceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GALOAD> GALOAD { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("LSU_ACALVIN");

            modelBuilder.Entity<GALOAD>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GA_LOAD");

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

                entity.Property(e => e.Loaded_Ind)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("LOADED_IND");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
