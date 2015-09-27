using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;

namespace TfsWitAdminTools.Core
{
    public enum LifeCycle
    {
        Transient, Singletone, PerThread
    }

    public class DiManager : IDisposable
    {
        private WindsorContainer _Container;

        public static readonly DiManager Current = new DiManager();

        public void Register<TContract, TService>(LifeCycle lifeCycle, Func<TContract> factoryMethod = null)
            where TContract : class
            where TService : TContract
        {
            ComponentRegistration<TContract> component = Component.For<TContract>().ImplementedBy<TService>();

            switch (lifeCycle)
            {
                case LifeCycle.Transient:
                    component = component.LifestyleTransient();
                    break;
                case LifeCycle.Singletone:
                    component = component.LifestyleSingleton();
                    break;
                case LifeCycle.PerThread:
                    component = component.LifestylePerThread();
                    break;
                default:
                    throw new NotSupportedException("Lifecycle");
            }

            if (factoryMethod != null)
            {
                component = component.UsingFactoryMethod(factoryMethod, managedExternally: true);
            }

            _Container.Register(component);

        }

        public void Register<TService>(TService @object)
            where TService : class
        {
            _Container.Register(Component.For<TService>().UsingFactoryMethod(() => @object));
        }

        public void Init()
        {
            _Container = new WindsorContainer();
        }

        public void Dispose()
        {
            _Container.Dispose();
        }

        public TService Resolve<TService>(object args = null)
        {
            if (args != null)
                return _Container.Resolve<TService>(args);
            else
                return _Container.Resolve<TService>();
        }
    }
}
