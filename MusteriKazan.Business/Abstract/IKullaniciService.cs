using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.Business.Abstract
{
    public interface IKullaniciService
    {
        Kullanici Login(string kullaniciAdi, string sifre);
    }
}
