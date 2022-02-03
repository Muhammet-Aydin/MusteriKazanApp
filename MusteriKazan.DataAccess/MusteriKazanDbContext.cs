using Microsoft.EntityFrameworkCore;
using MusteriKazan.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.DataAccess
{
    public class MusteriKazanDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-RJN403B\\SQLEXPRESS;Database= MusteriUygulama2;Integrated Security=True;");
        }

        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<MusteriAktivasyon> MusteriAktivasyonlar { get; set; }
       
        public DbSet<Kullanici> Kullanicilar { get; set; }
    }
}
