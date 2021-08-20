namespace KristaShop.API.Models.Responses {
    public class BadRequestResponse {
        public string Message { get; }

        public BadRequestResponse(string operationName) {
            Message = "Произошла ошибка при выполнении операции " + operationName;
        }
    }
}
