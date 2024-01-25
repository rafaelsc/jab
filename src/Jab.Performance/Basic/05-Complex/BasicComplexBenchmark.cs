namespace Jab.Performance.Basic.Complex; 

using BenchmarkDotNet.Attributes;
using Jab.Performance.Basic.Mixed;
using Jab.Performance.Basic.Scoped;
using Jab.Performance.Basic.Singleton;
using Jab.Performance.Basic.Transient;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using MEDI = Microsoft.Extensions.DependencyInjection;
using Jab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Jab;

[ShortRunJob]
[MemoryDiagnoser]
public class BasicComplexBenchmark
{
    private readonly MEDI.ServiceProvider _provider;
    private readonly ContainerComplex _container = new();
    private readonly ImprovedContainerComplex _improvedContainer = new();

    public BasicComplexBenchmark()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IComplex1, Complex1>();
        serviceCollection.AddScoped<IComplex2, Complex2>();
        serviceCollection.AddScoped<IComplex3, Complex3>();
        serviceCollection.AddTransient<IService1, Service1>();
        serviceCollection.AddTransient<IService2, Service2>();
        serviceCollection.AddTransient<IService3, Service3>();
        serviceCollection.AddTransient<IMix1, Mix1>();
        serviceCollection.AddTransient<IMix2, Mix2>();
        serviceCollection.AddTransient<IMix3, Mix3>();
        serviceCollection.AddTransient<ITransient1, Transient1>();
        serviceCollection.AddTransient<ITransient2, Transient2>();
        serviceCollection.AddTransient<ITransient3, Transient3>();
        serviceCollection.AddSingleton<ISingleton1, Singleton1>();
        serviceCollection.AddSingleton<ISingleton2, Singleton2>();
        serviceCollection.AddSingleton<ISingleton3, Singleton3>();
        _provider = serviceCollection.BuildServiceProvider();
    }

    [Params(1, 10, 100)]
    public int NumbersOfCalls { get; set; }

    [Params(1, 2, 3)]
    public int NumbersOfClasses { get; set; }

    [Benchmark(Baseline = true)]
    public void Jab()
    {
        for (var i = 0; i < NumbersOfCalls; i++)
        {
            using var scope = _container.CreateScope();

            if (NumbersOfClasses >= 1)
                scope.GetService<IComplex1>();
            if (NumbersOfClasses >= 2)
                scope.GetService<IComplex2>();
            if (NumbersOfClasses >= 3)
                scope.GetService<IComplex3>();
        }
    }

    [Benchmark]
    public void Improved_Jab()
    {
        for (var i = 0; i < NumbersOfCalls; i++)
        {
            using var scope = _improvedContainer.CreateScope();

            if (NumbersOfClasses >= 1)
                scope.GetService<IComplex1>();
            if (NumbersOfClasses >= 2)
                scope.GetService<IComplex2>();
            if (NumbersOfClasses >= 3)
                scope.GetService<IComplex3>();
        }
    }

    [Benchmark]
    public void MEDI()
    {
        for (var i = 0; i < NumbersOfCalls; i++)
        {
            using var scope = _provider.CreateScope();

            if (NumbersOfClasses >= 1)
                scope.ServiceProvider.GetService<IComplex1>();
            if (NumbersOfClasses >= 2)
                scope.ServiceProvider.GetService<IComplex2>();
            if (NumbersOfClasses >= 3)
                scope.ServiceProvider.GetService<IComplex3>();
        }
    }
}

