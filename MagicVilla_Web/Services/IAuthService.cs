﻿using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto objToCreate);

        Task<T> RegisterAsync<T>(RegistrationRequestDto registrationRequestDto);

    }
}
