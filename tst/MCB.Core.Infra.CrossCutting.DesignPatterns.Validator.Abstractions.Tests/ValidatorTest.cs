using FluentAssertions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Tests.ValidatorTests
{
    public class ValidatorTest
    {
        [Fact]
        public void Validator_Should_Validate()
        {
            // Arrange
            var customerValidator = new CustomerValidator();
            var invalidCustomer = new Customer()
            {
                Id = Guid.Empty,
                Name = string.Empty,
                BirthDate = default(DateTime),
                IsActive = false
            };
            var underAgeCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Customer A",
                BirthDate = DateTime.UtcNow.AddYears(-17),
                IsActive = true
            };
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = "Customer A",
                BirthDate = DateTime.UtcNow.AddYears(-19),
                IsActive = true
            };

            // Act
            var invalidCustomerValidationResult = customerValidator.Validate(invalidCustomer);
            var underAgeCustomerValidationResult = customerValidator.Validate(underAgeCustomer);
            var customerValidationResult = customerValidator.Validate(customer);

            // Assert
            invalidCustomerValidationResult.Should().NotBeNull();
            invalidCustomerValidationResult.HasError.Should().BeTrue();
            invalidCustomerValidationResult.IsValid.Should().BeFalse();
            invalidCustomerValidationResult.HasValidationMessage.Should().BeTrue();
            invalidCustomerValidationResult.ValidationMessageCollection.Should().HaveCount(4);

            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[0].ValidationMessageType.Should().Be(ValidationMessageType.Error);
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[0].Code.Should().Be("CustomerGuidIsRequired");
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[0].Description.Should().Be("Customer Id is Required");

            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[1].ValidationMessageType.Should().Be(ValidationMessageType.Error);
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[1].Code.Should().Be("CustomerNameIsRequired");
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[1].Description.Should().Be("Customer Name is Required");

            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[2].ValidationMessageType.Should().Be(ValidationMessageType.Error);
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[2].Code.Should().Be("CustomerBirthDateIsRequired");
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[2].Description.Should().Be("Customer BirthDate is Required");

            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[3].ValidationMessageType.Should().Be(ValidationMessageType.Warning);
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[3].Code.Should().Be("CustomerIsNotActive");
            invalidCustomerValidationResult.ValidationMessageCollection.ToArray()[3].Description.Should().Be("Customer is not active");

            underAgeCustomerValidationResult.Should().NotBeNull();
            underAgeCustomerValidationResult.HasError.Should().BeFalse();
            underAgeCustomerValidationResult.IsValid.Should().BeTrue();
            underAgeCustomerValidationResult.HasValidationMessage.Should().BeTrue();
            underAgeCustomerValidationResult.ValidationMessageCollection.Should().HaveCount(1);

            underAgeCustomerValidationResult.ValidationMessageCollection.ToArray()[0].ValidationMessageType.Should().Be(ValidationMessageType.Information);
            underAgeCustomerValidationResult.ValidationMessageCollection.ToArray()[0].Code.Should().Be("CustomerIsUnderAge");
            underAgeCustomerValidationResult.ValidationMessageCollection.ToArray()[0].Description.Should().Be("Customer is under age");

            customerValidationResult.Should().NotBeNull();
            customerValidationResult.HasError.Should().BeFalse();
            customerValidationResult.IsValid.Should().BeTrue();
            customerValidationResult.HasValidationMessage.Should().BeFalse();
            customerValidationResult.ValidationMessageCollection.Should().HaveCount(0);

        }
    }

    public class CustomerValidator
        : IValidator<Customer>
    {
        public ValidationResult Validate(Customer instance)
        {
            if (instance == null)
                return new ValidationResult();

            var validationMessageCollection = new List<ValidationMessage>();

            if (instance.Id == Guid.Empty)
                validationMessageCollection.Add(new ValidationMessage(ValidationMessageType.Error, code: "CustomerGuidIsRequired", description: "Customer Id is Required"));
            if(string.IsNullOrWhiteSpace(instance.Name))
                validationMessageCollection.Add(new ValidationMessage(ValidationMessageType.Error, code: "CustomerNameIsRequired", description: "Customer Name is Required"));
            if (instance.BirthDate == default)
                validationMessageCollection.Add(new ValidationMessage(ValidationMessageType.Error, code: "CustomerBirthDateIsRequired", description: "Customer BirthDate is Required"));
            else
            {
                /*
                 * This age calc is wrong because not see the month and day of the current year, but is only a test
                 */
                var age = DateTime.UtcNow.Year - instance.BirthDate.Year;
                if (age < 18)
                    validationMessageCollection.Add(new ValidationMessage(ValidationMessageType.Information, code: "CustomerIsUnderAge", description: "Customer is under age"));
            }
            if(!instance.IsActive)
                validationMessageCollection.Add(new ValidationMessage(ValidationMessageType.Warning, code: "CustomerIsNotActive", description: "Customer is not active"));

            if (validationMessageCollection.Count > 0)
                return new ValidationResult(validationMessageCollection);
            else
                return new ValidationResult();
        }

        public Task<ValidationResult> ValidateAsync(Customer instance, CancellationToken cancellationToken)
        {
            return Task.FromResult(Validate(instance));
        }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }

        public Customer()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            BirthDate = DateTime.Now;
        }
    }
}
