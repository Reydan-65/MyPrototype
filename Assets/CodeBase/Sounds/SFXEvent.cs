using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Sounds
{
    [System.Serializable]
    public class SFXEvent : UnityEvent<AudioClip, float, float, float> { }
}
