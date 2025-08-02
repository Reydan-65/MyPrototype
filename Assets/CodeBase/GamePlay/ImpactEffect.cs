using UnityEngine;

namespace CodeBase.GamePlay
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2f;

        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;
            
            if (timer > lifeTime)
                Destroy(gameObject);
        }
    }
}
