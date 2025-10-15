using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPC.API.DTOs;
using SPC.API.Helpers;
using SPC.DB;
using SPC.Models;
using System.Data.OleDb;

namespace SPC.API.Controllers
{

    /// Controller pour la gestion des clients

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Récupère tous les clients
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ClientDto>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all clients");

                var clients = new List<Client>();

                using (var conn = DBConnexion.GetConnexion())
                {
                    conn.Open();
                    string query = "SELECT * FROM Client";

                    using (var cmd = new OleDbCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                ClientName = reader["Client"] as string ?? ""
                            });
                        }
                    }
                }

                // ✅ Convertir en DTOs
                var dtos = clients.Select(c => c.ToDto());

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting clients");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des clients",
                    detail = ex.Message
                });
            }
        }

        /// Récupère un client spécifique par son nom
        
        [HttpGet("{clientName}")]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ClientDto> GetByName(string clientName)
        {
            try
            {
                _logger.LogInformation("Getting client by name: {ClientName}", clientName);

                Client client = null;

                using (var conn = DBConnexion.GetConnexion())
                {
                    conn.Open();
                    string query = "SELECT * FROM Client WHERE Client = ?";

                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientName", clientName);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                client = new Client
                                {
                                    ClientName = reader["Client"] as string ?? ""
                                };
                            }
                        }
                    }
                }

                if (client == null)
                {
                    return NotFound(new
                    {
                        message = $"Client '{clientName}' introuvable"
                    });
                }

                return Ok(client.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client by name {ClientName}", clientName);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération du client",
                    detail = ex.Message
                });
            }
        }

        /// Récupère uniquement les noms des clients
     
        [HttpGet("names")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<string>> GetNames()
        {
            try
            {
                _logger.LogInformation("Getting all client names");

                // Utilise la méthode existante
                var clientNames = ClientManager.GetClientsNames();

                return Ok(clientNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client names");
                return StatusCode(500, new
                {
                    message = "Erreur lors de la récupération des noms de clients",
                    detail = ex.Message
                });
            }
        }

        /// Crée un nouveau client
    
       
        [HttpPost]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ClientDto> Create([FromBody] ClientDto clientDto)
        {
            try
            {
                if (clientDto == null || string.IsNullOrWhiteSpace(clientDto.ClientName))
                {
                    return BadRequest(new
                    {
                        message = "Le nom du client est requis"
                    });
                }

                _logger.LogInformation("Creating new client: {ClientName}", clientDto.ClientName);

                // Vérifier si le client existe déjà
                using (var conn = DBConnexion.GetConnexion())
                {
                    conn.Open();

                    // Check existence
                    string checkQuery = "SELECT COUNT(*) FROM Client WHERE Client = ?";
                    using (var checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ClientName", clientDto.ClientName);
                        var count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            return Conflict(new
                            {
                                message = $"Le client '{clientDto.ClientName}' existe déjà"
                            });
                        }
                    }

                    // ✅ Utilise la méthode existante pour insérer
                    ClientManager.AddClient(clientDto.ClientName);
                }

                _logger.LogInformation("Client created successfully: {ClientName}", clientDto.ClientName);

                return CreatedAtAction(
                    nameof(GetByName),
                    new { clientName = clientDto.ClientName },
                    clientDto
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating client {ClientName}", clientDto?.ClientName);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la création du client",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Supprime un client
        /// </summary>
        /// <param name="clientName">Nom du client à supprimer</param>
        [HttpDelete("{clientName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(string clientName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(clientName))
                {
                    return BadRequest(new
                    {
                        message = "Le nom du client est requis"
                    });
                }

                _logger.LogInformation("Deleting client: {ClientName}", clientName);

                using (var conn = DBConnexion.GetConnexion())
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM Client WHERE Client = ?";
                    using (var cmd = new OleDbCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientName", clientName);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            return NotFound(new
                            {
                                message = $"Client '{clientName}' introuvable"
                            });
                        }
                    }
                }

                _logger.LogInformation("Client deleted successfully: {ClientName}", clientName);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting client {ClientName}", clientName);
                return StatusCode(500, new
                {
                    message = "Erreur lors de la suppression du client",
                    detail = ex.Message
                });
            }
        }
    }
}
