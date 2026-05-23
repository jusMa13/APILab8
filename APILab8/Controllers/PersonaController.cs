using Contactos.Models;
using Microsoft.AspNetCore.Mvc;

namespace APILab8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : Controller
    {
        private readonly string _connectionString;

        public PersonaController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaContactos");

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var personas = await Contactos.Models.Persona.ObtenerTodas(_connectionString);
            return Ok(personas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Persona nuevaPersona)
        {
            var exito = await Persona.Insertar(_connectionString, nuevaPersona);
            if (exito)
            {
                return Ok(new {mensaje = "Persona insertada correctamente" });
            }
            return BadRequest(new {mensaje = "Error al insertar persona" });
        }

    }
}
