using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP1_KarineDunberry.Models
{
    public class ZhaoContext : DbContext
    {
         public ZhaoContext(DbContextOptions<ZhaoContext> options) : base(options)
        {
        }

        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<Evaluation> Evaluation { get; set; }
        public DbSet<Reservation> Reservation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Promotion>().ToTable("Promotion");
            //modelBuilder.Entity<Evaluation>().ToTable("Evaluation");
            //modelBuilder.Entity<Reservation>().ToTable("Reservation");
        }

    }
}
