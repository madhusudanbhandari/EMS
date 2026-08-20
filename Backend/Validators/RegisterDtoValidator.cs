using FluentValidation;
using Backend.Dtos.Auth;
using System.Data;

namespace Backend.Validators;


public class RegisterDtoValidator:AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x=>x.Name)
        .NotEmpty()
        .WithMessage("Name is required")
        .MaximumLength(100)
        .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x=>x.Email)
        .NotEmpty()
        .WithMessage("Email cannot be empty")
        .EmailAddress()
        .WithMessage("Please provide a valid email address");

        RuleFor(x=>x.ConfirmPassword)
        .NotEmpty()
        .WithMessage("Please confirm your password")
        .Equal(x=>x.Password)
        .WithMessage("Passwords did not match");
    }
}