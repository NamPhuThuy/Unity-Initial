using UnityEngine;

namespace NamPhuThuy.Helpers
{
    
    public class DontDestroyThisGameObject : MonoBehaviour
    {
        #region Private Serializable Fields

        [Header("Flags")] 
        [SerializeField] private bool enable = true;

        #endregion

        #region MonoBehaviour Callbacks

        void Start()
        {
            if (enable)
                DontDestroyOnLoad(this.gameObject);
        }
        
        #endregion
    }
}