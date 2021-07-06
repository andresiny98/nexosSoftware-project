using bibliotecaNexos.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaNexos.Validators
{
    public class LibroValidation : AbstractValidator<Libro>
    {
        public LibroValidation()
        {
            RuleFor(l => l.IdLibro).NotNull().NotEqual(0).InclusiveBetween(1, 9999999);
            RuleFor(l => l.Titulo).NotEmpty().NotNull();
            RuleFor(l => l.Año).NotEmpty().NotNull().InclusiveBetween(1, 3000);
            RuleFor(l => l.Paginas).NotEmpty().NotNull().InclusiveBetween(1, 100000);
            RuleFor(l => l.IdAutor).NotEmpty().NotNull().NotEqual(0).InclusiveBetween(10000, 9999999999);
            RuleFor(l => l.IdEditorial).NotEmpty().NotNull().NotEqual(0).InclusiveBetween(10000, 9999999999);
         
        }
    }
}
