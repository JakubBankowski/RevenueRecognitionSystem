namespace RevenueRecognitionSystem.Tests;

public class ContractDurationValidator
{
    public bool IsDurationValid(DateTime startDate, DateTime endDate)
    {
        int days = (endDate - startDate).Days;
        return days >= 3 && days <= 30;
    }
}

public class ContractDurationValidatorTests
{
    [Theory]
    [InlineData(3, true)] 
    [InlineData(15, true)]  
    [InlineData(30, true)]  
    [InlineData(2, false)]  
    [InlineData(31, false)] 
    public void IsDurationValid_VariousDayRanges_ReturnsExpectedResult(int daysCount, bool expectedResult)
    {
        var validator = new ContractDurationValidator();
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(daysCount);

        var result = validator.IsDurationValid(startDate, endDate);

        Assert.Equal(expectedResult, result);
    }
}