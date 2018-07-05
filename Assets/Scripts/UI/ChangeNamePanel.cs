using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNamePanel : MonoBehaviour {
    
    public InputField m_TextField;

    public void ConfirmChangeName()
    {
        gameObject.SetActive(false);
        string name = m_TextField.text;
        if (name == "") name = "Driver";
        AppManager.instance.SaveName(name);
        AppManager.instance.RenameGameSpark(name);
        RecordManager.Record(GlobalKeys.m_RenamedKey);
    }


}
