using System.Collections.Generic;
using DAndRElectronics.ButtonViewModels;

namespace DAndRElectronics.Services
{
    public interface IButtonViewModelFactoryService
    {
        ButtonViewModel CreateViewModel(string buttonName, int col, int row);
        ButtonViewModel CreateViewModelFromString(string content);
        IEnumerable<ButtonViewModel> ReadFile(string fileName);
    }

}