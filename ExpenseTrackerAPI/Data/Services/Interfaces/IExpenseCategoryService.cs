namespace ExpenseTrackerAPI.Data.Services.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<ExpenseCategory?> GetExpenseCategoryById(int ExpenseCategoryId);
        Task<List<ExpenseCategory>?> GetAllExpenseCategorysByUserId(string userId);
        Task CreateExpenseCategory(CreateExpenseCategoryDTO model);
        Task UpdateExpenseCategory(int ExpenseCategoryId, UpdateExpenseCategoryDTO model);
        Task DeleteExpenseCategory(int ExpenseCategoryId);
    }
}
