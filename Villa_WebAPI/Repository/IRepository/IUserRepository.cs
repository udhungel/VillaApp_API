﻿using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponsetDto> Login(LoginRequestDto loginRequestDto);

        Task<UserDto> Register(RegistrationRequestDto registrationRequestDto);

    }
}
