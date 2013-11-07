using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {
	
	GameObject cam; // our camera
	public int currentScreen;
	public bool isLive;
	int highestWave;
	// positions for the screens
	Vector3 pos0 = new Vector3(-514,0, -10);
	Vector3 pos1 = new Vector3(-410, 0, -10);
	Vector3 pos2 = new Vector3(-280, 0, -10);
	Vector3 pos3 = new Vector3(-150, 0, -10);
	Vector3 pos4 = new Vector3(0, 0, -10);
	Vector3 destination = Vector3.zero;
	
	// our screen empty game objects for managing
	GameObject titleScreen;
	GameObject hangerScreen;
	GameObject missionScreen;
	GameObject levelScreen;
	public GameObject gameOverPrefab;

	// sounds
	public AudioClip bgm;
	
	public AudioClip screenTransition;
	public AudioClip confirmSound;
	public AudioClip errorSound;
	
	public bool gameOver;
	
	// Use this for initialization
	void Start () {
	
		cam = GameObject.Find("tk2dCamera");
		currentScreen = 0;

		// init the screen gameobjects
		titleScreen = GameObject.Find("Title");
		hangerScreen = GameObject.Find("Hanger");
		missionScreen = GameObject.Find("MissionControl");
		levelScreen = GameObject.Find("Level");
		PlayerPrefs.SetInt("SelectedShip", 1);
		
		PlayerPrefs.SetInt ("CurrentWeapon", 1);
		
		// don't start level until we are ready
		gameOverPrefab.SetActive(false);
		levelScreen.SetActive(false);
		
		gameOver = false;
		isLive = false; // we haven't started playing yet!
		
		//bgm.loop = true;
		//bgm.Play();
		AudioSource.PlayClipAtPoint(bgm, transform.position, 0.5f);

	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(0);
		}
		// CONTROLLING THE SCREEN CAMERA
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick button 1")) && currentScreen != 3) {
            
			if (currentScreen == 0 || currentScreen == -1) {
				// advance a screen
				currentScreen += 1;

				AudioSource.PlayClipAtPoint(screenTransition, transform.position);
			
			} else if (currentScreen == 1) {
				
				
				// only if middle ship is selected
				int shipCheck = PlayerPrefs.GetInt("SelectedShip");
				//Debug.Log("shipcheck: " + shipCheck);
				if (shipCheck == 2) {
					currentScreen += 1;
					AudioSource.PlayClipAtPoint(screenTransition, transform.position);
				} else {
					AudioSource.PlayClipAtPoint(errorSound, transform.position);
				}
			
			} else if (currentScreen == 2) {
				
				// if we are on go!
				MissionControl mC = (MissionControl) missionScreen.GetComponent(typeof(MissionControl));
				//Debug.Log(mC.curRow); 
				if (mC.curRow == 5) {
					if (mC.curCol == 3) {
						AudioSource.PlayClipAtPoint(screenTransition, transform.position);
						currentScreen += 1;		
					}
				}
				
				

			}
			
			if (currentScreen > 3){
				currentScreen = 0;	
			}
			
    	} else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("joystick button 2"))&& currentScreen != 3) {
            currentScreen -= 1;
			
			if (currentScreen <= -1)
				currentScreen = -1;
			
			if (currentScreen == 0) {
				AudioSource.PlayClipAtPoint(screenTransition, transform.position);
			
			} else if (currentScreen == 1) {
				//currentScreen += 1;
				AudioSource.PlayClipAtPoint(screenTransition, transform.position);
			
			}
    	}
		
		if (currentScreen == -1) {
			cam.transform.position = Vector3.Lerp(cam.transform.position, pos0, 3.0f * Time.deltaTime);
		}
	
		// GAMESTATES
		if (currentScreen == 0) { // title
			cam.transform.position = Vector3.Lerp(cam.transform.position, pos1, 3.0f * Time.deltaTime);
		}
		
		if (currentScreen == 1) { // hanger
			cam.transform.position = Vector3.Lerp(cam.transform.position, pos2, 3.0f * Time.deltaTime);
		}
		
		if (currentScreen == 2) { // customize
			cam.transform.position = Vector3.Lerp(cam.transform.position, pos3, 3.0f * Time.deltaTime);
			
			GameObject demoPlayer = GameObject.FindGameObjectWithTag("shipIcon");
			if (demoPlayer)
				demoPlayer.tag = "Player";
			
			// disable game cause we are customizing yo!
			if (isLive) {
				gameOverPrefab.SetActive(false);
				levelScreen.SetActive(false);
				LevelLogic LL = (LevelLogic) levelScreen.GetComponent(typeof(LevelLogic));
				LL.disableTimer();
				isLive = false;
				
			}
			
			
			
		}
		
		if (currentScreen == 3) { // play
			
			if (cam.transform.position != pos4)
				cam.transform.position = Vector3.Lerp(cam.transform.position, pos4, 3.0f * Time.deltaTime);
			
			GameObject demoPlayer = GameObject.Find("ShipIcon");
			demoPlayer.tag = "shipIcon";
			LevelLogic LL = (LevelLogic) levelScreen.GetComponent(typeof(LevelLogic));				
			
			
			if (isLive && LL.gameOver == true) {
				gameOver = true;
				gameOverPrefab.SetActive(true);
			} else {
				gameOver = false;
				gameOverPrefab.SetActive(false);			
			}
			
			
			// trigger is live
			if (!isLive) {
				
				// enable the renderer
				levelScreen.SetActive(true);
				LL.reset();
				LL.enableTimer();				
				isLive = true;
				LL.currentWave = highestWave;
			}
						
			
			
		}
		
	}
	
	public void resetGame() {
			isLive = false;
			gameOver = false;
			if (cam.transform.position != pos4)
				cam.transform.position = Vector3.Lerp(cam.transform.position, pos4, 3.0f * Time.deltaTime);
			
			GameObject demoPlayer = GameObject.Find("ShipIcon");
			demoPlayer.tag = "shipIcon";
			LevelLogic LL = (LevelLogic) levelScreen.GetComponent(typeof(LevelLogic));				
			LL.gameOver = false;
			
			gameOver = false;
			gameOverPrefab.SetActive(false);			
			
		
			// enable the renderer
			levelScreen.SetActive(false);
			levelScreen.SetActive(true);
			
			highestWave =LL.currentWave;
		
			LL.reset();
			LL.enableTimer();
			LL.currentWave = highestWave;
			
			//LL.c
			
			isLive = true;
			
	}
	
	
}
