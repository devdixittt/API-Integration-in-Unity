using DG.Tweening;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public GameObject popup;

    public void ShowPopup()
    {
        popup.transform.localScale = Vector3.zero;
        popup.SetActive(true);
        popup.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    public void HidePopup()
    {
        popup.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => popup.SetActive(false));
    }
}
