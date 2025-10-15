using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.API.Helpers;
using SPC.Models;
using System.Collections.ObjectModel;

namespace SPC.API.Controllers
{
    /// <summary>
    /// Controller pour la gestion des enregistrements complets SPC
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnregistrementsController : ControllerBase
    {
        private readonly ILogger<EnregistrementsController> _logger;

        public EnregistrementsController(ILogger<EnregistrementsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Récupère tous les enregistrements non finis (NF)
        /// </summary>
        [HttpGet("notfinished")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SPCEnregCompletDto>> GetNotFinished()
        {
            try
            {
                _logger.LogInformation("Getting all not finished enregistrements");

                // ✅ Utilise la méthode existante avec stat="NF"
                var enregistrements = SPCEnregCompletManager.GetAll("NF");

                // Convertir ObservableCollection en DTOs
                var dtos = enregistrements.Select(e => e.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting not finished enregistrements");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des enregistrements non finis",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère les enregistrements finis par mois et année
        /// </summary>
        /// <param name="month">Mois (1-12)</param>
        /// <param name="year">Année (ex: 2025)</param>
        [HttpGet("finished")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SPCEnregCompletDto>> GetFinished(
            [FromQuery] string month,
            [FromQuery] string year)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(month) || string.IsNullOrWhiteSpace(year))
                {
                    return BadRequest(new
                    {
                        message = "Les paramètres 'month' et 'year' sont requis"
                    });
                }

                _logger.LogInformation("Getting finished enregistrements for {Month}/{Year}", month, year);

                // ✅ Utilise la méthode existante avec stat="F"
                var enregistrements = SPCEnregCompletManager.GetAll("F", month, year);

                var dtos = enregistrements.Select(e => e.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting finished enregistrements for {Month}/{Year}", month, year);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des enregistrements finis",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère le dernier statut de série par numéro d'opération
        /// </summary>
        /// <param name="operationNo">Numéro d'opération</param>
        [HttpGet("laststatus/{operationNo}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<object> GetLastStatusByOperation(string operationNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(operationNo))
                {
                    return BadRequest(new
                    {
                        message = "Le numéro d'opération est requis"
                    });
                }

                _logger.LogInformation("Getting last serie status for operation: {OperationNo}", operationNo);

                // ✅ Utilise la méthode existante
                var nature = SPCEnregCompletManager.LastSerieByOpStatus(operationNo);

                return Ok(new
                {
                    operationNo,
                    nature,
                    isFinished = nature != null && nature.EndsWith("F")
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting last status for operation {OperationNo}", operationNo);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération du statut",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Recherche des enregistrements par série
        /// </summary>
        /// <param name="noSerie">Numéro de série</param>
        [HttpGet("search/serie/{noSerie}")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SPCEnregCompletDto>> SearchBySerie(string noSerie)
        {
            try
            {
                _logger.LogInformation("Searching enregistrements by serie: {NoSerie}", noSerie);

                // Récupère tous les non finis et les filtre
                var enregistrements = SPCEnregCompletManager.GetAll("NF");
                var filtered = enregistrements
                    .Where(e => e.NoSerie != null && e.NoSerie.Contains(noSerie, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var dtos = filtered.Select(e => e.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching by serie {NoSerie}", noSerie);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la recherche par série",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Recherche des enregistrements par machine
        /// </summary>
        /// <param name="noMachine">Numéro de machine</param>
        [HttpGet("search/machine/{noMachine}")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SPCEnregCompletDto>> SearchByMachine(string noMachine)
        {
            try
            {
                _logger.LogInformation("Searching enregistrements by machine: {NoMachine}", noMachine);

                var enregistrements = SPCEnregCompletManager.GetAll("NF");
                var filtered = enregistrements
                    .Where(e => e.NoMachine != null && e.NoMachine.Equals(noMachine, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var dtos = filtered.Select(e => e.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching by machine {NoMachine}", noMachine);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la recherche par machine",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Recherche des enregistrements par opérateur
        /// </summary>
        /// <param name="operationNo">Numéro d'opération de l'opérateur</param>
        [HttpGet("search/operateur/{operationNo}")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SPCEnregCompletDto>> SearchByOperateur(string operationNo)
        {
            try
            {
                _logger.LogInformation("Searching enregistrements by operateur: {OperationNo}", operationNo);

                var enregistrements = SPCEnregCompletManager.GetAll("NF");
                var filtered = enregistrements
                    .Where(e => e.OperationNo != null && e.OperationNo.Equals(operationNo, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var dtos = filtered.Select(e => e.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching by operateur {OperationNo}", operationNo);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la recherche par opérateur",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Recherche des enregistrements par client
        /// </summary>
        /// <param name="client">Nom du client</param>
        [HttpGet("search/client/{client}")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SPCEnregCompletDto>> SearchByClient(string client)
        {
            try
            {
                _logger.LogInformation("Searching enregistrements by client: {Client}", client);

                var enregistrements = SPCEnregCompletManager.GetAll("NF");
                var filtered = enregistrements
                    .Where(e => e.Client != null && e.Client.Contains(client, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var dtos = filtered.Select(e => e.ToDto()).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching by client {Client}", client);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la recherche par client",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtient des statistiques pour une période donnée
        /// </summary>
        /// <param name="month">Mois (1-12)</param>
        /// <param name="year">Année (ex: 2025)</param>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<object> GetStatistics(
            [FromQuery] string month,
            [FromQuery] string year)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(month) || string.IsNullOrWhiteSpace(year))
                {
                    return BadRequest(new
                    {
                        message = "Les paramètres 'month' et 'year' sont requis"
                    });
                }

                _logger.LogInformation("Getting statistics for {Month}/{Year}", month, year);

                var enregistrements = SPCEnregCompletManager.GetAll("F", month, year);

                var stats = new
                {
                    period = $"{month}/{year}",
                    totalRecords = enregistrements.Count,
                    totalQuantite = enregistrements.Sum(e => e.Quantite ?? 0),
                    uniqueSeries = enregistrements.Select(e => e.NoSerie).Distinct().Count(),
                    uniqueMachines = enregistrements.Select(e => e.NoMachine).Distinct().Count(),
                    uniqueClients = enregistrements.Select(e => e.Client).Distinct().Count(),
                    uniqueOperateurs = enregistrements.Select(e => e.OperationNo).Distinct().Count(),

                    // Moyennes des mesures
                    averageHA1 = enregistrements.Where(e => e.HA1.HasValue).Any()
                        ? enregistrements.Where(e => e.HA1.HasValue).Average(e => e.HA1)
                        : null,
                    averageHI1 = enregistrements.Where(e => e.HI1.HasValue).Any()
                        ? enregistrements.Where(e => e.HI1.HasValue).Average(e => e.HI1)
                        : null,
                    averageTraction1 = enregistrements.Where(e => e.Traction1.HasValue).Any()
                        ? enregistrements.Where(e => e.Traction1.HasValue).Average(e => e.Traction1)
                        : null
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics for {Month}/{Year}", month, year);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des statistiques",
                    detail = ex.Message
                });
            }
        }
    }
}
