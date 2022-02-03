using System;
using System.Collections.Generic;
using System.Text;

namespace MusteriKazan.Entities
{
    public class MusteriAktivasyon
    {
        public int ID { get; set; }
        public int MusteriId { get; set; }
        public string Kod { get; set; }
        public bool IsUse { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
