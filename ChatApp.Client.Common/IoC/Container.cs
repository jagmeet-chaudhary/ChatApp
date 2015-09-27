using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class IocContainer
    {
        private IUnityContainer _unityContainer = new UnityContainer();

        public IocContainer()
        {
            _unityContainer.RegisterInstance(this);
        }

        #region register
        public void RegisterInstance<T>(T instance)
        {
            _unityContainer.RegisterInstance(instance);
        }

        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            _unityContainer.RegisterType<TFrom, TTo>();
        }

        #endregion

        #region Generic resolve
        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public T Resolve<T>(string name, object value)
        {
            return _unityContainer.Resolve<T>(new ParameterOverride(name, value));
        }

        public T Resolve<T>(string name1, object value1, string name2, object value2)
        {
            return _unityContainer.Resolve<T>(new ParameterOverride(name1, value1), new ParameterOverride(name2, value2));
        }

        public T Resolve<T>(string name1, object value1, string name2, object value2, string name3, object value3)
        {
            return _unityContainer.Resolve<T>(new ParameterOverride(name1, value1), new ParameterOverride(name2, value2), new ParameterOverride(name3, value3));
        }
        public T Resolve<T>(IDictionary<string,object> parameters)
        {
            parameters.Select(x=>new ParameterOverride(x.Key,x.Value)).ToArray();
            return _unityContainer.Resolve<T>(parameters.Select(x => new ParameterOverride(x.Key, x.Value)).ToArray());
        }
        #endregion
    }
}
