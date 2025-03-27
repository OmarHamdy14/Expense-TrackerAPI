namespace ExpenseTrackerAPI.Data.Services.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<ExpenseCategory?> GetExpenseCategoryById(int ExpenseCategoryId);
        Task<List<ExpenseCategory>?> GetAllExpensesByUserId(string userId);
        Task CreateExpepnse(CreateExpenseCategoryDTO model);
        Task UpdateExpepnse(int ExpenseCategoryId, UpdateExpenseCategoryDTO model);
        Task DeleteExpepnse(int ExpenseCategoryId);
    }
}
