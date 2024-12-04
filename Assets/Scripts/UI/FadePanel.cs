using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FadePanel : MonoBehaviour
{
    private VisualElement BG;

    private void Awake()
    {
        BG = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Background");
    }
    public void FadeIn(float duration)
    {
        DOVirtual.Float(0, 1, duration, value =>
        {
            BG.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
    public void FadeOut(float duration)
    {
        DOVirtual.Float(1, 0, duration, value =>
        {
            BG.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
}
