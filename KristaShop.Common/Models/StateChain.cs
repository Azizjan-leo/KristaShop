using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;

namespace KristaShop.Common.Models {
    public class StateChain {
        private IDictionary<State, StateChainItem> Chain { get; }

        public StateChain(List<StateChainItem> chain) {
            Chain = chain.ToDictionary(k => k.State, v => v);
        }
        
        public State GetNextState(State currentState) {
            return Chain.ContainsKey(currentState) ? Chain[currentState].NextState : currentState;
        }

        public string GetNextAction(State currentState) {
            return Chain.ContainsKey(currentState) ? Chain[currentState].NextAction : "";
        }
    }
    public class StateChainItem {
        public State State { get; }
        public string NextAction { get; }
        public State NextState { get; }

        public StateChainItem(State state, string nextAction, State nextState) {
            State = state;
            NextAction = nextAction;
            NextState = nextState;
        }
    }
}