﻿using System;
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
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Koszyk> Koszyk { get; set; }
        public DbSet<AnonimowyKoszyk> AnonimowyKoszyk { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
