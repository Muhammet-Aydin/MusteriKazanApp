using MusteriKazan.Business.Abstract;
using MusteriKazan.DataAccess.Abstract;
using MusteriKazan.DataAccess.Concrete;
using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.Business.Concrete
{
    public class KullaniciManager : IKullaniciService
    {
        private IKullaniciRepository _kullaniciRepository;
        public KullaniciManager()
        {
            _kullaniciRepository = new KullaniciRepository();
        }

        public Kullanici Login(string kullaniciAdi, string sifre)
        {
            return _kullaniciRepository.Login(kullaniciAdi, sifre);
        }

    }
}
