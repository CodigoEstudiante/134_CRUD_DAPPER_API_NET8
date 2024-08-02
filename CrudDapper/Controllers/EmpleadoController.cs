using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CrudDapper.Services;
using CrudDapper.Models;

namespace CrudDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoService _service;
        public EmpleadoController(EmpleadoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<List<Empleado>>> lista()
        {
            return Ok(await _service.Lista());
        }

        [HttpGet]
        [Route("Obtener/{id}")]
        public async Task<ActionResult<List<Empleado>>> obtener(int id)
        {
            var empleado = await _service.Obtener(id);

            if (empleado == null)
                return NotFound("No se encontró el empleado");
            else
                return Ok(empleado);

        }

        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult> crear(Empleado objeto)
        {
            var respuesta = await _service.Crear(objeto);

            if (respuesta != "")
                return BadRequest(respuesta);
            else
                return Ok("Empleado registrado");

        }

        [HttpPut]
        [Route("Editar")]
        public async Task<ActionResult> editar(Empleado objeto)
        {
            var respuesta = await _service.Editar(objeto);

            if (respuesta != "")
                return BadRequest(respuesta);
            else
                return Ok("Empleado modificado");

        }

        [HttpDelete]
        [Route("Eliminar")]
        public async Task<ActionResult> eliminar(int id)
        {
            await _service.Eliminar(id);
            return Ok();
        }

    }
}
