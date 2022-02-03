using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusteriKazan.Web.Models
{
    public class Musteri
    {
        public int MusteriId { get; set; }
        public string KullaniciId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public string Eposta { get; set; }
        public string Adres { get; set; }
        public bool IsActive { get; set; }
        public DateTime KayıtTarih { get; set; }
        public Nullable<DateTime> AktivasyonTarih { get; set; }
    }
}