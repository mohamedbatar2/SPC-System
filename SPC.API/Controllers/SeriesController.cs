using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.Tools;
using SPC.Models;
using Microsoft.AspNetCore.Authorization;

namespace SPC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _logger;

        public SeriesController(ILogger<SeriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SPCEnregCompletDto>>> GetAll(
            [FromQuery] string status = "NF")
        {
            try
            {
                _logger.LogInformation("Getting all series with status: {Status}", status);

                var series = await SPCEnregCompletManager.GetAllAsync(status);

                var dtos = series.Select(s => new SPCEnregCompletDto
                {
                    NoSerie = s.NoSerie,
                    NoMachine = s.NoMachine,
                    OperationNo = s.OperationNo,
                    Client = s.Client,
                    Ref = s.Ref,
                    Section = s.Section,
                    Connexion = s.Connexion,
                    Denudage = (decimal)s.Denudage,
                    NoOutil = s.NoOutil,
                    DateCreation = (DateTime)s.DateCreation,  // UTILISER DateMaj au lieu de DateCreation
                    Nature = s.Nature,
                    Quantite = (decimal)s.Quantite,
                    Repere = s.Repere,
                    NomOperateur = s.Name,
                    LibelleMachine = null,  // SAFE NAVIGATION
                    Status = status
                });

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all series");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des séries",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("{noSerie}")]
        [ProducesResponseType(typeof(SPCEnregDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SPCEnregDto>> GetById(string noSerie)
        {
            try
            {
                var serie = await SPCEnregManager.GetSerieAsync(noSerie);

                if (serie == null)
                    return NotFound(new { message = $"Série {noSerie} introuvable" });

                var dto = new SPCEnregDto
                {
                    NoSerie = serie.NoSerie,
                    NoMachine = serie.NoMachine,
                    OperationNo = serie.OperationNo,
                    Client = serie.Client,
                    Ref = serie.Ref,
                    Section = serie.Section,
                    Connexion = serie.Connexion,
                    Denudage = serie.Denudage,
                    HA = serie.HA,
                    HI = serie.HI,
                    Traction = serie.Traction,
                    NoOutil = serie.NoOutil,
                    DateCreation = serie.DateCreation  // ✅ DateMaj du model
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting serie {NoSerie}", noSerie);
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        [HttpGet("last")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetLast()
        {
            try
            {
                var lastSerie = await SPCEnregManager.GetLastSerieAsync();
                return Ok(lastSerie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting last serie");
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        [HttpGet("last/{operatorId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetLastByOperator(string operatorId)
        {
            try
            {
                var lastSerie = await SPCEnregCompletManager.LastSerieByOpStatusAsync(operatorId);
                return Ok(lastSerie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting last serie for operator");
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        [HttpGet("{noSerie}/id")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetId(string noSerie)
        {
            try
            {
                var id = await SPCEnregManager.GetIdAsync(noSerie);
                if (id == 0)
                    return NotFound(new { message = $"Série {noSerie} introuvable" });

                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ID for serie");
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        [HttpGet("history")]
        [ProducesResponseType(typeof(IEnumerable<SPCEnregCompletDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SPCEnregCompletDto>>> GetHistory(
            [FromQuery] string status,
            [FromQuery] string month,
            [FromQuery] string year)
        {
            try
            {
                var series = await SPCEnregCompletManager.GetAllAsync(status, month, year);

                var dtos = series.Select(s => new SPCEnregCompletDto
                {
                    NoSerie = s.NoSerie,
                    NoMachine = s.NoMachine,
                    OperationNo = s.OperationNo,
                    Client = s.Client,
                    Ref = s.Ref,
                    DateCreation = (DateTime)s.DateCreation,
                    Status = status
                });

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting history");
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Create([FromBody] SPCEnregDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.NoSerie))
                    return BadRequest(new { message = "Le numéro de série est requis" });

                var serie = new SPCEnreg
                {
                    NoSerie = dto.NoSerie,
                    NoMachine = dto.NoMachine,
                    OperationNo = dto.OperationNo,
                    Client = dto.Client,
                    Ref = dto.Ref,
                    Section = dto.Section,
                    Connexion = dto.Connexion,
                    Denudage = dto.Denudage,
                    HA = dto.HA,
                    HI = dto.HI,
                    Traction = dto.Traction,
                    NoOutil = dto.NoOutil,
                    DateCreation = dto.DateCreation ?? DateTime.Now  // ✅ DateMaj avec valeur par défaut
                };

                await SPCEnregManager.InsertNewAsync(serie);
                var id = await SPCEnregManager.GetIdAsync(dto.NoSerie);

                return CreatedAtAction(nameof(GetById), new { noSerie = dto.NoSerie }, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating serie");
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }

        [HttpPost("complete")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> CreateComplete([FromBody] CreateSerieRequest request)
        {
            try
            {
                if (request?.Serie == null || request.Detail == null)
                    return BadRequest(new { message = "Données invalides" });

                // Créer la série
                var serie = new SPCEnreg
                {
                    NoSerie = request.Serie.NoSerie,
                    NoMachine = request.Serie.NoMachine,
                    OperationNo = request.Serie.OperationNo,
                    Client = request.Serie.Client,
                    Ref = request.Serie.Ref,
                    Section = request.Serie.Section,
                    Connexion = request.Serie.Connexion,
                    Denudage = request.Serie.Denudage,
                    NoOutil = request.Serie.NoOutil,
                    DateCreation = DateTime.Now
                };

                await SPCEnregManager.InsertNewAsync(serie);
                var idEnrg = await SPCEnregManager.GetIdAsync(request.Serie.NoSerie);

                // Créer le détail
                var detail = new SPCEnregDetail
                {
                    IdEnrg = idEnrg,
                    Nature = request.Detail.Nature,
                    Quantite = request.Detail.Quantite,
                    Repere = request.Detail.Repere,
                    DateCreation = DateTime.Now
                };

                await SPCEnregDetailManager.InsertNewAsync(detail);

                return CreatedAtAction(nameof(GetById), new { noSerie = request.Serie.NoSerie }, idEnrg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating complete serie");
                return StatusCode(500, new { message = "Erreur serveur", detail = ex.Message });
            }
        }
        
    }
}
