using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusteriKazan.Api.ILogger;
using MusteriKazan.Api.Logger;
using MusteriKazan.Business.Abstract;
using MusteriKazan.Business.Concrete;
using MusteriKazan.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MusteriKazan.Api.Controllers
{
    [ApiController]
    [Route("api/musteri")]
    public class MusteriController : Controller
    {
        private IMusteriService _musteriService;
        private IMusteriLogger _musteriLogger;
     
        public MusteriController()
        {
            _musteriService = new MusteriManager();
            _musteriLogger = new MusteriLogger();
            //_aS400MusteriManager = new AS400MusteriManager();
        }

        [Authorize]
        [HttpGet("GetMusteriById/{id}")]
        public BaseResult<Musteri> GetMusteriById(int id)
        {
            var result = new BaseResult<Musteri>
            {
                StatusCode = 200,
                Result = _musteriService.GetMusteriById(id),
                HasError = false
            };
            return result;
        }

        [Authorize]
        [HttpGet("GetMusteriByTelefon/{telefon}")]
        public Musteri GetMusteriByTelefon(string telefon)
        {
            return _musteriService.GetMusteriByTelefon(telefon);
        }

        [Authorize]
        [HttpGet("GetPasifMusteriById/{musteriId}")]
        public BaseResult<Musteri> GetPasifMusteriById(int musteriId)
        {
            try
            {
                var result = _musteriService.GetPasifMusteriById(musteriId);

                if (result != null)
                {
                    return new BaseResult<Musteri>
                    {
                        StatusCode = 200,
                        Result = result,
                        HasError = false,
                        Message = "OK"
                    };
                }
                else
                {
                    return new BaseResult<Musteri>
                    {
                        StatusCode = 400,
                        Result = new Musteri(),
                        HasError = true,
                        Message = "Pasif müşteri bulunamadı."
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResult<Musteri>
                {
                    StatusCode = 400,
                    Result = new Musteri(),
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [Authorize]
        [HttpPost("CreateMusteri")]
        public BaseResult<Musteri> CreateMusteri(Musteri musteri)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var hasMusteriByEposta = _musteriService.GetMusteriByEposta(musteri.Eposta);
                    if (hasMusteriByEposta != null)
                    {
                        return new BaseResult<Musteri>
                        {
                            StatusCode = 400,
                            Result = hasMusteriByEposta,
                            HasError = true,
                            Message = "Bu epostaya ait kayıt zaten mevcut. Müşteri durumu: " + hasMusteriByEposta.IsActive
                        };
                    }

                    var hasMusteriByTelefon = _musteriService.GetMusteriByTelefon(musteri.Telefon);
                    if (hasMusteriByTelefon != null)
                    {
                        return new BaseResult<Musteri>
                        {
                            StatusCode = 400,
                            Result = hasMusteriByTelefon,
                            HasError = true,
                            Message = "Bu telefona ait kayıt zaten mevcut. Müşteri durumu: " + hasMusteriByTelefon.IsActive
                        };
                    }

                    var result = _musteriService.CreateMusteri(musteri);

                    if (result != null)
                    {
                        Random generator = new Random();
                        int code = generator.Next(100000, 999999);

                        MusteriAktivasyon musteriAktivasyon = new MusteriAktivasyon
                        {
                            CreateDate = DateTime.Now,
                            FinishDate = DateTime.Now.AddDays(1),
                            Kod = code.ToString(),
                            IsUse = false,
                            MusteriId = result.MusteriId
                        };

                        var aktivasyonResponse = CreateMusteriAktivasyon(musteriAktivasyon);
                        if (aktivasyonResponse.HasError)
                        {
                            _musteriLogger.MusteriLog(musteri.KullaniciId, "Müşteri Aktivasyon Kaydı Oluşturma", result.MusteriId + " idli müşteri aktivasyon kaydı oluşturma sıırasında hata oluştu. " + aktivasyonResponse.Message);
                            return new BaseResult<Musteri>
                            {
                                StatusCode = 400,
                                Result = result,
                                HasError = true,
                                Message = "Müşteri Aktivasyon kayıt sırasında bir hata oluştu."
                            };
                        }
                        else
                        {
                            _musteriLogger.MusteriLog(musteri.KullaniciId, "Müşteri Aktivasyon Kaydı Oluşturma", result.MusteriId + " idli müşteri aktivasyon kaydı başarıyla gerçekleşti.");

                            //bool sendSms = true;
                            //bool sendSms = SMSGonder(musteri.Telefon, code);

                            _musteriLogger.MusteriLog(musteri.KullaniciId, "SMS Gönderimi", result.MusteriId + " idli müşteriye " + code + " aktivasyon kodlu sms gönderildi.");

                            scope.Complete();
                            return new BaseResult<Musteri>
                            {
                                StatusCode = 200,
                                Result = result,
                                HasError = false,
                                Message = "OK"
                            };
                        }
                    }
                    else
                    {
                        _musteriLogger.MusteriLog(musteri.KullaniciId, "Müşteri Kayıt İşlemi", "CreateMusteri null döndü.");
                        return new BaseResult<Musteri>
                        {
                            StatusCode = 400,
                            Result = result,
                            HasError = true,
                            Message = "Müşteri kayıt işleminde bir hata oluştu."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _musteriLogger.MusteriLog(musteri.KullaniciId, "Müşteri Kayıt İşlemi", ex.Message);
                return new BaseResult<Musteri>
                {
                    StatusCode = 401,
                    Result = new Musteri(),
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [Authorize]
        [HttpPost("MusteriAktifEt")]
        public BaseResult<Musteri> MusteriAktifEt(MusteriAktivasyon musteri)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var result = _musteriService.GetMusteriAktivasyon(musteri.MusteriId);
                    if (result != null)
                    {
                        if (result.Kod == musteri.Kod)
                        {
                            var aktivasyonResult = _musteriService.AktivasyonKodKullan(result);

                            if (aktivasyonResult != null)
                            {
                                var musteriResult = _musteriService.MusteriAktifEt(musteri.MusteriId);
                                if (musteriResult != null)
                                {
                                    _musteriLogger.MusteriLog(musteriResult.KullaniciId, "Müşteri Aktivasyon", musteri.MusteriId + " idli müşteri aktivasyonu başarıyla gerçekleşti.");

                                    scope.Complete();
                                    return new BaseResult<Musteri>
                                    {
                                        StatusCode = 200,
                                        Result = musteriResult,
                                        HasError = false,
                                        Message = "Aktivasyon işlemi başarıyla gerçekleşti."
                                    };
                                }
                                else
                                {
                                    _musteriLogger.MusteriLog(musteriResult.KullaniciId, "Müşteri Aktivasyon", musteri.MusteriId + " idli müşteri aktivasyonu sırasında hata oluştu.");

                                    return new BaseResult<Musteri>
                                    {
                                        StatusCode = 400,
                                        Result = musteriResult,
                                        HasError = true,
                                        Message = "Müşteri Aktive Edilemedi."
                                    };
                                }
                            }
                            else
                            {
                                return new BaseResult<Musteri>
                                {
                                    StatusCode = 404,
                                    Result = new Musteri(),
                                    HasError = true,
                                    Message = "Müşteri Aktivasyon Kodu sonlanmış. Yeniden göndermek ister misiniz ?"
                                };
                            }
                        }
                        else
                        {
                            return new BaseResult<Musteri>
                            {
                                StatusCode = 400,
                                Result = new Musteri(),
                                HasError = true,
                                Message = "Aktivasyon Kodu Yanlış. Lütfen tekrar deneyin."
                            };
                        }
                    }
                    else
                    {
                        return new BaseResult<Musteri>
                        {
                            StatusCode = 400,
                            Result = new Musteri(),
                            HasError = true,
                            Message = "Müşteri Aktivasyon kaydı bulunamadı."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _musteriLogger.MusteriLog("", "Müşteri Aktivasyon", musteri.MusteriId + " idli müşteri için hata oluştu. " + ex.Message);
                return new BaseResult<Musteri>
                {
                    StatusCode = 401,
                    Result = new Musteri(),
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [Authorize]
        [HttpGet("GetMusterilerByKullaniciId/{kullaniciId}")]
        public BaseResult<List<Musteri>> GetMusterilerByKullaniciId(string kullaniciId)
        {
            try
            {
                var result = _musteriService.GetMusteriByKullaniciId(kullaniciId);

                return new BaseResult<List<Musteri>>
                {
                    StatusCode = 200,
                    Result = result,
                    HasError = false,
                    Message = "Aktif müşteriler getirildi."
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<List<Musteri>>
                {
                    StatusCode = 401,
                    Result = new List<Musteri>(),
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [Authorize]
        [HttpGet("GetMusterilerByKullaniciId/Pasif/{kullaniciId}")]
        public BaseResult<List<Musteri>> GetPasifMusterilerByKullaniciId(string kullaniciId)
        {
            try
            {
                var result = _musteriService.GetPasifMusterilerByKullaniciId(kullaniciId);

                return new BaseResult<List<Musteri>>
                {
                    StatusCode = 200,
                    Result = result,
                    HasError = false,
                    Message = "Pasif müşteriler getirildi."
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<List<Musteri>>
                {
                    StatusCode = 401,
                    Result = new List<Musteri>(),
                    HasError = true,
                    Message = ex.Message
                };
            }
        }

        [Authorize]
        [HttpPost("ReSendSMS/{musteriId}/{kullaniciId}")]
        public BaseResult<Musteri> ReSendSMS(int musteriId, string kullaniciId)
        {
            try
            {
                var musteri = _musteriService.GetReSendMusteri(musteriId, kullaniciId);

                if (musteri != null)
                {
                    Random generator = new Random();
                    int code = generator.Next(100000, 999999);

                    MusteriAktivasyon musteriAktivasyon = new MusteriAktivasyon
                    {
                        CreateDate = DateTime.Now,
                        FinishDate = DateTime.Now.AddDays(1),
                        Kod = code.ToString(),
                        IsUse = false,
                        MusteriId = musteri.MusteriId
                    };

                    var result = _musteriService.CreateMusteriAktivasyon(musteriAktivasyon);

                    if (result != null)
                    {
                        _musteriLogger.MusteriLog(kullaniciId, "ReSend SMS Gönderimi", musteri.MusteriId + " idli müşteriye " + code + " aktivasyon kodlu sms gönderildi.");
                        return new BaseResult<Musteri>
                        {
                            HasError = false,
                            Message = "SMS gönderildi.",
                            Result = musteri,
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        _musteriLogger.MusteriLog(kullaniciId, "ReSend Müşteri Aktivasyon Kaydı Oluşturma", musteri.MusteriId + " idli müşteri aktivasyon kaydı sırasında hata oluştu.");
                        return new BaseResult<Musteri>
                        {
                            HasError = true,
                            Message = "Aktivasyon kayıt işleminde bir hata oluştu.",
                            Result = musteri,
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new BaseResult<Musteri>
                    {
                        HasError = true,
                        Message = "SMS gönderilmek istenen müşteri bu kullanıcıya ait değil.",
                        Result = musteri,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                _musteriLogger.MusteriLog(kullaniciId, "ReSend Müşteri Aktivasyon Kaydı Oluşturma", ex.Message);
                return new BaseResult<Musteri>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new Musteri(),
                    StatusCode = 401
                };
            }
        }

        [Authorize]
        [HttpPost("UpdateKod/{kullaniciId}/{musteriId}")]
        public BaseResult<MusteriAktivasyon> UpdateKod(string kullaniciId, int musteriId)
        {
            try
            {
                var musteriAktivasyon = _musteriService.GetMusteriAktivasyon(musteriId);

                if (musteriAktivasyon != null)
                {
                    if (musteriAktivasyon.IsUse || musteriAktivasyon.FinishDate < DateTime.Now)
                    {
                        return new BaseResult<MusteriAktivasyon>
                        {
                            HasError = true,
                            Message = "Müşteri Aktivasyon Kodu Daha Önce Kullanılmış",
                            Result = new MusteriAktivasyon(),
                            StatusCode = 400
                        };
                    }

                    var musteri = _musteriService.GetReSendMusteri(musteriId, kullaniciId);
                    if (musteri == null)
                    {
                        return new BaseResult<MusteriAktivasyon>
                        {
                            HasError = true,
                            Message = "Müşteri Bulunamadı.",
                            Result = new MusteriAktivasyon(),
                            StatusCode = 400
                        };
                    }

                    Random generator = new Random();
                    int code = generator.Next(100000, 999999);

                    var result = _musteriService.UpdateKod(musteriAktivasyon, code.ToString());

                    if (result != null)
                    {
                        _musteriLogger.MusteriLog(kullaniciId, "Güncellenen Kod SMS Gönderimi", result.MusteriId + " idli müşteriye güncellenmiş sms kodu " + code + " başarıyla gödnerildiç");
                        return new BaseResult<MusteriAktivasyon>
                        {
                            HasError = false,
                            Message = "OK",
                            Result = result,
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        _musteriLogger.MusteriLog(kullaniciId, "Müşteri Aktivasyon Kodu Güncelleme", result.MusteriId + " idli müşterinin sms kodu güncellenmek isterken hata oluştu.");
                        return new BaseResult<MusteriAktivasyon>
                        {
                            HasError = true,
                            Message = "Aktivasyon kodu güncellenemedi.",
                            Result = new MusteriAktivasyon(),
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new BaseResult<MusteriAktivasyon>
                    {
                        HasError = true,
                        Message = "Müşteri aktivasyon kaydı bulunamadı.",
                        Result = new MusteriAktivasyon(),
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                _musteriLogger.MusteriLog(kullaniciId, "Müşteri Aktivasyon Kodu Güncelleme", "Müşterinin sms kodu güncellenmek isterken hata oluştu. " + ex.Message);
                return new BaseResult<MusteriAktivasyon>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new MusteriAktivasyon(),
                    StatusCode = 401
                };
            }
        }

        [Authorize]
        [HttpGet("TumMusteriler")]
        public BaseResult<List<Musteri>> TumMusteriler()
        {
            try
            {
                var result = _musteriService.TumMusteriler();

                if (result != null)
                {
                    return new BaseResult<List<Musteri>>
                    {
                        HasError = false,
                        Message = "OK",
                        Result = result,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new BaseResult<List<Musteri>>
                    {
                        HasError = true,
                        Message = "Müşteriler listesi getirilemedi",
                        Result = new List<Musteri>(),
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResult<List<Musteri>>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new List<Musteri>(),
                    StatusCode = 401
                };
            }
        }

        [Authorize]
        [HttpGet("TumMusterilerByPaging/{index}/{take}")]
        public BaseResult<List<Musteri>> TumMusterilerByPaging(int index, int take)
        {
            try
            {
                var result = _musteriService.TumMusterilerByPaging(index, take);

                if (result != null)
                {
                    return new BaseResult<List<Musteri>>
                    {
                        HasError = false,
                        Message = "OK",
                        Result = result,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new BaseResult<List<Musteri>>
                    {
                        HasError = true,
                        Message = "Müşteriler listesi getirilemedi",
                        Result = new List<Musteri>(),
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResult<List<Musteri>>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new List<Musteri>(),
                    StatusCode = 401
                };
            }
        }

        [Authorize]
        [HttpGet("MusterilerCount")]
        public BaseResult<MusterilerCount> MusterilerCount()
        {
            try
            {
                var result = _musteriService.MusterilerCount();

                return new BaseResult<MusterilerCount>
                {
                    HasError = false,
                    Message = "OK",
                    Result = result,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<MusterilerCount>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new MusterilerCount(),
                    StatusCode = 401
                };
            }
        }

        [Authorize]
        [HttpPost("MusteriGuncelle")]
        public BaseResult<Musteri> MusteriGuncelle(Musteri musteriReq)
        {
            try
            {
                var musteri = _musteriService.GetMusteriById(musteriReq.MusteriId);

                if (musteri == null)
                {
                    _musteriLogger.MusteriLog(musteriReq.KullaniciId, "Müşteri Güncelleme", "Güncellenmek isteyen müşteri bulunamadı.");

                    return new BaseResult<Musteri>
                    {
                        HasError = true,
                        Message = "Güncellenmek isteyen müşteri bulunamadı.",
                        Result = musteriReq,
                        StatusCode = 400
                    };
                }
                else
                {
                    var checkMusteri = _musteriService.CheckUpdatedMusteri(musteriReq.MusteriId, musteriReq.Telefon, musteriReq.Eposta);

                    if (checkMusteri != 0)
                    {
                        return new BaseResult<Musteri>
                        {
                            StatusCode = 400,
                            Result = new Musteri(),
                            HasError = true,
                            Message = "Bu eposta veya telefona ait bir müşteri zaten mevcut."
                        };
                    }

                    if (musteri.IsActive)
                    {
                        return new BaseResult<Musteri>
                        {
                            HasError = true,
                            Message = "Güncellenmek istenen müşteri aktif.",
                            Result = musteriReq,
                            StatusCode = 400
                        };
                    }
                    else
                    {
                        musteriReq.AktivasyonTarih = musteri.AktivasyonTarih;
                        musteri.IsActive = musteri.IsActive;
                        musteriReq.KayıtTarih = musteri.KayıtTarih;
                        musteriReq.KullaniciId = musteri.KullaniciId;
                        var result = _musteriService.MusteriGuncelle(musteriReq);

                        if (result == null)
                        {
                            _musteriLogger.MusteriLog(musteriReq.KullaniciId, "Müşteri Güncelleme", "Güncelleme esnasında bir hata oluştu.");

                            return new BaseResult<Musteri>
                            {
                                HasError = true,
                                Message = "Güncelleme esnasında bir hata oluştu.",
                                Result = musteriReq,
                                StatusCode = 400
                            };
                        }
                        else
                        {
                            _musteriLogger.MusteriLog(musteriReq.KullaniciId, "Müşteri Güncelleme", musteri.MusteriId + " idli müşteri güncelleme başarıyla gerçekleşti. Önceki müşteri bilgileri : " + musteri.ToString());
                            return new BaseResult<Musteri>
                            {
                                HasError = false,
                                Message = "Müşteri bilgileri başarıyla güncellendi.",
                                Result = result,
                                StatusCode = 200
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _musteriLogger.MusteriLog(musteriReq.KullaniciId, "Müşteri Güncelleme", ex.Message);

                return new BaseResult<Musteri>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = musteriReq,
                    StatusCode = 400
                };
            }
        }

        [Authorize]
        private BaseResult<MusteriAktivasyon> CreateMusteriAktivasyon(MusteriAktivasyon musteriAktivasyon)
        {
            try
            {
                if (musteriAktivasyon.MusteriId == 0)
                {
                    return new BaseResult<MusteriAktivasyon>
                    {
                        HasError = true,
                        Message = "Aktivasyon Kayıtta Müşteri Id boş olamaz.",
                        Result = musteriAktivasyon,
                        StatusCode = 400
                    };
                }
                if (String.IsNullOrEmpty(musteriAktivasyon.Kod))
                {
                    return new BaseResult<MusteriAktivasyon>
                    {
                        HasError = true,
                        Message = "Aktivasyon Kayıtta Aktivasyon Kodu boş olamaz.",
                        Result = musteriAktivasyon,
                        StatusCode = 400
                    };
                }

                var hasMusteriAktivasyon = _musteriService.GetMusteriAktivasyon(musteriAktivasyon.MusteriId);
                if (hasMusteriAktivasyon != null)
                {
                    if (hasMusteriAktivasyon.IsUse)
                    {
                        return new BaseResult<MusteriAktivasyon>
                        {
                            HasError = true,
                            Message = "Bu müşteri için daha önce aktivasyon işlemi gerçekleşmiştir.",
                            Result = new MusteriAktivasyon(),
                            StatusCode = 400
                        };
                    }
                    else
                    {
                        return new BaseResult<MusteriAktivasyon>
                        {
                            HasError = true,
                            Message = "Müşteriye ait kullanılmamış aktivasyon kodu zaten var.",
                            Result = new MusteriAktivasyon(),
                            StatusCode = 400
                        };
                    }
                }

                var result = _musteriService.CreateMusteriAktivasyon(musteriAktivasyon);

                if (result != null)
                {
                    return new BaseResult<MusteriAktivasyon>
                    {
                        HasError = false,
                        Message = "OK",
                        Result = result,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new BaseResult<MusteriAktivasyon>
                    {
                        HasError = true,
                        Message = "Aktivasyon kayıt işleminde bir hata oluştu.",
                        Result = result,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResult<MusteriAktivasyon>
                {
                    HasError = true,
                    Message = ex.Message,
                    Result = new MusteriAktivasyon(),
                    StatusCode = 401
                };
            }
        }




    }
}
