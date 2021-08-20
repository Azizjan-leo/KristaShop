using System;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;

namespace KristaShop.Common.Implementation.ChainOfResponsibility {
    public static class ChainOperationExtensions {
        public static IChainOperation<TOutput> Next<TInput, TOutput>(this IChainOperation<TInput> operation, Func<TInput, TOutput> function) {
            var nextOperation = new FuncChainSyncOperation<TInput, TOutput>(function);
            nextOperation.Handle(operation.Result);
            return nextOperation;
        }
        
        public static IChainOperation<TOutput> Next<TInput, TOutput>(this IChainOperation<TInput> operation, IChainOperation<TInput, TOutput> nextOperation) {
            nextOperation.Handle(operation.Result);
            return nextOperation;
        }
        
        public static async Task<IAsyncChainOperation<TOutput>> NextAsync<TInput, TOutput>(this Task<IAsyncChainOperation<TInput>> currentOperation, Func<TInput, Task<TOutput>> function) {
            var nextOperation = new FuncChainAsyncOperation<TInput, TOutput>(function);
            await nextOperation.HandleAsync(await currentOperation.GetResultAsync());
            return nextOperation;
        }
        
        public static async Task<IAsyncChainOperation<TOutput>> NextAsync<TInput, TOutput>(this Task<IAsyncChainOperation<TInput>> currentOperation, IAsyncChainOperation<TInput, TOutput> nextOperation) {
            await nextOperation.HandleAsync(await currentOperation.GetResultAsync());
            return nextOperation;
        }

        public static async Task<IAsyncChainOperation<TCurrentOutput>> NextAsync<TCurrentOutput, TNextInput, TNextOutput>(
            this Task<IAsyncChainOperation<TCurrentOutput>> currentOperation,
            Func<TCurrentOutput, TNextInput> inputFunction,
            Func<Task<IAsyncChainOperation<TNextInput>>, Task<TNextOutput>> builder,
            ChainInputOption inputOption = ChainInputOption.ThrowExceptionIfNull) {
            var nextInput = inputFunction.Invoke(await currentOperation.GetResultAsync());
            if (nextInput == null) {
                if (inputOption == ChainInputOption.ThrowExceptionIfNull) {
                    throw new ExceptionBase($"Input for the next chain operation is null. To skip next chain operation use {nameof(ChainInputOption)}.{ChainInputOption.SkipIfNull}");
                }

                return await currentOperation;
            }
            
            await builder.Invoke(_createFirstChainItemAsync(nextInput));
            return await currentOperation;
        }

        public static async Task<TOutput> GetResultAsync<TOutput>(this Task<IAsyncChainOperation<TOutput>> currentOperation) {
            return (await currentOperation).Result;
        }
        
        public static async Task<TOutput> ExecuteAsync<TOutput>(this Task<IAsyncChain<TOutput>> chain) {
            return await (await chain).ExecuteAsync();
        }
        
        private static async Task<IAsyncChainOperation<TInput>> _createFirstChainItemAsync<TInput>(TInput input) {
            var operation = new FuncChainAsyncOperation<TInput, TInput>(async x => x);
            await operation.HandleAsync(input);
            return operation;
        }
    }
}