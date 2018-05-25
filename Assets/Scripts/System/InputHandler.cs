using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public enum ControlSchemes
{
    SingleHand = 1,
    BothHand = 2,
    Gravity = 3
}

public class InputHandler : MonoBehaviour {

    public static InputHandler current;

    public ControlSchemes m_ControlScheme;
    
	public bool paused = false;
    public float m_Turn;
    public bool m_Accelerate;

    const string m_PlayerPrefControlScheme = "ControlScheme";
    bool flag = false;

    public InputHandler() {
        current = this;
    }

    public void beforeStart() {
        Time.timeScale = 0;
        paused = true;
    }

    public void pause() {
        Time.timeScale = 0;
        paused = true;
    }

    public void resume() {
        Time.timeScale = 1f;
        paused = false;
    }

    public void ResetControls()
    {
        m_Turn = 0;
        m_Accelerate = false;
    }

    /*
    bool swipeFlag = false;
    bool startPosFlag = false;
	float accTime;
    Vector2 startPos;
    Vector2 curPos;
    float xDistance;
    float yDistance;
	public PlayerPhysics physics;
	*/
    void Awake () {
        Input.simulateMouseWithTouches = true;
	}
	
    void Start()
    {
		//accTime = 0.0f;
        current = this;
        LoadControlScheme();
    }

    void LoadControlScheme()
    {
        if (PlayerPrefs.GetInt(m_PlayerPrefControlScheme) != 0)
        {
            m_ControlScheme = (ControlSchemes)PlayerPrefs.GetInt(m_PlayerPrefControlScheme);
        }
        else
        {
			m_ControlScheme = ControlSchemes.BothHand;
        }
    }

    public void SetControlScheme(ControlSchemes control)
    {
        if (Enum.IsDefined(typeof(ControlSchemes), control))
        {
            m_ControlScheme = control;
            PlayerPrefs.SetInt(m_PlayerPrefControlScheme, (int)control);
        }
    }
    
    public void LeftTurnPlayer(bool turn)
    {
        if (turn)
            m_Turn = m_Turn <= 0.0f ? -1.0f : m_Turn - 1.0f;
        else
            m_Turn = 0;
    }

    public void RightTurnPlayer(bool turn)
    {
        if (turn)
            m_Turn = m_Turn >= 0.0f ? 1.0f : m_Turn + 1.0f;
        else
            m_Turn = 0;
    }
    
    public void AccelerateButton(bool accelerate)
    {
        m_Accelerate = accelerate;
    }

    void AccelarePlayer(){
        flag = true;
        Player.current.Accelerate();
	}
	void RecoverPlayer(){
        Player.current.Recover();
	}
    /*
    void detectSwipe ()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began && startPosFlag == true)
        {
            startPos = Input.GetTouch(0).position;
            startPosFlag = false;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            startPosFlag = true;
        }
        curPos = Input.GetTouch(0).position;
        xDistance = Mathf.Abs(curPos.x - startPos.x);
        yDistance = Mathf.Abs(curPos.y - startPos.y);
        if (xDistance < yDistance)
        {
            if(curPos.y - startPos.y > 0)
            {
                swipeFlag = true;
            }
            else
            {
                swipeFlag = false;
            }
        }
    }
	*/

