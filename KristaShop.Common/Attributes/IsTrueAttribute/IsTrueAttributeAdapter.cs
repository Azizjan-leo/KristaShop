using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace KristaShop.Common.Attributes.IsTrueAttribute {
    public class IsTrueAttributeAdapter : AttributeAdapterBase<IsTrueAttribute> {
        public IsTrueAttributeAdapter(IsTrueAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer) {
        }
        public override void AddValidation(ClientModelValidationContext context) {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-istrue",
                GetErrorMessage(context));
        }
        public override string GetErrorMessage(
            ModelValidationContextBase validationContext) {
            return GetErrorMessage(validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
