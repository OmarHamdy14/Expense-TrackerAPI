namespace ExpenseTrackerAPI.Data.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense?> GetExpenseById(int expenseId);
        Task<List<Expense>?> GetAllExpensesByUserId(string userId);
        Task<List<Expense>?> GetAllExpensesByCategoryIdAndUserId(int categoryId, string userId);
        Task<List<Expense>?> GetAllExpensesPastWeekByUserId(string userId);
        Task<List<Expense>?> GetAllExpensesLastMonthByUserId(string userId);
        Task<List<Expense>?> GetAllExpensesLast3MonthsByUserId(string userId);
        Task<List<Expense>?> GetAllExpensesCustomPeriodByUserId(string userId, DateTime startDate, DateTime endDate);
        Task<Expense> CreateExpepnse(CreateExpenseDTO model);
        Task<Expense> UpdateExpepnse(int expenseId, UpdateExpenseDTO model);
        Task DeleteExpepnse(int expenseId);
    }
}
