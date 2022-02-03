using MusteriKazan.Web.Attributes;
using MusteriKazan.Web.Models;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusteriKazan.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["KullaniciAdi"] != null)
            {
                if (!String.IsNullOrEmpty(Session["KullaniciAdi"].ToString()))
                {
                    return RedirectToAction("KullaniciPanel", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            TempData["Error"] = "";
            if (String.IsNullOrEmpty(username))
            {
                TempData["Error"] = "Kullanıcı Adı Boş Olamaz";
                return RedirectToAction("Index", "Home");
            }

            if (String.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Şifre Adı Boş Olamaz";
                return RedirectToAction("Index", "Home");
            }

            BaseResult<Kullanici> response = await Agents.BaseAgent.Login("api/kullanici/Login/" + username + "/" + password + "", new Kullanici());

            if (response != null)
            {
                if (response.HasError)
                {
                    TempData["Error"] = response.Message;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session.Add("KullaniciAdi", username);
                    Session.Add("AccessToken", response.Result.Token);
                    Session.Add("TokenEndDate", response.Result.TokenEndDate);
                    Session.Timeout = 1440; //bir gün

                    return RedirectToAction("KullaniciPanel", "Home");
                    //return RedirectToAction("KullaniciSMSDogrulama", "Home");
                }
            }
            else
            {
                TempData["Error"] = "Bir hatayla karşılaşıldı.";
                return RedirectToAction("Index", "Home");
            }

        }

        [Route("Logout")]
        [HttpGet]
        public ActionResult Logout()
        {
            TempData["Error"] = "";
            Session["KullaniciAdi"] = null;
            Session["AccessToken"] = null;
            Session["TokenEndDate"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult KullaniciSMSDogrulama()
        {
            return View();
        }

        [LoginControl]
        [Route("KullaniciPanel")]
        [HttpGet]
        public async Task<ActionResult> KullaniciPanel()
        {
            TempData["activeKullaniciPanel"] = "active";
            var counts = await Agents.BaseAgent.MusterilerCount(Session["AccessToken"].ToString());
            if(counts != null)
            {
                if (counts.Result != null)
                {
                    return View(counts.Result);
                }
            }
            
            return View(new MusterilerCount());
        }


    }
}