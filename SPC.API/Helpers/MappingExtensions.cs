using SPC.API.DTOs;
using SPC.Models;

namespace SPC.API.Helpers
{
    /// <summary>
    /// Extensions pour mapper Models ↔ DTOs
    /// </summary>
    public static class MappingExtensions
    {
        // ============================================================
        // OUTIL - MAPPING COMPLET
        // ============================================================

        /// <summary>
        /// Convertit un model Outil en OutilDto
        /// </summary>
        public static OutilDto ToDto(this Outil outil)
        {
            if (outil == null) return null;

            return new OutilDto
            {
                Code = outil.code,
                NOutil = outil.NOutil,
                Connexion = outil.Connexion,
                Sec = outil.Sec,
                Hame = outil.Hame,
                TolHa = outil.TolHa,
                Hisolant = outil.Hisolant,
                TolHi = outil.TolHi,
                Trac = outil.Trac,
                Denu = outil.Denu,
                Vld = outil.Vld,
                Lame = outil.Lame,
                Lisolant = outil.Lisolant,
                Produit = outil.Produit,
                DateFC = outil.DateFC,
                Cla = outil.Cla,
                Bat = outil.Bat,
                Empl = outil.Empl,
                Reg = outil.Reg,
                Ph = outil.Ph,
                Emplcnx = outil.Emplcnx,
                DateSi = outil.DateSi,
                Clip = outil.Clip,
                Ema = outil.Ema,
                TypeFil = outil.TypeFil,
                UAP = outil.UAP,
                Zone = outil.Zone
            };
        }

        /// <summary>
        /// Convertit un OutilDto en model Outil
        /// </summary>
        public static Outil ToModel(this OutilDto dto)
        {
            if (dto == null) return null;

            return new Outil
            {
                code = dto.Code,
                NOutil = dto.NOutil,
                Connexion = dto.Connexion,
                Sec = dto.Sec,
                Hame = dto.Hame,
                TolHa = dto.TolHa,
                Hisolant = dto.Hisolant,
                TolHi = dto.TolHi,
                Trac = dto.Trac,
                Denu = dto.Denu,
                Vld = dto.Vld,
                Lame = dto.Lame,
                Lisolant = dto.Lisolant,
                Produit = dto.Produit,
                DateFC = dto.DateFC,
                Cla = dto.Cla,
                Bat = dto.Bat,
                Empl = dto.Empl,
                Reg = dto.Reg,
                Ph = dto.Ph,
                Emplcnx = dto.Emplcnx,
                DateSi = dto.DateSi,
                Clip = dto.Clip,
                Ema = dto.Ema,
                TypeFil = dto.TypeFil,
                UAP = dto.UAP,
                Zone = dto.Zone
            };
        }

        // ============================================================
        // MACHINE
        // ============================================================

        public static MachineDto ToDto(this Machine machine)
        {
            if (machine == null) return null;

            return new MachineDto
            {
                NMachine = machine.NMachine,
                Libelle = machine.Libelle,
                TypeSPC = machine.TypeSPC
            };
        }

        // ============================================================
        // OPERATEUR
        // ============================================================

        public static OperateurDto ToDto(this Operateur operateur)
        {
            if (operateur == null) return null;

            return new OperateurDto
            {
                id = operateur.id,
                Name = operateur.Name,
                OperationNo = operateur.OperationNo
            };
        }

        // ============================================================
        // CLIENT
        // ============================================================

        public static ClientDto ToDto(this Client client)
        {
            if (client == null) return null;

            return new ClientDto
            {
                ClientName = client.ClientName
            };
        }

        // ============================================================
        // OTAPRVNTF
        // ============================================================

        public static OtaPrvntfDto ToDto(this OtaPrvntf prvntf)
        {
            if (prvntf == null) return null;

            return new OtaPrvntfDto
            {
                Cd = prvntf.cd,
                QtAct = prvntf.QtAct,
                PrvQt = prvntf.PrvQt,
                DtPrv = prvntf.DtPrv
            };
        }

        public static OtaPrvntf ToModel(this OtaPrvntfDto dto)
        {
            if (dto == null) return null;

            return new OtaPrvntf
            {
                cd = dto.Cd,
                QtAct = dto.QtAct,
                PrvQt = dto.PrvQt,
                DtPrv = dto.DtPrv
            };
        }
      

        public static SPCEnregCompletDto ToDto(this SPCEnregComplet complet, string status = "NF")
        {
            if (complet == null) return null;

            return new SPCEnregCompletDto
            {
                NoSerie = complet.NoSerie,
                NoMachine = complet.NoMachine,
                OperationNo = complet.OperationNo,
                Client = complet.Client,
                Ref = complet.Ref,
                Section = complet.Section,
                Connexion = complet.Connexion,
                Denudage = (decimal)complet.Denudage,
                NoOutil = complet.NoOutil,
                DateCreation = (DateTime)complet.DateCreation,
                Nature = complet.Nature,
                Quantite = (decimal)complet.Quantite,
                Repere = complet.Repere,
                NomOperateur = complet.Name,
                LibelleMachine = null,
                Status = status
            };
        }


    }

}
