/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using AlastairLundy.Extensions.IO.Files;
using AlastairLundy.Extensions.IO.Files.Abstractions;

using AlastairLundy.Extensions.Processes.Abstractions;
using AlastairLundy.Extensions.Processes.Piping;
using AlastairLundy.Extensions.Processes.Piping.Abstractions;
using AlastairLundy.Extensions.Processes.Utilities;
using AlastairLundy.Extensions.Processes.Utilities.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AlastairLundy.Extensions.Processes.DependencyInjection;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Sets up Dependency Injection for Process Extensions' interface-able types.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="lifetime">The service lifetime to use if specified; Singleton otherwise.</param>
    /// <returns>The updated service collection with the added Process Extension services set up.</returns>
    public static IServiceCollection AddProcessExtensions(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.TryAddSingleton<IFilePathResolver, FilePathResolver>();
                
                services.AddSingleton<IProcessRunnerUtility, ProcessRunnerUtility>();
                services.AddSingleton<IPipedProcessRunner, PipedProcessRunner>();
                services.AddSingleton<IProcessRunner, ProcessRunner>();
                services.AddSingleton<IProcessPipeHandler, ProcessPipeHandler>();
                break;
            case ServiceLifetime.Scoped:
                services.TryAddScoped<IFilePathResolver, FilePathResolver>();
                
                services.AddScoped<IProcessRunnerUtility, ProcessRunnerUtility>();
                services.AddScoped<IPipedProcessRunner, PipedProcessRunner>();
                services.AddScoped<IProcessRunner, ProcessRunner>();
                services.AddScoped<IProcessPipeHandler, ProcessPipeHandler>();
                break;
            case ServiceLifetime.Transient:
                services.TryAddTransient<IFilePathResolver, FilePathResolver>();
                
                services.AddTransient<IProcessRunnerUtility, ProcessRunnerUtility>();
                services.AddTransient<IPipedProcessRunner, PipedProcessRunner>();
                services.AddTransient<IProcessRunner, ProcessRunner>();
                services.AddTransient<IProcessPipeHandler, ProcessPipeHandler>();
                break;
        }

        return services;
    }
}