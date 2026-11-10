using System;
using CoffeeManagement.BUS.Exceptions;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DAL.Repositories;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.BUS.Services
{
    public sealed class AuthenticationService
    {
        private readonly UserRepository repository;
        private readonly PasswordHasher hasher;

        public AuthenticationService() : this(new UserRepository(), new PasswordHasher()) { }
        public AuthenticationService(UserRepository repository, PasswordHasher hasher)
        {
            this.repository = repository;
            this.hasher = hasher;
        }

        public UserSessionDto SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                throw new BusinessException("Enter both username and password.");
            UserCredential credential = repository.FindByUsername(userName.Trim());
            if (credential == null || !credential.IsActive || !hasher.Verify(password, credential.PasswordHash))
                throw new BusinessException("Username or password is incorrect.");
            repository.RecordLogin(credential.Session.UserId);
            return credential.Session;
        }

        public void CreateInitialAdministrator(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Trim().Length < 3 || userName.Trim().Length > 50)
                throw new BusinessException("Username must contain 3 to 50 characters.");
            repository.CreateFirstAdmin(userName.Trim(), hasher.Hash(password));
        }

        public bool IsInitialSetupRequired() { return repository.CountUsers() == 0; }

        public System.Data.DataTable GetAccounts() { return repository.GetAll(); }
        public System.Data.DataTable GetRoles() { return repository.GetRoles(); }
        public void CreateAccount(UserSessionDto actor, string userName, string password, string roleName, int employeeId)
        {
            if (actor == null || (actor.RoleName != "Admin" && actor.RoleName != "Manager")) throw new BusinessException("You do not have permission to manage accounts.");
            if (string.IsNullOrWhiteSpace(userName) || userName.Trim().Length < 3 || userName.Trim().Length > 50) throw new BusinessException("Username must contain 3 to 50 characters.");
            if (employeeId <= 0) throw new BusinessException("Select an employee.");
            if (actor.RoleName == "Manager" && string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase)) throw new BusinessException("Managers cannot create administrator accounts.");
            if (actor.UserId <= 0) throw new BusinessException("Sign in again before managing accounts.");
            repository.CreateAccount(userName.Trim(), hasher.Hash(password), roleName, employeeId, actor.UserId);
        }
        public void SetAccountActive(UserSessionDto actor, int userId, bool active, string targetRole)
        {
            if (actor == null || (actor.RoleName != "Admin" && actor.RoleName != "Manager")) throw new BusinessException("You do not have permission to manage accounts.");
            if (actor.UserId == userId) throw new BusinessException("You cannot change the status of your own account.");
            if (actor.RoleName == "Manager" && string.Equals(targetRole, "Admin", StringComparison.OrdinalIgnoreCase)) throw new BusinessException("Managers cannot change administrator accounts.");
            repository.SetActive(userId, active, actor.UserId);
        }
    }
}
