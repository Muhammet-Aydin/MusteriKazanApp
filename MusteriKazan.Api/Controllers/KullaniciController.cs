using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MusteriKazan.Api.ILogger;
using MusteriKazan.Api.Logger;
using MusteriKazan.Business.Abstract;
using MusteriKazan.Business.Concrete;
using MusteriKazan.Web.Models;
using System;

namespace MusteriKazan.Api.Controllers
{
    [ApiController]
    [Route("api/kullanici")]
    public class KullaniciController : Controller
    {
        private IKullaniciService _kullaniciService;
   
        readonly IConfiguration _configuration;
        public KullaniciController(IConfiguration configuration)
        {
            _kullaniciService = new KullaniciManager();
            _configuration = configuration;
            //_kullaniciLog = new KullaniciLogger();
        }

        [HttpPost("Login/{kullaniciadi}/{sifre}")]
        public BaseResult<Kullanici> Login(string kullaniciadi, string sifre)
        {
            try
            {
                var result = _kullaniciService.Login(kullaniciadi.ToUpper(), sifre);

                if (result != null)
                {
                    TokenHandler.TokenHandler tokenHandler = new TokenHandler.TokenHandler(_configuration);
                    Models.Token token = tokenHandler.CreateAccessToken(result);

                    result.RefreshToken = token.RefreshToken;
                    result.RefreshTokenEndDate = token.Expiration.AddDays(1);
                    result.Token = token.AccessToken;
                    result.TokenEndDate = token.Expiration;

                    
                    return new BaseResult<Kullanici>
                    {
                        StatusCode = 200,
                        Result = result,
                        HasError = false,
                        Message = "OK"
                    };
                }
                else
                {
                  
                    return new BaseResult<Kullanici>
                    {
                        StatusCode = 400,
                        Result = result,
                        HasError = true,
                        Message = "Kullanıcı adı veya şifre hatalı."
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResult<Kullanici>
                {
                    StatusCode = 401,
                    Result = new Kullanici(),
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

    }
}
