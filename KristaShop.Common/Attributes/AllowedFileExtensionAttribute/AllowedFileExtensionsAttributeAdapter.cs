using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace KristaShop.Common.Attributes.AllowedFileExtensionAttribute {
    public class AllowedFileExtensionsAttributeAdapter : AttributeAdapterBase<AllowedFileExtensionsAttribute> {
        private readonly AllowedFileExtensionsAttribute _attribute;

        public AllowedFileExtensionsAttributeAdapter(AllowedFileExtensionsAttribute attribute, IStringLocalizer stringLocalizer) 
            : base(attribute, stringLocalizer) {
            _attribute = attribute;
        }

        public override void AddValidation(ClientModelValidationContext context) {
            MergeAttribute(context.Attributes, "accept", _attribute.Extensions);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext) {
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}