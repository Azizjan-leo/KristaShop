namespace KristaShop.Common.Exceptions {
    public class EmptyListException : ExceptionBase {
        public EmptyListException() : base($"List should not be empty", $"Список не должен быть пустым") { }
    }
}