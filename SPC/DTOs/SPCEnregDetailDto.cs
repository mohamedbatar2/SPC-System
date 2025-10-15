using System;

namespace SPC.DTOs
{
    /// <summary>
    /// DTO complet pour les détails d'enregistrement SPC
    /// </summary>
    public class SPCEnregDetailDto
    {
        // ========== Identifiants ==========
        public int Id { get; set; }
        public int IdEnrg { get; set; }

        // ========== Informations générales ==========
        public required string Nature { get; set; }
        public int? Quantite { get; set; }
        public required string Repere { get; set; }
        public DateTime? DateCreation { get; set; }

        // ========== Longueurs ==========
        public decimal? LongueurM { get; set; }
        public decimal? LongueurM2 { get; set; }

        // ========== Qualité ==========
        public string Claquage { get; set; }
        public string Marquage { get; set; }

        // ========== Mesures Extrémité A (principale) ==========
        // Hauteur âme
        public decimal? HA1 { get; set; }
        public decimal? HA2 { get; set; }
        public decimal? HA3 { get; set; }

        // Hauteur isolant
        public decimal? HI1 { get; set; }
        public decimal? HI2 { get; set; }
        public decimal? HI3 { get; set; }

        // Traction
        public decimal? Traction1 { get; set; }
        public decimal? Traction2 { get; set; }
        public decimal? Traction3 { get; set; }

        // Aspect
        public required string AspectCnx { get; set; }
        public string ContactAspect1 { get; set; }
        public string ContactAspect2 { get; set; }
        public string ContactAspect3 { get; set; }

        // ========== Dénudage ==========
        public decimal? Denudage1 { get; set; }
        public decimal? Denudage2 { get; set; }
        public decimal? Denudage3 { get; set; }

        // ========== Tolérances ==========
        public decimal? Tol1 { get; set; }
        public decimal? Tol2 { get; set; }
        public decimal? Tol3 { get; set; }

        // ========== Clips ==========
        public string Clip1 { get; set; }
        public string Clip2 { get; set; }
        public string Clip3 { get; set; }

        // ========== Mesures Extrémité B ==========
        // Hauteur âme B
        public decimal? HA1B { get; set; }
        public decimal? HA2B { get; set; }
        public decimal? HA3B { get; set; }

        // Hauteur isolant B
        public decimal? HI1B { get; set; }
        public decimal? HI2B { get; set; }
        public decimal? HI3B { get; set; }

        // Traction B
        public decimal? Traction1B { get; set; }
        public decimal? Traction2B { get; set; }
        public decimal? Traction3B { get; set; }

        // Aspect B
        public string AspectCnxB { get; set; }

        // ========== Mesures Extrémité C ==========
        // Hauteur âme C
        public decimal? HA1C { get; set; }
        public decimal? HA2C { get; set; }
        public decimal? HA3C { get; set; }

        // Hauteur isolant C
        public decimal? HI1C { get; set; }
        public decimal? HI2C { get; set; }
        public decimal? HI3C { get; set; }

        // Traction C
        public decimal? Traction1C { get; set; }
        public decimal? Traction2C { get; set; }
        public decimal? Traction3C { get; set; }

        // Aspect C
        public string AspectCnxC { get; set; }
    }
}
