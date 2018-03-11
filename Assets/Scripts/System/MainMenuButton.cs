using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;

public enum MainMenuButtonType {
	Return = 0,
	Select,
	Store,
	Setting
}

public class MainMenuButton : MonoBehaviour {
	public MainMenuButtonType type;
	public Canvas mainMenu;
	bool paused;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startGame() {
		Time.timeScale = 1f;
		mainMenu.enabled = false;
		InputHandler.current.paused = false;
	}

	public void goSelect() {

	}

	public void goStore() {

	}

	public void goSetting() {

	}

	void OnMouseDown() {
		print(3);
	}

	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)) {

		}
		print(4);
	}
}
