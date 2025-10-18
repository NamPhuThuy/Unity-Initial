using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NamPhuThuy.Common
{
    
    public class TweenHelper : MonoBehaviour
    {
        public static void PopupScaleSquence(Transform objTransform)
        {
            // Debug.Log($"TweenHelper: PopupScaleSquence - {objTransform.name}");
            objTransform.gameObject.SetActive(true);
            
            Vector3 originalScale = objTransform.localScale;
            objTransform.localScale = Vector3.zero;
                
            Sequence seq = DOTween.Sequence();
            seq.Append(objTransform.DOScale(originalScale * 1.2f, 0.3f));
            seq.Append(objTransform.DOScale(originalScale * .8f, 0.3f));
            seq.Append(objTransform.DOScale(originalScale, 0.3f));
        }
        
        public static void PopupScaleSquence(Transform objTransform, float targetScale)
        {
            // Debug.Log($"TweenHelper: PopupScaleSquence - {objTransform.name}");
            objTransform.gameObject.SetActive(true);
            
            Vector3 originalScale = Vector3.one * targetScale;
            objTransform.localScale = Vector3.zero;
                
            Sequence seq = DOTween.Sequence();
            seq.Append(objTransform.DOScale(originalScale * 1.2f, 0.3f));
            seq.Append(objTransform.DOScale(originalScale * .8f, 0.3f));
            seq.Append(objTransform.DOScale(originalScale, 0.3f));
        }
    }
}