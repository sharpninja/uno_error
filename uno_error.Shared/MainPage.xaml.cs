using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using uno_error.ViewModels;
using System.Diagnostics;
using uno_error.Services;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace uno_error
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private MainViewModel _viewModel;

        public static DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainPage), new PropertyMetadata(null));

        public MainViewModel? ViewModel {
            get => GetValue(MainPage.ViewModelProperty) as MainViewModel;
            set 
            {
                if (value is null) return;

                SetValue(ViewModelProperty, value);
            }
        }

        public ProjectService ProjectService { get; }

        public MainPage()
        {
            ProjectService = App.Host.Services.GetService<ProjectService>();

            ProjectService.RequestExitApp += () =>
            {
                Process.GetCurrentProcess().CloseMainWindow();
            };

            ViewModel = App.Host.Services.GetRequiredService<MainViewModel>();
            this.InitializeComponent();
        }
    }
}
