using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC.Models
{
    public class EnregComplet
    {
        public string NoSerie { get; set; }
        public string NoMachine { get; set; }
        public string RessourceNo { get; set; }
        public string OperationNo { get; set; }
        public string Client { get; set; }
        public string NoEquipement { get; set; }
        public string UAP { get; set; }
        public string Section { get; set; }
        public string NoContact { get; set; }
        public string NoContact2 { get; set; }
        public string NoOutil { get; set; }
        public string NoOutil2 { get; set; }
        public decimal? AH { get; set; }
        public decimal? FH { get; set; }
        public decimal? Traction { get; set; }
        public decimal? LongueurD { get; set; }
        public decimal? LongueurD2 { get; set; }


        public string Ref { get; set; }
        public decimal? Denudage { get; set; }
        public string Connexion { get; set; }

        //Details
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

    }
}
