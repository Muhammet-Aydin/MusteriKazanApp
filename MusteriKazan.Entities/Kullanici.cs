using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusteriKazan.Entities
{
    public class Kullanici
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int KullaniciId { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        public string KullaniciSifre { get; set; }

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public string TcKimlikNo { get; set; }

    }
}
