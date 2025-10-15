using SPC.API.DTOs;
using SPC.Models;

namespace SPC.API.Helpers
{
    public static class SPCEnregDetailExtensions
    {
        public static SPCEnregDetailDto ToDto(this SPCEnregDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));

            return new SPCEnregDetailDto
            {
                // Identifiants
                Id = detail.Id,
                IdEnrg = detail.IdEnrg,

                // Informations générales
                Nature = detail.Nature ?? "",
                Quantite = (int?)detail.Quantite,
                Repere = detail.Repere ?? "",
                DateCreation = detail.DateCreation,

                // Longueurs
                LongueurM = detail.LongueurM,
                LongueurM2 = detail.LongueurM2,

                // Qualité
                Claquage = detail.Claquage,
                Marquage = detail.Marquage,

                // Mesures Extrémité A
                HA1 = detail.HA1,
                HA2 = detail.HA2,
                HA3 = detail.HA3,
                HI1 = detail.HI1,
                HI2 = detail.HI2,
                HI3 = detail.HI3,
                Traction1 = detail.Traction1,
                Traction2 = detail.Traction2,
                Traction3 = detail.Traction3,
                AspectCnx = detail.AspectCnx ?? "",
                ContactAspect1 = detail.ContactAspect1,
                ContactAspect2 = detail.ContactAspect2,
                ContactAspect3 = detail.ContactAspect3,

                // Dénudage
                Denudage1 = detail.Denudage1,
                Denudage2 = detail.Denudage2,
                Denudage3 = detail.Denudage3,

                // Tolérances
                Tol1 = detail.Tol1,
                Tol2 = detail.Tol2,
                Tol3 = detail.Tol3,

                // Clips
                Clip1 = detail.Clip1,
                Clip2 = detail.Clip2,
                Clip3 = detail.Clip3,

                // Mesures Extrémité B
                HA1B = detail.HA1B,
                HA2B = detail.HA2B,
                HA3B = detail.HA3B,
                HI1B = detail.HI1B,
                HI2B = detail.HI2B,
                HI3B = detail.HI3B,
                Traction1B = detail.Traction1B,
                Traction2B = detail.Traction2B,
                Traction3B = detail.Traction3B,
                AspectCnxB = detail.AspectCnxB,

                // Mesures Extrémité C
                HA1C = detail.HA1C,
                HA2C = detail.HA2C,
                HA3C = detail.HA3C,
                HI1C = detail.HI1C,
                HI2C = detail.HI2C,
                HI3C = detail.HI3C,
                Traction1C = detail.Traction1C,
                Traction2C = detail.Traction2C,
                Traction3C = detail.Traction3C,
                AspectCnxC = detail.AspectCnxC
            };
        }

        public static SPCEnregDetail ToModel(this SPCEnregDetailDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new SPCEnregDetail
            {
                Id = dto.Id,
                IdEnrg = dto.IdEnrg,
                Nature = dto.Nature,
                Quantite = dto.Quantite,
                Repere = dto.Repere,
                DateCreation = dto.DateCreation,
                LongueurM = dto.LongueurM,
                LongueurM2 = dto.LongueurM2,
                Claquage = dto.Claquage,
                Marquage = dto.Marquage,
                HA1 = dto.HA1,
                HA2 = dto.HA2,
                HA3 = dto.HA3,
                HI1 = dto.HI1,
                HI2 = dto.HI2,
                HI3 = dto.HI3,
                Traction1 = dto.Traction1,
                Traction2 = dto.Traction2,
                Traction3 = dto.Traction3,
                AspectCnx = dto.AspectCnx,
                ContactAspect1 = dto.ContactAspect1,
                ContactAspect2 = dto.ContactAspect2,
                ContactAspect3 = dto.ContactAspect3,
                Denudage1 = dto.Denudage1,
                Denudage2 = dto.Denudage2,
                Denudage3 = dto.Denudage3,
                Tol1 = dto.Tol1,
                Tol2 = dto.Tol2,
                Tol3 = dto.Tol3,
                Clip1 = dto.Clip1,
                Clip2 = dto.Clip2,
                Clip3 = dto.Clip3,
                HA1B = dto.HA1B,
                HA2B = dto.HA2B,
                HA3B = dto.HA3B,
                HI1B = dto.HI1B,
                HI2B = dto.HI2B,
                HI3B = dto.HI3B,
                Traction1B = dto.Traction1B,
                Traction2B = dto.Traction2B,
                Traction3B = dto.Traction3B,
                AspectCnxB = dto.AspectCnxB,
                HA1C = dto.HA1C,
                HA2C = dto.HA2C,
                HA3C = dto.HA3C,
                HI1C = dto.HI1C,
                HI2C = dto.HI2C,
                HI3C = dto.HI3C,
                Traction1C = dto.Traction1C,
                Traction2C = dto.Traction2C,
                Traction3C = dto.Traction3C,
                AspectCnxC = dto.AspectCnxC
            };
        }
    }
}
