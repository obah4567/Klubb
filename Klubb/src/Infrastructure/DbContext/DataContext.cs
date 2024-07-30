using Klubb.src.Domain.Models.Authentification;
using Klubb.src.Domain.Models.Messages;
using Klubb.src.Domain.Models.Profil;
using Microsoft.EntityFrameworkCore;

namespace Klubb.src.Infrastructure.Dbontext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
        
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<UserProfil> UpdateProfil => Set<UserProfil>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfil)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfil>(p => p.UserId);

            modelBuilder.Entity<UserProfil>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
