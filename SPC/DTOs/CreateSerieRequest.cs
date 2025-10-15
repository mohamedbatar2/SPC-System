namespace SPC.DTOs
{
    /// <summary>
    /// Requête pour créer une série complète avec son détail
    /// </summary>
    public class CreateSerieRequest
    {
        public required SPCEnregDto Serie { get; set; }
        public required SPCEnregDetailDto Detail { get; set; }
    }
}
