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
    public class LibrosController : ControllerBase
    {
        private readonly dbnexosContext _context;
        private Mensaje mensaje = new Mensaje();
        private Paginador<Libro> _Paginador;
        private readonly int _Limite = 12;

        public LibrosController(dbnexosContext context)
        {
            _context = context;
        }

        // GET: api/Libros
        //Devuelve todos los libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibroes()
        {
            var query = from l in _context.Libroes
                        join a in _context.Autors on l.IdAutor equals a.IdAutor
                        join e in _context.Editorials on l.IdEditorial equals e.IdEditorial
                        select new { a.Nombre, l.Titulo, YEAR=l.Año, l.Paginas, EDITORIAL = e.Nombre };

            var libros = await query.ToListAsync();
         
            if (libros == null)
            {
                return NotFound();
            }

            return Ok(libros);

        }

        // GET: api/Libros/5
        //Consultar libro por id   a este metodo le metemos para buscar por titulo, año etc !!Importante
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libroes.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        // GET: api/Libros/find/termino
        //Consultar por autor, o por titulo, o año, o editorial
        [HttpGet("find/{find}")]
        public async Task<ActionResult<Libro>> GetFind(string find)
        {
            var query = from l in _context.Libroes
                        join a in _context.Autors on l.IdAutor equals a.IdAutor
                        join e in _context.Editorials on l.IdEditorial equals e.IdEditorial
                        where a.Nombre.Contains(find) || l.Titulo.Contains(find) || l.Año.ToString().Contains(find) || e.Nombre.Contains(find)
                        select new { a.Nombre, l.Titulo, YEAR=l.Año, l.Paginas, EDITORIAL = e.Nombre };

            var libros = await query.ToListAsync();
          
            if (libros == null)
            {
                return NotFound();
            }

            return Ok(libros);
        }

        // PUT: api/Libros/5
        // Editar libro
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            int max = 0;
            int contador = 0;
            //Se valida que el id recibido por la url sea igual al recibido por el objeto libro
            if (id != libro.IdLibro)
            {
                return BadRequest();
            }

            //se valida que el libro exista.
            if (!LibroExists(id))
            {
                mensaje.status = 404;
                mensaje.Message = "No existe el libro ingresado en la base de datos.";
                return NotFound(mensaje);
            }

            //Se hace la validación para comprobar que el autor este registrado
            if (!AutorExists(libro.IdAutor))
            {
                mensaje.status = 404;
                mensaje.Message = "El autor no esta registrado.";
                return NotFound(mensaje);
            }
            //Se hace la validación para comprobar que la editorial este registrada
            else if (!EditorialExists(libro.IdEditorial))
            {
                mensaje.status = 404;
                mensaje.Message = "La editorial no esta registrada.";
                return NotFound(mensaje);
            }

            try
            {
                var datos = Parametrizador(libro);

                foreach (var data in datos)
                {
                    max = data.MAXIMO;
                    contador = data.CONTEO;
                }
                //Se valida si es -1 o si el valor del conteo de libros  menor al valor maximo de libros de la editorial
                if ((max > 0 && contador < max) || max == -1 || contador == max || contador == 0)
                {
                    _context.Entry(libro).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    mensaje.Message = "El libro se ha actualizado correctamente.";
                    return Ok(mensaje);
                }
                mensaje.status = 400;
                mensaje.Message = "No es posible actualizar el libro, se alcanzó el máximo permitido.";
                return BadRequest(mensaje);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // POST: api/Libros
        // Guardar libro 
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
            _context.Libroes.Add(libro);
            int max = 0;
            int contador = 0;

            //Se valida si el libro ya esta registrado en la bd
            if (LibroExists(libro.IdLibro))
            {
                mensaje.status = 409;
                mensaje.Message = "Ya existe el libro en la base de datos.";
                return Conflict(mensaje);
            }

            //Se hace la validación para comprobar que el autor este registrado
            if (!AutorExists(libro.IdAutor))
            {
                mensaje.status = 404;
                mensaje.Message = "El autor no esta registrado.";
                return NotFound(mensaje);
            }
            //Se hace la validación para comprobar que la editorial este registrada
            else if (!EditorialExists(libro.IdEditorial))
            {
                mensaje.status = 404;
                mensaje.Message = "La editorial no esta registrada.";
                return NotFound(mensaje);
            }

            //Se pregunta a la bd por el numero de libros asociados a la editorial que se le va a asignar al libro
            //Se le pregunta por el maximo de libros que tiene definido la editorial
            try
            {
                var datos =  Parametrizador(libro);
                int n = 0;

                foreach (var data in datos)
                {
                    n++;
                    max = data.MAXIMO;
                    contador = data.CONTEO;
                }

                //Se valida si es -1 o si el valor del conteo de libros en menor al valor maximo de libros de la editorial
                if ((max > 0 && contador < max) || max == -1 || n==0)
                {
             
                    await _context.SaveChangesAsync();
                    mensaje.Message = "El libro se ha guardado correctamente.";
                    return Ok(mensaje);
                }
                    mensaje.status = 409;
                    mensaje.Message = "No es posible registrar el libro, se alcanzó el máximo permitido.";
                    return Conflict(mensaje);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/Libros/5
        //Eliminar libro
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libroes.FindAsync(id);
            if (libro == null)
            {
                mensaje.status = 409;
                mensaje.Message = "No se encontro el libro.";
                return NotFound(mensaje);
            }

            _context.Libroes.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Este metodo es el encargado de contar cuantos libros se han registrado en base al limite de libros por editorial
        private dynamic Parametrizador(Libro libro)
        {
            var datos = from l in _context.Libroes
                        join e in _context.Editorials on l.IdEditorial equals e.IdEditorial
                        where l.IdEditorial == libro.IdEditorial
                        group e by new { e.MaximoLibros } into g
                        select new { CONTEO = g.Count(), MAXIMO = g.Key.MaximoLibros };
            return datos;
        }

        //Valida que el libro exista devuelve verdadero en caso de si exista
        private bool LibroExists(int id)
        {
            return _context.Libroes.Any(l => l.IdLibro == id);
        }

        //Valida que el autor exista devuelve verdadero en caso de si exista
        private bool AutorExists(long id)
        {
            return _context.Autors.Any(a => a.IdAutor == id);
        }

        //Valida que la editorial exista devuelve verdadero en caso de si exista
        private bool EditorialExists(long id)
        {
            return _context.Editorials.Any(e => e.IdEditorial == id);
        }


        
    }
}
