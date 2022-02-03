using MusteriKazan.Business.Abstract;
using MusteriKazan.DataAccess.Abstract;
using MusteriKazan.DataAccess.Concrete;
using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.Business.Concrete
{
    public class MusteriManager : IMusteriService
    {
        private IMusteriRepository _musteriRepository;
        public MusteriManager()
        {
            _musteriRepository = new MusteriRepository();
        }

        public Musteri CreateMusteri(Musteri musteri)
        {
            return _musteriRepository.CreateMusteri(musteri);
        }

        public Musteri GetPasifMusteriById(int musteriId)
        {
            return _musteriRepository.GetPasifMusteriById(musteriId);
        }

        public int CheckUpdatedMusteri(int musteriId, string telefon, string eposta)
        {
            return _musteriRepository.CheckUpdatedMusteri(musteriId, telefon, eposta);
        }

        public void DeleteMusteri(int id)
        {
            _musteriRepository.DeleteMusteri(id);
        }

        public Musteri GetMusteriByEposta(string eposta)
        {
            return _musteriRepository.GetMusteriByEposta(eposta);
        }

        public Musteri GetMusteriById(int id)
        {
            return _musteriRepository.GetMusteriById(id);
        }

        public Musteri GetMusteriByTelefon(string telefon)
        {
            return _musteriRepository.GetMusteriByTelefon(telefon);
        }

        public Musteri UpdateMusteri(Musteri musteri)
        {
            return _musteriRepository.UpdateMusteri(musteri);
        }

        public MusteriAktivasyon CreateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon)
        {
            return _musteriRepository.CreateMusteriAktivasyon(musteriAktivasyon);
        }

        public Musteri MusteriAktifEt(int musteriId)
        {
            var musteri = GetMusteriById(musteriId);
            if (musteri != null)
            {
                Entities.Musteri entitiyModel = new Entities.Musteri
                {
                    MusteriId = musteri.MusteriId,
                    IsActive = true,
                    AktivasyonTarih = DateTime.Now
                };
                _musteriRepository.MusteriAktifEt(musteri);
                return musteri;
            }
            else
            {
                return null;
            }
        }

        public MusteriAktivasyon AktivasyonKodKullan(MusteriAktivasyon musteriAktivasyon)
        {
            if (!musteriAktivasyon.IsUse)
            {
                if (DateTime.Now < musteriAktivasyon.FinishDate)
                {
                    return _musteriRepository.UpdateMusteriAktivasyon(musteriAktivasyon);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public MusteriAktivasyon UpdateKod(MusteriAktivasyon musteriAktivasyon, string kod)
        {
            if (!musteriAktivasyon.IsUse)
            {
                if (DateTime.Now < musteriAktivasyon.FinishDate)
                {
                    return _musteriRepository.UpdateKod(musteriAktivasyon, kod);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public MusteriAktivasyon GetMusteriAktivasyon(int musteriId)
        {
            return _musteriRepository.GetMusteriAktivasyon(musteriId);
        }

        public List<Musteri> GetMusteriByKullaniciId(string kullaniciId)
        {
            return _musteriRepository.GetMusterilerByKullaniciId(kullaniciId);
        }

        public List<Musteri> GetPasifMusterilerByKullaniciId(string kullaniciId)
        {
            return _musteriRepository.GetPasifMusterilerByKullaniciId(kullaniciId);
        }

        public Musteri GetReSendMusteri(int musteriId, string kullaniciId)
        {
            return _musteriRepository.GetReSendMusteri(musteriId, kullaniciId);
        }

        public List<Musteri> TumMusteriler()
        {
            return _musteriRepository.TumMusteriler();
        }

      
        public List<Musteri> TumMusterilerByPaging(int index, int take)
        {
            return _musteriRepository.TumMusterilerByPaging(index, take);
        }

        public MusterilerCount MusterilerCount()
        {
            return _musteriRepository.MusterilerCount();
        }

        public Musteri MusteriGuncelle(Musteri musteri)
        {
            return _musteriRepository.MusteriGuncelle(musteri);
        }

    }
}
