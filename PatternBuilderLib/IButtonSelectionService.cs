using System;
using System.Collections.Generic;

namespace PatternBuilderLib
{
    public interface IButtonSelectionService
    {
        void ButtonColorChanged(int color);
        void Subscribe(object sender, Action<int> onColorChangedAction);
    }

    public class ButtonSelectionService:IButtonSelectionService
    {
        Dictionary<object, Action<int>> _subscribers = new Dictionary<object, Action<int>>();
        public void ButtonColorChanged(int color)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Value(color);
            }
        }

        public void Subscribe(object sender, Action<int> onColorChangedAction)
        {
            _subscribers[sender] = onColorChangedAction;
        }
    }
}