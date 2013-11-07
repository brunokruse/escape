using UnityEngine;
using System.Collections;

public class HangerSelection : MonoBehaviour {
	
	int currentShip;
	
	public GameObject ship1;
	public GameObject ship2;
	public GameObject ship3;
	public GameObject selector;
	
	MissionControl missionControlScript;
	ScreenManager screenManagerScript;
	
	public AudioClip moveSelector;
	private bool m_isAxisInUse = false;
	// Use this for initialization
	void Start () {
		
		currentShip = 1;
		
		GameObject mC = GameObject.Find("MissionControl");
		missionControlScript = mC.GetComponent<MissionControl>();
		
		GameObject screenManager = GameObject.Find("ScreenManager");
		screenManagerScript = screenManager.GetComponent<ScreenManager>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (screenManagerScript.currentScreen == 1) {
			// ship selection
			if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxisRaw("Horizontal") > 0 && m_isAxisInUse == false)) {
	            
				if (currentShip < 3) {
					currentShip += 1;
					AudioSource.PlayClipAtPoint(moveSelector, transform.position);
				} else if (currentShip >= 3) {
					currentShip = 1;	
					AudioSource.PlayClipAtPoint(moveSelector, transform.position);
				}
				
				m_isAxisInUse = true;
				// update mission control
				PlayerPrefs.SetInt("SelectedShip", currentShip); // update selected ship
				missionControlScript.updateIcon();
					
	    	} else if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetAxisRaw("Horizontal") < 0  && m_isAxisInUse == false)) {
				
				if (currentShip > 1) {
					currentShip -= 1;	
					AudioSource.PlayClipAtPoint(moveSelector, transform.position);
				} else if (currentShip <= 1) {
					currentShip = 3;
					AudioSource.PlayClipAtPoint(moveSelector, transform.position);
				}
				
				m_isAxisInUse = true;
				// update mission control
				PlayerPrefs.SetInt("SelectedShip", currentShip); // update selected ship
				missionControlScript.updateIcon();
			}
			
			if (Input.GetAxisRaw("Horizontal") == 0)
				m_isAxisInUse = false;
			//m_isAxisInUse = false;
			
			// move the selector to the current ship
			if (currentShip == 1) { selector.transform.position = ship1.transform.position; }
			else if (currentShip == 2) { 
				
				selector.transform.position = ship2.transform.position;
				GameObject ship2Desc = GameObject.Find("Ship2Desc");
				GameObject ship2Name = GameObject.Find("Ship2Name");
				tk2dTextMesh s1 = ship2Desc.GetComponent<tk2dTextMesh>();
				tk2dTextMesh s2 = ship2Name.GetComponent<tk2dTextMesh>();
				s1.color = new Color(255, 255, 0, 255);
				s2.color = new Color(255, 255, 0, 255);	
					
			
			}
			else if (currentShip == 3) { selector.transform.position = ship3.transform.position; }
			
			
			if (currentShip != 2) {
				GameObject ship2Desc = GameObject.Find("Ship2Desc");
				GameObject ship2Name = GameObject.Find("Ship2Name");
				tk2dTextMesh s1 = ship2Desc.GetComponent<tk2dTextMesh>();
				tk2dTextMesh s2 = ship2Name.GetComponent<tk2dTextMesh>();
				s1.color = new Color(255, 255, 255, 255);
				s2.color = new Color(255, 255, 255, 255);	
			}
			
			
		}
		
	}
	
}
