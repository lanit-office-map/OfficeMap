using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Repositories.Filters;
using UserService.Repositories.Interfaces;
using UserServiceApi.Database;
using UserServiceApi.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Repositories
{
    /// <summary>
    /// Repository class for working with users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly UserServiceDBContext dbContext;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(
            [FromServices] UserServiceDBContext dbContext,
            [FromServices] ILogger<UserRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Delete <see cref="DbUser"/> with specified Id.
        /// </summary>
        public void Delete(DbUser entity)
        {
            var dbUser = Get(new UserFilter(entity.EmployeeId));

            if (dbUser == null)
            {
                return;
            }

            dbUser.Employee.Obsolete = true;
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Create <see cref="DbUser"/>.
        /// </summary>
        public void Create(DbUser entity)
        {
            logger.LogTrace("Creating user started.");

            dbContext.Employees.Add(entity.Employee);
            dbContext.SaveChanges();
            logger.LogTrace($"Employee saved in database with id '{entity.EmployeeId}'.");

            dbContext.Users.Add(entity);
            logger.LogTrace($"User saved in database with id '{entity.EmployeeId}'.");
        }

        /// <summary>
        /// Update <see cref="DbUser"/> with new data from the given entity.
        /// </summary>
        public DbUser Update(DbUser entity)
        {
            var dbUser = Get(new UserFilter(entity.EmployeeId));

            if (dbUser == null)
            {
                return entity;
            }

            bool userChanged = false;

            userChanged = UpdateProperty(
                dbUser.Employee.FirstName,
                entity.Employee.FirstName,
                () => dbUser.Employee.FirstName = entity.Employee.FirstName);

            userChanged = UpdateProperty(
                dbUser.Employee.SecondName,
                entity.Employee.SecondName,
                () => dbUser.Employee.SecondName = entity.Employee.SecondName);

            userChanged = UpdateProperty(
                dbUser.Employee.Mail,
                entity.Employee.Mail,
                () => dbUser.Employee.Mail = entity.Employee.Mail);

            userChanged = UpdateProperty(
                dbUser.Employee.Login,
                entity.Employee.Login,
                () => dbUser.Employee.Login = entity.Employee.Login);

            if (userChanged)
            {
                dbContext.SaveChanges();
            }

            entity.Employee.FirstName = dbUser.Employee.FirstName;
            entity.Employee.SecondName = dbUser.Employee.SecondName;
            entity.Employee.Mail = dbUser.Employee.Mail;
            entity.Employee.Login = dbUser.Employee.Login;

            return entity;
        }

        /// <summary>
        /// Retrieve all <see cref="DbUser"/> entities, satisfying given <see cref="UserFilter"/>.
        /// </summary>
        //public IEnumerable<DbUser> Find(UserFilter filter)

        /// <summary>
        /// Get <see cref="DbUser"/> from DB filtered by <see cref="UserFilter"/>.
        /// </summary>
        public DbUser Get(UserFilter id)
        {

        }

        private bool UpdateProperty(
            string dbUserProperty,
            string entityProperty,
            Action propertyUpdating)
        {
            if (!string.Equals(dbUserProperty, entityProperty)
                && !string.IsNullOrEmpty(entityProperty))
            {
                propertyUpdating.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
