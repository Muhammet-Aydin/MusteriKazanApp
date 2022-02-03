using MusteriKazan.Api.ILogger;
using MusteriKazan.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusteriKazan.Api.Logger
{
    public class KullaniciLogger /*: IKullaniciLogger*/
    {
        public KullaniciLogger()
        {
            
        }
        public void KullaniciLog(string kullaniciId, string islem, string info)
        {
            using (var musteriKazanDbContext = new MusteriKazanDbContext())
            {
                //Entities.Log log = new Entities.Log
                //{
                //    Islem = islem,
                //    IslemSonuc = info,
                //    KayıtTarih = DateTime.Now,
                //    KullaniciId = kullaniciId
                //};

                //musteriKazanDbContext.Logs.Add(log);
                musteriKazanDbContext.SaveChanges();
            }
        }
    }
}
