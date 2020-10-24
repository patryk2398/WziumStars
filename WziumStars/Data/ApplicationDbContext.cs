using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WziumStars.Models;

namespace WziumStars.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kategoria> Kategoria { get; set; }
        public DbSet<Podkategoria> Podkategoria { get; set; }
        public DbSet<Produkt> Produkt { get; set; }
        public DbSet<Kupon> Kupon { get; set; }
    }
}
