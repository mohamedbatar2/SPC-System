using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.API.Helpers;
using SPC.Models;
using SPC.Tools;

namespace SPC.API.Controllers
{
    /// <summary>
    /// Controller pour la gestion des outils
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OutilsController : ControllerBase
    {
        private readonly ILogger<OutilsController> _logger;

        public OutilsController(ILogger<OutilsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Récupère tous les outils
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OutilDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OutilDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all outils");

                var outils = await OutilManager.GetOutilsAsync();

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                var dtos = outils.Select(o => o.ToDto());

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting outils");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des outils",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère les outils par condition (numéro et connexion)
        /// </summary>
        /// <param name="noOutil">Numéro d'outil</param>
        /// <param name="connexion">Type de connexion</param>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<OutilDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OutilDto>>> GetByCondition(
            [FromQuery] string noOutil,
            [FromQuery] string connexion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(noOutil) && string.IsNullOrWhiteSpace(connexion))
                {
                    return BadRequest(new
                    {
                        message = "Au moins un paramètre (noOutil ou connexion) est requis"
                    });
                }

                _logger.LogInformation("Getting outils by condition: NoOutil={NoOutil}, Connexion={Connexion}",
                    noOutil, connexion);

                var outils = await OutilManager.GetOutilsByCndOAsync(noOutil, connexion);

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                var dtos = outils.Select(o => o.ToDto());

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting outils by condition");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la recherche des outils",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère un outil spécifique par son code
        /// </summary>
        /// <param name="code">Code de l'outil</param>
        [HttpGet("{code:int}")]
        [ProducesResponseType(typeof(OutilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OutilDto>> GetByCode(int code)
        {
            try
            {
                _logger.LogInformation("Getting outil by code: {Code}", code);

                var outils = await OutilManager.GetOutilsAsync();
                var outil = outils.FirstOrDefault(o => o.code == code);

                if (outil == null)
                {
                    return NotFound(new
                    {
                        message = $"Outil avec le code {code} introuvable"
                    });
                }

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                return Ok(outil.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting outil by code {Code}", code);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération de l'outil",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère un outil par son numéro
        /// </summary>
        /// <param name="noOutil">Numéro de l'outil</param>
        [HttpGet("byNumber/{noOutil}")]
        [ProducesResponseType(typeof(OutilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OutilDto>> GetByNumber(string noOutil)
        {
            try
            {
                _logger.LogInformation("Getting outil by number: {NoOutil}", noOutil);

                var outils = await OutilManager.GetOutilsAsync();
                var outil = outils.FirstOrDefault(o => o.NOutil == noOutil);

                if (outil == null)
                {
                    return NotFound(new
                    {
                        message = $"Outil {noOutil} introuvable"
                    });
                }

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                return Ok(outil.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting outil by number {NoOutil}", noOutil);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération de l'outil",
                    detail = ex.Message
                });
            }
        }
    }
}
