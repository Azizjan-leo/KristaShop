using System;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace KristaShop.Common.Attributes.RequiredThisOrOtherAttribute {
    public class RequiredThisOrOtherAttributeAdapter : AttributeAdapterBase<RequiredThisOrOtherAttribute> {
        private readonly RequiredThisOrOtherAttribute _attribute;

        public RequiredThisOrOtherAttributeAdapter(RequiredThisOrOtherAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer) {
            _attribute = attribute;
        }

        public override void AddValidation(ClientModelValidationContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-required-this", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-required-this-orother", _attribute.OtherProperty);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext) {
            var otherName = GetOtherPropertyDisplayName(validationContext, _attribute);
            return _attribute.FormatErrorMessage(validationContext.ModelMetadata.GetDisplayName(), otherName);
        }

        public static string GetOtherPropertyDisplayName(ModelValidationContextBase validationContext,
            RequiredThisOrOtherAttribute attribute) {
            var otherPropertyDisplayName = attribute.OtherPropertyDisplayName;
            if (otherPropertyDisplayName == null && validationContext.ModelMetadata.ContainerType != null) {
                var otherProperty = validationContext.MetadataProvider.GetMetadataForProperty(validationContext.ModelMetadata.ContainerType, attribute.OtherProperty);
                if (otherProperty != null) return otherProperty.GetDisplayName();
            }

            return attribute.OtherProperty;
        }
    }
}