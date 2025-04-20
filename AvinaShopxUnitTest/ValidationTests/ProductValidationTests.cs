using System.ComponentModel.DataAnnotations;
using AvinaShop.Data;
using FluentAssertions;
using Xunit;


public class ProductValidationTests
{
    [Fact]
    public void Product_Should_Have_Required_Name()
    {
        // Arrange
        var product = new Product
        {
            Name = string.Empty // Set Name to empty to trigger the validation error
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().Contain(r => r.MemberNames.Contains("Name") &&
                                                r.ErrorMessage.Contains("is required"));
    }

    [Fact]
    public void Product_Should_Have_Valid_Price_Range()
    {
        // Arrange
        var product = new Product
        {
            Price = 0.005m // Invalid price (below the minimum range)
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().Contain(r => r.MemberNames.Contains("Price") &&
                                                r.ErrorMessage.Contains("must be between"));
    }

    [Fact]
    public void Product_Should_Pass_Valid_Price()
    {
        // Arrange
        var product = new Product
        {
            Name = "Valid Product",  // Set Name to a valid, non-empty value
            Price = 500m // Valid price within the range
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().BeEmpty(); // No validation errors expected
    }


    // Helper method to validate model using DataAnnotations
    private static IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}