using System.Collections.Generic;

namespace Common.Services
{
    public interface IButton
    {
        string DisplayButtonName { get; }
        void CopyTo(IButton button);
    }
    public interface IButtonService
    {
        /// <summary>
        /// Return a list of compatible buttons
        /// </summary>
        /// <param name="selectedBtn"></param>
        /// <returns></returns>
        IEnumerable<IButton> Buttons(object selectedBtn);
    }
}