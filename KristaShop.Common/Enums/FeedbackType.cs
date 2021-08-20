namespace KristaShop.Common.Enums {
    public enum FeedbackType {
        Basic = 0,
        ComplaintsAndSuggestions,
        ManagementContacts
    }

    public static class FeedbackTypeExtension {
        public static string ToReadableString(this FeedbackType type) {
            switch (type) {
                case FeedbackType.Basic:
                    return "Обратная связь";
                case FeedbackType.ComplaintsAndSuggestions:
                    return "Жалобы и предложения";
                case FeedbackType.ManagementContacts:
                    return "Связь с руководством";
                default:
                    return "---";
            }
        }
    }
}
