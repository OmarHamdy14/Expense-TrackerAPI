namespace ExpenseTrackerAPI.Data.Services.Implementation
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IEntityBaseRepository<ExpenseCategory> _base;
        private readonly IMapper _mapper;
        public ExpenseCategoryService(IAccountService accountService, IEntityBaseRepository<ExpenseCategory> ebase, IMapper mapper)
        {
            _base = ebase;
            _mapper = mapper;
        }
        public async Task<ExpenseCategory?> GetExpenseCategoryById(int ExpenseCategoryId)
        {
            var ExpenseCategory = await _base.Get(e => e.Id == ExpenseCategoryId);
            return ExpenseCategory;
        }
        public async Task<List<ExpenseCategory>?> GetAllExpensesByUserId(string userId)
        {
            var ExpenseCategorys = await _base.GetAll(e => e.UserId == userId);
            return ExpenseCategorys;
        }
        public async Task CreateExpepnse(CreateExpenseCategoryDTO model)
        {
            var ExpenseCategory = _mapper.Map<ExpenseCategory>(model);
            await _base.Create(ExpenseCategory);
        }
        public async Task UpdateExpepnse(int ExpenseCategoryId, UpdateExpenseCategoryDTO model)
        {
            var ExpenseCategory = await _base.Get(e => e.Id == ExpenseCategoryId);
            _mapper.Map(ExpenseCategory, model);
            await _base.Update(ExpenseCategory);
        }
        public async Task DeleteExpepnse(int ExpenseCategoryId)
        {
            var ExpenseCategory = await _base.Get(e => e.Id == ExpenseCategoryId);
            await _base.Remove(ExpenseCategory);
        }
    }
}