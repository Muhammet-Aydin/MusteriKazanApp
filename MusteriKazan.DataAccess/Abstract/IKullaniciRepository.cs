using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.DataAccess.Abstract
{
    public interface IKullaniciRepository
    {
        Kullanici Login(string kullaniciAdi, string sifre);
    }
}
