/*
Github: https://github.com/NamPhuThuy
*/

using UnityEngine;

namespace NamPhuThuy
{
    public class DontDestroyOnLoadTag : MonoBehaviour
    {
        [SerializeField] 
        [Tooltip("Destroy duplicate instances if another object with this component already exists")]
        private bool destroyDuplicates = false;

        private void Awake()
        {
            if (destroyDuplicates)
            {
                // Check if another instance already exists
                DontDestroyOnLoadTag[] instances = Object.FindObjectsByType<DontDestroyOnLoadTag>(FindObjectsSortMode.None);
                if (instances.Length > 1)
                {
                    Debug.LogWarning($"Duplicate DontDestroyOnLoad object found: {gameObject.name}. Destroying this instance.");
                    Destroy(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}