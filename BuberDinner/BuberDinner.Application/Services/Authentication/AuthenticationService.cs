using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Services.Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
                                     IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public AuthenticationResult Login(string email, string password)
        {
            if (_userRepository.GetUsersByEmail(email) is not User user)
            {
                throw new Exception("User with given email already exists.");
            }

            if (user.Password != password)
            {
                throw new Exception("Invalid password.");
            }

            // Create JWT token
            Guid userId = Guid.NewGuid();
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token
                );
        }

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            if (_userRepository.GetUsersByEmail(email) is not null)
            {
                throw new Exception("User with given email already exists.");
            }
            
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            _userRepository.Add(user);

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token
                );
        }
    }
}