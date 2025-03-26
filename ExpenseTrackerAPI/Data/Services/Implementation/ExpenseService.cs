using AutoMapper;
using ExpenseTrackerAPI.Data.Base.Interfaces;
using ExpenseTrackerAPI.Data.Services.Interfaces;
using ExpenseTrackerAPI.Models;
using System;

namespace ExpenseTrackerAPI.Data.Services.Implementation
{
    public class ExpenseService : IExpenseService
    {
        private readonly IOtherServices _otherServices;
        private readonly IEntityBaseRepository<Expense> _base;
        private readonly IMapper _mapper;
        public ExpenseService(IAccountService accountService, IEntityBaseRepository<Expense> ebase, IMapper mapper, IOtherServices otherServices)
        {
            _base = ebase;
            _mapper = mapper;
            _otherServices = otherServices;
        }
        public async Task<Expense?> GetExpenseById(int expenseId)
        {
            var expense = await _base.Get(e => e.Id == expenseId);
            return expense;
        }
        public async Task<List<Expense>?> GetAllExpensesByUserId(string userId)
        {
            var expenses = await _base.GetAll(e => e.UserId == userId);
            return expenses;
        }
        public async Task<List<Expense>?> GetAllExpensesByCategoryIdAndUserId(int categoryId, string userId)
        {
            var expenses = await _base.GetAll(e => e.CategoryId == categoryId && e.UserId == userId);
            return expenses;
        }
        public async Task<List<Expense>?> GetAllExpensesPastWeekByUserId(string userId)
        {
            var endDate = _otherServices.GetLastFriday(DateTime.Today);
            var startDate = endDate.AddDays(-7);
            var expenses = await _base.GetAll(e => e.UserId == userId && e.Date>=startDate && e.Date<=endDate);
            return expenses;
        }
        public async Task<List<Expense>?> GetAllExpensesLastMonthByUserId(string userId)
        {
            var lastMonth = DateTime.Today.Month;
            var expenses = await _base.GetAll(e => e.UserId == userId && e.Date.Month == lastMonth-1);
            return expenses;
        }
        public async Task<List<Expense>?> GetAllExpensesLast3MonthsByUserId(string userId)
        {
            var lastMonth = DateTime.Today.Month;
            var expenses = await _base.GetAll(e => e.UserId == userId && (e.Date.Month == lastMonth - 1 || e.Date.Month == lastMonth - 2 || e.Date.Month == lastMonth - 3));
            return expenses;
        }
        public async Task<List<Expense>?> GetAllExpensesCustomPeriodByUserId(string userId, DateTime startDate,DateTime endDate)
        {
            var lastMonth = DateTime.Today.Month;
            var expenses = await _base.GetAll(e => e.UserId == userId && e.Date >= startDate && e.Date <= endDate);
            return expenses;
        }
        public async Task CreateExpepnse(CreateExpenseDTO model)
        {
            var expense = _mapper.Map<Expense>(model);
            await _base.Create(expense);
        }
        public async Task UpdateExpepnse(int expenseId, UpdateExpenseDTO model)
        {
            var expense = await _base.Get(e => e.Id == expenseId);
            _mapper.Map(expense, model);
            await _base.Update(expense);    
        }
        public async Task DeleteExpepnse(int expenseId)
        {
            var expense = await _base.Get(e => e.Id == expenseId);
            await _base.Remove(expense);
        }

    }
}
