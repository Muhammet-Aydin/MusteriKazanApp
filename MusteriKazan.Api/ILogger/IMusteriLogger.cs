using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusteriKazan.Api.ILogger
{
    public interface IMusteriLogger
    {
        void MusteriLog(string kullaniciId, string islem, string islemSonuc);
    }
}
