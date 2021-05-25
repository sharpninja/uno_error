using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace uno_error.Controls
{
    public sealed partial class PowerBar
    {
        public PowerBar()
        {
            this.InitializeComponent();
        }

        public PowerBar(IEnumerable<IPowerCommand> commands = null) : this()
        {
            if(commands is not null)
            {
                Commands = new ObservableCollection<IPowerCommand>(commands);
            }
        }

        public ObservableCollection<IPowerCommand> Commands
        {
            get => (ObservableCollection<IPowerCommand>)GetValue(CommandsProperty);
            set => SetValue(CommandsProperty, value);
        }

        public static DependencyProperty CommandsProperty =
            DependencyProperty.Register(nameof(Commands), typeof(IEnumerable<IPowerCommand>), typeof(PowerBar),
                new PropertyMetadata(new ObservableCollection<IPowerCommand>()));
    }
}
