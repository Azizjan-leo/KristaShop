using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace KristaShop.Common.Models {
    public class OperationResult {
        private const string ErrorText = "Ошибка: процедура не выполнилась";
        private const string SuccessText = "Успешно: процедура выполнилась";

        public OperationResult(bool succeeded, string title, string type, string additionalInfo,
            IEnumerable<string> messages) {
            IsSuccess = succeeded;
            AlertTitle = title;
            AlertType = type;
            AdditionalInfo = additionalInfo;
            Messages = messages.ToArray();
        }

        public bool IsSuccess { get; set; }
        public string AlertType { get; set; }
        public string AlertTitle { get; set; }
        public string AdditionalInfo { get; set; }
        public string[] Messages { get; set; }

        public static OperationResult Success(List<string> messages = null, string additionalInfo = null) {
            if (messages == null)
                messages = new List<string> { SuccessText };

            return new OperationResult(true, "Успешно!", "success", additionalInfo, messages);
        }

        public static OperationResult Success(string message) {
            if (string.IsNullOrEmpty(message))
                message = SuccessText;
            return new OperationResult(true, "Успешно!", "success", string.Empty, new List<string> {message});
        }

        public static string SuccessAjaxRedirect(string redirectUrlValue) {
            return JsonConvert.SerializeObject(new { redirectUrl = redirectUrlValue });
        }

        public static OperationResult Failure(List<string> messages = null, string additionalInfo = null) {
            if (messages == null)
                messages = new List<string>();
            if(!messages.Any())
                messages.Add(ErrorText);
            return new OperationResult(false, "Ошибка!", "error", additionalInfo, messages);
        }

        public static OperationResult Failure(string message) {
            if (string.IsNullOrEmpty(message))
                message = ErrorText;
            return new OperationResult(false, "Ошибка!", "error", string.Empty, new List<string> {message});
        }

        public static string FailureAjax(List<string> messages = null, string additionalInfo = null) {
            if (messages == null)
                messages = new List<string>();
            if (!messages.Any())
                messages.Add(ErrorText);
            return FailureAjax(new OperationResult(false, "Ошибка!", "error", additionalInfo, messages));
        }

        public static string FailureAjax(string message) {
            if (string.IsNullOrEmpty(message))
                message = ErrorText;
            return FailureAjax(new OperationResult(false, "Ошибка!", "error", string.Empty, new List<string> { message }));
        }

        public static string FailureAjax(OperationResult or) {
            return System.Text.Json.JsonSerializer.Serialize(or, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public static string FailureAjaxRedirect(string redirectUrlValue) {
            return JsonConvert.SerializeObject(new { redirectUrl = redirectUrlValue });
        }

    }
}