namespace SPC.API.DTOs
{
    /// <summary>
    /// DTO pour les séries complètes (vue complète)
    /// </summary>
    public class SPCEnregCompletDto
    {
        // Informations série
        public required string NoSerie { get; set; }
        public required string NoMachine { get; set; }
        public required string OperationNo { get; set; }
        public string? Client { get; set; }  
        public string? Ref { get; set; }    
        public string? Section { get; set; }  
        public string? Connexion { get; set; } 
        public decimal? Denudage { get; set; }
        public string? NoOutil { get; set; }   
        public DateTime? DateCreation { get; set; }  

        // Informations détail
        public string? Nature { get; set; }   
        public decimal? Quantite { get; set; }    
        public string? Repere { get; set; }   

        // Nom opérateur
        public string? NomOperateur { get; set; } 

        // Nom machine
        public string? LibelleMachine { get; set; } 

        // Status
        public string Status { get; set; } = "NF";  // NF = Non Fini, F = Fini
    }
}
