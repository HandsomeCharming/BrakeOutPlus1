using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinModel : MonoBehaviour {

    MeshRenderer m_Renderer;
    public GameObject m_RedCoin;

    const string RedMat = "Materials/Coin Red";
    const string YellowMat = "Materials/Coin";

    short isRed = -1; //-1 for not sure, 0 for red, 1 for yellow
    // Use this for initialization
    void Awake () {
        m_Renderer = GetComponent<MeshRenderer>();
        isRed = -1;
    }
	
    public void TurnRed()
    {
        if (isRed == 0) return;
        isRed = 0;
        m_RedCoin.SetActive(true);
        m_Renderer.enabled = false;
    }

    public void TurnYellow()
    {
        if (isRed == 1) return;
        isRed = 1;
        m_RedCoin.SetActive(false);
        m_Renderer.enabled = true;
    }

    public void touched()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void disabled()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void resetCoin()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
}
