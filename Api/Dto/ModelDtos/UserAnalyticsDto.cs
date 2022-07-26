namespace Api.Dto
{
    public class UserAnalyticsDto
    {
        public string Username { get; set; }
        public string NickName { get; set; }
        public decimal TotalCostOfExpenses { get; set; }
        public int NumberOfExpenses { get; set; }
        public int NumberOfBusinesses { get; set; }
        public int NumberOfCategories { get; set; }
        public int NumberOfFrequents { get; set; }
    }
}