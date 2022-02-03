using MusteriKazan.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;
using MusteriKazan.Web.Models;
using System.Linq;

namespace MusteriKazan.DataAccess.Concrete
{
    public class KullaniciRepository : IKullaniciRepository
    {
        public Web.Models.Kullanici Login(string kullaniciAdi, string sifre)
        {
            try
            {
                Web.Models.Kullanici kullanici = new Web.Models.Kullanici();
                using (var musteriKazanDbContext = new MusteriKazanDbContext())
                {
                    var result = musteriKazanDbContext.Kullanicilar.Where(c => c.KullaniciAdi == kullaniciAdi && c.KullaniciSifre == sifre).FirstOrDefault();

                    if (result != null)
                    {
                        kullanici.Ad = result.Ad;
                        kullanici.KullaniciAdi = result.KullaniciAdi;
                        kullanici.KullaniciSifre = result.KullaniciSifre;
                        kullanici.Soyad = result.Soyad;
                        kullanici.TcKimlikNo = result.TcKimlikNo;

                        return kullanici;
                    }                   
                    
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
