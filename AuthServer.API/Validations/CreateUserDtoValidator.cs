﻿using FluentValidation;
using Security.Core.Dtos;

namespace AuthServer.API.Validations
{
	public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
	{
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!").EmailAddress().WithMessage("Email is wrong!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required!");
        }
    }
}