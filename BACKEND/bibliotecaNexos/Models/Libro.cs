using System;
using System.Collections.Generic;

#nullable disable

namespace bibliotecaNexos.Models
{
    public partial class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public int Año { get; set; }
        public int Paginas { get; set; }
        public long IdEditorial { get; set; }
        public long IdAutor { get; set; }

        public virtual Autor IdAutorNavigation { get; set; }
        public virtual Editorial IdEditorialNavigation { get; set; }
    }
}
