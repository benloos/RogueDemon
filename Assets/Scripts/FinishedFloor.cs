using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedFloor : MonoBehaviour
{

    public bool FinalLevel = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EndSequence());
        }
    }

    public Image blackImage;
    IEnumerator EndSequence()
    {
        Time.timeScale = 0.25f;
        LeanTween.alpha(blackImage.rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeInOutCubic);
        yield return new WaitForSeconds(0.75f);
        if (FinalLevel)
        {
            GameManager.current.LoadMenuScene();
        } else
        {
            GameManager.current.LoadNextScene();
        }
    }
}
