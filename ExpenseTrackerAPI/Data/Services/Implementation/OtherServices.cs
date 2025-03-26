using ExpenseTrackerAPI.Data.Services.Interfaces;

namespace ExpenseTrackerAPI.Data.Services.Implementation
{
    public class OtherServices : IOtherServices
    {
        public DateTime GetLastFriday(DateTime date)
        {
            int daysSinceFriday = (int)date.DayOfWeek - (int)DayOfWeek.Friday; 
            if (daysSinceFriday < 0)
            {
                daysSinceFriday += 7; // Go back to the previous Friday
            }
            return date.AddDays(-daysSinceFriday);
        }
    }
}
