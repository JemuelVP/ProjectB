using Xunit;
using System.Text.RegularExpressions;

namespace ProjectB.Tests
{
    public class EmailValidationTests
    {
        [Theory]
        [InlineData("example@example.com", true)]
        [InlineData("user.name+tag+sorting@example.com", true)]
        [InlineData("user_name@example.co.uk", true)]
        [InlineData("user-name@sub.example.com", true)]
        [InlineData("user@com", false)]
        [InlineData("user@.com.my", false)]
        [InlineData("user.name@example.", false)]
        [InlineData("user@%*.com", false)]
        [InlineData("@example.com", false)]
        [InlineData("user@sub@domain.com", false)]
        public void EmailValidation_ShouldReturnExpectedResult(string email, bool expectedResult)
        {
            // Act
            bool actualResult = EmailValidation(email);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        public static bool EmailValidation(string email)
        {
            var validFormat =
                @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]+$";

            var regex = new Regex(validFormat);

            return regex.IsMatch(email);
        }
    }
}
