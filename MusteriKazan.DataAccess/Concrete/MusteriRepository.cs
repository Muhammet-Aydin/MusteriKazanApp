using MusteriKazan.DataAccess.Abstract;
using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusteriKazan.DataAccess.Concrete
{
    public class MusteriRepository : IMusteriRepository
    {
        public Musteri CreateMusteri(Musteri musteri)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                Entities.Musteri entitiyModel = new Entities.Musteri
                {
                    Ad = musteri.Ad,
                    Adres = musteri.Adres,
                    Eposta = musteri.Eposta,
                    KullaniciId = musteri.KullaniciId,
                    Soyad = musteri.Soyad,
                    Telefon = musteri.Telefon,
                    IsActive = musteri.IsActive,
                    KayıtTarih = DateTime.Now,
                    AktivasyonTarih = null
                };
                var result = musteriKazanDbContext.Musteriler.Add(entitiyModel);
                musteriKazanDbContext.SaveChanges();

                return GetPasifMusteriByTelefon(musteri.Telefon);
            }
        }

        public void DeleteMusteri(int id)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var deletedMusteri = musteriKazanDbContext.Musteriler.Find(id);
                musteriKazanDbContext.Musteriler.Remove(deletedMusteri);
                musteriKazanDbContext.SaveChanges();
            }
        }

        public Musteri GetPasifMusteriById(int musteriId)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {                
                var result = musteriKazanDbContext.Musteriler.Where(c => c.MusteriId == musteriId && c.IsActive == false).FirstOrDefault();

                if (result != null)
                {
                    Musteri musteri = new Musteri
                    {
                        Ad = result.Ad,
                        Adres = result.Adres,
                        Eposta = result.Eposta,
                        KullaniciId = result.KullaniciId,
                        Soyad = result.Soyad,
                        Telefon = result.Telefon,
                        MusteriId = result.MusteriId,
                        IsActive = result.IsActive
                    };
                    return musteri;
                }
                else
                {
                    return null;
                }
            }
        }

        public Musteri GetMusteriByEposta(string eposta)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.Where(c => c.Eposta == eposta.Trim()).FirstOrDefault();

                if (result != null)
                {
                    Musteri musteri = new Musteri
                    {
                        Ad = result.Ad,
                        Adres = result.Adres,
                        Eposta = result.Eposta,
                        KullaniciId = result.KullaniciId,
                        Soyad = result.Soyad,
                        Telefon = result.Telefon,
                        MusteriId = result.MusteriId,
                        IsActive = result.IsActive
                    };
                    return musteri;
                }
                else
                {
                    return null;
                }
            }
        }

        public Musteri GetMusteriById(int id)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.Find(id);
                if (result != null)
                {
                    Musteri musteri = new Musteri
                    {
                        Ad = result.Ad,
                        Adres = result.Adres,
                        Eposta = result.Eposta,
                        KullaniciId = result.KullaniciId,
                        Soyad = result.Soyad,
                        Telefon = result.Telefon,
                        MusteriId = result.MusteriId,
                        IsActive = result.IsActive,
                        KayıtTarih = result.KayıtTarih,
                        AktivasyonTarih = result.AktivasyonTarih
                    };
                    return musteri;
                }
                else
                {
                    return null;
                }
            }
        }

        public Musteri GetMusteriByTelefon(string telefon)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.Where(c => c.Telefon == telefon.Trim()).FirstOrDefault();
                if (result != null)
                {
                    Musteri musteri = new Musteri
                    {
                        Ad = result.Ad,
                        Adres = result.Adres,
                        Eposta = result.Eposta,
                        KullaniciId = result.KullaniciId,
                        Soyad = result.Soyad,
                        Telefon = result.Telefon,
                        MusteriId = result.MusteriId,
                        IsActive = result.IsActive
                    };
                    return musteri;
                }
                else
                {
                    return null;
                }
            }
        }

        public int CheckUpdatedMusteri(int musteriId, string telefon, string eposta)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.Where(c => c.MusteriId != musteriId && (c.Eposta.Trim() == eposta.Trim() || c.Telefon.Trim() == telefon.Trim())).Count();
                return result;
            }
        }

        public Musteri GetReSendMusteri(int musteriId, string kullaniciId)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.Where(c => c.MusteriId == musteriId && c.IsActive == false && c.AktivasyonTarih == null && c.KullaniciId == kullaniciId).FirstOrDefault();
                if (result != null)
                {
                    Musteri musteri = new Musteri
                    {
                        Ad = result.Ad,
                        Adres = result.Adres,
                        Eposta = result.Eposta,
                        KullaniciId = result.KullaniciId,
                        Soyad = result.Soyad,
                        Telefon = result.Telefon,
                        MusteriId = result.MusteriId,
                        IsActive = result.IsActive
                    };
                    return musteri;
                }
                else
                {
                    return null;
                }
            }
        }

        public Musteri GetPasifMusteriByTelefon(string telefon)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.Where(c => c.Telefon == telefon.Trim() && c.IsActive == false).FirstOrDefault();
                if (result != null)
                {
                    Musteri musteri = new Musteri
                    {
                        Ad = result.Ad,
                        Adres = result.Adres,
                        Eposta = result.Eposta,
                        KullaniciId = result.KullaniciId,
                        Soyad = result.Soyad,
                        Telefon = result.Telefon,
                        MusteriId = result.MusteriId,
                        IsActive = result.IsActive
                    };
                    return musteri;
                }
                else
                {
                    return null;
                }
            }
        }

        public Musteri UpdateMusteri(Musteri musteri)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                Entities.Musteri entitiyModel = new Entities.Musteri
                {
                    Ad = musteri.Ad,
                    Adres = musteri.Adres,
                    Eposta = musteri.Eposta,
                    KullaniciId = musteri.KullaniciId,
                    Soyad = musteri.Soyad,
                    Telefon = musteri.Telefon,
                    MusteriId = musteri.MusteriId,
                    IsActive = musteri.IsActive
                };
                musteriKazanDbContext.Musteriler.Update(entitiyModel);
                musteriKazanDbContext.SaveChanges();
                return musteri;
            }
        }

        //SMS Gönderiminden sonra MusteriAktivasyon tablosuna kayıt atar.
        public MusteriAktivasyon CreateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                Entities.MusteriAktivasyon entitiyModel = new Entities.MusteriAktivasyon
                {
                    CreateDate = musteriAktivasyon.CreateDate,
                    FinishDate = musteriAktivasyon.CreateDate.AddDays(1),
                    Kod = musteriAktivasyon.Kod,
                    MusteriId = musteriAktivasyon.MusteriId,
                    IsUse = false
                };
                musteriKazanDbContext.MusteriAktivasyonlar.Add(entitiyModel);
                musteriKazanDbContext.SaveChanges();
                return musteriAktivasyon;
            }
        }

        public Musteri MusteriAktifEt(Musteri musteri)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                Entities.Musteri entitiyModel = new Entities.Musteri
                {
                    MusteriId = musteri.MusteriId,
                    IsActive = true,
                    AktivasyonTarih = DateTime.Now,
                    Ad = musteri.Ad,
                    Adres = musteri.Adres,
                    Eposta = musteri.Eposta,
                    KayıtTarih = musteri.KayıtTarih,
                    KullaniciId = musteri.KullaniciId,
                    Soyad = musteri.Soyad,
                    Telefon = musteri.Telefon
                };
                musteriKazanDbContext.Musteriler.Update(entitiyModel);
                musteriKazanDbContext.SaveChanges();
                return musteri;
            }
        }

        public MusteriAktivasyon UpdateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                Entities.MusteriAktivasyon entitiyModel = new Entities.MusteriAktivasyon
                {
                    Kod = musteriAktivasyon.Kod,
                    MusteriId = musteriAktivasyon.MusteriId,
                    IsUse = true,
                    CreateDate = musteriAktivasyon.CreateDate,
                    FinishDate = musteriAktivasyon.FinishDate,
                    ID = musteriAktivasyon.ID
                };
                musteriKazanDbContext.MusteriAktivasyonlar.Update(entitiyModel);
                musteriKazanDbContext.SaveChanges();

                musteriAktivasyon.IsUse = true;
                return musteriAktivasyon;
            }
        }

        //SMS Gitmediyse Tekrar SMS Gönder kullanıcı tıkladıysa aktivasyon kodunu yeni oluşturulan kod ile günceller.
        public MusteriAktivasyon UpdateKod(MusteriAktivasyon musteriAktivasyon, string kod)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                Entities.MusteriAktivasyon entitiyModel = new Entities.MusteriAktivasyon
                {
                    Kod = kod,
                    MusteriId = musteriAktivasyon.MusteriId,
                    IsUse = musteriAktivasyon.IsUse,
                    CreateDate = musteriAktivasyon.CreateDate,
                    FinishDate = musteriAktivasyon.FinishDate,
                    ID = musteriAktivasyon.ID
                };
                musteriKazanDbContext.MusteriAktivasyonlar.Update(entitiyModel);
                musteriKazanDbContext.SaveChanges();

                musteriAktivasyon.IsUse = true;
                return musteriAktivasyon;
            }
        }

        public MusteriAktivasyon GetMusteriAktivasyon(int musteriId)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.MusteriAktivasyonlar.Where(c => c.MusteriId == musteriId).OrderByDescending(c => c.CreateDate).FirstOrDefault();
                if (result != null)
                {
                    return new MusteriAktivasyon
                    {
                        CreateDate = result.CreateDate,
                        MusteriId = result.MusteriId,
                        FinishDate = result.FinishDate,
                        ID = result.ID,
                        IsUse = result.IsUse,
                        Kod = result.Kod
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Musteri> GetMusterilerByKullaniciId(string kullaniciId)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var entitiyResult = musteriKazanDbContext.Musteriler.Where(c => c.IsActive == true && c.KullaniciId == kullaniciId).ToList();

                List<Musteri> result = new List<Musteri>();
                foreach (var item in entitiyResult)
                {
                    result.Add(new Musteri
                    {
                        Ad = item.Ad,
                        Soyad = item.Soyad,
                        Adres = item.Adres,
                        AktivasyonTarih = item.AktivasyonTarih,
                        Eposta = item.Eposta,
                        IsActive = item.IsActive,
                        KayıtTarih = item.KayıtTarih,
                        KullaniciId = item.KullaniciId,
                        MusteriId = item.MusteriId,
                        Telefon = item.Telefon
                    });
                }

                return result;
            }
        }

        public List<Musteri> GetPasifMusterilerByKullaniciId(string kullaniciId)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var entitiyResult = musteriKazanDbContext.Musteriler.Where(c => c.IsActive == false && c.KullaniciId == kullaniciId).ToList();

                List<Musteri> result = new List<Musteri>();
                foreach (var item in entitiyResult)
                {
                    result.Add(new Musteri
                    {
                        Ad = item.Ad,
                        Soyad = item.Soyad,
                        Adres = item.Adres,
                        AktivasyonTarih = item.AktivasyonTarih,
                        Eposta = item.Eposta,
                        IsActive = item.IsActive,
                        KayıtTarih = item.KayıtTarih,
                        KullaniciId = item.KullaniciId,
                        MusteriId = item.MusteriId,
                        Telefon = item.Telefon
                    });
                }

                return result;
            }
        }

        public List<Musteri> TumMusteriler()
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.OrderByDescending(c => c.KayıtTarih).ToList();
                List<Musteri> response = new List<Musteri>();
                foreach (var item in result)
                {
                    response.Add(new Musteri
                    {
                        Ad = item.Ad,
                        KayıtTarih = item.KayıtTarih,
                        Adres = item.Adres,
                        AktivasyonTarih = item.AktivasyonTarih,
                        Eposta = item.Eposta,
                        IsActive = item.IsActive,
                        KullaniciId = item.KullaniciId,
                        MusteriId = item.MusteriId,
                        Soyad = item.Soyad,
                        Telefon = item.Telefon
                    });
                }
                return response;
            }
        }

        public MusterilerCount MusterilerCount()
        {
            MusterilerCount response = new MusterilerCount();
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var pasifMusteriler = musteriKazanDbContext.Musteriler.Where(c => c.IsActive == false).Count();
                response.PasifMusterilerCount = pasifMusteriler;

                var aktifMusteriler = musteriKazanDbContext.Musteriler.Where(c => c.IsActive == true).Count();
                response.AktifMusterilerCount = aktifMusteriler;

                var tumMusteriler = musteriKazanDbContext.Musteriler.Count();
                response.TumMusterilerCount = tumMusteriler;

                return response;
            }
        }

        public Musteri MusteriGuncelle(Musteri musteri)
        {
            try
            {
                using (var musteriKazanDbContext = new MusteriKazanDbContext())
                {
                    Entities.Musteri entitiyMusteri = new Entities.Musteri
                    {
                        Ad = musteri.Ad,
                        Adres = musteri.Adres,
                        AktivasyonTarih = musteri.AktivasyonTarih,
                        Eposta = musteri.Eposta,
                        IsActive = musteri.IsActive,
                        KayıtTarih = musteri.KayıtTarih,
                        KullaniciId = musteri.KullaniciId,
                        MusteriId = musteri.MusteriId,
                        Soyad = musteri.Soyad,
                        Telefon = musteri.Telefon
                    };
                    musteriKazanDbContext.Musteriler.Update(entitiyMusteri);
                    musteriKazanDbContext.SaveChanges();

                    return musteri;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Musteri> TumMusterilerByPaging(int index = 0, int take = 20)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                var result = musteriKazanDbContext.Musteriler.OrderByDescending(c => c.MusteriId).Skip(index * take).Take(take).ToList();
                List<Musteri> response = new List<Musteri>();
                foreach (var item in result)
                {
                    response.Add(new Musteri
                    {
                        Ad = item.Ad,
                        KayıtTarih = item.KayıtTarih,
                        Adres = item.Adres,
                        AktivasyonTarih = item.AktivasyonTarih,
                        Eposta = item.Eposta,
                        IsActive = item.IsActive,
                        KullaniciId = item.KullaniciId,
                        MusteriId = item.MusteriId,
                        Soyad = item.Soyad,
                        Telefon = item.Telefon
                    });
                }
                return response;
            }
        }


    }
}
