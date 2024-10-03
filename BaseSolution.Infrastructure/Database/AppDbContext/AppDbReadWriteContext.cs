using BaseSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Database.AppDbContext
{
    public class AppDbReadWriteContext : DbContext
    {
        public AppDbReadWriteContext(DbContextOptions<AppDbReadWriteContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbReadWriteContext).Assembly);
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        //public DbSet<Participant> Participants { get; set; }
        //public DbSet<Registration> Registrations { get; set; }
        //public DbSet<Review> Reviews { get; set; }
    }
}
