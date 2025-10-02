namespace SPC.API.DTOs
{
    /// <summary>
    /// DTO pour la maintenance préventive des outils
    /// </summary>
    public class OtaPrvntfDto
    {
        public required int Cd { get; set; }
        public int QtAct { get; set; }
        public int PrvQt { get; set; }
        public DateTime DtPrv { get; set; }
    }

    /// <summary>
    /// Requête pour mettre à jour le préventif
    /// </summary>
    public class UpdatePrvntfRequest
    {
        public required string NoOutil { get; set; }
        public int Quantite { get; set; }
    }
}
