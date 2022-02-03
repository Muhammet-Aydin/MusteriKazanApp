using MusteriKazan.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MusteriKazan.Web.Agents
{
    public class BaseAgent
    {
        private static string apiUrl = ConfigurationManager.AppSettings["ApiUrl"].ToString();

        /// <summary>
        /// Get Müşteri By Id
        /// </summary>
        /// <param name="musteriId"></param>
        /// <returns></returns>
        public static async Task<BaseResult<Musteri>> GetMusteriById(string musteriId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync("api/musteri/GetMusteriById/" + musteriId);
                if (response.IsSuccessStatusCode)
                {
                    BaseResult<Musteri> result = await response.Content.ReadAsAsync<BaseResult<Musteri>>();
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Kullanıcı login olması için apiye yönlendirir.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="kullanici"></param>
        /// <returns></returns>
        public static async Task<BaseResult<Kullanici>> Login(string url, Kullanici kullanici)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(kullanici), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl + url), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<Kullanici> result = await response.Content.ReadAsAsync<BaseResult<Kullanici>>();
                        return result;
                    }
                    else
                    {
                        BaseResult<Kullanici> result = new BaseResult<Kullanici>
                        {
                            HasError = true,
                            Message = response.Content.ToString(),
                            Result = new Kullanici(),
                            StatusCode = 400
                        };
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                BaseResult<Kullanici> result = new BaseResult<Kullanici>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new Kullanici(),
                    StatusCode = 400
                };
                return result;
            }
        }

        /// <summary>
        /// Müşteri kayıt 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="musteri"></param>
        /// <returns></returns>
        public static async Task<BaseResult<Musteri>> CreateMusteri(string url, Musteri musteri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(musteri), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl + url), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<Musteri> result = await response.Content.ReadAsAsync<BaseResult<Musteri>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<BaseResult<Musteri>> MusteriAktiveEt(string url, MusteriAktivasyon musteri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(musteri), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl + url), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<Musteri> result = await response.Content.ReadAsAsync<BaseResult<Musteri>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Kullanıcının kaydettiği aktif müşterileri listeleyen metottur.
        /// </summary>
        /// <param name="kullaniciId"></param>
        /// <returns></returns>
        public static async Task<BaseResult<List<Musteri>>> GetMusterilerByKullaniciId(string kullaniciId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("api/musteri/GetMusterilerByKullaniciId/" + kullaniciId);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<List<Musteri>> result = await response.Content.ReadAsAsync<BaseResult<List<Musteri>>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// SMS Aktivasyon işlemini tamamlamamış pasif müşterileri listeleyen metottur.
        /// </summary>
        /// <param name="kullaniciId"></param>
        /// <returns></returns>
        public static async Task<BaseResult<List<Musteri>>> GetPasifMusterilerByKullaniciId(string kullaniciId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("api/musteri/GetMusterilerByKullaniciId/Pasif/" + kullaniciId);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<List<Musteri>> result = await response.Content.ReadAsAsync<BaseResult<List<Musteri>>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<BaseResult<Musteri>> ReSendSMS(int musteriId, string kullaniciId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(new Musteri()), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl + "api/musteri/ReSendSMS/" + musteriId + "/" + kullaniciId), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<Musteri> result = await response.Content.ReadAsAsync<BaseResult<Musteri>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Tüm müşterileri yönetici kullanıcıları için getirir.
        /// </summary>
        /// <param name="kullaniciId"></param>
        /// <returns></returns>
        public static async Task<BaseResult<List<Musteri>>> TumMusteriler()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("api/musteri/TumMusteriler");
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<List<Musteri>> result = await response.Content.ReadAsAsync<BaseResult<List<Musteri>>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<BaseResult<MusteriAktivasyon>> UpdateKod(string kullaniciId, int musteriId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(new MusteriAktivasyon()), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl + "api/musteri/UpdateKod/" + kullaniciId + "/" + musteriId), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<MusteriAktivasyon> result = await response.Content.ReadAsAsync<BaseResult<MusteriAktivasyon>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Müşterilerin toplam sayılarını döner.
        /// </summary>
        /// <returns></returns>
        public static async Task<BaseResult<MusterilerCount>> MusterilerCount(string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("api/musteri/MusterilerCount");
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<MusterilerCount> result = await response.Content.ReadAsAsync<BaseResult<MusterilerCount>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Pasif müşteriyi id ile bulur.
        /// </summary>
        /// <param name="musteriId"></param>
        /// <returns></returns>
        public static async Task<BaseResult<Musteri>> GetPasifMusteriById(int musteriId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync("api/musteri/GetPasifMusteriById/" + musteriId);
                if (response.IsSuccessStatusCode)
                {
                    BaseResult<Musteri> result = await response.Content.ReadAsAsync<BaseResult<Musteri>>();
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// müşteri güncelleme
        /// </summary>
        /// <param name="musteri"></param>
        /// <returns></returns>
        public static async Task<BaseResult<Musteri>> UpdateMusteri(Musteri musteri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(musteri), Encoding.UTF8);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //GET Method
                    HttpResponseMessage response = await client.PostAsync(new Uri(apiUrl + "api/musteri/MusteriGuncelle"), httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        BaseResult<Musteri> result = await response.Content.ReadAsAsync<BaseResult<Musteri>>();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}