namespace Api.Dto
{
    public class UserAnalyticsDto
    {
        public string Username { get; set; }
        public string NickName { get; set; }
        public int TotalCostOfExpenses { get; set; }
        public int NumberOfExpenses { get; set; }
        public int NumberOfLocations { get; set; }
        public int NumberOfCategories { get; set; }
        public int NumberOfFrequents { get; set; }
    }
}