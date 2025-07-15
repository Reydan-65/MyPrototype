using System;

namespace CodeBase.Infrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InjectAttribute : Attribute { }
}
