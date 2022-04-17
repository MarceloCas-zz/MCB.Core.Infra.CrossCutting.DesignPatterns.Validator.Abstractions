using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;

namespace MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models
{
    public class ValidationMessage
    {
        // Properties
        public ValidationMessageType ValidationMessageType { get; }
        public string Code { get; }
        public string Description { get; }

        // Constructors
        public ValidationMessage(
            ValidationMessageType validationMessageType,
            string code,
            string description
        )
        {
            ValidationMessageType = validationMessageType;
            Code = code;
            Description = description;
        }
    }
}
