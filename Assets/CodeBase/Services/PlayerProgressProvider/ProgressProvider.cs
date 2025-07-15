using CodeBase.Data;
using System;

namespace CodeBase.Infrastructure.Services.PlayerProgressProvider
{
    public class ProgressProvider : IProgressProvider, IDisposable
    {
        private PlayerProgress playerProgress;
        public PlayerProgress PlayerProgress
        {
            get => playerProgress ?? (playerProgress = PlayerProgress.GetDefaultProgress());
            set
            {
                if (value == null) return;

                if (playerProgress != null)
                    playerProgress.CopyFrom(value);
                else
                    playerProgress = value;
            }
        }

        public void Dispose()
        {
            PlayerProgress = null;
        }
    }
}