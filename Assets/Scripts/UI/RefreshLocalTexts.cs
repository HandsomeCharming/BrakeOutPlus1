using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshLocalTexts : MonoBehaviour {

    public LocalText[] localTexts;

    public void Refresh()
    {
        foreach(var text in localTexts)
        {
            text.Refresh();
        }
    }
}
