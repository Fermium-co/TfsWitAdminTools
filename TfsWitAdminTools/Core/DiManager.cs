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

        public void Register<TContract, TService>(LifeCycle lifeCycle)
            where TContract : class
            where TService : TContract
        {
            switch (lifeCycle)
            {
                case LifeCycle.Transient:
                    _Container.Register(Component.For<TContract>().ImplementedBy<TService>().LifestyleTransient());
                    break;
                case LifeCycle.Singletone:
                    _Container.Register(Component.For<TContract>().ImplementedBy<TService>().LifestyleSingleton());
                    break;
                case LifeCycle.PerThread:
                    _Container.Register(Component.For<TContract>().ImplementedBy<TService>().LifestylePerThread());
                    break;
                default:
                    throw new NotSupportedException("Lifecycle");
            }

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
