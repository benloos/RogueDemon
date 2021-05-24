using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private float tweenTime = 6f;
    private float tweenDistance = 8f;
    private LeanTweenType easeType = LeanTweenType.easeOutCubic;

    private bool isOpen = false;
    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            LeanTween.moveY(gameObject, tweenDistance, tweenTime).setEase(easeType);
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            LeanTween.moveY(gameObject, -tweenDistance, tweenTime).setEase(easeType);
        }
    }
}
