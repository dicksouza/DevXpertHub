using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Web.Adapters;

// Classe para adaptar mensagens traduzidas
public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly ValidationAttributeAdapterProvider _baseProvider = new();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        if (attribute is RequiredAttribute requiredAttribute)
        {
            requiredAttribute.ErrorMessage = "O campo {0} é obrigatório.";
        }
        else if (attribute is StringLengthAttribute stringLengthAttribute)
        {
            stringLengthAttribute.ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.";
        }
        return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}