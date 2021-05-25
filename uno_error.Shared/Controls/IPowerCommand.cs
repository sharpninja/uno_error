using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace uno_error.Controls
{
    public interface IPowerCommand : ICommand
    {
        string Name { get; }
        string Description { get; }
        string Id { get; }
    }
}
