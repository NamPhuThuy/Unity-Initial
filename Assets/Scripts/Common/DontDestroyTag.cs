using UnityEngine;

namespace NamPhuThuy.Common
{
    
    public class DontDestroyTag : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("Flags")] 
        [SerializeField] private bool enable = true;

        #endregion

        #region MonoBehaviour Callbacks

        void Start()
        {
            if (enable)
                DontDestroyOnLoad(gameObject);
        }
        
        #endregion
    }
}