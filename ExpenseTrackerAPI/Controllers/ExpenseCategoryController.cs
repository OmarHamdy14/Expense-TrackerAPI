using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService, IAccountService accountService, IMapper mapper)
        {
            _expenseCategoryService = expenseCategoryService;
            _accountService = accountService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("GetCategoryById/{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            if (categoryId <= 0) return BadRequest(new { Message = "Invalid category Id." });
            var category = await _expenseCategoryService.GetExpenseCategoryById(categoryId);
            if (category == null) return NotFound(new { Message = "No Category found." });
            return Ok(category);
        }
        [Authorize]
        [HttpGet("GetAllCategoriesByUserId/{userId}")]
        public async Task<IActionResult> GetAllCategoriesByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });

            var categories = await _expenseCategoryService.GetAllExpenseCategorysByUserId(userId);
            if (!categories.Any()) return NotFound(new { Message = "No Categories found." });
            return Ok(categories);
        }
        [Authorize]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody]CreateExpenseCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _expenseCategoryService.CreateExpenseCategory(model); 
                    return CreatedAtAction("GetCategoryById", new { categoryId = category.Id }, category);
                }
                catch(Exception ex)
                {
                    return StatusCode(500, new { Message = "Something went wrong." });
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPut("UpdateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody]UpdateExpenseCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _expenseCategoryService.UpdateExpenseCategory(categoryId,model);
                    return CreatedAtAction("GetCategoryById", new { categoryId = category.Id }, category);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Something went wrong." });
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpDelete("DeleteExpenseCategory")]
        public async Task<IActionResult> DeleteExpenseCategory(int categoryId)
        {
            if (categoryId <= 0) return BadRequest(new { Message = "Invalid Id" });
            try
            {
                await _expenseCategoryService.DeleteExpenseCategory(categoryId);
                return Ok(new { Message = "Deletion is succeeded." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}