    void Update()
    {
        //Dev restart
        if (Input.GetKey(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (GameManager.current.state == GameManager.GameState.Start)
        {
            //Start game detect
            /*if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                GameManager.current.StartGame();
                resume();
            }
            else if (Input.touchCount == 1)
            {
				if(!IsPointerOverUIObject (Input.GetTouch (0).position))
				{
					//Button script here
					print("start game");
					GameManager.current.StartGame();
					resume();
				}
            }*/
        }
        else if(GameManager.current.state == GameManager.GameState.Dead)
        {
        }

        if (Player.current != null)
        {

        }

        
    }

    void FixedUpdate ()
    {
        if (Player.current != null)
        {
            if (GameManager.current.state == GameManager.GameState.Running)
            {
//#if UNITY_IOS || UNITY_ANDROID
                //Old input 1/5/2018
                /*if (Input.touchCount == 1)
                {
                    Touch touch = Input.touches[0];
                    if (touch.position.x < Screen.width / 4.0f)
                    {
                        Player.current.RotateLeft();
                    }
                    else if (touch.position.x > Screen.width * 3 / 4.0f)
                    {
                        Player.current.RotateRight();
                    }
                    else
                    {
                        AccelarePlayer();
                        flag = true;
                    }
                }

                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.touches[0];
                    Touch touch2 = Input.touches[1];
                    if ((touch1.position.x <= Screen.width / 4.0f && touch2.position.x >= Screen.width / 4.0f && touch2.position.x <= Screen.width * 3 / 4.0f) || (touch1.position.x >= Screen.width / 4.0f && touch1.position.x <= Screen.width * 3 / 4.0f && touch2.position.x <= Screen.width / 4.0f))
                    {
                        Player.current.RotateLeft();
                        AccelarePlayer();
                        flag = true;
                    }
                    else if ((touch1.position.x >= Screen.width * 3 / 4.0f && touch2.position.x >= Screen.width / 4.0f && touch2.position.x <= Screen.width * 3 / 4.0f) || (touch1.position.x >= Screen.width / 4.0f && touch1.position.x <= Screen.width * 3 / 4.0f && touch2.position.x >= Screen.width / 4.0f))
                    {
                        Player.current.RotateRight();
                        AccelarePlayer();
                        flag = true;
                    }
                }*/
//#else
                if (((Input.GetKey(KeyCode.Space)) || (Input.GetKey(KeyCode.W))) && Input.GetKey(KeyCode.A))
                {
                    Player.current.RotateLeft();
                    AccelarePlayer();
                }
                else if (((Input.GetKey(KeyCode.Space)) || (Input.GetKey(KeyCode.W))) && Input.GetKey(KeyCode.D))
                {
                    Player.current.RotateRight();
                    AccelarePlayer();
                }
                else if ((Input.GetKey(KeyCode.Space)) || (Input.GetKey(KeyCode.W)))
                {
                    //accTime = 0.0f;
                    AccelarePlayer();
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    Player.current.RotateLeft();
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Player.current.RotateRight();
                }
//#endif
               
                if(m_ControlScheme == ControlSchemes.Gravity)
                {
                    m_Turn = Input.acceleration.x;
                    if (Mathf.Abs(m_Turn) < 0.05f) m_Turn = 0;
                    if (Input.touchCount > 0)
                        AccelarePlayer();
                }

                if(m_Turn < 0)
                {
                    Player.current.RotateLeft(-m_Turn);
                }
                else if(m_Turn > 0)
                {
                    Player.current.RotateRight(m_Turn);
                }
                if (m_Accelerate)
                {
                    AccelarePlayer();
                }

                /*
                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.touches[0];
                    Touch touch2 = Input.touches[1];
                    if ((touch1.position.x <= Screen.width / 2.0f && touch2.position.x >= Screen.width / 2.0f) || (touch1.position.x >= Screen.width / 2.0f && touch2.position.x <= Screen.width / 2.0f))
                    {
                        AccelarePlayer();
                        flag = true;
                    }
                }
                */

                /*
                if (Input.touchCount >= 1) 
                {
                    detectSwipe ();

                    if (swipeFlag == true)
                        ;
                    {
                        //accTime = 0.0f;
                        //AccelarePlayer();
                        //flag = true;
                    }
                }
                if (accTime >= 0.5f) 
                {
                    flag = false;
                }
                */
                if (flag != true)
                {
                    RecoverPlayer();
                }
                flag = false;
            }
        }
		//accTime += Time.deltaTime;

        
        
    }

	private bool IsPointerOverUIObject(Vector2 position) {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = position;
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}
}
