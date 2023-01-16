using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Persistence
{
    public interface IUserRepository
    {
        User? GetUsersByEmail(string email);
        void Add(User user);
    }
}