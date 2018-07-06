using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActivateButtonWithDelay : MonoBehaviour {
    public float enableTime = 1.0f;

    private void OnEnable()
    {
        GetComponent<Button>().enabled = false;
        EnableButton();
    }

    void EnableButton()
    {
        StartCoroutine(EnableButtonIEnu());
    }

    IEnumerator EnableButtonIEnu()
    {
        yield return new WaitForSecondsRealtime(enableTime);

        GetComponent<Button>().enabled = true;
    }
}
