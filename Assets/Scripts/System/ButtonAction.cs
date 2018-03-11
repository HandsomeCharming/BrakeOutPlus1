using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType {
	EnterMainMenu,
	EnterGame,
	EnterSelect,
	EnterStore
}

public class ButtonAction: MonoBehaviour {

	public ButtonType buttonType;

	public void OnclickButton() {
		switch (buttonType) {
		case ButtonType.EnterMainMenu:
			Application.LoadLevel("flatDesign");
			break;
		case ButtonType.EnterGame:
			break;
		case ButtonType.EnterSelect:
			Application.LoadLevel("select");
			break;
		case ButtonType.EnterStore:
			Application.LoadLevel("store");
			break;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
