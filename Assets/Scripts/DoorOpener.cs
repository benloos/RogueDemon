using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private float tweenTime = 6f;
    private float tweenDistance = 4.5f;
    private float initialPosition;

    private bool isOpen = false;

    public bool isMineDoor = false;
    public GameObject doorBlocker;

    private void Awake()
    {
        initialPosition = transform.position.y;
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            LeanTween.cancel(gameObject);
            LeanTween.moveY(gameObject, initialPosition + tweenDistance, tweenTime).setEase(LeanTweenType.easeOutCubic);
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            LeanTween.cancel(gameObject);
            LeanTween.moveY(gameObject, initialPosition, tweenTime/5).setEase(LeanTweenType.easeOutBounce);
        }
    }

    public void Block()
    {
        if (isMineDoor)
        {
            Instantiate(doorBlocker, transform.position, transform.rotation * Quaternion.Euler(0, 90f, 0));
        }
    }
}
