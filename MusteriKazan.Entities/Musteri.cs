using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusteriKazan.Entities
{
    public class Musteri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MusteriId { get; set; }

        [StringLength(20)]
        public string KullaniciId { get; set; }

        [StringLength(100)]
        public string Ad { get; set; }

        [StringLength(100)]
        public string Soyad { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        [StringLength(100)]
        public string Eposta { get; set; }

        [StringLength(500)]
        public string Adres { get; set; }
        public bool IsActive { get; set; }
        public DateTime KayıtTarih { get; set; }
        public Nullable<DateTime> AktivasyonTarih { get; set; }


    }
}
