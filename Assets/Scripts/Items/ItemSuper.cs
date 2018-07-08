using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSuper : MonoBehaviour {

    public int m_SlotOnMesh;
    public int m_Level;

    protected const float itemEnlargeSize = 3.0f;

    public virtual void MoveToCallBack()
    {

    }

    public void StartAnim()
    {
        if (gameObject.GetComponent<MoveToDecSpeedWithoutRot>() == null)
            gameObject.AddComponent<MoveToDecSpeedWithoutRot>();
        if(GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;
        MoveToDecSpeedWithoutRot anim = gameObject.GetComponent<MoveToDecSpeedWithoutRot>();
		anim.enabled = true;
		anim.maxSpeed = 100;
        anim.minSpeed = 50;
        anim.to = transform.position;
        transform.position = transform.position + new Vector3(0, 100, 0);
        anim.from = transform.position;
        anim.callbackFct = MoveToCallBack;
        anim.resetAnim();
    }

    public virtual void Disable()
    {

    }

    protected void ScaleUp(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
