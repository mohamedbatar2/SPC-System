using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.API.Helpers;  // ✅ AJOUTER pour ToDto()
using SPC.Models;
using SPC.Tools;

namespace SPC.API.Controllers
{
    /// <summary>
    /// Controller pour la gestion de la maintenance préventive des outils
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PreventifController : ControllerBase
    {
        private readonly ILogger<PreventifController> _logger;

        public PreventifController(ILogger<PreventifController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Récupère les informations de préventif d'un outil
        /// </summary>
        /// <param name="noOutil">Numéro de l'outil</param>
        /// <returns>Informations de maintenance préventive</returns>
        [HttpGet("{noOutil}")]
        [ProducesResponseType(typeof(OtaPrvntfDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OtaPrvntfDto>> GetById(string noOutil)
        {
            try
            {
                _logger.LogInformation("Getting preventif for outil: {NoOutil}", noOutil);

                var prvntf = await OtaPrvntfManager.GetPrvntfAsync(noOutil);

                if (prvntf == null)
                {
                    return NotFound(new
                    {
                        message = $"Préventif pour outil {noOutil} introuvable"
                    });
                }

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                return Ok(prvntf.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting preventif for {NoOutil}", noOutil);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération du préventif",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Met à jour le compteur de maintenance préventive
        /// </summary>
        /// <param name="request">Données de mise à jour (NoOutil et Quantite)</param>
        /// <returns>204 No Content si succès</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdatePrvntfRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.NoOutil))
                {
                    return BadRequest(new
                    {
                        message = "Le numéro d'outil est requis"
                    });
                }

                if (request.Quantite <= 0)
                {
                    return BadRequest(new
                    {
                        message = "La quantité doit être supérieure à 0"
                    });
                }

                _logger.LogInformation("Updating preventif for {NoOutil} with quantity {Quantite}",
                    request.NoOutil, request.Quantite);

                await OtaPrvntfManager.UpdatePrvntfAsync(request.NoOutil, request.Quantite);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating preventif");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la mise à jour du préventif",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère tous les préventifs nécessitant une maintenance
        /// </summary>
        /// <returns>Liste des outils nécessitant une maintenance</returns>
        [HttpGet("due")]
        [ProducesResponseType(typeof(IEnumerable<OtaPrvntfDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  ActionResult<IEnumerable<OtaPrvntfDto>> GetDueMaintenance()
        {
            try
            {
                _logger.LogInformation("Getting tools due for maintenance");

                // Note: Cette méthode devrait être ajoutée dans OtaPrvntfManager si nécessaire
                // Pour l'instant, on retourne un exemple

                return Ok(new List<OtaPrvntfDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting due maintenance");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des maintenances dues",
                    detail = ex.Message
                });
            }
        }
    }
}
