using System;
using System.ComponentModel;
using System.Threading.Tasks;
using FluentAssertions;
using KristaShop.Common.Implementation.ChainOfResponsibility;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.Common.Models.Structs;
using NUnit.Framework;

namespace Tests.Business.Common {
    public class ChainOfResponsibilityTests {
        [SetUp]
        public void Setup() {
        }

        [TearDown]
        public void Dispose() {
        }
        
        [Test]
        public void Should_CreateChain_When_CallCreate() {
            var chainBuilder = ChainBuilder.Create("", builder => builder);
            
            chainBuilder.Should().NotBeNull();
        }
        
        [Test]
        public void Should_ExecuteFunction_When_NextStepProvided() {
            var actual = ChainBuilder.Create("1", builder => builder
                .Next(int.Parse)
            ).Execute();

            actual.Should().Be(1);
        }
        
        [Test]
        public void Should_ExecuteFunction_When_TwoStepProvided() {
            var actual = ChainBuilder.Create("3", builder => builder
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
            ).Execute();

            actual.Should().Be("4");
        }
        
        [Test]
        public void Should_ExecuteFunction_When_TenStepProvided() {
            var actual = ChainBuilder.Create("0", builder => builder
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
            ).Execute();

            actual.Should().Be("5");
        }
        
        [Test]
        public async Task Should_AsyncCreateChain_When_CreateAsync() {
            var chainBuilder = await ChainBuilder.CreateAsync("1", async builder => await builder);
            
            chainBuilder.Should().NotBeNull();
        }
        
        [Test]
        public async Task Should_AsyncExecuteFunction_When_NextAsyncStepProvided() {
            var actual =await ChainBuilder.CreateAsync("1", async builder => await builder
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x)))
                ).ExecuteAsync();

            actual.Should().Be(1);
        }
        
        [Test]
        public async Task Should_AsyncExecuteFunction_When_TenStepProvided() {
            var actual = await ChainBuilder.CreateAsync("0", async builder => await builder
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString()))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString()))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString()))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString()))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString()))
            ).ExecuteAsync();

            actual.Should().Be("5");
        }
        
        [Test]
        public void Should_CreateChain_When_StepsHasObjectHandlers() {
            var actual = ChainBuilder.Create("1", builder => builder
                .Next(new StringToIntIncrementSyncOperation())
                .Next(new IntToStringSyncOperation())
            ).Execute();

            actual.Should().Be("2");
        }
        
        [Test]
        public void Should_CreateChain_When_StepsHasObjectHandlersAndFunctionHandlers() {
            var actual = ChainBuilder.Create("1", builder => builder
                .Next(new StringToIntIncrementSyncOperation())
                .Next(new IntToStringSyncOperation())
                .Next(x => int.Parse(x) + 1)
                .Next(x => x.ToString())
            ).Execute();

            actual.Should().Be("3");
        }
        
        [Test]
        public async Task Should_CreateAsyncChain_When_StepsHasObjectHandlers() {
            var actual = await ChainBuilder.CreateAsync("1", async builder => await builder
                .NextAsync(new StringToIntIncrementAsyncOperation())
                .NextAsync(new IntToStringAsyncOperation())
            ).ExecuteAsync();

            actual.Should().Be("2");
        }
        
        [Test]
        public async Task Should_CreateAsyncChain_When_StepsHasObjectHandlersAndFunctionHandlers() {
            var actual = await ChainBuilder.CreateAsync("1", async builder => await builder
                .NextAsync(new StringToIntIncrementAsyncOperation())
                .NextAsync(new IntToStringAsyncOperation())
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1))
                .NextAsync(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString()))
                ).ExecuteAsync();

            actual.Should().Be("3");
        }
        
        [Test]
        public void Should_CreateChain_When_StepsHasDifferentHandlersAsObjects() {
            var actual = ChainBuilder.Create("1", builder => builder
                .Next(new StringToIntIncrementSyncOperation())
                .Next(new IntToStringSyncOperation())
                .Next(new FuncChainSyncOperation<string, int>(x => int.Parse(x) + 1))
                .Next(new FuncChainSyncOperation<int, string>(x => x.ToString()))
            ).Execute();

            actual.Should().Be("3");
        }
        
        [Test]
        public async Task Should_AsyncCreateChain_When_StepsHasDifferentHandlersAsObjects() {
            var actual = await ChainBuilder.CreateAsync("1", async builder => await builder
                .NextAsync(new StringToIntIncrementAsyncOperation())
                .NextAsync(new IntToStringAsyncOperation())
                .NextAsync(new FuncChainAsyncOperation<string, int>(async x => await Task.Delay(1000).ContinueWith(_ => int.Parse(x) + 1)))
                .NextAsync(new FuncChainAsyncOperation<int, string>(async x => await Task.Delay(1000).ContinueWith(_ => x.ToString())))
                ).ExecuteAsync();

            actual.Should().Be("3");
        }
        
        [Test]
        public void Should_CreateChain_When_OneHandlerHasSyncAndAsyncFunctions() {
            var actual = ChainBuilder.Create("1", builder => builder
                .Next(new StringToIntIncrementChainOperation())
                .Next(new IntToStringChainOperation())
            ).Execute();

            actual.Should().Be("2");
        }
        
        [Test]
        public async Task Should_AsyncCreateChain_When_OneHandlerHasSyncAndAsyncFunctions() {
            var actual = await ChainBuilder.CreateAsync("1", async builder => await builder
                .NextAsync(new StringToIntIncrementChainOperation())
                .NextAsync(new IntToStringChainOperation())
            ).ExecuteAsync();

            actual.Should().Be("2");
        }
        
        [Test]
        public async Task Should_DeferredChainCreation_When_OneHandlerHasSyncAndAsyncFunctions() {
            var actual = await ChainBuilder.CreateAsync("1", async builder => await builder
                .NextAsync(new StringToIntIncrementChainOperation())
                .NextAsync(new IntToStringChainOperation())
            ).ExecuteAsync();

            actual.Should().Be("2");
        }

        private class StringToIntIncrementSyncOperation : ChainSyncOperation<string, int> {
            protected override int HandleInput(string input) {
                return int.Parse(input) + 1;
            }
        }

        private class StringToIntIncrementAsyncOperation : ChainAsyncOperation<string, int> {
            protected override async Task<int> HandleInputAsync(string input) {
                return await Task.Delay(1000).ContinueWith(_ => int.Parse(input) + 1);
            }
        }

        private class IntToStringSyncOperation : ChainSyncOperation<int, string> {
            protected override string HandleInput(int input) {
                return input.ToString();
            }
        }

        private class IntToStringAsyncOperation : ChainAsyncOperation<int, string> {
            protected override async Task<string> HandleInputAsync(int input) {
                return await Task.Delay(1000).ContinueWith(_ => input.ToString());
            }
        }

        private class StringToIntIncrementChainOperation : ChainOperation<string, int> {
            protected override int HandleInput(string input) {
                return int.Parse(input) + 1;
            }

            protected override async Task<int> HandleInputAsync(string input) {
                return await Task.Delay(1000).ContinueWith(_ => int.Parse(input) + 1);
            }
        }

        private class IntToStringChainOperation : ChainOperation<int, string> {
            protected override string HandleInput(int input) {
                return input.ToString();
            }

            protected override async Task<string> HandleInputAsync(int input) {
                return await Task.Delay(1000).ContinueWith(_ => input.ToString());
            }
        }
    }
}