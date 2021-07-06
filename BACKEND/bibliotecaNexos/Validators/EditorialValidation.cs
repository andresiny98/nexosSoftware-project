using bibliotecaNexos.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaNexos.Validators
{
    public class EditorialValidation : AbstractValidator<Editorial>
    {
        
        public EditorialValidation()
        {
            RuleFor(e => e.IdEditorial).NotNull().NotEqual(0).InclusiveBetween(10000, 9999999999);
            RuleFor(e => e.Nombre).NotEmpty().WithMessage("El nombre no es valido.");
            RuleFor(e => e.DireccionCorrespondencia).NotEmpty().WithMessage("La dirección no es valida.");
            RuleFor(e => e.Telefono).NotNull().NotEqual(0).WithMessage("El numero de telefono no es valido");
            RuleFor(e => e.CorreoElectronico).EmailAddress().NotNull().NotEmpty().WithMessage("El correo electronico no es valido.");
            RuleFor(e => e.MaximoLibros).NotNull().NotEqual(0).GreaterThan(-2).WithMessage("El valor ingresado no es valido para el Máximo de libros");

        }

        // Must be greater than 18 years old

    }
}
