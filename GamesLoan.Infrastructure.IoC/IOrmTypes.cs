﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesLoan.Infrastructure.IoC
{
    public interface IOrmTypes
    {
        IServiceCollection ResolveOrm(IServiceCollection services, IConfiguration configuration = null);
    }
}
