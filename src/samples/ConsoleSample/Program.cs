#nullable enable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Jab;
using ModuleSample;

new ServiceProvider().GetService<Program>().Run(args);


//[ServiceProvider]
//[Import(typeof(IModule))]
//[Singleton(typeof(Program))]
//[Singleton(typeof(Logger))]
partial class ServiceProvider : global::System.IDisposable, System.IAsyncDisposable,
   global::System.IServiceProvider,
   IServiceProvider<ModuleSample.ServiceDefinedInAModule>,
   IServiceProvider<Logger>,
   IServiceProvider<Program>,
   IServiceProvider<System.IServiceProvider>
{
    private readonly Lazy<Scope> _rootScope;
    
    private readonly Lazy<Logger> _Logger;
    private readonly Lazy<Program> _Program;
    private readonly Lazy<ModuleSample.ServiceDefinedInAModule> _ServiceDefinedInAModule;

    public ServiceProvider()
    {
        _rootScope = new(() => CreateScope());

        //Main Module - Singletons
        _Logger = new(() => new Logger());
        _Program = new(() => new Program(_Logger.Value, this.GetService<ModuleSample.ServiceDefinedInAModule>()));

        //Module: IModule - Singletons
        _ServiceDefinedInAModule = new(() => new ModuleSample.ServiceDefinedInAModule());
    }


    [DebuggerHidden]
    public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new NotImplementedException();
    System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService() => this;

    Logger IServiceProvider<Logger>.GetService() => _Logger.Value;
    Program IServiceProvider<Program>.GetService() => _Program.Value;

    ModuleSample.ServiceDefinedInAModule IServiceProvider<ModuleSample.ServiceDefinedInAModule>.GetService() => _ServiceDefinedInAModule.Value;

    object? global::System.IServiceProvider.GetService(global::System.Type type)
    {
        if (type == typeof(ModuleSample.ServiceDefinedInAModule)) return this.GetService<ModuleSample.ServiceDefinedInAModule>();
        if (type == typeof(Logger)) return this.GetService<Logger>();
        if (type == typeof(Program)) return this.GetService<Program>();
        if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
        return null;
    }

    private global::System.Collections.Generic.List<WeakReference>? _disposables;

    private void TryAddDisposable(object? value)
    {
        if (value is not global::System.IDisposable && value is not System.IAsyncDisposable)
            return;

        lock (this)
        {
            (_disposables ??= new global::System.Collections.Generic.List<WeakReference>()).Add(new WeakReference(value));
        }
    }

    public void Dispose()
    {
        static void TryDispose(object? value) => (value as IDisposable)?.Dispose();

        TryDispose(_ServiceDefinedInAModule);
        TryDispose(_Logger.Value);
        TryDispose(_Program.Value);
        TryDispose(_rootScope);
        if (_disposables != null)
        {
            foreach (var service in _disposables)
            {
                TryDispose(service);
            }
        }
    }

    public async global::System.Threading.Tasks.ValueTask DisposeAsync()
    {
        static global::System.Threading.Tasks.ValueTask TryDispose(object? value)
        {
            switch (value)
            {
                case System.IAsyncDisposable asyncDisposable:
                    return asyncDisposable.DisposeAsync();
                case global::System.IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
            return ValueTask.CompletedTask;
        }

        await TryDispose(_ServiceDefinedInAModule);
        await TryDispose(_Logger.Value);
        await TryDispose(_Program.Value);
        await TryDispose(_rootScope);
        if (_disposables != null)
        {
            foreach (var service in _disposables)
            {
                await TryDispose(service);
            }
        }
    }

    public Scope CreateScope() => new Scope(this);

    public partial class Scope : global::System.IDisposable,
       System.IAsyncDisposable,
       global::System.IServiceProvider,
       IServiceProvider<ModuleSample.ServiceDefinedInAModule>,
       IServiceProvider<Logger>,
       IServiceProvider<Program>,
       IServiceProvider<System.IServiceProvider>
    {

        private ServiceProvider _root;

        public Scope(ServiceProvider root)
        {
            _root = root;
        }

        [DebuggerHidden]
        public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new NotImplementedException();

        ModuleSample.ServiceDefinedInAModule IServiceProvider<ModuleSample.ServiceDefinedInAModule>.GetService()
        {
            return _root.GetService<ModuleSample.ServiceDefinedInAModule>();
        }

        Logger IServiceProvider<Logger>.GetService()
        {
            return _root.GetService<Logger>();
        }

        Program IServiceProvider<Program>.GetService()
        {
            return _root.GetService<Program>();
        }

        System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService()
        {
            return this;
        }

        object? global::System.IServiceProvider.GetService(global::System.Type type)
        {
            if (type == typeof(ModuleSample.ServiceDefinedInAModule)) return this.GetService<ModuleSample.ServiceDefinedInAModule>();
            if (type == typeof(Logger)) return this.GetService<Logger>();
            if (type == typeof(Program)) return this.GetService<Program>();
            if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
            return null;
        }

        private global::System.Collections.Generic.List<object>? _disposables;

        private void TryAddDisposable(object? value)
        {
            if (value is global::System.IDisposable || value is System.IAsyncDisposable)
                lock (this)
                {
                    (_disposables ??= new global::System.Collections.Generic.List<object>()).Add(value);
                }
        }

        public void Dispose()
        {
            void TryDispose(object? value) => (value as IDisposable)?.Dispose();

            if (_disposables != null)
            {
                foreach (var service in _disposables)
                {
                    TryDispose(service);
                }
            }
        }

        public async global::System.Threading.Tasks.ValueTask DisposeAsync()
        {
            global::System.Threading.Tasks.ValueTask TryDispose(object? value)
            {
                if (value is System.IAsyncDisposable asyncDisposable)
                {
                    return asyncDisposable.DisposeAsync();
                }
                else if (value is global::System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
                return default;
            }

            if (_disposables != null)
            {
                foreach (var service in _disposables)
                {
                    await TryDispose(service);
                }
            }
        }

    }
}

partial class Program
{
    private readonly Logger _logger;
    private readonly ServiceDefinedInAModule _serviceDefinedInAModule;

    public Program(Logger logger, ServiceDefinedInAModule serviceDefinedInAModule)
    {
        _logger = logger;
        _serviceDefinedInAModule = serviceDefinedInAModule;
    }

    public void Run(string[] args)
    {
        _logger.Log("Starting");
        _logger.LogError("Error happened");
    }
}