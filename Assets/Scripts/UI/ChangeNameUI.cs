using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNameUI : MonoBehaviour {

    public GameObject m_ChangeNameUI;
    public InputField m_TextField;
    public Text m_NameOnSetting;
    public GameObject m_RenameHint;

    private void OnEnable()
    {
        m_NameOnSetting.text = AppManager.instance.GetUserName();
        m_RenameHint.SetActive(!RecordManager.HasRecord(GlobalKeys.m_RenamedKey));
    }

    public void StartChangeName()
    {
        m_TextField.text = AppManager.instance.GetUserName();
        m_ChangeNameUI.SetActive(true);
        RecordManager.Record(GlobalKeys.m_RenamedKey);
    }

    public void ConfirmChangeName()
    {
        m_ChangeNameUI.SetActive(false);
        string name = m_TextField.text;
        if (name == "") name = "Driver";
        AppManager.instance.SaveName(name);
        AppManager.instance.RenameGameSpark(name);
        m_NameOnSetting.text = AppManager.instance.GetUserName();
    }

    
}
