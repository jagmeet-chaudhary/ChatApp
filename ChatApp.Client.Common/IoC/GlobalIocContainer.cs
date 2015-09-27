using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class GlobalIocContainer
    {
        private static object _lockObject = new object();
        IocContainer _container;
        private static volatile GlobalIocContainer _globalContainer;
        private GlobalIocContainer()
        {
            _container = new IocContainer();
            InitializeContainer();
        }

        private void InitializeContainer()
        {
            _container.RegisterType<IReceiver, ChatAppConsole>();
            _container.RegisterType<CommandFactory, ChatCommandFactory>();
        }
        public IocContainer IocContainer { get { return _container; } }
        public static GlobalIocContainer Container 
        {
            get
            {
                if(_globalContainer  == null)
                {
                    lock (_lockObject)
                    {
                        if (_globalContainer == null)
                        {
                            _globalContainer = new GlobalIocContainer();
                            
                        }
                    }
                }
                return _globalContainer;
            }
        }

    }
}
