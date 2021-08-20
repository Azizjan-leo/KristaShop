using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Attributes.AllowedFileExtensionAttribute;
using KristaShop.Common.Attributes.IsTrueAttribute;
using KristaShop.Common.Attributes.RequiredThisOrOtherAttribute;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace KristaShop.Common.Attributes {
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider {
        readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer) {
            switch (attribute) {
                case AllowedFileExtensionsAttribute allowedFileExtensionsAttribute:
                    return new AllowedFileExtensionsAttributeAdapter(allowedFileExtensionsAttribute, stringLocalizer);
                case RequiredThisOrOtherAttribute.RequiredThisOrOtherAttribute requiredThisOrOtherAttribute:
                    return new RequiredThisOrOtherAttributeAdapter(requiredThisOrOtherAttribute, stringLocalizer);
                case IsTrueAttribute.IsTrueAttribute isTrueAttribute: 
                    return new IsTrueAttributeAdapter(isTrueAttribute, stringLocalizer);
                default:
                    return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}