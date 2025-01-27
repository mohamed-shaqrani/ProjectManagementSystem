﻿using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration;

public record RegisterViewModel(string Email, string Password, string Username,string phone);

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Not empty");
    }
}