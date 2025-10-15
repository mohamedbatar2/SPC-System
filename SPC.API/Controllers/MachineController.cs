using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.API.Helpers;
using SPC.DB;
using SPC.Models;
using System.Data.OleDb;

namespace SPC.API.Controllers
{
    /// <summary>
    /// Controller pour la gestion des machines
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    
    public class MachinesController : ControllerBase
    {
        private readonly ILogger<MachinesController> _logger;

        public MachinesController(ILogger<MachinesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Récupère toutes les machines
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MachineDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all machines");

                var machines = new List<Machine>();

                using (var conn = DBConnexion.GetConnexion())
                {
                    await conn.OpenAsync();
                    string query = "SELECT NMachine, Libelle, TypeSPC FROM machines";

                    using (var cmd = new OleDbCommand(query, conn))
                    using (var reader = (OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            machines.Add(new Machine
                            {
                                NMachine = reader["NMachine"] as string ?? "",
                                Libelle = reader["Libelle"] as string ?? "",
                                TypeSPC = reader["TypeSPC"] as string ?? ""
                            });
                        }
                    }
                }

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                var dtos = machines.Select(m => m.ToDto());

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machines");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des machines",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère une machine spécifique par son numéro
        /// </summary>
        /// <param name="nMachine">Numéro de la machine</param>
        [HttpGet("{nMachine}")]
        [ProducesResponseType(typeof(MachineDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MachineDto>> GetByNumber(string nMachine)
        {
            try
            {
                _logger.LogInformation("Getting machine by number: {NMachine}", nMachine);

                Machine machine = null;

                using (var conn = DBConnexion.GetConnexion())
                {
                    await conn.OpenAsync();
                    string query = "SELECT NMachine, Libelle, TypeSPC FROM machines WHERE NMachine = ?";

                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NMachine", nMachine);

                        using (var reader = (OleDbDataReader)await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                machine = new Machine
                                {
                                    NMachine = reader["NMachine"] as string ?? "",
                                    Libelle = reader["Libelle"] as string ?? "",
                                    TypeSPC = reader["TypeSPC"] as string ?? ""
                                };
                            }
                        }
                    }
                }

                if (machine == null)
                {
                    return NotFound(new
                    {
                        message = $"Machine '{nMachine}' introuvable"
                    });
                }

                // ✅ UTILISER LA MÉTHODE D'EXTENSION ToDto()
                return Ok(machine.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machine by number {NMachine}", nMachine);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération de la machine",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère le libellé d'une machine
        /// </summary>
        /// <param name="nMachine">Numéro de la machine</param>
        [HttpGet("{nMachine}/libelle")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetLibelle(string nMachine)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nMachine))
                {
                    return BadRequest(new
                    {
                        message = "Le numéro de machine est requis"
                    });
                }

                _logger.LogInformation("Getting libelle for machine: {NMachine}", nMachine);

                // ✅ Utilise la méthode ASYNC
                var libelle = await MachineManager.GetLibelleAsync(nMachine);

                if (string.IsNullOrEmpty(libelle))
                {
                    return NotFound(new
                    {
                        message = $"Libellé de la machine '{nMachine}' introuvable"
                    });
                }

                return Ok(new { libelle });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting libelle for machine {NMachine}", nMachine);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération du libellé",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Récupère le type SPC d'une machine
        /// </summary>
        /// <param name="nMachine">Numéro de la machine</param>
        [HttpGet("{nMachine}/typespc")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetTypeSPC(string nMachine)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nMachine))
                {
                    return BadRequest(new
                    {
                        message = "Le numéro de machine est requis"
                    });
                }

                _logger.LogInformation("Getting TypeSPC for machine: {NMachine}", nMachine);

                // ✅ Utilise la méthode ASYNC
                var typeSPC = await MachineManager.GetTypeSPCAsync(nMachine);

                if (string.IsNullOrEmpty(typeSPC))
                {
                    return NotFound(new
                    {
                        message = $"Type SPC de la machine '{nMachine}' introuvable"
                    });
                }

                return Ok(new { typeSPC });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting TypeSPC for machine {NMachine}", nMachine);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération du type SPC",
                    detail = ex.Message
                });
            }
        }
    }
}