//[ServiceProvider]
//[Scoped(typeof(IComplex1), typeof(Complex1))]
//[Scoped(typeof(IComplex2), typeof(Complex2))]
//[Scoped(typeof(IComplex3), typeof(Complex3))]
//[Transient(typeof(IService1), typeof(Service1))]
//[Transient(typeof(IService2), typeof(Service2))]
//[Transient(typeof(IService3), typeof(Service3))]
//[Transient(typeof(IMix1), typeof(Mix1))]
//[Transient(typeof(IMix2), typeof(Mix2))]
//[Transient(typeof(IMix3), typeof(Mix3))]
//[Transient(typeof(ITransient1), typeof(Transient1))]
//[Transient(typeof(ITransient2), typeof(Transient2))]
//[Transient(typeof(ITransient3), typeof(Transient3))]
//[Singleton(typeof(ISingleton1), typeof(Singleton1))]
//[Singleton(typeof(ISingleton2), typeof(Singleton2))]
//[Singleton(typeof(ISingleton3), typeof(Singleton3))]
internal partial class ContainerComplex : global::System.IDisposable,
       System.IAsyncDisposable,
       global::System.IServiceProvider,
       Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider,
       Microsoft.Extensions.DependencyInjection.IServiceScopeFactory,
       Microsoft.Extensions.DependencyInjection.IServiceProviderIsService,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService1>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService2>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService3>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>,
       IServiceProvider<System.IServiceProvider>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>
{
    private Scope? _rootScope;
    private Jab.Performance.Basic.Singleton.Singleton1? _ISingleton1;
    private Jab.Performance.Basic.Singleton.Singleton2? _ISingleton2;
    private Jab.Performance.Basic.Singleton.Singleton3? _ISingleton3;

    Jab.Performance.Basic.Transient.ITransient1 IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>.GetService()
    {
        Jab.Performance.Basic.Transient.Transient1 service = new Jab.Performance.Basic.Transient.Transient1();
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IService1 IServiceProvider<Jab.Performance.Basic.Complex.IService1>.GetService()
    {
        Jab.Performance.Basic.Complex.Service1 service = new Jab.Performance.Basic.Complex.Service1(this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Transient.ITransient2 IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>.GetService()
    {
        Jab.Performance.Basic.Transient.Transient2 service = new Jab.Performance.Basic.Transient.Transient2();
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IService2 IServiceProvider<Jab.Performance.Basic.Complex.IService2>.GetService()
    {
        Jab.Performance.Basic.Complex.Service2 service = new Jab.Performance.Basic.Complex.Service2(this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Transient.ITransient3 IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>.GetService()
    {
        Jab.Performance.Basic.Transient.Transient3 service = new Jab.Performance.Basic.Transient.Transient3();
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IService3 IServiceProvider<Jab.Performance.Basic.Complex.IService3>.GetService()
    {
        Jab.Performance.Basic.Complex.Service3 service = new Jab.Performance.Basic.Complex.Service3(this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Singleton.ISingleton1 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>.GetService()
    {
        if (_ISingleton1 == null)
            lock (this)
                if (_ISingleton1 == null)
                {
                    _ISingleton1 = new Jab.Performance.Basic.Singleton.Singleton1();
                }
        return _ISingleton1;
    }

    Jab.Performance.Basic.Mixed.IMix1 IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>.GetService()
    {
        Jab.Performance.Basic.Mixed.Mix1 service = new Jab.Performance.Basic.Mixed.Mix1(this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>(), this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Singleton.ISingleton2 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>.GetService()
    {
        if (_ISingleton2 == null)
            lock (this)
                if (_ISingleton2 == null)
                {
                    _ISingleton2 = new Jab.Performance.Basic.Singleton.Singleton2();
                }
        return _ISingleton2;
    }

    Jab.Performance.Basic.Mixed.IMix2 IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>.GetService()
    {
        Jab.Performance.Basic.Mixed.Mix2 service = new Jab.Performance.Basic.Mixed.Mix2(this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>(), this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Singleton.ISingleton3 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>.GetService()
    {
        if (_ISingleton3 == null)
            lock (this)
                if (_ISingleton3 == null)
                {
                    _ISingleton3 = new Jab.Performance.Basic.Singleton.Singleton3();
                }
        return _ISingleton3;
    }

    Jab.Performance.Basic.Mixed.IMix3 IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>.GetService()
    {
        Jab.Performance.Basic.Mixed.Mix3 service = new Jab.Performance.Basic.Mixed.Mix3(this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>(), this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IComplex1 IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>.GetService() => GetRootScope().GetService<Jab.Performance.Basic.Complex.IComplex1>();

    Jab.Performance.Basic.Complex.IComplex2 IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>.GetService() => GetRootScope().GetService<Jab.Performance.Basic.Complex.IComplex2>();

    Jab.Performance.Basic.Complex.IComplex3 IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>.GetService() => GetRootScope().GetService<Jab.Performance.Basic.Complex.IComplex3>();

    System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService()
    {
        return this;
    }

    Microsoft.Extensions.DependencyInjection.IServiceScopeFactory IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>.GetService()
    {
        return this;
    }

    Microsoft.Extensions.DependencyInjection.IServiceProviderIsService IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>.GetService()
    {
        return this;
    }

    object? global::System.IServiceProvider.GetService(global::System.Type type)
    {
        if (type == typeof(Jab.Performance.Basic.Transient.ITransient1)) return this.GetService<Jab.Performance.Basic.Transient.ITransient1>();
        if (type == typeof(Jab.Performance.Basic.Complex.IService1)) return this.GetService<Jab.Performance.Basic.Complex.IService1>();
        if (type == typeof(Jab.Performance.Basic.Transient.ITransient2)) return this.GetService<Jab.Performance.Basic.Transient.ITransient2>();
        if (type == typeof(Jab.Performance.Basic.Complex.IService2)) return this.GetService<Jab.Performance.Basic.Complex.IService2>();
        if (type == typeof(Jab.Performance.Basic.Transient.ITransient3)) return this.GetService<Jab.Performance.Basic.Transient.ITransient3>();
        if (type == typeof(Jab.Performance.Basic.Complex.IService3)) return this.GetService<Jab.Performance.Basic.Complex.IService3>();
        if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton1)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
        if (type == typeof(Jab.Performance.Basic.Mixed.IMix1)) return this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
        if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton2)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
        if (type == typeof(Jab.Performance.Basic.Mixed.IMix2)) return this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
        if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton3)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
        if (type == typeof(Jab.Performance.Basic.Mixed.IMix3)) return this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
        if (type == typeof(Jab.Performance.Basic.Complex.IComplex1)) return this.GetService<Jab.Performance.Basic.Complex.IComplex1>();
        if (type == typeof(Jab.Performance.Basic.Complex.IComplex2)) return this.GetService<Jab.Performance.Basic.Complex.IComplex2>();
        if (type == typeof(Jab.Performance.Basic.Complex.IComplex3)) return this.GetService<Jab.Performance.Basic.Complex.IComplex3>();
        if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
        if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
        if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>();
        return null;
    }

    object? Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(global::System.Type type, object? key)
    {
        return null;
    }

    object Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetRequiredKeyedService(global::System.Type type, object? key) => ((Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider)this).GetKeyedService(type, key) ?? throw new Exception();

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

        TryDispose(_ISingleton1);
        TryDispose(_ISingleton2);
        TryDispose(_ISingleton3);
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

        await TryDispose(_ISingleton1);
        await TryDispose(_ISingleton2);
        await TryDispose(_ISingleton3);
        await TryDispose(_rootScope);
        if (_disposables != null)
        {
            foreach (var service in _disposables)
            {
                await TryDispose(service);
            }
        }
    }

    [DebuggerHidden]
    public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new NotImplementedException();

    [DebuggerHidden]
    public T GetService<T>(string name) => this is INamedServiceProvider<T> provider ? provider.GetService(name) : throw new NotImplementedException();

    public Scope CreateScope() => new Scope(this);

    Microsoft.Extensions.DependencyInjection.IServiceScope Microsoft.Extensions.DependencyInjection.IServiceScopeFactory.CreateScope() => this.CreateScope();

    bool Microsoft.Extensions.DependencyInjection.IServiceProviderIsService.IsService(Type service) =>
        typeof(Jab.Performance.Basic.Transient.ITransient1) == service ||
        typeof(Jab.Performance.Basic.Complex.IService1) == service ||
        typeof(Jab.Performance.Basic.Transient.ITransient2) == service ||
        typeof(Jab.Performance.Basic.Complex.IService2) == service ||
        typeof(Jab.Performance.Basic.Transient.ITransient3) == service ||
        typeof(Jab.Performance.Basic.Complex.IService3) == service ||
        typeof(Jab.Performance.Basic.Singleton.ISingleton1) == service ||
        typeof(Jab.Performance.Basic.Mixed.IMix1) == service ||
        typeof(Jab.Performance.Basic.Singleton.ISingleton2) == service ||
        typeof(Jab.Performance.Basic.Mixed.IMix2) == service ||
        typeof(Jab.Performance.Basic.Singleton.ISingleton3) == service ||
        typeof(Jab.Performance.Basic.Mixed.IMix3) == service ||
        typeof(Jab.Performance.Basic.Complex.IComplex1) == service ||
        typeof(Jab.Performance.Basic.Complex.IComplex2) == service ||
        typeof(Jab.Performance.Basic.Complex.IComplex3) == service ||
        typeof(System.IServiceProvider) == service ||
        typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory) == service ||
        typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService) == service;

    public partial class Scope : global::System.IDisposable,
       System.IAsyncDisposable,
       global::System.IServiceProvider,
       Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider,
       Microsoft.Extensions.DependencyInjection.IServiceScope,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService1>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService2>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService3>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>,
       IServiceProvider<System.IServiceProvider>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>
    {
        private Jab.Performance.Basic.Complex.Complex1? _IComplex1;
        private Jab.Performance.Basic.Complex.Complex2? _IComplex2;
        private Jab.Performance.Basic.Complex.Complex3? _IComplex3;

        private Jab.Performance.Basic.Complex.ContainerComplex _root;

        public Scope(Jab.Performance.Basic.Complex.ContainerComplex root)
        {
            _root = root;
        }

        [DebuggerHidden]
        public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new Exception();

        [DebuggerHidden]
        public T GetService<T>(string name) => this is INamedServiceProvider<T> provider ? provider.GetService(name) : throw new Exception();

        Jab.Performance.Basic.Transient.ITransient1 IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>.GetService()
        {
            Jab.Performance.Basic.Transient.Transient1 service = new Jab.Performance.Basic.Transient.Transient1();
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IService1 IServiceProvider<Jab.Performance.Basic.Complex.IService1>.GetService()
        {
            Jab.Performance.Basic.Complex.Service1 service = new Jab.Performance.Basic.Complex.Service1(this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Transient.ITransient2 IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>.GetService()
        {
            Jab.Performance.Basic.Transient.Transient2 service = new Jab.Performance.Basic.Transient.Transient2();
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IService2 IServiceProvider<Jab.Performance.Basic.Complex.IService2>.GetService()
        {
            Jab.Performance.Basic.Complex.Service2 service = new Jab.Performance.Basic.Complex.Service2(this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Transient.ITransient3 IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>.GetService()
        {
            Jab.Performance.Basic.Transient.Transient3 service = new Jab.Performance.Basic.Transient.Transient3();
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IService3 IServiceProvider<Jab.Performance.Basic.Complex.IService3>.GetService()
        {
            Jab.Performance.Basic.Complex.Service3 service = new Jab.Performance.Basic.Complex.Service3(this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Singleton.ISingleton1 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>.GetService()
        {
            return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
        }

        Jab.Performance.Basic.Mixed.IMix1 IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>.GetService()
        {
            Jab.Performance.Basic.Mixed.Mix1 service = new Jab.Performance.Basic.Mixed.Mix1(this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>(), this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Singleton.ISingleton2 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>.GetService()
        {
            return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
        }

        Jab.Performance.Basic.Mixed.IMix2 IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>.GetService()
        {
            Jab.Performance.Basic.Mixed.Mix2 service = new Jab.Performance.Basic.Mixed.Mix2(this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>(), this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Singleton.ISingleton3 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>.GetService()
        {
            return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
        }

        Jab.Performance.Basic.Mixed.IMix3 IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>.GetService()
        {
            Jab.Performance.Basic.Mixed.Mix3 service = new Jab.Performance.Basic.Mixed.Mix3(this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>(), this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IComplex1 IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>.GetService()
        {
            if (_IComplex1 == null)
                lock (this)
                    if (_IComplex1 == null)
                    {
                        _IComplex1 = new Jab.Performance.Basic.Complex.Complex1(this.GetService<Jab.Performance.Basic.Complex.IService1>(), this.GetService<Jab.Performance.Basic.Complex.IService2>(), this.GetService<Jab.Performance.Basic.Complex.IService3>(), this.GetService<Jab.Performance.Basic.Mixed.IMix1>(), this.GetService<Jab.Performance.Basic.Mixed.IMix2>(), this.GetService<Jab.Performance.Basic.Mixed.IMix3>(), this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>(), this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
                    }
            return _IComplex1;
        }

        Jab.Performance.Basic.Complex.IComplex2 IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>.GetService()
        {
            if (_IComplex2 == null)
                lock (this)
                    if (_IComplex2 == null)
                    {
                        _IComplex2 = new Jab.Performance.Basic.Complex.Complex2(this.GetService<Jab.Performance.Basic.Complex.IService1>(), this.GetService<Jab.Performance.Basic.Complex.IService2>(), this.GetService<Jab.Performance.Basic.Complex.IService3>(), this.GetService<Jab.Performance.Basic.Mixed.IMix1>(), this.GetService<Jab.Performance.Basic.Mixed.IMix2>(), this.GetService<Jab.Performance.Basic.Mixed.IMix3>(), this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>(), this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
                    }
            return _IComplex2;
        }

        Jab.Performance.Basic.Complex.IComplex3 IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>.GetService()
        {
            if (_IComplex3 == null)
                lock (this)
                    if (_IComplex3 == null)
                    {
                        _IComplex3 = new Jab.Performance.Basic.Complex.Complex3(this.GetService<Jab.Performance.Basic.Complex.IService1>(), this.GetService<Jab.Performance.Basic.Complex.IService2>(), this.GetService<Jab.Performance.Basic.Complex.IService3>(), this.GetService<Jab.Performance.Basic.Mixed.IMix1>(), this.GetService<Jab.Performance.Basic.Mixed.IMix2>(), this.GetService<Jab.Performance.Basic.Mixed.IMix3>(), this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>(), this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
                    }
            return _IComplex3;
        }

        System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService()
        {
            return this;
        }

        Microsoft.Extensions.DependencyInjection.IServiceScopeFactory IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>.GetService()
        {
            return _root;
        }

        Microsoft.Extensions.DependencyInjection.IServiceProviderIsService IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>.GetService()
        {
            return _root;
        }

        object? global::System.IServiceProvider.GetService(global::System.Type type)
        {
            if (type == typeof(Jab.Performance.Basic.Transient.ITransient1)) return this.GetService<Jab.Performance.Basic.Transient.ITransient1>();
            if (type == typeof(Jab.Performance.Basic.Complex.IService1)) return this.GetService<Jab.Performance.Basic.Complex.IService1>();
            if (type == typeof(Jab.Performance.Basic.Transient.ITransient2)) return this.GetService<Jab.Performance.Basic.Transient.ITransient2>();
            if (type == typeof(Jab.Performance.Basic.Complex.IService2)) return this.GetService<Jab.Performance.Basic.Complex.IService2>();
            if (type == typeof(Jab.Performance.Basic.Transient.ITransient3)) return this.GetService<Jab.Performance.Basic.Transient.ITransient3>();
            if (type == typeof(Jab.Performance.Basic.Complex.IService3)) return this.GetService<Jab.Performance.Basic.Complex.IService3>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton1)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
            if (type == typeof(Jab.Performance.Basic.Mixed.IMix1)) return this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton2)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
            if (type == typeof(Jab.Performance.Basic.Mixed.IMix2)) return this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton3)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
            if (type == typeof(Jab.Performance.Basic.Mixed.IMix3)) return this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
            if (type == typeof(Jab.Performance.Basic.Complex.IComplex1)) return this.GetService<Jab.Performance.Basic.Complex.IComplex1>();
            if (type == typeof(Jab.Performance.Basic.Complex.IComplex2)) return this.GetService<Jab.Performance.Basic.Complex.IComplex2>();
            if (type == typeof(Jab.Performance.Basic.Complex.IComplex3)) return this.GetService<Jab.Performance.Basic.Complex.IComplex3>();
            if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
            if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
            if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>();
            return null;
        }

        object? Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(global::System.Type type, object? key)
        {
            return null;
        }

        object Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetRequiredKeyedService(global::System.Type type, object? key) => ((Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider)this).GetKeyedService(type, key) ?? throw new Exception();

        System.IServiceProvider Microsoft.Extensions.DependencyInjection.IServiceScope.ServiceProvider => this;

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

            TryDispose(_IComplex1);
            TryDispose(_IComplex2);
            TryDispose(_IComplex3);
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

            await TryDispose(_IComplex1);
            await TryDispose(_IComplex2);
            await TryDispose(_IComplex3);
            if (_disposables != null)
            {
                foreach (var service in _disposables)
                {
                    await TryDispose(service);
                }
            }
        }

    }
    private Scope GetRootScope()
    {
        if (_rootScope == default)
            lock (this)
                if (_rootScope == default)
                {
                    _rootScope = CreateScope();
                }
        return _rootScope;
    }
}

internal partial class ImprovedContainerComplex : global::System.IDisposable,
       System.IAsyncDisposable,
       global::System.IServiceProvider,
       Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider,
       Microsoft.Extensions.DependencyInjection.IServiceScopeFactory,
       Microsoft.Extensions.DependencyInjection.IServiceProviderIsService,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService1>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService2>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService3>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>,
       IServiceProvider<System.IServiceProvider>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>
{
    private Scope? _rootScope;
    private Jab.Performance.Basic.Singleton.Singleton1? _ISingleton1;
    private Jab.Performance.Basic.Singleton.Singleton2? _ISingleton2;
    private Jab.Performance.Basic.Singleton.Singleton3? _ISingleton3;

    Jab.Performance.Basic.Transient.ITransient1 IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>.GetService()
    {
        Jab.Performance.Basic.Transient.Transient1 service = new Jab.Performance.Basic.Transient.Transient1();
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IService1 IServiceProvider<Jab.Performance.Basic.Complex.IService1>.GetService()
    {
        Jab.Performance.Basic.Complex.Service1 service = new Jab.Performance.Basic.Complex.Service1(this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Transient.ITransient2 IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>.GetService()
    {
        Jab.Performance.Basic.Transient.Transient2 service = new Jab.Performance.Basic.Transient.Transient2();
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IService2 IServiceProvider<Jab.Performance.Basic.Complex.IService2>.GetService()
    {
        Jab.Performance.Basic.Complex.Service2 service = new Jab.Performance.Basic.Complex.Service2(this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Transient.ITransient3 IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>.GetService()
    {
        Jab.Performance.Basic.Transient.Transient3 service = new Jab.Performance.Basic.Transient.Transient3();
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IService3 IServiceProvider<Jab.Performance.Basic.Complex.IService3>.GetService()
    {
        Jab.Performance.Basic.Complex.Service3 service = new Jab.Performance.Basic.Complex.Service3(this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Singleton.ISingleton1 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>.GetService()
    {
        return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Singleton.Singleton1>(ref this._ISingleton1);
    }

    Jab.Performance.Basic.Mixed.IMix1 IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>.GetService()
    {
        Jab.Performance.Basic.Mixed.Mix1 service = new Jab.Performance.Basic.Mixed.Mix1(this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>(), this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Singleton.ISingleton2 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>.GetService()
    {
        return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Singleton.Singleton2>(ref this._ISingleton2);
    }

    Jab.Performance.Basic.Mixed.IMix2 IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>.GetService()
    {
        Jab.Performance.Basic.Mixed.Mix2 service = new Jab.Performance.Basic.Mixed.Mix2(this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>(), this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Singleton.ISingleton3 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>.GetService()
    {
        return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Singleton.Singleton3>(ref this._ISingleton3);
    }

    Jab.Performance.Basic.Mixed.IMix3 IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>.GetService()
    {
        Jab.Performance.Basic.Mixed.Mix3 service = new Jab.Performance.Basic.Mixed.Mix3(this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>(), this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
        TryAddDisposable(service);
        return service;
    }

    Jab.Performance.Basic.Complex.IComplex1 IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>.GetService() => GetRootScope().GetService<Jab.Performance.Basic.Complex.IComplex1>();

    Jab.Performance.Basic.Complex.IComplex2 IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>.GetService() => GetRootScope().GetService<Jab.Performance.Basic.Complex.IComplex2>();

    Jab.Performance.Basic.Complex.IComplex3 IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>.GetService() => GetRootScope().GetService<Jab.Performance.Basic.Complex.IComplex3>();

    System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService() => this;

    Microsoft.Extensions.DependencyInjection.IServiceScopeFactory IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>.GetService() => this;

    Microsoft.Extensions.DependencyInjection.IServiceProviderIsService IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>.GetService() => this;

    object? global::System.IServiceProvider.GetService(global::System.Type type)
    {
        if (type == typeof(Jab.Performance.Basic.Transient.ITransient1)) return this.GetService<Jab.Performance.Basic.Transient.ITransient1>();
        if (type == typeof(Jab.Performance.Basic.Complex.IService1)) return this.GetService<Jab.Performance.Basic.Complex.IService1>();
        if (type == typeof(Jab.Performance.Basic.Transient.ITransient2)) return this.GetService<Jab.Performance.Basic.Transient.ITransient2>();
        if (type == typeof(Jab.Performance.Basic.Complex.IService2)) return this.GetService<Jab.Performance.Basic.Complex.IService2>();
        if (type == typeof(Jab.Performance.Basic.Transient.ITransient3)) return this.GetService<Jab.Performance.Basic.Transient.ITransient3>();
        if (type == typeof(Jab.Performance.Basic.Complex.IService3)) return this.GetService<Jab.Performance.Basic.Complex.IService3>();
        if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton1)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
        if (type == typeof(Jab.Performance.Basic.Mixed.IMix1)) return this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
        if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton2)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
        if (type == typeof(Jab.Performance.Basic.Mixed.IMix2)) return this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
        if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton3)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
        if (type == typeof(Jab.Performance.Basic.Mixed.IMix3)) return this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
        if (type == typeof(Jab.Performance.Basic.Complex.IComplex1)) return this.GetService<Jab.Performance.Basic.Complex.IComplex1>();
        if (type == typeof(Jab.Performance.Basic.Complex.IComplex2)) return this.GetService<Jab.Performance.Basic.Complex.IComplex2>();
        if (type == typeof(Jab.Performance.Basic.Complex.IComplex3)) return this.GetService<Jab.Performance.Basic.Complex.IComplex3>();
        if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
        if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
        if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>();
        return null;
    }

    object? Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(global::System.Type type, object? key) => null;

    object Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetRequiredKeyedService(global::System.Type type, object? key) => ((Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider)this).GetKeyedService(type, key) ?? throw new Exception();

    private global::System.Collections.Generic.List<object>? _disposables;

    private void TryAddDisposable(object? value)
    {
        if (value is not global::System.IDisposable and not System.IAsyncDisposable)
            return;

        lock (this)
        {
            (_disposables ??= new global::System.Collections.Generic.List<object>()).Add(value);
        }
    }

    public void Dispose()
    {
        void TryDispose(object? value) => (value as IDisposable)?.Dispose();

        TryDispose(_ISingleton1);
        TryDispose(_ISingleton2);
        TryDispose(_ISingleton3);
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

        await TryDispose(_ISingleton1);
        await TryDispose(_ISingleton2);
        await TryDispose(_ISingleton3);
        await TryDispose(_rootScope);
        if (_disposables != null)
        {
            foreach (var service in _disposables)
            {
                await TryDispose(service);
            }
        }
    }

    [DebuggerHidden]
    public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new NotImplementedException();

    [DebuggerHidden]
    public T GetService<T>(string name) => this is INamedServiceProvider<T> provider ? provider.GetService(name) : throw new NotImplementedException();

    public Scope CreateScope() => new Scope(this);

    Microsoft.Extensions.DependencyInjection.IServiceScope Microsoft.Extensions.DependencyInjection.IServiceScopeFactory.CreateScope() => this.CreateScope();

    bool Microsoft.Extensions.DependencyInjection.IServiceProviderIsService.IsService(Type service) =>
        typeof(Jab.Performance.Basic.Transient.ITransient1) == service ||
        typeof(Jab.Performance.Basic.Complex.IService1) == service ||
        typeof(Jab.Performance.Basic.Transient.ITransient2) == service ||
        typeof(Jab.Performance.Basic.Complex.IService2) == service ||
        typeof(Jab.Performance.Basic.Transient.ITransient3) == service ||
        typeof(Jab.Performance.Basic.Complex.IService3) == service ||
        typeof(Jab.Performance.Basic.Singleton.ISingleton1) == service ||
        typeof(Jab.Performance.Basic.Mixed.IMix1) == service ||
        typeof(Jab.Performance.Basic.Singleton.ISingleton2) == service ||
        typeof(Jab.Performance.Basic.Mixed.IMix2) == service ||
        typeof(Jab.Performance.Basic.Singleton.ISingleton3) == service ||
        typeof(Jab.Performance.Basic.Mixed.IMix3) == service ||
        typeof(Jab.Performance.Basic.Complex.IComplex1) == service ||
        typeof(Jab.Performance.Basic.Complex.IComplex2) == service ||
        typeof(Jab.Performance.Basic.Complex.IComplex3) == service ||
        typeof(System.IServiceProvider) == service ||
        typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory) == service ||
        typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService) == service;

    public partial class Scope : global::System.IDisposable,
       System.IAsyncDisposable,
       global::System.IServiceProvider,
       Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider,
       Microsoft.Extensions.DependencyInjection.IServiceScope,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService1>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService2>,
       IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IService3>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>,
       IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>,
       IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>,
       IServiceProvider<System.IServiceProvider>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>
    {
        private Jab.Performance.Basic.Complex.Complex1? _IComplex1;
        private Jab.Performance.Basic.Complex.Complex2? _IComplex2;
        private Jab.Performance.Basic.Complex.Complex3? _IComplex3;

        private readonly Jab.Performance.Basic.Complex.ImprovedContainerComplex _root;

        public Scope(Jab.Performance.Basic.Complex.ImprovedContainerComplex root)
        {
            _root = root;
        }

        [DebuggerHidden]
        public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new Exception();

        [DebuggerHidden]
        public T GetService<T>(string name) => this is INamedServiceProvider<T> provider ? provider.GetService(name) : throw new Exception();

        Jab.Performance.Basic.Transient.ITransient1 IServiceProvider<Jab.Performance.Basic.Transient.ITransient1>.GetService()
        {
            Jab.Performance.Basic.Transient.Transient1 service = new Jab.Performance.Basic.Transient.Transient1();
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IService1 IServiceProvider<Jab.Performance.Basic.Complex.IService1>.GetService()
        {
            Jab.Performance.Basic.Complex.Service1 service = new Jab.Performance.Basic.Complex.Service1(this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Transient.ITransient2 IServiceProvider<Jab.Performance.Basic.Transient.ITransient2>.GetService()
        {
            Jab.Performance.Basic.Transient.Transient2 service = new Jab.Performance.Basic.Transient.Transient2();
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IService2 IServiceProvider<Jab.Performance.Basic.Complex.IService2>.GetService()
        {
            Jab.Performance.Basic.Complex.Service2 service = new Jab.Performance.Basic.Complex.Service2(this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Transient.ITransient3 IServiceProvider<Jab.Performance.Basic.Transient.ITransient3>.GetService()
        {
            Jab.Performance.Basic.Transient.Transient3 service = new Jab.Performance.Basic.Transient.Transient3();
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IService3 IServiceProvider<Jab.Performance.Basic.Complex.IService3>.GetService()
        {
            Jab.Performance.Basic.Complex.Service3 service = new Jab.Performance.Basic.Complex.Service3(this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Singleton.ISingleton1 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>.GetService()
        {
            return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
        }

        Jab.Performance.Basic.Mixed.IMix1 IServiceProvider<Jab.Performance.Basic.Mixed.IMix1>.GetService()
        {
            Jab.Performance.Basic.Mixed.Mix1 service = new Jab.Performance.Basic.Mixed.Mix1(this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>(), this.GetService<Jab.Performance.Basic.Transient.ITransient1>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Singleton.ISingleton2 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>.GetService()
        {
            return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
        }

        Jab.Performance.Basic.Mixed.IMix2 IServiceProvider<Jab.Performance.Basic.Mixed.IMix2>.GetService()
        {
            Jab.Performance.Basic.Mixed.Mix2 service = new Jab.Performance.Basic.Mixed.Mix2(this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>(), this.GetService<Jab.Performance.Basic.Transient.ITransient2>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Singleton.ISingleton3 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>.GetService()
        {
            return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
        }

        Jab.Performance.Basic.Mixed.IMix3 IServiceProvider<Jab.Performance.Basic.Mixed.IMix3>.GetService()
        {
            Jab.Performance.Basic.Mixed.Mix3 service = new Jab.Performance.Basic.Mixed.Mix3(this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>(), this.GetService<Jab.Performance.Basic.Transient.ITransient3>());
            TryAddDisposable(service);
            return service;
        }

        Jab.Performance.Basic.Complex.IComplex1 IServiceProvider<Jab.Performance.Basic.Complex.IComplex1>.GetService()
        {
            //new Jab.Performance.Basic.Complex.Complex1(this.GetService<Jab.Performance.Basic.Complex.IService1>(), this.GetService<Jab.Performance.Basic.Complex.IService2>(), this.GetService<Jab.Performance.Basic.Complex.IService3>(), this.GetService<Jab.Performance.Basic.Mixed.IMix1>(), this.GetService<Jab.Performance.Basic.Mixed.IMix2>(), this.GetService<Jab.Performance.Basic.Mixed.IMix3>(), this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>(), this.GetService<Jab.Performance.Basic.Transient.ITransient1>());

            //return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Complex.Complex1>(ref this._IComplex1, Factory);

            //Complex1 Factory()
            //{

            //    IService1 service1 = this.GetService<Jab.Performance.Basic.Complex.IService1>();
            //    IService2 service2 = this.GetService<Jab.Performance.Basic.Complex.IService2>();
            //    IService3 service3 = this.GetService<Jab.Performance.Basic.Complex.IService3>();
            //    IMix1 mix1 = this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
            //    IMix2 mix2 = this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
            //    IMix3 mix3 = this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
            //    ISingleton1 singleton1 = this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
            //    ITransient1 transient1 = this.GetService<Jab.Performance.Basic.Transient.ITransient1>();
            //    var c = new Jab.Performance.Basic.Complex.Complex1(service1, service2, service3, mix1, mix2, mix3, singleton1, transient1);
            //    return c;
            //}

            IService1 service1 = this.GetService<Jab.Performance.Basic.Complex.IService1>();
            IService2 service2 = this.GetService<Jab.Performance.Basic.Complex.IService2>();
            IService3 service3 = this.GetService<Jab.Performance.Basic.Complex.IService3>();
            IMix1 mix1 = this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
            IMix2 mix2 = this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
            IMix3 mix3 = this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
            ISingleton1 singleton1 = this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
            ITransient1 transient1 = this.GetService<Jab.Performance.Basic.Transient.ITransient1>();

            return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Complex.Complex1>(ref this._IComplex1, Factory);

            Complex1 Factory()
            {
                var c = new Jab.Performance.Basic.Complex.Complex1(service1, service2, service3, mix1, mix2, mix3, singleton1, transient1);
                return c;
            }

        }

        Jab.Performance.Basic.Complex.IComplex2 IServiceProvider<Jab.Performance.Basic.Complex.IComplex2>.GetService()
        {
            //return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Complex.Complex2>(ref this._IComplex2, () => new Jab.Performance.Basic.Complex.Complex2(this.GetService<Jab.Performance.Basic.Complex.IService1>(), this.GetService<Jab.Performance.Basic.Complex.IService2>(), this.GetService<Jab.Performance.Basic.Complex.IService3>(), this.GetService<Jab.Performance.Basic.Mixed.IMix1>(), this.GetService<Jab.Performance.Basic.Mixed.IMix2>(), this.GetService<Jab.Performance.Basic.Mixed.IMix3>(), this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>(), this.GetService<Jab.Performance.Basic.Transient.ITransient2>()));

            IService1 service1 = this.GetService<Jab.Performance.Basic.Complex.IService1>();
            IService2 service2 = this.GetService<Jab.Performance.Basic.Complex.IService2>();
            IService3 service3 = this.GetService<Jab.Performance.Basic.Complex.IService3>();
            IMix1 mix1 = this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
            IMix2 mix2 = this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
            IMix3 mix3 = this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
            ISingleton2 singleton2 = this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
            ITransient2 transient2 = this.GetService<Jab.Performance.Basic.Transient.ITransient2>();

            return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Complex.Complex2>(ref this._IComplex2, () => new Jab.Performance.Basic.Complex.Complex2(service1, service2, service3, mix1, mix2, mix3, singleton2, transient2));
        }

        Jab.Performance.Basic.Complex.IComplex3 IServiceProvider<Jab.Performance.Basic.Complex.IComplex3>.GetService()
        {
            //return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Complex.Complex3>(ref this._IComplex3, () => new Jab.Performance.Basic.Complex.Complex3(this.GetService<Jab.Performance.Basic.Complex.IService1>(), this.GetService<Jab.Performance.Basic.Complex.IService2>(), this.GetService<Jab.Performance.Basic.Complex.IService3>(), this.GetService<Jab.Performance.Basic.Mixed.IMix1>(), this.GetService<Jab.Performance.Basic.Mixed.IMix2>(), this.GetService<Jab.Performance.Basic.Mixed.IMix3>(), this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>(), this.GetService<Jab.Performance.Basic.Transient.ITransient3>()));

            IService1 service1 = this.GetService<Jab.Performance.Basic.Complex.IService1>();
            IService2 service2 = this.GetService<Jab.Performance.Basic.Complex.IService2>();
            IService3 service3 = this.GetService<Jab.Performance.Basic.Complex.IService3>();
            IMix1 mix1 = this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
            IMix2 mix2 = this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
            IMix3 mix3 = this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
            ISingleton3 singleton3 = this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
            ITransient3 transient3 = this.GetService<Jab.Performance.Basic.Transient.ITransient3>();

            return LazyInitializer.EnsureInitialized<Jab.Performance.Basic.Complex.Complex3>(ref this._IComplex3, () => new Jab.Performance.Basic.Complex.Complex3(service1, service2, service3, mix1, mix2, mix3, singleton3, transient3));
        }

        System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService() => this;

        Microsoft.Extensions.DependencyInjection.IServiceScopeFactory IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>.GetService() => _root;

        Microsoft.Extensions.DependencyInjection.IServiceProviderIsService IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>.GetService() => _root;

        object? global::System.IServiceProvider.GetService(global::System.Type type)
        {
            if (type == typeof(Jab.Performance.Basic.Transient.ITransient1)) return this.GetService<Jab.Performance.Basic.Transient.ITransient1>();
            if (type == typeof(Jab.Performance.Basic.Complex.IService1)) return this.GetService<Jab.Performance.Basic.Complex.IService1>();
            if (type == typeof(Jab.Performance.Basic.Transient.ITransient2)) return this.GetService<Jab.Performance.Basic.Transient.ITransient2>();
            if (type == typeof(Jab.Performance.Basic.Complex.IService2)) return this.GetService<Jab.Performance.Basic.Complex.IService2>();
            if (type == typeof(Jab.Performance.Basic.Transient.ITransient3)) return this.GetService<Jab.Performance.Basic.Transient.ITransient3>();
            if (type == typeof(Jab.Performance.Basic.Complex.IService3)) return this.GetService<Jab.Performance.Basic.Complex.IService3>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton1)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
            if (type == typeof(Jab.Performance.Basic.Mixed.IMix1)) return this.GetService<Jab.Performance.Basic.Mixed.IMix1>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton2)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
            if (type == typeof(Jab.Performance.Basic.Mixed.IMix2)) return this.GetService<Jab.Performance.Basic.Mixed.IMix2>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton3)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
            if (type == typeof(Jab.Performance.Basic.Mixed.IMix3)) return this.GetService<Jab.Performance.Basic.Mixed.IMix3>();
            if (type == typeof(Jab.Performance.Basic.Complex.IComplex1)) return this.GetService<Jab.Performance.Basic.Complex.IComplex1>();
            if (type == typeof(Jab.Performance.Basic.Complex.IComplex2)) return this.GetService<Jab.Performance.Basic.Complex.IComplex2>();
            if (type == typeof(Jab.Performance.Basic.Complex.IComplex3)) return this.GetService<Jab.Performance.Basic.Complex.IComplex3>();
            if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
            if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
            if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>();
            return null;
        }

        object? Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(global::System.Type type, object? key) => null;

        object Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetRequiredKeyedService(global::System.Type type, object? key) => ((Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider)this).GetKeyedService(type, key) ?? throw new Exception();

        System.IServiceProvider Microsoft.Extensions.DependencyInjection.IServiceScope.ServiceProvider => this;

        private global::System.Collections.Generic.List<object>? _disposables;

        private void TryAddDisposable(object? value)
        {
            if (value is not global::System.IDisposable and not System.IAsyncDisposable)
                return;

            lock (this)
            {
                (_disposables ??= new global::System.Collections.Generic.List<object>()).Add(value);
            }
        }

        public void Dispose()
        {
            void TryDispose(object? value) => (value as IDisposable)?.Dispose();

            TryDispose(_IComplex1);
            TryDispose(_IComplex2);
            TryDispose(_IComplex3);
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

            await TryDispose(_IComplex1);
            await TryDispose(_IComplex2);
            await TryDispose(_IComplex3);
            if (_disposables != null)
            {
                foreach (var service in _disposables)
                {
                    await TryDispose(service);
                }
            }
        }

    }
    private Scope GetRootScope() => LazyInitializer.EnsureInitialized<Scope>(ref this._rootScope, CreateScope);
}