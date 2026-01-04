namespace Shared.Common.Domain.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated
/// </summary>
public class BusinessRuleValidationException : Exception
{
    public string RuleName { get; }

    public BusinessRuleValidationException(string ruleName, string message)
        : base(message)
    {
        RuleName = ruleName;
    }

    public BusinessRuleValidationException(string ruleName, string message, Exception innerException)
        : base(message, innerException)
    {
        RuleName = ruleName;
    }
}
