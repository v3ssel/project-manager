using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTeam> EmployeeTeam { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                        .HasMany(x => x.ProjectFiles)
                        .WithOne(x => x.Project)
                        .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<EmployeeTeam>()
                        .HasKey(et => new { et.EmployeeId, et.TeamId });

            modelBuilder.Entity<Team>()
                        .HasOne(l => l.Lead)
                        .WithMany(t => t.LeadTeams)
                        .HasForeignKey(k => k.LeadId);

            modelBuilder.Entity<Team>()
                        .HasMany(t => t.Members)
                        .WithMany(m => m.Teams)
                        .UsingEntity<EmployeeTeam>(
                            l => l.HasOne<Employee>().WithMany().HasForeignKey(et => et.EmployeeId).OnDelete(DeleteBehavior.NoAction),
                            r => r.HasOne<Team>().WithMany().HasForeignKey(et => et.TeamId).OnDelete(DeleteBehavior.Cascade)
                         );
        }
    }
}
