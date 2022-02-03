using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusteriKazan.Web.Models
{
    public class Kullanici
    {
        [StringLength(5, MinimumLength = 3,
        ErrorMessage = "First Name should be minimum 3 characters and a maximum of 50 characters")]
        [Required(ErrorMessage = "{0} is required")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string KullaniciSifre { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TcKimlikNo { get; set; }
        public string UnvanKod { get; set; }
        public string SirketKod { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public DateTime? TokenEndDate { get; set; }



    }
}