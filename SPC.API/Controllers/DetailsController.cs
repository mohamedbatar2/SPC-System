using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.API.Helpers;
using SPC.Models;
using System.Collections.ObjectModel;

namespace SPC.API.Controllers
{
    
    /// Controller pour la gestion des détails d'enregistrement SPC
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnregDetailsController : ControllerBase
    {
        private readonly ILogger<EnregDetailsController> _logger;

        public EnregDetailsController(ILogger<EnregDetailsController> logger)
        {
            _logger = logger;
        }

        
        /// Récupère tous les détails d'enregistrement
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SPCEnregDetailDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all enreg details");

                var details = await SPCEnregDetailManager.GetEnregDetailsAsync();

                // Convertir ObservableCollection en List puis en DTOs
                var dtos = details.Select(d => d.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting enreg details");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des détails d'enregistrement",
                    detail = ex.Message
                });
            }
        }

        
        /// Récupère un détail d'enregistrement spécifique par ID
     
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SPCEnregDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SPCEnregDetailDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting enreg detail by ID: {Id}", id);

                var details = await SPCEnregDetailManager.GetEnregDetailsAsync();
                var detail = details.FirstOrDefault(d => d.Id == id);

                if (detail == null)
                {
                    return NotFound(new
                    {
                        message = $"Détail d'enregistrement avec l'ID {id} introuvable"
                    });
                }

                return Ok(detail.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting enreg detail by ID {Id}", id);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération du détail d'enregistrement",
                    detail = ex.Message
                });
            }
        }

        /// Récupère tous les détails pour un enregistrement spécifique
        
        [HttpGet("byEnreg/{idEnrg:int}")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SPCEnregDetailDto>>> GetByEnregId(int idEnrg)
        {
            try
            {
                _logger.LogInformation("Getting enreg details for IdEnrg: {IdEnrg}", idEnrg);

                var allDetails = await SPCEnregDetailManager.GetEnregDetailsAsync();
                var filteredDetails = allDetails.Where(d => d.IdEnrg == idEnrg);

                var dtos = filteredDetails.Select(d => d.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting enreg details for IdEnrg {IdEnrg}", idEnrg);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des détails d'enregistrement",
                    detail = ex.Message
                });
            }
        }

        /// Crée un nouveau détail d'enregistrement
       
        [HttpPost]
        [ProducesResponseType(typeof(SPCEnregDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SPCEnregDetailDto>> Create([FromBody] SPCEnregDetailDto detailDto)
        {
            try
            {
                if (detailDto == null)
                {
                    return BadRequest(new
                    {
                        message = "Les données du détail d'enregistrement sont requises"
                    });
                }

                _logger.LogInformation("Creating new enreg detail for IdEnrg: {IdEnrg}", detailDto.IdEnrg);

                // Convertir DTO en modèle
                var detail = detailDto.ToModel();

                // Insérer dans la base de données
                await SPCEnregDetailManager.InsertNewAsync(detail);

                _logger.LogInformation("Enreg detail created successfully for IdEnrg: {IdEnrg}", detailDto.IdEnrg);

                // Retourner le DTO créé
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = detail.Id },
                    detailDto
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating enreg detail");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la création du détail d'enregistrement",
                    detail = ex.Message
                });
            }
        }

        
        /// Récupère les statistiques pour un enregistrement
     
        [HttpGet("stats/{idEnrg:int}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetStats(int idEnrg)
        {
            try
            {
                _logger.LogInformation("Getting stats for IdEnrg: {IdEnrg}", idEnrg);

                var allDetails = await SPCEnregDetailManager.GetEnregDetailsAsync();
                var details = allDetails.Where(d => d.IdEnrg == idEnrg).ToList();

                if (!details.Any())
                {
                    return Ok(new
                    {
                        idEnrg,
                        totalRecords = 0,
                        message = "Aucun détail trouvé"
                    });
                }

                var stats = new
                {
                    idEnrg,
                    totalRecords = details.Count,
                    totalQuantite = details.Sum(d => d.Quantite ?? 0),
                    averageHA1 = details.Where(d => d.HA1.HasValue).Average(d => d.HA1),
                    averageHI1 = details.Where(d => d.HI1.HasValue).Average(d => d.HI1),
                    averageTraction1 = details.Where(d => d.Traction1.HasValue).Average(d => d.Traction1),
                    distinctNatures = details.Select(d => d.Nature).Distinct().Count(),
                    distinctReperes = details.Select(d => d.Repere).Distinct().Count()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stats for IdEnrg {IdEnrg}", idEnrg);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des statistiques",
                    detail = ex.Message
                });
            }
        }
    }
}
