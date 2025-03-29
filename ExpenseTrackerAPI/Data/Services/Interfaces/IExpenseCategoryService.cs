namespace ExpenseTrackerAPI.Data.Services.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<ExpenseCategory?> GetExpenseCategoryById(int ExpenseCategoryId);
        Task<List<ExpenseCategory>?> GetAllExpenseCategorysByUserId(string userId);
        Task<ExpenseCategory> CreateExpenseCategory(CreateExpenseCategoryDTO model);
        Task<ExpenseCategory> UpdateExpenseCategory(int ExpenseCategoryId, UpdateExpenseCategoryDTO model);
        Task DeleteExpenseCategory(int ExpenseCategoryId);
    }
}
