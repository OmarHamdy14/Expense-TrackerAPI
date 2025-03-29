using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public ExpenseController(IExpenseService expenseService, IAccountService accountService, IMapper mapper)
        {
            _expenseService = expenseService;
            _accountService = accountService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("GetExpenseById/{categoryId}")]
        public async Task<IActionResult> GetExpenseById(int expenseId)
        {
            if (expenseId <= 0) return BadRequest(new { Message = "Invalid expense Id." });
            var expense = await _expenseService.GetExpenseById(expenseId);
            if (expense == null) return NotFound(new { Message = "No Expense found." });
            return Ok(expense);
        }
        [Authorize]
        [HttpGet("GetAllExpensesByUserId/{userId}")]
        public async Task<IActionResult> GetAllExpensesByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });

            var expenses = await _expenseService.GetAllExpensesByUserId(userId);
            if (!expenses.Any()) return NotFound(new { Message = "No Expenses found." });
            return Ok(expenses);
        }
        [Authorize]
        [HttpGet("GetAllExpensesByCategoryIdAndUserId/{userId}/{categoryId}")]
        public async Task<IActionResult> GetAllExpensesByCategoryIdAndUserId(string userId, int categoryId)
        {
            if (categoryId <= 0) return BadRequest(new { Message = "Invalid category Id." });
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });

            var expenses = await _expenseService.GetAllExpensesByCategoryIdAndUserId(categoryId, userId);
            if (!expenses.Any()) return NotFound(new { Message = "No Expenses found." });
            return Ok(expenses);
        }
        [Authorize]
        [HttpGet("GetAllExpensesPastWeekByUserId/{userId}")]
        public async Task<IActionResult> GetAllExpensesPastWeekByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });

            var expenses = await _expenseService.GetAllExpensesPastWeekByUserId(userId);
            if (!expenses.Any()) return NotFound(new { Message = "No Expenses found." });
            return Ok(expenses);
        }
        [Authorize]
        [HttpGet("GetAllExpensesLastMonthByUserId/{userId}")]
        public async Task<IActionResult> GetAllExpensesLastMonthByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });

            var expenses = await _expenseService.GetAllExpensesLastMonthByUserId(userId);
            if (!expenses.Any()) return NotFound(new { Message = "No Expenses found." });
            return Ok(expenses);
        }
        [Authorize]
        [HttpGet("GetAllExpensesLast3MonthsByUserId/{userId}")]
        public async Task<IActionResult> GetAllExpensesLast3MonthsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });

            var expenses = await _expenseService.GetAllExpensesLast3MonthsByUserId(userId);
            if (!expenses.Any()) return NotFound(new { Message = "No Expenses found." });
            return Ok(expenses);
        }
        [Authorize]
        [HttpGet("GetAllExpensesCustomPeriodByUserId/{userId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetAllExpensesCustomPeriodByUserId(string userId, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Message = "Invalid User Id." });
            var user = await _accountService.FindUserById(userId);
            if (user == null) return NotFound(new { Message = "No User Found." });
            if (startDate == null || endDate == null || startDate > endDate) return BadRequest(new {Message = "Invalid StartDate or EndDate."}); 

            var expenses = await _expenseService.GetAllExpensesCustomPeriodByUserId(userId,startDate,endDate);
            if (!expenses.Any()) return NotFound(new { Message = "No Expenses found." });
            return Ok(expenses);
        }
        [Authorize]
        [HttpPost("CreateExpense")]
        public async Task<IActionResult> CreateExpense([FromBody]CreateExpenseDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var expense = await _expenseService.CreateExpepnse(model);
                    return CreatedAtAction("GetExpenseById", new { expenseId = expense.Id }, expense);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Something went wrong." });
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPut("UpdateExpense/{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, [FromBody]UpdateExpenseDTO model)
        {
            if (expenseId <= 0) return BadRequest(new { Message = "Invalid Id" });

            if (ModelState.IsValid)
            {
                try
                {
                    var expense = await _expenseService.UpdateExpepnse(expenseId,model);
                    return CreatedAtAction("GetExpenseById", new { expenseId = expense.Id }, expense);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Something went wrong." });
                }
            }
            return BadRequest(ModelState);
        }\
        [Authorize]
        [HttpDelete("DeleteExpense")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            if (expenseId <= 0) return BadRequest(new { Message = "Invalid Id" });
            try
            {
                await _expenseService.DeleteExpepnse(expenseId);
                return Ok(new { Message = "Deletion is succeeded." });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}