using GamesLoan.Application.Interfaces.Services;
using GamesLoan.Application.Interfaces.Services.Standard;
using GamesLoan.Application.Services;
using GamesLoan.Application.Services.Standard;
using Microsoft.Extensions.DependencyInjection;

namespace GamesLoan.Application.IoC.Services
{
    public static class ServicesIoC
    {
        public static void ApplicationServicesIoC(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));

            services.AddScoped<IAutenticationService, AutenticationService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTypeService, UserTypeService>();
        }
    }
}
