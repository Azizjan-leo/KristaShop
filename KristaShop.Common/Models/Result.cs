using System;
using System.Collections.Generic;

namespace KristaShop.Common.Models {
    public enum ResultType {
        Success,
        Error
    }

    public interface IResult {
        ResultType Type { get; set; } 
        string ReadableMessage { get; set; }
        string ErrorMessage { get; set; }
        Exception Exception { get; set; }
        bool IsSuccess { get; }
    }

    public interface IResult<TModel> : IResult {
        TModel Model { get; set; }
    }

    public partial class Result : IResult {
        public ResultType Type { get; set; }
        public string ReadableMessage { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccess => Type == ResultType.Success;

        public Result(ResultType type, string errorMessage, string readableMessage = "", Exception exception = null) {
            Type = type;
            ErrorMessage = errorMessage;
            ReadableMessage = readableMessage;
            Exception = exception;
        }

        public Result(ResultType type, string errorMessage, Exception exception = null) {
            Type = type;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public override string ToString() {
            return $"{nameof(ErrorMessage)}: {ErrorMessage};\r\n {nameof(ReadableMessage)}: {ReadableMessage}";
        }
    }

    public class Result<TModel> : Result, IResult<TModel> {
        public TModel Model { get; set; }

        public Result(TModel model, ResultType type, string errorMessage, string readableMessage = "", Exception exception = null)
            : base(type, errorMessage, readableMessage, exception) {
            Model = model;
        }

        public Result(TModel model, ResultType type, string errorMessage, Exception exception = null)
            : base(type, errorMessage, exception) {
            Model = model;
        }
    }

    public partial class Result {
        public static IResult Success(string readableMessage = "") {
            return new Result(ResultType.Success, string.Empty, readableMessage);
        }

        public static IResult Error(string errorMessage, string readableMessage = "", Exception exception = null) {
            return new Result(ResultType.Error, errorMessage, readableMessage, exception);
        }

        public static IResult Error(string errorMessage, Exception exception = null) {
            return new Result(ResultType.Error, errorMessage, exception);
        }

        public static IResult<TModel> SuccessModel<TModel>(TModel model, string readableMessage = "") {
            return new Result<TModel>(model, ResultType.Success, string.Empty, readableMessage);
        }

        public static IResult<TModel> ErrorModel<TModel>(TModel model, string errorMessage, string readableMessage = "", Exception exception = null) where TModel : class {
            return new Result<TModel>(model, ResultType.Error, errorMessage, readableMessage, exception);
        }

        public static IResult<TModel> ErrorModel<TModel>(TModel model, string errorMessage, Exception exception = null) where TModel : class {
            return new Result<TModel>(model, ResultType.Error, errorMessage, exception);
        }
    }

    public static class ResultExtension {
        public static OperationResult ToOperationResult(this IResult result) {
            if (result.IsSuccess) {
                return OperationResult.Success(result.ReadableMessage);
            }

            return OperationResult.Failure(result.ReadableMessage);
        }
    }
}
