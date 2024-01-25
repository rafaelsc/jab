using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Jab;
using static Jab.JabHelpers;

namespace Jab.Performance.Basic.Singleton
{
    internal partial class ImprovedContainerSingleton : global::System.IDisposable, System.IAsyncDisposable,
       global::System.IServiceProvider,
       Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider,
       Microsoft.Extensions.DependencyInjection.IServiceScopeFactory,
       Microsoft.Extensions.DependencyInjection.IServiceProviderIsService,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>,
       IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>,
       IServiceProvider<System.IServiceProvider>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>,
       IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>
    {
        private readonly Lazy<Scope> _rootScope;

        private Lazy<Jab.Performance.Basic.Singleton.Singleton1> _ISingleton1;
        private Lazy<Jab.Performance.Basic.Singleton.Singleton2> _ISingleton2;
        private Lazy<Jab.Performance.Basic.Singleton.Singleton3> _ISingleton3;

        public ImprovedContainerSingleton()
        {
            _rootScope = new(() => CreateScope());

            //Main Module - Singletons
            _ISingleton1 = new(() => new Jab.Performance.Basic.Singleton.Singleton1());
            _ISingleton2 = new(() => new Jab.Performance.Basic.Singleton.Singleton2());
            _ISingleton3 = new(() => new Jab.Performance.Basic.Singleton.Singleton3());
        }

        Jab.Performance.Basic.Singleton.ISingleton1 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>.GetService() => _ISingleton1.Value;
        Jab.Performance.Basic.Singleton.ISingleton2 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>.GetService() => _ISingleton2.Value;
        Jab.Performance.Basic.Singleton.ISingleton3 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>.GetService() => _ISingleton3.Value;

        System.IServiceProvider IServiceProvider<System.IServiceProvider>.GetService() => this;
        Microsoft.Extensions.DependencyInjection.IServiceScopeFactory IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>.GetService() => this;
        Microsoft.Extensions.DependencyInjection.IServiceProviderIsService IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>.GetService() => this;

        object? global::System.IServiceProvider.GetService(global::System.Type type)
        {
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton1)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton2)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
            if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton3)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
            if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
            if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
            if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>();
            return null;
        }

        object? Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(global::System.Type type, object? key)
        {
            return null;
        }

        object Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetRequiredKeyedService(global::System.Type type, object? key) => ((Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider)this).GetKeyedService(type, key) ?? throw CreateServiceNotFoundException(type, key?.ToString());

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
        public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw CreateServiceNotFoundException<T>();

        [DebuggerHidden]
        public T GetService<T>(string name) => this is INamedServiceProvider<T> provider ? provider.GetService(name) : throw CreateServiceNotFoundException<T>(name);

        public Scope CreateScope() => new Scope(this);

        Microsoft.Extensions.DependencyInjection.IServiceScope Microsoft.Extensions.DependencyInjection.IServiceScopeFactory.CreateScope() => this.CreateScope();

        bool Microsoft.Extensions.DependencyInjection.IServiceProviderIsService.IsService(Type service) =>
            typeof(Jab.Performance.Basic.Singleton.ISingleton1) == service ||
            typeof(Jab.Performance.Basic.Singleton.ISingleton2) == service ||
            typeof(Jab.Performance.Basic.Singleton.ISingleton3) == service ||
            typeof(System.IServiceProvider) == service ||
            typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory) == service ||
            typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService) == service;

        public partial class Scope : global::System.IDisposable,
           System.IAsyncDisposable,
           global::System.IServiceProvider,
           Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider,
           Microsoft.Extensions.DependencyInjection.IServiceScope,
           IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>,
           IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>,
           IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>,
           IServiceProvider<System.IServiceProvider>,
           IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>,
           IServiceProvider<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>
        {

            private Jab.Performance.Basic.Singleton.ImprovedContainerSingleton _root;

            public Scope(Jab.Performance.Basic.Singleton.ImprovedContainerSingleton root)
            {
                _root = root;
            }

            [DebuggerHidden]
            public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw CreateServiceNotFoundException<T>();

            [DebuggerHidden]
            public T GetService<T>(string name) => this is INamedServiceProvider<T> provider ? provider.GetService(name) : throw CreateServiceNotFoundException<T>(name);

            Jab.Performance.Basic.Singleton.ISingleton1 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton1>.GetService()
            {
                return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
            }

            Jab.Performance.Basic.Singleton.ISingleton2 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton2>.GetService()
            {
                return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
            }

            Jab.Performance.Basic.Singleton.ISingleton3 IServiceProvider<Jab.Performance.Basic.Singleton.ISingleton3>.GetService()
            {
                return _root.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
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
                if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton1)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton1>();
                if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton2)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton2>();
                if (type == typeof(Jab.Performance.Basic.Singleton.ISingleton3)) return this.GetService<Jab.Performance.Basic.Singleton.ISingleton3>();
                if (type == typeof(System.IServiceProvider)) return this.GetService<System.IServiceProvider>();
                if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceScopeFactory)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
                if (type == typeof(Microsoft.Extensions.DependencyInjection.IServiceProviderIsService)) return this.GetService<Microsoft.Extensions.DependencyInjection.IServiceProviderIsService>();
                return null;
            }

            object? Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetKeyedService(global::System.Type type, object? key)
            {
                return null;
            }

            object Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider.GetRequiredKeyedService(global::System.Type type, object? key) => ((Microsoft.Extensions.DependencyInjection.IKeyedServiceProvider)this).GetKeyedService(type, key) ?? throw CreateServiceNotFoundException(type, key?.ToString());

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
}
