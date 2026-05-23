using Microsoft.AspNetCore.Mvc;
using Contactos.Models; // Tu biblioteca de clases externa
using System.Threading.Tasks;
using System.Collections.Generic;

namespace APILab8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly string _connectionString;

        public PersonaController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaContactos")!;
        }

        // GET: api/personas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var personas = await Persona.ObtenerTodas(_connectionString);
            return Ok(personas);
        }

        // POST: api/personas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Persona nuevaPersona)
        {
            var exito = await Persona.Insertar(_connectionString, nuevaPersona);
            if (exito)
            {
                return Ok(new { mensaje = "Persona registrada con éxito" });
            }
            return BadRequest(new { mensaje = "No se pudo registrar la persona" });
        }
    }
}