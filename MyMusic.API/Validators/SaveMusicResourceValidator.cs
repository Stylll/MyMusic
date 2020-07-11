using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MyMusic.API.Resources;

namespace MyMusic.API.Validators
{
    public class SaveMusicResourceValidator : AbstractValidator<SaveMusicResource>
    {
        public SaveMusicResourceValidator()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(50).WithMessage("Name cannot be more than 50 characters");
            RuleFor(m => m.ArtistId).NotEmpty().WithMessage("Artist Id must not be 0 or empty");
        }
    }
}
