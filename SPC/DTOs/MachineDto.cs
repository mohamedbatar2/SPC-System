namespace SPC.DTOs
{
    /// <summary>
    /// DTO pour les machines
    /// </summary>
    public class MachineDto
    {
        public required string NMachine { get; set; }
        public required string Libelle { get; set; }
        public required string TypeSPC { get; set; }
    }
}
