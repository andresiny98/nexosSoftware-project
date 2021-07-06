using System;
using System.Collections.Generic;

#nullable disable

namespace bibliotecaNexos.Models
{
    public partial class Editorial
    {
        public Editorial()
        {
            Libroes = new HashSet<Libro>();
        }

        public long IdEditorial { get; set; }
        public string Nombre { get; set; }
        public string DireccionCorrespondencia { get; set; }
        public long Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public int MaximoLibros { get; set; }

        public virtual ICollection<Libro> Libroes { get; set; }
    }
}
