using System;
using System.Threading.Tasks;
using Windows.Storage;
using uno_error.ViewModels;

using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Microsoft.UI.Xaml.Data;

#if !UAP
#nullable enable
#endif

namespace uno_error.Services
{
    public class DialogService
    {
        public async Task<(bool yes, StorageFile? target)> SaveProjectAsync(ProjectViewModel projectViewModel)
        {
            var picker = new FileSavePicker
            {
                SuggestedFileName = projectViewModel.Name + ".pumlproj"
            };

            picker.FileTypeChoices.Add("uno_error Project", new[] { ".pumlproj" });

            var target = await picker.PickSaveFileAsync();

            return (target?.Path != null, target);
        }

        public async Task<(bool yes, StorageFile? file)> OpenProjectAsync()
        {
            var picker = new FileOpenPicker();

            picker.FileTypeFilter.Add(".pumlproj");

            var target = await picker.PickSingleFileAsync();

            return (target?.Path != null, target);
        }

        internal async Task<(ContentDialogResult dialogResult, string name)> GetNewProjectName()
        {
            var template = App.CurrentMainPage.Resources["TextInputContent"] as DataTemplate;
            var inputViewModel = new TextInputViewModel
            {
                Question = "Enter name for new Project."
            };
            (ContentDialogResult dialogResult, string name) result = default;
            var contentControl = new ContentControl
            {
                DataContext = inputViewModel,
                ContentTemplate = template
            };

            var dialog = new ContentDialog
            {
                DataContext = inputViewModel,
                Title = "New Project",
                Content = contentControl,
                CloseButtonText = "Cancel",
                CloseButtonCommand = new RelayCommand(() => { result.dialogResult = ContentDialogResult.Secondary; }),
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonText = "Create"
            };

            var binding = new Binding
            {
                Path = new PropertyPath(nameof(TextInputViewModel.Answer)),
                Mode=BindingMode.OneWay
            };

            typeof(Binding).GetProperty("DataContext").SetValue(binding, inputViewModel);

            dialog.SetBinding(ContentDialog.PrimaryButtonCommandParameterProperty, binding);

            dialog.PrimaryButtonCommand = new RelayCommand<string>(answer =>
            {
                if (!string.IsNullOrWhiteSpace(answer))
                {
                    result.dialogResult = ContentDialogResult.Primary;
                    result.name = answer;
                    dialog.Hide();
                }
            },
                answer => !string.IsNullOrWhiteSpace(answer));


            await dialog.ShowAsync();
            return result;
        }
    }
}
