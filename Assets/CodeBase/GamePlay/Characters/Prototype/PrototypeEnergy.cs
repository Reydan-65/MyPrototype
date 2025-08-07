using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeEnergy : PrototypeResource, IEnergy, IProgressLoadHandler
    {
        public void Consume(float amount) => ChangeValue(amount);
        public void Restore(float amount) => RestoreResource(amount);

        public void LoadProgress(PlayerProgress progress)
        {
            max = progress.PrototypeStats.Energy.Value;
            current = max;
            InvokeChangedEvent();
        }
    }
}