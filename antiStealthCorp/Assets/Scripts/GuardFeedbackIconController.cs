using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFeedbackIconController : MonoBehaviour
{
    public void ShowExclamationMark(Transform parent)
    {
        StartCoroutine(AnimateIcon(parent));
    }

    IEnumerator AnimateIcon(Transform parent)
    {
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.5f);

        transform.SetParent(parent);
        gameObject.SetActive(false);
    }
}