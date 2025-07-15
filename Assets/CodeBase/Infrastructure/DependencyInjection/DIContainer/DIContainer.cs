using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

namespace CodeBase.Infrastructure.DependencyInjection
{
    public class DIContainer : IService, IDisposable
    {
        private readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        #region Registration

        public void RegisterSingle<TypeService>(TypeService implementation)
            where TypeService : class, IService
        {
            services.Add(typeof(TypeService), implementation);
        }

        public void RegisterSingle<TypeService>() 
            where TypeService : class, IService
        {
            services.Add(typeof(TypeService), (IService) CreateImplementation(typeof(TypeService)));
        }

        public void RegisterSingle<TypeService, TImplementation>()
            where TypeService : class, IService
            where TImplementation : class, IService
        {
            services.Add(typeof(TypeService), (IService) CreateImplementation(typeof(TImplementation)));
        }

        public void UnregisterSingle<TypeService>() 
            where TypeService : class, IService
        {
            services.Remove(typeof(TypeService));
        }

        private object GetService(Type type)
        {
            if (services.ContainsKey(type))
                return services[type];
            else
                return null;
        }

        private object CreateImplementation(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructors()[0];

            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();

            object[] parameters = new object[parameterInfos.Length];

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                parameters[i] = GetService(parameterInfos[i].ParameterType);

                if (parameters[i] == null)
                {
                    Debug.LogError("Зависимость для сервиса ещё не была создана!");
                }
            }

            return Activator.CreateInstance(type, parameters);
        }

        public TType CreateAndInject<TType>()
        {
           return (TType) CreateImplementation(typeof(TType));
        }

        #endregion

        #region MonoInject

        public object[] GetAllServicesToInject(MethodInfo methodInfo)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();

            object[] parametrs = new object[parameterInfos.Length];

            for (int i = 0;i < parameterInfos.Length;i++)
            {
                IService objectToInject;
                services.TryGetValue(parameterInfos[i].ParameterType, out objectToInject);

                if (objectToInject == null)
                {
                    Debug.Log("Зависимость для MonoBehaviour не была создана!");
                }

                parametrs[i] = objectToInject;
            }

            return parametrs;
        }

        public void InjectToMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            MethodInfo[] allMethods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            for (int i = 0; i < allMethods.Length; i++)
            {
                object[] attributes = allMethods[i].GetCustomAttributes(false);

                for (int j = 0; j < attributes.Length; j++)
                {
                    if (attributes[j] is InjectAttribute)
                    {
                        allMethods[i].Invoke(monoBehaviour, GetAllServicesToInject(allMethods[i]));
                    }
                }
            }
        }

        public void InjectToGameObject(GameObject gameObject)
        {
            MonoBehaviour[] monoBehaviours = gameObject.GetComponentsInChildren<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in monoBehaviours)
            {
                InjectToMonoBehaviour(behaviour);
            }
        }

        public void InjectToAllMonoBehaviour()
        {
            MonoBehaviour[] monoBehaviours = GameObject.FindObjectsByType<MonoBehaviour>(0);

            for (int i = 0; i < monoBehaviours.Length; i++)
            {
                InjectToMonoBehaviour(monoBehaviours[i]);
            }
        }

        #endregion

        #region Instantiate

        public GameObject Instantiate(GameObject gameObject)
        {
            GameObject newObject = GameObject.Instantiate(gameObject);

            InjectToGameObject( newObject );
            return newObject;
        }

        #endregion

        public void Dispose()
        {
            foreach (var service in services.Values.OfType<IDisposable>())
            {
                service.Dispose();
            }
            services.Clear();
        }
    }
}
