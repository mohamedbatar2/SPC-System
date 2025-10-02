namespace SPC.API.DTOs
{
    public class SPCEnregDetailDto
    {
        public int Id { get; set; }
        public int IdEnrg { get; set; }
        public required string Nature { get; set; }
        public int? Quantite { get; set; }
        public required string Repere { get; set; }
        public decimal? HA1 { get; set; }
        public decimal? HA2 { get; set; }
        public decimal? HA3 { get; set; }
        public decimal? HI1 { get; set; }
        public decimal? HI2 { get; set; }
        public decimal? HI3 { get; set; }
        public decimal? Traction1 { get; set; }
        public decimal? Traction2 { get; set; }
        public decimal? Traction3 { get; set; }
        public required string AspectCnx { get; set; }
        public DateTime? DateCreation { get; set; }
    }

}
