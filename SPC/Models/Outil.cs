using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;

namespace SPC.Models
{
    public class Outil
    {
        public int code { get; set; }
        public string NOutil { get; set; }
        public string Connexion { get; set; }
        public string Sec { get; set; }
        public decimal? Hame { get; set; }
        public decimal? TolHa { get; set; }
        public decimal? Hisolant { get; set; }
        public decimal? TolHi { get; set; }
        public int? Trac { get; set; }
        public decimal? Denu { get; set; }
        public int? Vld { get; set; }
        public decimal? Lame { get; set; }
        public decimal? Lisolant { get; set; }
        public string Produit { get; set; }
        public DateTime DateFC { get; set; }
        public decimal? Cla { get; set; }
        public decimal? Bat { get; set; }
        public string Empl { get; set; }
        public string Reg { get; set; }
        public string Ph { get; set; }
        public string Emplcnx { get; set; }
        public DateTime DateSi { get; set; }
        public string Clip { get; set; }
        public string Ema { get; set; }
        public string TypeFil { get; set; }
        public string UAP { get; set; }
        public string Zone { get; set; }
    }
}
