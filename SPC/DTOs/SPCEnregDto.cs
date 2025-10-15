using System;
public class SPCEnregDto
{
    /// <summary>
    ///   Enregistrement d'une série complète
    /// </summary>
    public required string NoSerie { get; set; }
    public required string NoMachine { get; set; }
    public required string OperationNo { get; set; }
    public required string Client { get; set; }
    public required string Ref { get; set; }
    public required string Section { get; set; }
    public required string Connexion { get; set; }
    public decimal? Denudage { get; set; }
    public decimal? HA { get; set; }
    public decimal? HI { get; set; }
    public decimal? Traction { get; set; }
    public required string NoOutil { get; set; }
    public DateTime? DateCreation { get; set; }
}
