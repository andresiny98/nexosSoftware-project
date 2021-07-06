using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bibliotecaNexos.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace bibliotecaNexos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorialsController : ControllerBase
    {
        private readonly dbnexosContext _context;
        private Mensaje mensaje = new Mensaje();

        public EditorialsController(dbnexosContext context)
        {
            _context = context;
        }

        // GET: api/Editorials
        //Devuelve todas las editoriales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditorials()
        {
            return await _context.Editorials.ToListAsync();
        }

        // GET: api/Editorials/5
        // consultar editorial por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Editorial>> GetEditorial(long id)
        {
            var editorial = await _context.Editorials.FindAsync(id);

            if (editorial == null)
            {
                mensaje.status = 404;
                mensaje.Message = "No se encontro la editorial con el id "+ id +".";
                return NotFound(mensaje);
            }
            return editorial;
        }

        // PUT: api/Editorials/5
        // Editar editorial
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditorial(int id, Editorial editorial)
        {
            //Se valida que el id recibido por la url sea igual al recibido por el objeto editorial
            if (id != editorial.IdEditorial)
            {
                return BadRequest();
            }
          

             //se valida que la editorial exista.
                if (!EditorialExists(id))
                {
                    mensaje.status = 404;
                    mensaje.Message = "No existe la editorial ingresada en la base de datos.";
                    return NotFound(mensaje);
                }
                else        
                {
                try
                {
                    _context.Entry(editorial).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            
                }
            
            mensaje.Message = "La editorial se ha actualizado correctamente.";
            return Ok(mensaje);
        }

        // POST: api/Editorials
        // Guardar nueva editorial
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial)
        {
            _context.Editorials.Add(editorial);
            //Se valida que la editorial no este previamente registrada
            if (EditorialExists(editorial.IdEditorial))
            {
                //Se devuelve status code 409 que indica que genera conflicto el id porque ya existe
                mensaje.status = 409;
                mensaje.Message = "Ya existe la editorial en la base de datos.";
                return Conflict(mensaje);
            }
            //Si no existe el id en la bd procede a guardar la nueva editorial
            try
            {
      
                await _context.SaveChangesAsync();
                mensaje.Message = "La editorial se ha guardado correctamente.";
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(mensaje);

        }

 

        private bool EditorialExists(long id)
        {
            return _context.Editorials.Any(e => e.IdEditorial == id);
        }
    }
}
