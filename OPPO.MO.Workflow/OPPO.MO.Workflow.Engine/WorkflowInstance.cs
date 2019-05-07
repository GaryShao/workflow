using System;
using Stateless;

namespace OPPO.MO.Workflow.Engine
{
    public class WorkflowInstance
    {
        private State _currentState = State.Initial;
        
        public void Create()
        {
            var machine = new StateMachine<State, Trigger>(() => _currentState, 
                state => _currentState = state);            
        }        
    }
}