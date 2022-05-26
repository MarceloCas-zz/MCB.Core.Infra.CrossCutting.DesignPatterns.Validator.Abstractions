using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;

namespace MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models
{
    public struct ValidationResult
    {
        // Fields
        private readonly List<ValidationMessage> _validationMessageCollection;

        // Properties
        public IEnumerable<ValidationMessage> ValidationMessageCollection => _validationMessageCollection.AsReadOnly();
        public bool HasValidationMessage => _validationMessageCollection.Count > 0;
        public bool HasError => _validationMessageCollection.Any(q => q.ValidationMessageType == ValidationMessageType.Error);
        public bool IsValid => !HasValidationMessage || !HasError;

        // Constructors
        public ValidationResult(ICollection<ValidationMessage> validationMessageCollection)
        {
            _validationMessageCollection = validationMessageCollection.ToList();
        }
    }
}
