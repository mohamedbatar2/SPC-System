namespace SPC.API.DTOs
{
    /// <summary>
    /// DTO complet pour les outils
    /// </summary>
    public class OutilDto
    {
        // Identifiants
        public int Code { get; set; }
        public required string NOutil { get; set; }
        public required string Connexion { get; set; }

        // Caractéristiques techniques
        public required string Sec { get; set; }

        // Hauteur âme
        public decimal? Hame { get; set; }
        public decimal? TolHa { get; set; }

        // Hauteur isolant
        public decimal? Hisolant { get; set; }
        public decimal? TolHi { get; set; }

        // Traction
        public int? Trac { get; set; }

        // Dénudage
        public decimal? Denu { get; set; }

        // Validation
        public int? Vld { get; set; }

        // Longueurs
        public decimal? Lame { get; set; }
        public decimal? Lisolant { get; set; }

        // Informations produit
        public required string Produit { get; set; }
        public DateTime DateFC { get; set; }

        // Classification
        public decimal? Cla { get; set; }
        public decimal? Bat { get; set; }

        // Emplacement
        public required string Empl { get; set; }
        public required string Reg { get; set; }
        public required string Ph { get; set; }
        public required string Emplcnx { get; set; }

        // Date
        public DateTime DateSi { get; set; }

        // Autres
        public required string Clip { get; set; }
        public required string Ema { get; set; }
        public required string TypeFil { get; set; }
        public required string UAP { get; set; }
        public required  string Zone { get; set; }
    }
}
