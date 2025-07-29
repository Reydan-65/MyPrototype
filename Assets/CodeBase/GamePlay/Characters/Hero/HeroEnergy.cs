using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroEnergy : HeroResource, IEnergy, IProgressLoadHandler
    {
        public void Consume(float amount) => ChangeValue(amount);
        public void Restore(float amount) => RestoreResource(amount);

        public void LoadProgress(PlayerProgress progress)
        {
            max = progress.HeroStats.MaxEnergy;
            current = max;
            InvokeChangedEvent();
        }
    }
}