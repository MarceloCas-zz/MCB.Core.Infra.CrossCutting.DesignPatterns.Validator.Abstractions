using FluentAssertions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Tests.ModelsTests
{
    public class ValidationResultTest
    {

        [Fact]
        public void ValidationResult_Should_Correctly_Initialized()
        {
            // Arrange
            var validationMessageCollection = new List<ValidationMessage>
            {
                new ValidationMessage(Enums.ValidationMessageType.Information, "1", "INFO"),
                new ValidationMessage(Enums.ValidationMessageType.Warning, "2", "WARNING"),
                new ValidationMessage(Enums.ValidationMessageType.Error, "3", "ERROR"),
            };

            // Act
            var validationResult = new ValidationResult(validationMessageCollection);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.HasValidationMessage.Should().BeTrue();
            validationResult.HasError.Should().BeTrue();
            validationResult.IsValid.Should().BeFalse();

            validationResult.ValidationMessageCollection.Should().NotBeNull();
            validationResult.ValidationMessageCollection.Should().NotBeSameAs(validationResult.ValidationMessageCollection);
            validationResult.ValidationMessageCollection.Should().HaveCount(3);
        }
    }
}
