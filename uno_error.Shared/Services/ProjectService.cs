using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

using Newtonsoft.Json;

using uno_error.ViewModels;

using Uno.Disposables;
using Uno.Extensions;

using Windows.Storage;
using Windows.Storage.Streams;

using Buffer = System.Buffer;


#if !UAP
#nullable enable
#endif

namespace uno_error.Services
{
    public class ProjectService
    {
        public DialogService DialogService { get; }
        public bool HasProject(ProjectViewModel? project) => project is not null;

        public event Action? RequestExitApp;

        public ProjectService(DialogService dialogService)
        {
            DialogService = dialogService;
        }

        public async Task ExecuteExit(ProjectViewModel? project)
        {
            if (project is not null)
            {
                await ExecuteCloseProject(project);
            }

            RequestExitApp?.Invoke();
        }

        public async Task ExecuteSaveProject(ProjectViewModel? project)
        { }

        public async Task ExecuteAddAssembly(ProjectViewModel? project)
        {

        }

        public async Task ExecuteCloseProject(ProjectViewModel? project)
        {
            if (project is null) return;

            if (project.IsDirty)
            {
                await project.SaveAsync();
            }

            project.TryDispose();
        }

        public async Task ExecuteNewProject(MainViewModel? viewModel)
        {
            var result = await DialogService.GetNewProjectName();
            if (result.dialogResult == ContentDialogResult.Secondary) return;

            if (viewModel?.Project is not null && viewModel.Project.IsDirty)
            {
                await ExecuteCloseProject(viewModel.Project);
            }

            var project = App.Host.Services.GetRequiredService<ProjectViewModel>();

            project.Name = result.name;

            viewModel.Project = project;
        }

        public async Task ExecuteOpenProject(MainViewModel? viewModel)
        {
            if (viewModel?.Project is not null && viewModel.Project.IsDirty)
            {
                await ExecuteCloseProject(viewModel.Project);
            }

            var result = await OpenAsync();

            viewModel.Project = result.project;
        }


        public async Task<(bool yes, ProjectViewModel project)> OpenAsync()
        {
            var result = await DialogService.OpenProjectAsync();

            if (result.yes)
            {
                using var readStream = await result.file.OpenReadAsync();
                var buffer = new Windows.Storage.Streams.Buffer((uint)readStream.Size);
                var bytes = await readStream.ReadAsync(buffer, (uint)readStream.Size, InputStreamOptions.None);
                var json = Encoding.UTF8.GetString(buffer.ToArray());

                var project = JsonConvert.DeserializeObject<ProjectViewModel>(json);

                return (true, project);
            }

            return (false, null);
        }
    }
}
