using Microsoft.Extensions.Options;

namespace ExpenseTrackerAPI.Data.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext,IMapper mapper, IConfiguration configuration, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _configuration = configuration;
            _jwt = jwt.Value;
        }
        public async Task<ApplicationUser> FindUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<ApplicationUser> FindUserByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
        public async Task<IdentityResult> CreateUser(RegistrationDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var res = await _userManager.CreateAsync(user);
            if (res.Succeeded)
            {
                return await _userManager.AddToRoleAsync(user, "User");
            }
            return res;
        }
        public async Task<IdentityResult> CreateAdmin(RegistrationDTO model)
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
        public async Task<JwtSecurityToken> CreateJwtToken(LogInDTO model)
        {
            var user = await FindUserByName(model.UserName);
            var UserClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();

            foreach (var role in roles) RoleClaims.Add(new Claim("role", role));

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }.Union(UserClaims).Union(RoleClaims);

            var Skey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var signingCredentials = new SigningCredentials(Skey, SecurityAlgorithms.HmacSha256);

            var JwtSecurityToken = new JwtSecurityToken
            (
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.UtcNow.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials,
                claims : Claims
            );

            return JwtSecurityToken;
        }
        public async Task<AuthModel> GetToken(LogInDTO model)
        {
            var user = await FindUserByName(model.UserName);
            var res = await _userManager.CheckPasswordAsync(user, model.Password);
            if (res)
            {
                var jwtSecurityToken = await CreateJwtToken(model);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return new AuthModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = tokenString,
                    IsAuthenticated = true
                };
            }
            return new AuthModel()
            {
                IsAuthenticated = false
            };
        }
    }
}