using System;
using Common.Enums;

namespace Common.Services
{
    public interface IEditorService
    {
        void SetContent(object uiElement, string title, bool IsModal=false);
        void SetContent(object uiElement, string title, Action windowClosed, bool IsModal=false);
        void SetContentWithSize(object uiElement, string title, Action windowClosed, double width, double height,bool IsModal=false);
        void SetWidthAndHeight(double width, double height, EditWindowPosition position);
    }
}