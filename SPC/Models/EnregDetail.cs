using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPC.Models
{
    public class EnregDetail 
    {
        public int Id { get; set; }
        public int IdEnrg { get; set; }
        public decimal? Quantite { get; set; }
        public DateTime? DateCreation { get; set; }
        public decimal? AH1 { get; set; }
        public decimal? AH2 { get; set; }
        public decimal? AH3 { get; set; }
        public decimal? FH1 { get; set; }
        public decimal? FH2 { get; set; }
        public decimal? FH3 { get; set; }
        public decimal? Traction1 { get; set; }
        public decimal? Traction2 { get; set; }
        public decimal? Traction3 { get; set; }
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
    }
}
