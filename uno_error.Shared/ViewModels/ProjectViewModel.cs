using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;

using uno_error.Services;

using Windows.Storage;

namespace uno_error.ViewModels
{
    public class ProjectViewModel : ObservableObject
    {
        private string _name;
        private bool _isDirty;

        public ProjectViewModel(DialogService dialogService)
        {
            DialogService = dialogService;
        }

        public DialogService DialogService { get; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool IsDirty { get => _isDirty; set => SetProperty(ref _isDirty, value); }

        public async Task<bool> SaveAsync()
        {
            var (yes, target) = await DialogService.SaveProjectAsync(this);

            if (yes) return await SaveAsync(target);

            return false;
        }

        public async Task<bool> SaveAsync(StorageFile storageFile)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            using var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters);

            using var output = stream.GetOutputStreamAt(0);

            var bytes = Encoding.UTF8.GetBytes(json);
            var result = await output.WriteAsync(new Windows.Storage.Streams.Buffer((uint)bytes.Length));

            if (result == (uint) bytes.Length)
            {
                return await output.FlushAsync();
            }

            return false;
        }
    }
}
