using Microsoft.EntityFrameworkCore;
using VotingManagementSystem.Models;

namespace VotingManagementSystem.Data
{
    public class VotingContext : DbContext
    {
        public VotingContext(DbContextOptions<VotingContext> options) : base(options)
        {
        }

        public DbSet<Vote> Votes { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.VoterId)
                .IsUnique(false);

            modelBuilder.Entity<Candidate>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Name).IsRequired();
            });

            modelBuilder.Entity<Vote>(b =>
            {
                b.HasKey(v => v.Id);
                b.HasOne(v => v.Candidate)
                    .WithMany()
                    .HasForeignKey(v => v.CandidateId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(v => new { v.VoterId, v.ElectionId, v.PositionId }).IsUnique();
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Email).IsUnique();
                b.HasIndex(u => u.Username).IsUnique();
            });

            modelBuilder.Entity<Voter>(b =>
            {
                b.HasKey(v => v.Id);
                b.HasIndex(v => v.NationalId).IsUnique();
                b.HasOne<User>().WithMany().HasForeignKey(v => v.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Candidate>(b =>
            {
                b.HasOne<User>().WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne<Election>().WithMany().HasForeignKey(c => c.ElectionId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Position>().WithMany().HasForeignKey(c => c.PositionId).OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}