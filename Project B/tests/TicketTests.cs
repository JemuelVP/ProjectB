using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;


using System;
using Xunit;

public class PeakTimeTests
{
    private readonly Ticket ticket; // Replace with your actual class name

    public PeakTimeTests()
    {
        ticket = new Ticket(); // Initialize your class here
    }

    [Theory]
    [InlineData(17, false)] // Before peak time
    [InlineData(18, true)]  // At the beginning of peak time
    [InlineData(20, true)]  // Within peak time
    [InlineData(22, true)]  // At the end of peak time
    [InlineData(23, false)] // After peak time
    public void IsPeakTime_ShouldReturnCorrectValue(int hour, bool expected)
    {
        // Arrange
        DateTime startTime = new DateTime(2024, 6, 14, hour, 0, 0); // Use a fixed date for consistency

        // Act
        bool result = ticket.IsPeakTime(startTime);

        // Assert
        Assert.Equal(expected, result);
    }

    // Tests for IsEarlyTime method
    [Theory]
    [InlineData(9, false)]  // Before early time
    [InlineData(10, true)]  // At the beginning of early time
    [InlineData(12, true)]  // Within early time
    [InlineData(14, true)]  // At the end of early time
    [InlineData(15, false)] // After early time
    public void IsEarlyTime_ShouldReturnCorrectValue(int hour, bool expected)
    {
        // Arrange
        DateTime startTime = new DateTime(2024, 6, 14, hour, 0, 0); // Use a fixed date for consistency

        // Act
        bool result = ticket.IsEarlyTime(startTime);

        // Assert
        Assert.Equal(expected, result);
    }
}

