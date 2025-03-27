namespace ExpenseTrackerAPI.Data.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> FindUserById(string userId);
        Task<ApplicationUser> FindUserByName(string name);
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<IdentityResult> CreateUser(RegistrationDTO model);
        Task<IdentityResult> CreateAdmin(RegistrationDTO model);
        Task<IdentityResult> UpdateUser(string userId, UpdateUserDTO model);
        Task<IdentityResult> DeleteUser(string userId);
    }
}
