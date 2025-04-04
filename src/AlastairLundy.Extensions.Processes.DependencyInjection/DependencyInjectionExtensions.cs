/*
    AlastairLundy.Extensions.Processes  
    Copyright (C) 2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using AlastairLundy.Extensions.IO.Files;
using AlastairLundy.Extensions.IO.Files.Abstractions;

using AlastairLundy.Extensions.Processes.Abstractions.Piping;
using AlastairLundy.Extensions.Processes.Piping;
using AlastairLundy.Extensions.Processes.Utilities;

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

                services.AddSingleton<Abstractions.IProcessFactory, ProcessFactory>();
                services.AddSingleton<Abstractions.Utilities.IProcessRunnerUtility, ProcessRunnerUtility>();
                services.AddSingleton<Abstractions.IPipedProcessRunner, PipedProcessRunner>();
                services.AddSingleton<Abstractions.IProcessRunner, ProcessRunner>();
                services.AddSingleton<IProcessPipeHandler, Processes.Piping.ProcessPipeHandler>();
                break;
            case ServiceLifetime.Scoped:
                services.TryAddScoped<IFilePathResolver, FilePathResolver>();
                
                services.AddScoped<Abstractions.IProcessFactory, ProcessFactory>();

                services.AddScoped<Abstractions.Utilities.IProcessRunnerUtility, ProcessRunnerUtility>();
                services.AddScoped<Abstractions.IPipedProcessRunner, PipedProcessRunner>();
                services.AddScoped<Abstractions.IProcessRunner, ProcessRunner>();
                services.AddScoped<IProcessPipeHandler, ProcessPipeHandler>();
                break;
            case ServiceLifetime.Transient:
                services.TryAddTransient<IFilePathResolver, FilePathResolver>();
                
                services.AddTransient<Abstractions.IProcessFactory, ProcessFactory>();

                
                services.AddTransient<Abstractions.Utilities.IProcessRunnerUtility, ProcessRunnerUtility>();
                services.AddTransient<Abstractions.IPipedProcessRunner, PipedProcessRunner>();
                services.AddTransient<Abstractions.IProcessRunner, ProcessRunner>();
                services.AddTransient<IProcessPipeHandler, ProcessPipeHandler>();
                break;
        }

        return services;
    }
}