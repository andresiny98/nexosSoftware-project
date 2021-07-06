using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bibliotecaNexos.Models;

namespace bibliotecaNexos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorsController : ControllerBase
    {
        private readonly dbnexosContext _context;
        private Mensaje mensaje = new Mensaje();

        public AutorsController(dbnexosContext context)
        {
            _context = context;
        }

        // GET: api/Autors
        //Devuelve todos los autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutors()
        {
            return await _context.Autors.ToListAsync();
        }

        // GET: api/Autors/5
        //consultar autor por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(long id)
        {
            var autor = await _context.Autors.FindAsync(id);

            if (autor == null)
            {
                mensaje.status = 404;
                mensaje.Message = "No se encontro el autor con el id " + id + ".";
                return NotFound(mensaje);
            }

            return autor;
        }

        // PUT: api/Autors/5
        // Editar autor
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(long id, Autor autor)
        {
            //Se valida que el id recibido por la url sea igual al recibido por el objeto autor
            if (id != autor.IdAutor)
            {
                return BadRequest();
            }
            //Se valida que el autor exista
            if (!AutorExists(id))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _context.Entry(autor).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }

            }
            mensaje.Message = "El autor se ha actualizado correctamente.";
            return Ok(mensaje);
        }

        // POST: api/Autors
        // Guardar nuevo autor
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            _context.Autors.Add(autor);
            if (AutorExists(autor.IdAutor))
            {
                //Se devuelve status code 409 que indica que genera conflicto el id porque ya existe
                mensaje.status = 409;
                mensaje.Message = "Ya existe el autor en la base de datos.";
                return Conflict(mensaje);
            }

            try
            {
                mensaje.Message = "El autor se ha guardado correctamente.";
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(mensaje);
        }

       

        private bool AutorExists(long id)
        {
            return _context.Autors.Any(e => e.IdAutor == id);
        }
    }
}
