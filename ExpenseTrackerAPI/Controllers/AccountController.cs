using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });

            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "There is no user found." });

            return Ok(_mapper.Map<UserDTO>(user));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _accountService.GetAllUsers();
            if (!users.Any()) return NotFound(new { Message = "No Users Found." });
            return Ok(_mapper.Map<List<UserDTO>>(users));
        }
        [Authorize]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody]RegistrationDTO model)
        {
            if (ModelState.IsValid)
            {
                var res = await _accountService.CreateUser(model);
                if (res.Succeeded)
                {
                    return Ok(new { Message = "Registration is succeeded.", User = _mapper.Map<UserDTO>(model) });
                }
                return BadRequest(res.Errors);
            }
            return BadRequest(ModelState);
        }
        [AllowAnonymous]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody]LogInDTO model)
        {
            if (ModelState.IsValid)
            {
                var authModel = await _accountService.GetToken(model);
                if (authModel.IsAuthenticated)
                {
                    return Ok(new {Message = "LogIn is succeeded." , authModel = authModel  });
                }
                return Unauthorized(new { Message = "UnCorrect UserName or Password." });
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPatch("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody]UpdateUserDTO model)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            if (ModelState.IsValid)
            {
                var res = await _accountService.UpdateUser(userId, model);
                if (res.Succeeded)
                {
                    var user = await _accountService.FindUserById(userId);
                    return Ok(new {Message = "Update is succeeded." , User = _mapper.Map<UserDTO>(user) });
                }
                return BadRequest(res.Errors);
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var res = await _accountService.DeleteUser(userId);
            if (res.Succeeded)
            {
                return Ok(new { Message = "Delete is succeeded." });
            }
            return BadRequest(res.Errors);
        }
    }
}