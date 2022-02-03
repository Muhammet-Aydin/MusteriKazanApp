using MusteriKazan.Web.Agents;
using MusteriKazan.Web.Attributes;
using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusteriKazan.Web.Controllers
{
    public class MusteriController : Controller
    {
        // GET: Musteri
        [LoginControl]
        public ActionResult Index()
        {
            TempData["activeMusteriKaydet"] = "active";
            return View();
        }

        [LoginControl]
        [Route("CreateMusteri")]
        [HttpPost]
        public async Task<ActionResult> CreateMusteri(string ad, string soyad, string telefon, string eposta, string adres)
        {
            TempData["MusteriInfo"] = "";

            Musteri musteri = new Musteri();
            musteri.Ad = ad;
            musteri.Soyad = soyad;
            musteri.Telefon = telefon;
            musteri.Eposta = eposta;
            musteri.Adres = adres;
            musteri.IsActive = false;
            musteri.KayıtTarih = DateTime.Now;
            musteri.AktivasyonTarih = null;
            musteri.KullaniciId = Session["KullaniciAdi"].ToString();

            BaseResult<Musteri> result = await BaseAgent.CreateMusteri("api/musteri/CreateMusteri", musteri);

            if (result != null)
            {
                if (result.HasError)
                {
                    TempData["MusteriInfo"] = result.Message;
                    return RedirectToAction("Index", "Musteri");
                }
                else
                {
                    return RedirectToAction("SMSDogrulama", "Musteri", new { musteriId = result.Result.MusteriId });
                }
            }
            else
            {
                TempData["MusteriInfo"] = "Bir hatayla karşılaşıldı.";
                return RedirectToAction("Index", "Musteri");
            }
        }

        [LoginControl]
        [Route("SMSDogrulama")]
        [HttpGet]
        public async Task<ActionResult> SMSDogrulama(int musteriId = 0)
        {
            var result = await BaseAgent.GetPasifMusteriById(musteriId);
            return View(result.Result);
        }

        [LoginControl]
        [Route("Aktivasyon")]
        [HttpPost]
        public async Task<ActionResult> Aktivasyon(int musteriId, string kod)
        {
            TempData["Error"] = "";
            TempData["PanelInfo"] = "";
            if (musteriId == 0)
            {
                TempData["Error"] = "Müşteri Idsi 0 olamaz.";
                return RedirectToAction("SMSDogrulama", "Musteri", new { musteriId = musteriId });
            }
            MusteriAktivasyon musteriAktivasyon = new MusteriAktivasyon
            {
                MusteriId = musteriId,
                Kod = kod
            };

            BaseResult<Musteri> result = await BaseAgent.MusteriAktiveEt("api/musteri/MusteriAktifEt", musteriAktivasyon);

            if (result != null)
            {
                if (!result.HasError)
                {
                    TempData["PanelInfo"] = result.Message;
                    return RedirectToAction("KullaniciPanel", "Home");
                }
                else
                {
                    TempData["MusteriInfo"] = result.Message;
                    if (result.StatusCode == 404)
                    {
                        return RedirectToAction("ReSendSMS", "Musteri", new { musteriId = musteriId });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return RedirectToAction("SMSDogrulama", "Musteri", new { musteriId = musteriId });
                    }

                }
            }

            TempData["Error"] = result.Message;
            return RedirectToAction("SMSDogrulama", "Musteri", new { musteriId = musteriId });
        }

        [LoginControl]
        [Route("GetAktifMusteriler")]
        [HttpGet]
        public async Task<ActionResult> GetAktifMusteriler()
        {
            string kullaniciId = Session["KullaniciAdi"].ToString();
            TempData["PanelInfo"] = "";
            BaseResult<List<Musteri>> result = await BaseAgent.GetMusterilerByKullaniciId(kullaniciId);

            if (result != null)
            {
                if (!result.HasError)
                {
                    return View(result.Result);
                }
                else
                {
                    TempData["PanelInfo"] = result.Message;
                    return RedirectToAction("KullaniciPanel", "Home");
                }
            }

            TempData["PanelInfo"] = "Liste getirilemedi.";
            return RedirectToAction("KullaniciPanel", "Home");
        }

        [LoginControl]
        [Route("GetPasifMusteriler")]
        [HttpGet]
        public async Task<ActionResult> GetPasifMusteriler()
        {
            string kullaniciId = Session["KullaniciAdi"].ToString();
            TempData["PanelInfo"] = "";
            BaseResult<List<Musteri>> result = await BaseAgent.GetPasifMusterilerByKullaniciId(kullaniciId);

            if (result != null)
            {
                if (!result.HasError)
                {
                    return View(result.Result);
                }
                else
                {
                    TempData["PanelInfo"] = result.Message;
                    return RedirectToAction("KullaniciPanel", "Home");
                }
            }

            TempData["PanelInfo"] = "Liste getirilemedi.";
            return RedirectToAction("KullaniciPanel", "Home");
        }

        [LoginControl]
        [Route("ReSendSMS")]
        [HttpGet]
        public ActionResult ReSendSMS(int musteriId)
        {
            TempData["ReSendSmsError"] = "";
            return View(musteriId);
        }

        [LoginControl]
        [Route("ReSendSMSPost")]
        [HttpPost]
        public async Task<ActionResult> ReSendSMSPost(int musteriId = 0)
        {
            var result = await BaseAgent.ReSendSMS(musteriId, Session["KullaniciAdi"].ToString());

            if (result != null)
            {
                if (result.HasError)
                {
                    TempData["ReSendSmsError"] = result.Message;
                    return RedirectToAction("ReSendSMS", "Musteri");
                }
                else
                {
                    return RedirectToAction("SMSDogrulama", "Musteri", new { musteriId = musteriId });
                }
            }
            else
            {
                TempData["ReSendSmsError"] = "Beklenmedik bir hata oluştu.";
                return RedirectToAction("ReSendSMS", "Musteri");
            }
        }

        [LoginControl]
        [HttpGet]
        [Route("TumMusteriler")]
        public async Task<ActionResult> TumMusteriler()
        {
            TempData["activeTumMusteriler"] = "active";
            TempData["TumMusterilerInfo"] = "";
            var result = await BaseAgent.TumMusteriler();

            if (result != null)
            {
                if (result.HasError)
                {
                    TempData["TumMusterilerInfo"] = result.Message;
                    return View();
                }
                else
                {
                    return View(result.Result);
                }
            }
            else
            {
                TempData["TumMusterilerInfo"] = "Beklenmedik bir hata oluştu.";
                return View();
            }
        }

        [LoginControl]
        [HttpPost]
        [Route("UpdateKod")]
        public async Task<ActionResult> UpdateKod(int musteriId = 0)
        {
            var result = await BaseAgent.UpdateKod(Session["KullaniciAdi"].ToString(), musteriId);

            if (result != null)
            {
                if (!result.HasError)
                {
                    TempData["Error"] = "Aktivasyon Kodu yeniden gönderildi. Lütfen telefonunuzu kontrol edin. SMS gelmediği durumda yetkililer ile iletişime geçin.";
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
            }
            else
            {
                TempData["Error"] = "Beklenmedik bir hata oluştu.";
            }

            return RedirectToAction("SMSDogrulama", "Musteri", new { musteriId = musteriId });
        }

        [LoginControl]
        [HttpGet]
        [Route("UpdateMusteri")]
        public async Task<ActionResult> UpdateMusteri(int musteriId)
        {
            var result = await BaseAgent.GetPasifMusteriById(musteriId);

            if (result != null)
            {
                if (!result.HasError)
                {
                    return View(result.Result);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [LoginControl]
        [HttpPost]
        [Route("UpdateMusteri")]
        public async Task<ActionResult> UpdateMusteri(int musteriId, string ad, string soyad, string telefon, string eposta, string adres)
        {
            Musteri musteri = new Musteri();
            musteri.MusteriId = musteriId;
            musteri.Ad = ad;
            musteri.Soyad = soyad;
            musteri.Eposta = eposta;
            musteri.Adres = adres;
            musteri.Telefon = telefon;
            musteri.KullaniciId = Session["KullaniciAdi"].ToString();

            var result = await BaseAgent.UpdateMusteri(musteri);

            if (result != null)
            {
                if (!result.HasError)
                {
                    TempData["PasifMusterilerInfo"] = result.Message;
                    return RedirectToAction("GetPasifMusteriler", "Musteri");
                }
                else
                {
                    TempData["GuncellemeInfo"] = result.Message;
                    return RedirectToAction("UpdateMusteri", "Musteri", new { musteriId = musteriId });
                }
            }
            else
            {
                TempData["GuncellemeInfo"] = "Beklenmedik bir hata oluştu.";
                return RedirectToAction("UpdateMusteri", "Musteri", new { musteriId = musteriId });
            }
        }

    }
}