using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopJobs.Models;

namespace TopJobs.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<JobAd> JobAds { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<EducationEntry> EducationEntries { get; set; }
        public DbSet<EducationType> EducationTypes { get; set; }
        public DbSet<JobExperienceEntry> JobExperienceEntries { get; set; }
        public DbSet<PositionType> PositionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User", schema: "Identity");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role", schema: "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", schema: "Identity");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", schema: "Identity");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", schema: "Identity");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: "Identity");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", schema: "Identity");
            });

            builder.Entity<Company>()
                .HasMany(c => c.JobAds)
                .WithOne(j => j.Company)
                .IsRequired();
            builder.Entity<JobApplication>()
                .HasKey(jobApplication => new { jobApplication.JobAdId, jobApplication.UserId });
            builder.Entity<JobApplication>()
                .HasOne(jobApplication => jobApplication.JobAd)
                .WithMany(j => j.JobApplications)
                .HasForeignKey(jobApplication => jobApplication.JobAdId);
            builder.Entity<JobApplication>()
                .HasOne(jobApplication => jobApplication.User)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(jobApplication => jobApplication.UserId);
            builder.Entity<EducationEntry>()
                .HasOne(e => e.User)
                .WithMany(u => u.EducationEntries)
                .IsRequired();
            builder.Entity<EducationEntry>()
                .HasOne(e => e.EducationType)
                .WithMany(et => et.EducationEntries)
                .IsRequired();
            builder.Entity<JobExperienceEntry>()
                .HasOne(j => j.PositionType)
                .WithMany(p => p.JobExperienceEntries)
                .IsRequired();
            builder.Entity<JobExperienceEntry>()
                .HasOne(j => j.Company)
                .WithMany(c => c.JobExperienceEntries)
                .IsRequired();
                
        }
    }
}
