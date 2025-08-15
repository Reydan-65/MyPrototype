using UnityEngine;

namespace CodeBase.GamePlay
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] protected float lifeTime = 2f;
        
        protected float timer;

        protected void Update()
        {
            timer += Time.deltaTime;
            
            if (timer > lifeTime)
                Destroy(gameObject);
        }
    }
}
