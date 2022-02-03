using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.DataAccess.Abstract
{
    public interface IMusteriRepository
    {
        Musteri GetMusteriById(int id);
        Musteri GetMusteriByTelefon(string telefon);
        Musteri GetMusteriByEposta(string eposta);
        Musteri GetPasifMusteriById(int musteriId);
        void DeleteMusteri(int id);
        Web.Models.Musteri CreateMusteri(Musteri musteri);
        Musteri UpdateMusteri(Musteri musteri);
        MusteriAktivasyon CreateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon);
        Musteri GetPasifMusteriByTelefon(string telefon);
        Musteri MusteriAktifEt(Musteri musteri);
        MusteriAktivasyon GetMusteriAktivasyon(int musteriId);
        MusteriAktivasyon UpdateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon);
        List<Musteri> GetMusterilerByKullaniciId(string kullaniciId);
        List<Musteri> GetPasifMusterilerByKullaniciId(string kullaniciId);
        Musteri GetReSendMusteri(int musteriId, string kullaniciId);
        List<Musteri> TumMusteriler();
        MusteriAktivasyon UpdateKod(MusteriAktivasyon musteriAktivasyon, string kod);
        MusterilerCount MusterilerCount();
        Musteri MusteriGuncelle(Musteri musteri);
        int CheckUpdatedMusteri(int musteriId, string telefon, string eposta);
        List<Musteri> TumMusterilerByPaging(int index = 0, int take = 20);
    }
}
