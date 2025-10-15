using SPC.API.DTOs;
using SPC.Models;

namespace SPC.API.Helpers
{
    /// <summary>
    /// Extensions pour la conversion SPCEnregComplet -> SPCEnregCompletDto
    /// </summary>
    public static class SPCEnregCompletExtensions
    {
        /// <summary>
        /// Convertit un SPCEnregComplet en SPCEnregCompletDto
        /// </summary>
        public static SPCEnregCompletDto ToDto(this SPCEnregComplet enreg)
        {
            if (enreg == null)
                throw new ArgumentNullException(nameof(enreg));

            // Détermine le statut
            string status = "NF"; // Non Fini par défaut
            if (enreg.Nature != null && enreg.Nature.EndsWith("F"))
            {
                status = "F"; // Fini
            }

            return new SPCEnregCompletDto
            {
                // Informations série
                NoSerie = enreg.NoSerie ?? "",
                NoMachine = enreg.NoMachine ?? "",
                OperationNo = enreg.OperationNo ?? "",
                Client = enreg.Client,
                Ref = enreg.Ref,
                Section = enreg.Section,
                Connexion = enreg.Connexion,
                Denudage = enreg.Denudage ?? 0,
                NoOutil = enreg.NoOutil,
                DateCreation = enreg.DateCreation ?? DateTime.Now,

                // Informations détail
                Nature = enreg.Nature,
                Quantite = enreg.Quantite ?? 0,
                Repere = enreg.Repere,

                // Nom opérateur (calculé dans le modèle)
                NomOperateur = enreg.Name,

                // Libellé machine (peut être ajouté si besoin)
                LibelleMachine = null, // À calculer si nécessaire

                // Status
                Status = status
            };
        }

        /// <summary>
        /// Convertit un SPCEnregCompletDto en SPCEnregComplet
        /// </summary>
        public static SPCEnregComplet ToModel(this SPCEnregCompletDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new SPCEnregComplet
            {
                NoSerie = dto.NoSerie,
                NoMachine = dto.NoMachine,
                OperationNo = dto.OperationNo,
                Client = dto.Client,
                Ref = dto.Ref,
                Section = dto.Section,
                Connexion = dto.Connexion,
                Denudage = dto.Denudage,
                NoOutil = dto.NoOutil,
                DateCreation = dto.DateCreation,
                Nature = dto.Nature,
                Quantite = dto.Quantite,
                Repere = dto.Repere,
                Name = dto.NomOperateur
            };
        }
    }
}
    