using System;
using System.Collections.Generic;
using Common.Enums;

namespace DAndRElectronics.Services
{
   

    public interface IStateService
    {
        void Subscribe(object subscriber, Action<StateChangedTypes> action);
        void SubscribeButtonDelete(object subscriber, Action<object> action);
        void OnStateChanged(StateChangedTypes stateChangedType);
        void OnDeleteButton(object vm);
    }

    public class StateService: IStateService
    {
        private Dictionary<object, Action<StateChangedTypes>> _subscribers = new Dictionary<object, Action<StateChangedTypes>>();
        private Dictionary<object, Action<object>> _buttonDeleteSubscribers = new Dictionary<object, Action<object>>();
        public void Subscribe(object subscriber, Action<StateChangedTypes> action)
        {
            _subscribers[subscriber] = action;
        }
        public void SubscribeButtonDelete(object subscriber, Action<object> action)
        {
            _buttonDeleteSubscribers[subscriber] = action;
        }

        public void OnStateChanged(StateChangedTypes stateChangedType)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Value(stateChangedType);
            }
        }

        public void OnDeleteButton(object vm)
        {
            foreach (var subscriber in _buttonDeleteSubscribers)
            {
                subscriber.Value(vm);
            }
        }
    }
}