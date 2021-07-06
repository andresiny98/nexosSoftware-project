using System;
using System.Collections.Generic;

#nullable disable

namespace bibliotecaNexos.Models
{
    public partial class Autor
    {
        public Autor()
        {
            Libroes = new HashSet<Libro>();
        }

        public long IdAutor { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string CiudadProcedencia { get; set; }
        public string CorreoElectronico { get; set; }

        public virtual ICollection<Libro> Libroes { get; set; }
    }
}
