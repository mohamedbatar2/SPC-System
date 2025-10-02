using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.Tools;
using SPC.Models;
using SPC.Services;

namespace SPC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperateursController : ControllerBase
    {
        private readonly ILogger<OperateursController> _logger;

        public OperateursController(ILogger<OperateursController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Vérifie si un opérateur existe
        /// </summary>
        [HttpGet("{matricule}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckExists(string matricule)
        {
            try
            {
                _logger.LogInformation("Checking if operator exists: {Matricule}", matricule);

                var exists = await OperateurManager.CheckOpExistAsync(matricule);

                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking operator {Matricule}", matricule);
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        /// <summary>
        /// Récupère le nom d'un opérateur
        /// </summary>
        [HttpGet("{matricule}/name")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetName(string matricule)
        {
            try
            {
                _logger.LogInformation("Getting operator name: {Matricule}", matricule);

                var name = await OperateurManager.GetOpNameAsync(matricule);

                return Ok(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting operator name {Matricule}", matricule);
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }
    }
}
