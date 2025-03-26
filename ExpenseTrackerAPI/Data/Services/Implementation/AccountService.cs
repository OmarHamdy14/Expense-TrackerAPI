using AutoMapper;
using ExpenseTrackerAPI.Data.Services.Interfaces;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackerAPI.Data.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext,IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<ApplicationUser> FindUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
        public async Task<IdentityResult> CreateUser(RegistrationUserDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var res = await _userManager.CreateAsync(user);
            if (res.Succeeded)
            {
                return await _userManager.AddToRoleAsync(user, "User");
            }
            return res;
        }
        public async Task<IdentityResult> CreateAdmin(RegistrationUserDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var res = await _userManager.CreateAsync(user);
            if (res.Succeeded)
            {
                return await _userManager.AddToRoleAsync(user, "Admin");
            }
            return res;
        }
        public async Task<IdentityResult> UpdateUser(string userId, UpdateUserDTO model)
        {
            var user = await FindUserById(userId);
            _mapper.Map(user,model);
            return await _userManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await FindUserById(userId);
            return await _userManager.DeleteAsync(user);
        }
    }
}