using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using uno_error.Services;
using uno_error.ViewModels;

using static uno_error.ViewModels.MainViewModel;

namespace uno_error
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ProjectService>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<MainViewModelCommands>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<ProjectViewModel>();
        }
    }
}
