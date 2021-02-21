using System;
using System.Collections.Generic;

namespace DAndRElectronics.Services
{
    public enum StateChangedTypes
    {
        ProjectOpened
    }

    public interface IStateService
    {
        void Subscribe(object subscriber, Action<StateChangedTypes> action);
        void OnStateChanged(StateChangedTypes stateChangedType);
    }

    public class StateService: IStateService
    {
        private Dictionary<object, Action<StateChangedTypes>> _subscribers = new Dictionary<object, Action<StateChangedTypes>>();
        public void Subscribe(object subscriber, Action<StateChangedTypes> action)
        {
            _subscribers[subscriber] = action;
        }

        public void OnStateChanged(StateChangedTypes stateChangedType)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Value(stateChangedType);
            }
        }
    }
}