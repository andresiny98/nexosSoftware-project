using bibliotecaNexos.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaNexos.Validators
{
    public class AutorValidation : AbstractValidator<Autor>
    {
        public AutorValidation()
            {
            RuleFor(a => a.IdAutor).NotNull().NotEqual(0).InclusiveBetween(10000, 9999999999);
            RuleFor(a => a.Nombre).NotEmpty().WithMessage("El nombre no es valido.");
            RuleFor(a => a.FechaNacimiento).NotEmpty().NotNull();
            RuleFor(a => a.CiudadProcedencia).NotNull().NotEmpty();
            RuleFor(a => a.CorreoElectronico).EmailAddress().NotNull().NotEmpty().WithMessage("El correo electronico no es valido.");
        }

    }
}
