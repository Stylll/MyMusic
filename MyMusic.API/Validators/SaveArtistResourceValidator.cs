using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MyMusic.API.Resources;

namespace MyMusic.API.Validators
{
    public class SaveArtistResourceValidator : AbstractValidator<SaveArtistResource>
    {
        public SaveArtistResourceValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}
