using UnityEngine;

namespace Character
{
    public class VFXController : MonoBehaviour
    {
        public GameObject buffVFX,debuffVFX;
        private float timeCounter;

        private void Update()
        {
            if (buffVFX.activeInHierarchy)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= 1.2f)
                {
                    buffVFX.SetActive(false);
                    timeCounter = 0f;
                }
            }
            if (debuffVFX.activeInHierarchy)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= 1.2f)
                {
                    debuffVFX.SetActive(false);
                    timeCounter = 0f;
                }
            }
        }
    }
}
