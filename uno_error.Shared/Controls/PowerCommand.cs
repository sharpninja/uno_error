using System;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace uno_error.Controls
{
    public record PowerCommand<TData>(string Name, string Description, string Id, Action<TData> Execute, Func<TData, bool> CanExecute) :
        IPowerCommand
    {
        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter) => CanExecute?.Invoke((TData)parameter) ?? true;

        void ICommand.Execute(object parameter) => Execute?.Invoke((TData)parameter);
    }
}
