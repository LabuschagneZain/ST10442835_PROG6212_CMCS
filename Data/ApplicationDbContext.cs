using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ST10442835_PROG6212_CMCS.Models; // Replace 'YourApp' with your actual namespace

namespace ST10442835_PROG6212_CMCS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Example DbSet for the Claims table
        public DbSet<Claim> Claims { get; set; }
    }
}
