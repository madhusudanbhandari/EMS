using FluentValidation;
using Backend.Dtos.Auth;

namespace Backend.Validators;

public class LoginDtoValidator: AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x=>x.Email)
        .NotEmpty()
        .WithMessage("Email cannot be empty")
        .EmailAddress()
        .WithMessage("Please provide a valid email");

        RuleFor(x=>x.Password)
        .NotEmpty()
        .WithMessage("Password is required");
    }
}