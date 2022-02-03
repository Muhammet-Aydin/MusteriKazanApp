using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.Business.Abstract
{
    public interface IMusteriService
    {
        Musteri GetMusteriById(int id);
        Musteri GetMusteriByTelefon(string telefon);
        Musteri GetMusteriByEposta(string eposta);
        void DeleteMusteri(int id);
        Musteri CreateMusteri(Musteri musteri);
        Musteri GetPasifMusteriById(int musteriId);
        Musteri UpdateMusteri(Musteri musteri);
        MusteriAktivasyon CreateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon);
        Musteri MusteriAktifEt(int musteriId);
        MusteriAktivasyon AktivasyonKodKullan(MusteriAktivasyon musteriAktivasyon);
        MusteriAktivasyon GetMusteriAktivasyon(int musteriId);
        List<Musteri> GetMusteriByKullaniciId(string kullaniciId);
        List<Musteri> GetPasifMusterilerByKullaniciId(string kullaniciId);
        Musteri GetReSendMusteri(int musteriId, string kullaniciId);
        List<Musteri> TumMusteriler();
        MusteriAktivasyon UpdateKod(MusteriAktivasyon musteriAktivasyon, string kod);
        MusterilerCount MusterilerCount();
        Musteri MusteriGuncelle(Musteri musteri);
        List<Musteri> TumMusterilerByPaging(int index, int take);
        int CheckUpdatedMusteri(int musteriId, string telefon, string eposta);
    }
}
