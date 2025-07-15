using CodeBase.Infrastructure.DependencyInjection;
using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface ICoroutineRunner : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}