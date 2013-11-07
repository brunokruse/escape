using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleMovement : MonoBehaviour {
	
	// movement
	private Vector3 dir;
	public int lastDir = 0;
	float speed = 20;
	float rotationSpeed = 9.0f;
	Vector3 startingRotation;
	Quaternion endingRotation;
	
	// weapons
	// rate of fire
	public Transform laser; // laser weapon
	public Transform melee;
	public Transform tripleShot;
	public Transform mortarShot;
	
	public Transform explosionMesh;
	public Transform thruster;
	
	public Transform laserSpawn;
	private float NextFireAt = -1;
	
	private float TripleFireRate = 0.6f; //0.2f;
	private float MeleeFireRate = 1.0f; //0.75f;
	private float LaserFireRate = 1.5f; //0.1f;
	float fireRateMod = 0.0f;
	
	// health
	public Transform hpBar;
	public Transform shieldBar;
	public Transform damageNumber;
	float finalX = 0.0f;
	public int maxHp;
	public int currentHp;
	public int currentShield;
	public int maxShield;
	public List<Transform> hpBars = new List<Transform>();
	public bool isDead;
	
	ScreenManager screenManager;
	LevelLogic levelLogic;
	MissionControl missionControl;
	public float restartTimer = 1.0f;
	public bool isRestarting;
	
	int currentWeapon;
	
	public bool hasExploded;
	public bool restartMode;
	// sounds
	public AudioClip playerStruck;
	public AudioClip playerFireSound;
	public AudioClip gameOverSound;
	public AudioClip getShield;
	
	// Use this for initialization
	void Start () {
		isDead = false;
		hasExploded = false;
		isRestarting = false;
		restartMode = false;
		NextFireAt = Time.time; // restart fire timer
		dir = Vector3.zero; // no direction

		// set player stats from the preferences!
		maxHp = 1 + PlayerPrefs.GetInt("PHealth");
		currentHp = 1 + PlayerPrefs.GetInt("PHealth");
		
		maxShield = PlayerPrefs.GetInt("PShield");
		currentShield = 0;
		
		rotationSpeed = 9.0f + PlayerPrefs.GetInt("PTurnRate") * 2;
		speed = speed + PlayerPrefs.GetInt("PThrusters") * 2;	
		
		
		currentWeapon = PlayerPrefs.GetInt("CurrentWeapon");
		fireRateMod = PlayerPrefs.GetInt("W1FireRate") / 10.0f;
		
		// firerates and damage

		
		// keep track of other stuff
		screenManager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
		levelLogic = GameObject.Find("Level").GetComponent<LevelLogic>();
		missionControl = GameObject.Find("MissionControl").GetComponent<MissionControl>();
		LaserGun lG = laser.GetComponent<LaserGun>();
		
		lG.setStats((float)PlayerPrefs.GetInt("W1Damage") * 3, (float)PlayerPrefs.GetInt("W1Range"), LaserFireRate - fireRateMod, 0.5f);
		
		foreach (Transform child in transform)
		{
		    // do whatever you want with child transform object here
			if (child.name == "LaserGun") {
				laser = child;	
			}
		}
		
		
		
		//instantiate the health bar
		
		
		for (int z = 0; z < maxHp; z++) {
			Transform tmpB = hpBar;
			tmpB.tag = "hp" + z.ToString();
			Instantiate (tmpB, new Vector3(-46 + z * 1.25f,25,0), Quaternion.identity);
			//hpBars.Add(hpBar);
			
			finalX = -46 + z * 1.25f;
		}
		
		finalX += 1.25f;
		for (int s = 0; s < currentShield; s++) {
			Transform tmpB = shieldBar;
			tmpB.tag = "shield" + s.ToString();
			
			Instantiate (tmpB, new Vector3(finalX + s * 1.25f,25,0), Quaternion.identity);
			//hpBars.Add(hpBar);
			
		}
		
		Debug.Log("player born!");
		

		
	}
	
	public void addShield() {
		
		
		Debug.Log("adding shield!!!");
		AudioSource.PlayClipAtPoint(getShield, transform.position);

		if (currentShield > 0) {
			float xpos = -46.0f + currentShield + currentHp + 1.0f;
			Transform tmpB = shieldBar;

			int curAddOne = currentShield;
			tmpB.tag = "shield" + curAddOne.ToString();
			
			Instantiate (tmpB, new Vector3(xpos,25,0), Quaternion.identity);
			currentShield += 1;

			
		}
		
		if (currentShield == 0) {
			
			float xpos = -46.0f + currentHp + 1.0f;
			Transform tmpB = shieldBar;
			
			int curAddOne = currentShield;
			tmpB.tag = "shield" + curAddOne.ToString();
			
			Instantiate (tmpB, new Vector3(xpos,25,0), Quaternion.identity);
			currentShield += 1;
		}
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		

		//mySprite.color.a += .1f;
			
		// controls
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			
			
			if (transform.position.x <= -50)
				transform.position = new Vector3(-50, transform.position.y, transform.position.z);
			if (transform.position.x >= 50)	
				transform.position = new Vector3(50, transform.position.y, transform.position.z);
			if (transform.position.y <= -25)
				transform.position = new Vector3(transform.position.x, -25, transform.position.z);
			if (transform.position.y >= 25)	
				transform.position = new Vector3(transform.position.x, 25, transform.position.z);
						
			
			dir.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
			dir.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
			transform.position += dir;
			
			thruster.particleSystem.enableEmission = true; // turn ON thrusters		
			
		} else {
			thruster.particleSystem.enableEmission = false; // turn OFF thrusters		
			
		}
	
		// which direction to spin?
		if (!Input.GetKey (KeyCode.Space)) {
			
			
			if (!Input.GetButton("joystick button 1")) {
				
				//if (!Input.GetButton("joystick button 1")) {
				if (dir.x > 0) {
					lastDir = 0;
				} else if (dir.x < 0) {
					lastDir = 1;
				}
				
			}
				
		
			//}
		}
		
		if (Input.GetKey (KeyCode.Space) || Input.GetButton("joystick button 1")) {
		
			if (restartMode) {
				//screenManager.currentScreen = 2;
				levelLogic.killWave();
				//levelLogic.reset();
				isRestarting = false;
				screenManager.resetGame();
				
				levelLogic.enableTimer();
				//levelLogic.killWave();
				//levelLogic.restartLevel();
				//levelLogic.enableTimer();
				
				restartMode = false;
				isRestarting = false;

			
				if (gameObject != null)
	    		{  
					Destroy(gameObject);
				}	
			
				levelLogic.killWave();	
				
			}
		}
		
		if (lastDir == 0) {
			endingRotation = Quaternion.Euler(new Vector3(0, 0, 0)); // rotate right
			transform.rotation = Quaternion.Lerp(transform.rotation, endingRotation, Time.deltaTime * rotationSpeed);	
		} else if (lastDir == 1) {
			endingRotation = Quaternion.Euler(new Vector3(0, -180, 0)); // rotate left
			transform.rotation = Quaternion.Lerp(transform.rotation, endingRotation, Time.deltaTime * rotationSpeed);		
		}

		
		// weapons
		if((Input.GetKey (KeyCode.Space) || Input.GetButton("joystick button 1")) && !restartMode && !isDead) {
			if(Time.time >= NextFireAt) { // if the cooldown has been reached
				AudioSource.PlayClipAtPoint(playerFireSound, transform.position);
				
				if (currentWeapon == 3) { // laser	
					
					LaserGun lG = laser.GetComponent<LaserGun>();
					
					lG.fire();
					
					NextFireAt = Time.time + LaserFireRate - fireRateMod;
					
				}
				
				if (currentWeapon == 1) { // scatter
					GameObject.Instantiate(tripleShot, laserSpawn.transform.position, Quaternion.identity);
					NextFireAt = Time.time + TripleFireRate - fireRateMod;
				}
				
				if (currentWeapon == 2) { // melee
					GameObject.Instantiate(melee, laserSpawn.transform.position, Quaternion.identity);
					NextFireAt = Time.time + MeleeFireRate - fireRateMod;
				}
				if (currentWeapon == 4) { // mortar
			        GameObject.Instantiate(mortarShot, laserSpawn.transform.position, Quaternion.identity);
					
					NextFireAt = Time.time + MeleeFireRate - fireRateMod;
					
				}
			}
		} else if((Input.GetKeyDown(KeyCode.Backspace)|| Input.GetButton("joystick button 2")) && restartMode == true) {
			stopRestartTimer();
			
			screenManager.currentScreen = 2;
			
			isRestarting = false;
			
			restartMode = false;
				
			
			if (gameObject != null)
    		{  
				Destroy(gameObject);
			}	
		
			levelLogic.killWave();	
			
		}
		
		
		// are we dead yet?		
		if (isDead) {
			transform.renderer.enabled = false;	
			thruster.particleSystem.enableEmission = false; // turn OFF thrusterss
			
			
			restartTimer -= Time.deltaTime;
			isRestarting = true;	
			//pauseRestartTimer();
			
			//restartMode = true;
			if (restartTimer < 0) {
				Debug.Log("timer reached!");
				stopRestartTimer();
				//pauseRestartTimer();
				//pauseRestartTimer();
			}
			
			
			
		} else {
			transform.renderer.enabled = true;	
		}
		
		
		// render the health bar
		
	}
	
	void healthBar() {
		//Debug.Log("TAKE DAMAGE : CURRENT HEALTH" + currentHp);
		//Debug.Log("TAKE DAMAGE : CURRENT SHIELD" + currentShield);
		
		// shield bar
		if (currentShield <= 5) {
			GameObject s5 = GameObject.FindGameObjectWithTag("shield5");
			Destroy(s5);
		}
		if (currentShield <= 4) {
			GameObject s4 = GameObject.FindGameObjectWithTag("shield4");
			Destroy(s4);
		}
		
		if (currentShield <= 3) {
			GameObject s3 = GameObject.FindGameObjectWithTag("shield3");
			Destroy(s3);
		}
		
		if (currentShield <= 2) {
			GameObject s2 = GameObject.FindGameObjectWithTag("shield2");
			Destroy(s2);
		}
		
		if (currentShield <= 1) {
			GameObject s1 = GameObject.FindGameObjectWithTag("shield1");
			Destroy(s1);
		}
		
		
		if (currentShield <= 0) {
			
			GameObject s0 = GameObject.FindGameObjectWithTag("shield0");
			Destroy(s0);
			
			// health bar
			if (currentHp <= 5) {
				GameObject h5 = GameObject.FindGameObjectWithTag("hp5");
				Destroy(h5);
			}
			
			if (currentHp <= 4) {
				GameObject h4 = GameObject.FindGameObjectWithTag("hp4");
				Destroy(h4);
			}
			
			if (currentHp <= 3) {
				GameObject h3 = GameObject.FindGameObjectWithTag("hp3");
				Destroy(h3);
			}
			
			if (currentHp <= 2) {
				GameObject h2 = GameObject.FindGameObjectWithTag("hp2");
				Destroy(h2);
			}
			
			if (currentHp <= 1) {
				GameObject h1 = GameObject.FindGameObjectWithTag("hp1");
				Destroy(h1);
			}
		}
		// WHAT HAPPENS ON DEATH
		if (currentHp <= 0) {
			
			GameObject h0 = GameObject.FindGameObjectWithTag("hp0");
			Destroy(h0);
			
			if (!hasExploded) {
				
				int rotationAngle = 0;
				
				if (lastDir == 0)
					rotationAngle = -180;
				else if(lastDir == 1)
					rotationAngle = 0;
				
				Transform clone = (Transform)Instantiate(explosionMesh,transform.position, Quaternion.Euler(90,rotationAngle,0));
				
				SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
				explodeScript.blowUp();
				hasExploded = true;
			}
			
			
			Destroy(GameObject.Find("Thruster"));
			
			isDead = true;
			
			transform.collider.enabled = false;
			AudioSource.PlayClipAtPoint(gameOverSound, transform.position);

			currentHp = PlayerPrefs.GetInt("PHealth");
			
			//screenManager.currentScreen = 2;
			//screenManager.isLive = false;
			
		}
		

		
	}
	
	void startRestartTimer () {
		
	}
	
	void pauseRestartTimer() {
		restartTimer = 1.0f;
		restartMode = true;

		
	}
	void stopRestartTimer () {
		Debug.Log("stopRestartTimer");
		
		restartTimer = 1.0f;
		restartMode = true;
		// what happens when we die
	
	}
	
	
	public void takeDamage() {
		
		if (currentShield > 0) {
			currentShield -= 1;
		} else if (currentShield <= 0 && currentHp > 0) {
			currentHp -= 1;	
		}
			
		
		healthBar();
		
	}
	
	public void resetPlayer() {
		isDead = false;
		maxHp = 1 + PlayerPrefs.GetInt("PHealth");
		currentHp = 1 + PlayerPrefs.GetInt("PHealth");
		currentShield = PlayerPrefs.GetInt("PShield");
		rotationSpeed = 9.0f + PlayerPrefs.GetInt("PTurnRate") * 2;
		speed = speed + PlayerPrefs.GetInt("PThrusters") * 2;	
	}
	
	
	void OnParticleCollision(GameObject collision) { 
		
				
		//int checkStatus;
	    int checkStatus;
		
		Transform laserGun = collision.transform.parent;
			
		LaserGun gunScript = laserGun.GetComponent<LaserGun>();
	
		checkStatus = gunScript.status;
		 
		
		
		if ((collision.tag == "enemyLaser" || collision.name == "LaserFire1" || collision.name == "LaserFire") && checkStatus == 2) {
						
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			
			int damage = 1;
			dT.text = damage.ToString();
			
			takeDamage();

			GameObject.Instantiate(damageNumber, transform.position, Quaternion.identity);
			
			GameObject cam = GameObject.Find("tk2dCamera");
			CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
			cS.shakeCam();
			
			AudioSource.PlayClipAtPoint(playerStruck, transform.position);
		}
	}
	
	
	void OnCollisionEnter(Collision collision) {
		
		
		// enemy laser
		if (collision.gameObject.tag == "enemyLaser") {
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			
			int damage = 1;
			dT.text = damage.ToString();
			
			GameObject.Instantiate(damageNumber, collision.transform.position, Quaternion.identity);
			
			GameObject cam = GameObject.Find("tk2dCamera");
			CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
			cS.shakeCam();
			
			AudioSource.PlayClipAtPoint(playerStruck, transform.position);

		}
		
		// crashing into an enemy
		if (collision.gameObject.tag == "enemySpawn") {
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			
			int damage = 2;
			dT.text = damage.ToString();
			
			takeDamage();
			takeDamage();
			
			GameObject.Instantiate(damageNumber, collision.transform.position, Quaternion.identity);
			
			GameObject cam = GameObject.Find("tk2dCamera");
			CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
			cS.shakeCam();
			
			AudioSource.PlayClipAtPoint(playerStruck, transform.position);
			
			//Destroy(gameObject);

			
		}
		
		// cluster enemy bomb
		if (collision.gameObject.name == "ClusterEnemy(Clone)") {
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			
			int damage = 3;
			
			takeDamage();
			
			dT.text = damage.ToString();
			
			GameObject.Instantiate(damageNumber, collision.transform.position, Quaternion.identity);
			
			GameObject cam = GameObject.Find("tk2dCamera");
			CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
			cS.shakeCam();
			
			AudioSource.PlayClipAtPoint(playerStruck, transform.position);
			
		}
		
	}
	
}
