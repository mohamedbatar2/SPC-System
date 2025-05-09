using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC.Models
{
    public class EnregDetail
    {
        public int Id { get; set; }
        public int IdEnrg { get; set; }
        public decimal? Quantite { get; set; }
        public DateTime? DateCreation { get; set; }
        //public decimal? AH1 { get; set; }
        //public decimal? AH2 { get; set; }
        //public decimal? AH3 { get; set; }
        public decimal? ah1;
        public decimal? AH1
        {
            get
            {
                return ah1;
            }
            set
            {
                if (ah1 != value)
                {
                    ah1 = value;
                    ChangeColor?.Invoke(value, "AH1");
                }
            }
        }
        public decimal? ah2;
        public decimal? AH2
        {
            get
            {
                return ah2;
            }
            set
            {
                if (ah2 != value)
                {
                    ah2 = value;
                    ChangeColor?.Invoke(value, "AH2");
                }
            }
        }
        public decimal? ah3;
        public decimal? AH3
        {
            get
            {
                return ah3;
            }
            set
            {
                if (ah3 != value)
                {
                    ah3 = value;
                    ChangeColor?.Invoke(value, "AH3");
                }
            }
        }
        //public decimal? FH1 { get; set; }
        //public decimal? FH2 { get; set; }
        //public decimal? FH3 { get; set; }

        public decimal? fh1;
        public decimal? FH1
        {
            get
            {
                return fh1;
            }
            set
            {
                if (fh1 != value)
                {
                    fh1 = value;
                    ChangeColor?.Invoke(value, "FH1");
                }
            }
        }
        public decimal? fh2;
        public decimal? FH2
        {
            get
            {
                return fh2;
            }
            set
            {
                if (fh2 != value)
                {
                    fh2 = value;
                    ChangeColor?.Invoke(value, "FH2");
                }
            }
        }
        public decimal? fh3;
        public decimal? FH3
        {
            get
            {
                return fh3;
            }
            set
            {
                if (fh3 != value)
                {
                    fh3 = value;
                    ChangeColor?.Invoke(value, "FH3");
                }
            }
        }
        public decimal? traction1;
        public decimal? Traction1
        {
            get
            {
                return traction1;
            }
            set
            {
                if (traction1 != value)
                {
                    traction1 = value;
                    ChangeColor?.Invoke(value, "Traction1");
                }
            }
        }
        public decimal? traction2;
        public decimal? Traction2
        {
            get
            {
                return traction2;
            }
            set
            {
                if (traction2 != value)
                {
                    traction2 = value;
                    ChangeColor?.Invoke(value, "Traction2");
                }
            }
        }
        public decimal? traction3;
        public decimal? Traction3
        {
            get
            {
                return traction3;
            }
            set
            {
                if (traction3 != value)
                {
                    traction3 = value;
                    ChangeColor?.Invoke(value, "Traction3");
                }
            }
        }
        //public decimal? Traction1 { get; set; }
        //public decimal? Traction2 { get; set; }
        //public decimal? Traction3 { get; set; }
        public string Nature { get; set; }
        public decimal? LongueurM { get; set; }
        public decimal? LongueurM2 { get; set; }
        public string Repere { get; set; }
        //public string Ref {  get; set; }
        public string Claquage { get; set; }
        public string Marquage { get; set; }
        public string ContactAspect1 { get; set; }
        public string ContactAspect2 { get; set; }
        public string ContactAspect3 { get; set; }
        public decimal? Denudage1 { get; set; }
        public decimal? Denudage2 { get; set; }
        public decimal? Denudage3 { get; set; }
        public decimal? Tol1 { get; set; }
        public decimal? Tol2 { get; set; }
        public decimal? Tol3 { get; set; }
        public string Clip1 { get; set; }
        public string Clip2 { get; set; }
        public string Clip3 { get; set; }
        public string AspectCnx { get; set; }

        public Action<decimal? ,string> ChangeColor { get; set; }
    }
}
