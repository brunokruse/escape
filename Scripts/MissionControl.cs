using UnityEngine;
using System.Collections;
using System.Linq;

public class MissionControl : MonoBehaviour {
	
	int selectedShip;
	Vector3 iconPosition;
	
	public GameObject currentIcon1;
	public GameObject currentIcon2;
	public GameObject currentIcon3;
	
	GameObject playerObj;
	
	public Transform selectionSM;
	Transform selector;
	public Transform barPrefab;
	
	public int curCol = 1;
	public int curRow = 1;
	float curPosX;
	float curPosY;
	Vector3 selectorPos;
	
	// weapon slots manager
	// keep track of what's enabled disabled
	bool isLaser = false;
	bool isMelee = false;
	bool isTriple = false;
	bool isMortar = false;
	int weaponCount = 0;
	
	ScreenManager screenManagerScript;
	PlayerStats playerStats;
	// Use this for initialization
	
	// audio
	public AudioClip moveSelector;
	public AudioClip confirmSound;
	public AudioClip errorSound;
	public AudioClip resetSound;
	
	public AudioClip laserSound;
	public AudioClip meleeSound;
	public AudioClip tripleSound;
	
	private bool x_isAxisInUse = false;
	private bool y_isAxisInUse = false;

	void Start () {
		
		isTriple = true;
		curPosX = -167.0f;
		curPosY = 16.0f;
		selectorPos = new Vector3(curPosX, curPosY, 7.0f);
	
		selectedShip = PlayerPrefs.GetInt("SelectedShip");
		iconPosition = GameObject.Find("ShipIcon").transform.position;
		
		selector =  (Transform) Instantiate(selectionSM, selectorPos, transform.rotation);
		selector.transform.parent = transform;

		GameObject screenManager = GameObject.Find("ScreenManager");
		screenManagerScript = screenManager.GetComponent<ScreenManager>();
		
		playerStats = (PlayerStats) transform.GetComponent("PlayerStats");
		
		/*
		//currentIcon1 = GameObject.FindGameObjectWithTag("selectShip1");
		currentIcon1.transform.position = iconPosition;
		currentIcon1.transform.renderer.enabled = true;
		
		//currentIcon2 = GameObject.FindGameObjectWithTag("selectShip2");
		currentIcon2.transform.position = iconPosition;
		currentIcon2.transform.renderer.enabled = true;
		
		//currentIcon3 = GameObject.FindGameObjectWithTag("selectShip3");
		currentIcon3.transform.position = iconPosition;
		currentIcon3.transform.renderer.enabled = true;
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		if (screenManagerScript.currentScreen == 2) {
			
			
			
			// menu controls with arrows
			if ((Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetAxisRaw("Horizontal") < 0 && x_isAxisInUse == false)) && curCol != 1) {
				curCol -= 1;
				AudioSource.PlayClipAtPoint(moveSelector, transform.position);
				
				x_isAxisInUse = true;
			}
			
			if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxisRaw("Horizontal") > 0 && x_isAxisInUse == false)) {
				
				if (curRow == 5 && curCol == 3) {
					

				} else if (curRow == 1 && curCol != 4) {
						curCol += 1;
						AudioSource.PlayClipAtPoint(moveSelector, transform.position);
					
					} else if (curRow > 1 && curCol != 2) {
						curCol += 1;
						AudioSource.PlayClipAtPoint(moveSelector, transform.position);					
				}
				
				if (curRow == 5 && curCol == 2)
					curCol += 1;
		    	
				x_isAxisInUse = true;
				
				
				
				
				//if (curRow == 5)
				//	curRow = 4;
			}
			
			if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxisRaw("Vertical") < 0 && y_isAxisInUse == false)) {
				AudioSource.PlayClipAtPoint(moveSelector, transform.position);
				
				if (curRow != 5)
					curRow += 1;
				
				if (curRow >= 5) 
					curRow = 5;
				
				if (curCol >= 2)
					curCol = 2;
				
				y_isAxisInUse = true;
			}
	
			if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxisRaw("Vertical") > 0 && y_isAxisInUse == false)) {
				AudioSource.PlayClipAtPoint(moveSelector, transform.position);

				if (curRow != 1)
					curRow -= 1;
				
				//if (curRow == 5 || curRow == 4) {
				//		curRow = 2;	
				//}
				
				if (curRow != 1 && curCol >= 3) {
					curCol = 2;	
				}
				
				y_isAxisInUse = true;
			}
			
			if (Input.GetAxisRaw("Vertical") == 0)
				y_isAxisInUse = false;
			if (Input.GetAxisRaw("Horizontal") == 0)
				x_isAxisInUse = false;
			
			// add and remove keydown
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick button 1")) { // remove
				// draw stats text

				
				//Debug.Log("wC: " + weaponCount);
				
				// WEAPONS
				if (curRow == 1) {
					
					// triple shot
					if (curCol == 1) {

						isTriple = !isTriple;
						AudioSource.PlayClipAtPoint(tripleSound, transform.position);
						isLaser = false;
						isMelee = false;
						isMortar = false;
	
					}
					
					// melee
					if (curCol == 2) {
						
						isMelee = !isMelee;
						AudioSource.PlayClipAtPoint(meleeSound, transform.position);
						isLaser = false;
						isTriple = false;
						isMortar = false;

					}
									
					// laser
					if (curCol == 3) {
						isLaser = !isLaser;
						AudioSource.PlayClipAtPoint(laserSound, transform.position);

						isMelee = false;
						isTriple = false;
						isMortar = false;

					}
									
					// cannon
					if (curCol == 4) {
						
						/*
						isMortar = !isMortar;

						isLaser = false;
						isTriple = false;
						isMelee = false;
						*/

					}	
				}
				
				// FIRST COL of Stats ----------
				if (curCol == 1) {
					
					
					if (curRow == 2) {
						
						int curPHealth = PlayerPrefs.GetInt("PHealth");
						
						if (curPHealth < 4 && playerStats.totalPoints > 0) { // cap at 4
							playerStats.addHealth();
							GameObject startPos = GameObject.Find("PHealthPos");
							
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);

							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curPHealth);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						} else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
			
					}
					
					if (curRow == 3) {
						int curPShield = PlayerPrefs.GetInt("PShield");
						
						if (curPShield < 4 && playerStats.totalPoints > 0) { // cap 
							playerStats.addShield();
							
							GameObject startPos = GameObject.Find("PShieldPos");
							
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);

							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curPShield);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						}else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
					}
					
					if (curRow == 4) {
						
						int curPThrusters = PlayerPrefs.GetInt("PThrusters");
						
						if (curPThrusters < 4 && playerStats.totalPoints > 0) { // cap
							playerStats.addThrusters();
							
							GameObject startPos = GameObject.Find("PThrustersPos");
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);
							
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curPThrusters);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						}else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
						
					}
					
					if (curRow == 5) {
						
						int curPTurnRate = PlayerPrefs.GetInt("PTurnRate");
						
						if (curPTurnRate < 4 && playerStats.totalPoints > 0) {
							playerStats.addTurnRate();
							
							GameObject startPos = GameObject.Find("PTurnRatePos");
							
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curPTurnRate);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						}else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
						
					}
					
					//playerStats.setPlayerStats();
					
				}
				
				// SECOND COL of Stats ----------
				if (curCol == 2) {
					
				
					// reset stats
					if (curRow == 2) {
						
						int curW1Damage = PlayerPrefs.GetInt("W1Damage");
						
						if (curW1Damage < 4 && playerStats.totalPoints > 0) { // cap
							playerStats.addW1Damage();
							
							GameObject startPos = GameObject.Find("W1DamagePos");
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);
							
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curW1Damage);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);	
						}else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
					}

					if (curRow == 3) {
						
						int curW1FireRate = PlayerPrefs.GetInt("W1FireRate");
						
						if (curW1FireRate < 4 && playerStats.totalPoints > 0) {
							playerStats.addW1FireRate();
							
							GameObject startPos = GameObject.Find("W1FireRatePos");
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);
							
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curW1FireRate);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						}else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
					}
					
					if (curRow == 4) {
						int curW1Range = PlayerPrefs.GetInt("W1Range");
						
						if (curW1Range < 4 && playerStats.totalPoints > 0) {
							playerStats.addW1Range();
							GameObject startPos = GameObject.Find("W1RangePos");						
							AudioSource.PlayClipAtPoint(confirmSound, transform.position);
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curW1Range);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);	
						}else {
							AudioSource.PlayClipAtPoint(errorSound, transform.position);
						}
					}
					
					
					if (curRow == 5) {
						playerStats.resetStats();
						AudioSource.PlayClipAtPoint(resetSound, transform.position);

					    GameObject[] bSpawns;
				        bSpawns = GameObject.FindGameObjectsWithTag("playerAttribute");
						foreach (GameObject bS in bSpawns) {
							Destroy(bS.gameObject);
						}
		
					}
					
				}
				
				if (curCol == 3) {
					
				
					// reset stats
					if (curRow == 2) {
						int curW2Damage = PlayerPrefs.GetInt("W2Damage");
						
						if (curW2Damage < 4 && playerStats.totalPoints > 0) {
							playerStats.addW2Damage();
							
							GameObject startPos = GameObject.Find("W2DamagePos");
													
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curW2Damage);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);	
						}
					}
				
					if (curRow == 3) {
						int curW2FireRate = PlayerPrefs.GetInt("W2FireRate");
						
						if (curW2FireRate < 4 && playerStats.totalPoints > 0) {
							playerStats.addW2FireRate();
							
							GameObject startPos = GameObject.Find("W2FireRatePos");
													
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curW2FireRate);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						}
					}				
				
					if (curRow == 4) {
						
						int curW2Range = PlayerPrefs.GetInt("W2Range");
						
						if (curW2Range < 4 && playerStats.totalPoints > 0) {
							playerStats.addW2Range();
							
							GameObject startPos = GameObject.Find("W2RangePos");
							
							Transform bar = barPrefab;
							bar.tag = "playerAttribute";
			
							// nudge it over
							float offset = startPos.transform.position.x + (2.0f * curW2Range);
							Vector3 newPos = new Vector3(offset, 
														 startPos.transform.position.y, 
														 startPos.transform.position.z);
							
							Transform p1 = (Transform) Instantiate(bar, newPos, Quaternion.identity);
						}
					}
				}
					
			}
			
			
			// double check current weapons
			
			
			// set the weapons  colors and stuff
			if (isLaser) {
				
				GameObject laserText = GameObject.Find("Weapon3Text");
				tk2dTextMesh lt = laserText.GetComponent<tk2dTextMesh>();
				lt.color = new Color(255, 0, 0, 255);
				weaponCount += 1;
					
				PlayerPrefs.SetInt("CurrentWeapon", 3);
	
			} else {
				
				GameObject laserText = GameObject.Find("Weapon3Text");
				tk2dTextMesh lt = laserText.GetComponent<tk2dTextMesh>();			
				lt.color = new Color(255, 255, 255, 255);
				weaponCount -= 1;
				
			}			
			
			if (isMelee) {
				
				GameObject meleeText = GameObject.Find("Weapon2Text");
				tk2dTextMesh mT = meleeText.GetComponent<tk2dTextMesh>();
			
				mT.color = new Color(255, 0, 0, 255);
				weaponCount += 1;
				
				PlayerPrefs.SetInt("CurrentWeapon", 2);
		
			} else {
				GameObject meleeText = GameObject.Find("Weapon2Text");
				tk2dTextMesh mT = meleeText.GetComponent<tk2dTextMesh>();
			
				mT.color = new Color(255, 255, 255, 255);
				weaponCount -= 1;
			}	
			
			if (isTriple) {
				
				GameObject tripleText = GameObject.Find("Weapon1Text");
				tk2dTextMesh tT = tripleText.GetComponent<tk2dTextMesh>();
				
				tT.color = new Color(255, 0, 0, 255);
				weaponCount += 1;
				
				PlayerPrefs.SetInt("CurrentWeapon", 1);
	
			} else {
				
				GameObject tripleText = GameObject.Find("Weapon1Text");
				tk2dTextMesh tT = tripleText.GetComponent<tk2dTextMesh>();
			
				tT.color = new Color(255, 255, 255, 255);					
				weaponCount -= 1;
				
			}
			
			if (isMortar) {
				/*
				GameObject mortarText = GameObject.Find("Weapon4Text");
				tk2dTextMesh mT = mortarText.GetComponent<tk2dTextMesh>();
				
				mT.color = new Color(255, 0, 0, 255);
				weaponCount += 1;
				
				PlayerPrefs.SetInt("CurrentWeapon", 4);
				*/
	
			} else {
				
				//GameObject mortarText = GameObject.Find("Weapon4Text");
				//tk2dTextMesh tT = mortarText.GetComponent<tk2dTextMesh>();
			
				//tT.color = new Color(255, 255, 255, 255);					
				//weaponCount -= 1;
				
			}				
			
			
			// double check
			if (weaponCount <= 0)
				weaponCount = 0;
			if (weaponCount >= 2)
				weaponCount = 2;
			
			if (Input.GetKeyDown(KeyCode.X)) { // remove
				
			}
			
			
			
			// ROW 1 WEAPONS
			if (curRow == 1) {
				
				curPosY = 9.0f;
				
				// weapon 1
				if (curCol == 1) {
					//curPosX = -51.0f;
					//curPosX = -167.0f;
					curPosX = -163.0f;
				}
				// weapon 2
				if (curCol == 2) {
					curPosX = -143.0f;
				}
				// weapon 3
				if (curCol == 3) {
					curPosX = -126.0f;
				}
				// weapon 4
				if (curCol == 4) {
					curPosX = -110.0f;
				}
				
			}
			
			// x += 25
			// ROW 2 STATS 1
			if (curRow == 2) {
				curPosY = -2.2f;
				
				if (curCol == 4) { curCol = 3; }
				
			    if (curCol == 1) { curPosX = -163.0f; }
				if (curCol == 2) { curPosX = -137.0f; }
				if (curCol == 3) { curPosX = -117.0f;}
	
			}
			
			if (curRow == 3) {
				curPosY = -7.5f;
				
				if (curCol == 4) { curCol = 3; }
				
			    if (curCol == 1) { curPosX = -163.0f; }
				if (curCol == 2) { curPosX = -137.0f; }
				if (curCol == 3) { curPosX = -117.0f; }
			}
			
			if (curRow == 4) {
				curPosY = -12.8f;
			    if (curCol == 1) { curPosX = -163.0f; }
				if (curCol == 2) { curPosX = -137.0f; }
				if (curCol == 3) { curPosX = -117.0f; }
			}
			
			if (curRow == 5) {
				
				if (curCol == 1) {
					curPosY = -18.3f;
					curPosX = -163.0f;
				}
				
				if (curCol == 2) {
					curPosY = -21.0f;
					curPosX = -137.0f;
				}
				
				if (curCol == 3) {
					curPosY = -21.0f;
					curPosX = -115.0f;	
				}
			}
			
			
			selector.transform.position = new Vector3(curPosX, curPosY, 0.0f);
			//Debug.Log("row: " + curRow + "col: " + curCol);
			
			
			// draw stats text
			GameObject curPointsText = GameObject.Find ("PointsLeftText");
			tk2dTextMesh pL = curPointsText.GetComponent<tk2dTextMesh>();
			pL.text = playerStats.totalPoints.ToString() + "/14";
			
			//playerStats.totalPoints;
			
			
			
			
			
		}
	}
	
	
	// coming from the hanger selection
	public void updateIcon () {

		// update ship icon
		selectedShip = PlayerPrefs.GetInt("SelectedShip");
		
		/*
		if (selectedShip == 1) {
			currentIcon1.transform.renderer.enabled = true;
			currentIcon2.transform.renderer.enabled = false;
			currentIcon3.transform.renderer.enabled = false;
		}
		
		
		if (selectedShip == 2) {
			currentIcon1.transform.renderer.enabled = false;
			currentIcon2.transform.renderer.enabled = true;
			currentIcon3.transform.renderer.enabled = false;
		}
		
		if (selectedShip == 3) {
		
			currentIcon1.transform.renderer.enabled = false;
			currentIcon2.transform.renderer.enabled = false;
			currentIcon3.transform.renderer.enabled = true;
		}
	    */
		
	}
	
	public void upHealth () {
		
	}
	
	public void downHealth() {
		
	}
	
}
