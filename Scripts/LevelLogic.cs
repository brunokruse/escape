using UnityEngine;
using System.Collections;

public class LevelLogic : MonoBehaviour {

	public GameObject player1;
	public GameObject playerSpawn;
	
	float countdownTimer = 4.0f;
	public Transform timerPrefab;
	public bool timerEnabled;
	public int currentWave;
	
	public Transform sphereEnemy;
	public Transform steambotSpawn;
	public Transform miniBoss;

	public Transform clusterEnemy;
	public Transform clusterSpawn;
	
	public Transform shieldPowerup;
	
	public int goalCount;
	public int destroyCount;
	public bool isVictory;
	public bool gameoOver;
	public bool winGame;
	
	//LL.enableTimer();
	int maxWave = 5;
	bool bossMode;
	// sounds
	public AudioClip roundComplete;
	public bool gameOver = false;
	
	// Use this for initialization
	void Start () {
		Vector3 timerPos = new Vector3(-1.0f, -1.0f, 0.0f);
		timerPrefab = (Transform) Instantiate(timerPrefab, timerPos, transform.rotation);
		timerPrefab.renderer.enabled = false;
		
		gameOver = false;
		
		reset();
	}
	
	public void reset() {
		bossMode = false;
		isVictory = false;
		winGame = false;
		maxWave = 5;

		gameOver = false;
		
		currentWave = 0;
		goalCount = 1;
		destroyCount = 0;		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (!winGame) {
		
			// timer stuff
			if (timerEnabled) {
				
				// countdown
				
				countdownTimer -= Time.deltaTime;
				if (countdownTimer < 0) {
					Debug.Log("start level!");
					restartLevel();
					disableTimer();
				}
				
				// draw it too if appropriate
				timerPrefab.renderer.enabled = true;
				tk2dTextMesh cf = timerPrefab.GetComponent<tk2dTextMesh>();
				float roundedTime = Mathf.Round(countdownTimer * 100) / 100;
				
				if (roundedTime <= 1.0)
					cf.text = "Go!";
				else
					cf.text = roundedTime.ToString();
				
			
			} else {
				timerPrefab.renderer.enabled = false;
			}
			
			
			// show wave counter
			GameObject wC = GameObject.Find("WaveCounter");
			tk2dTextMesh wCM = wC.GetComponent<tk2dTextMesh>();
			wCM.text = "wave " + currentWave.ToString();
		
			
		//if (!gameOver)
		checkForGameOver();
		//else
		//	gameOver = true;

		// check for victory
		if (!isVictory)
			checkForVictory();
		} else {
			timerPrefab.renderer.enabled = true;
			GameObject wC = GameObject.Find("WaveCounter");
			tk2dTextMesh tPF = timerPrefab.GetComponent<tk2dTextMesh>();
			//tPF.text = "a winner is you!";
			tPF.text = "";
			gameOver = true;
		}
	}
	
	void checkForGameOver() {
		//S
		
		GameObject ourPlayer = GameObject.FindGameObjectWithTag("Player");
		
		if(ourPlayer) {
			SimpleMovement sM = ourPlayer.GetComponent<SimpleMovement>();
			
			if (sM.isRestarting) {
				gameOver = true;	
			} else {
				gameOver = false;	
			}
			
		} else {
			
			gameOver = false;
		}
		
	}
	
	void checkForVictory () {
		// if everything is dead!
		
		
		if (destroyCount == goalCount) {
			Debug.Log("victory!");
			isVictory = true;
					
			Vector3 shieldSpawn = new Vector3(0.0f, 0.0f, 0.0f);
			Transform sP = (Transform) Instantiate(shieldPowerup, shieldSpawn, Quaternion.Euler(-90, 0, 0));
			
			enableTimer();
		}
		
		/*
		GameObject[] eSpawns;
        eSpawns = GameObject.FindGameObjectsWithTag("enemySpawn");
		int num = eSpawns.Length;
		Debug.Log("num: " + num);
		*/
		
		/*
		if (num <= 0) {
			restartLevel();
			disableTimer();	
		}
		*/
	}
	
	public void enableTimer() {
		timerEnabled = true;
		countdownTimer = 4.00f;
		
	}
	
	public void disableTimer() {
		timerEnabled = false;
		countdownTimer = 4.00f;
		
		//shieldPowerup
	}
	
	public void killWave() {
		GameObject waveCounter = GameObject.Find("WaveCounter");
		tk2dTextMesh wC = waveCounter.GetComponent<tk2dTextMesh>();
		currentWave = 0;
		destroyCount = 0;
		
		// kill all enemies
        GameObject[] eSpawns;
        eSpawns = GameObject.FindGameObjectsWithTag("enemySpawn");
		foreach (GameObject eS in eSpawns) {
			Destroy(eS.gameObject);
		}
		
		// disable player
		//SimpleMovement sM = player1.GetComponent<SimpleMovement>();
		//sM.currentHp = 0;
		//sM.isDead = true;

	}
	
	public void restartLevel() {
		
		Debug.Log("restarting level!");
		isVictory = false;
		winGame = false;
		gameOver = false;
		disableTimer();
		
		// spawn the player first
		if(!GameObject.FindGameObjectWithTag("Player")) {
			GameObject p1 = (GameObject) Instantiate(player1, transform.position, transform.rotation);
		
			//player1 = (GameObject) Instantiate(player1, transform.position, transform.rotation);
			//SimpleMovement sM = player1.GetComponent<SimpleMovement>();
			//sM.isDead = false;
		
			p1.gameObject.tag = "Player";	
			p1.transform.renderer.enabled = true;
			p1.transform.position = playerSpawn.transform.position;
		}
	
		//bossMode = true;
		if (bossMode) {
			goalCount = 10; //currentWave + 8
			destroyCount = 0;
			Vector3 bossPos = new Vector3(15.0f, 3.0f, 0.0f); // these values will be coming from a text file			
			Transform enemyBoss = (Transform) Instantiate(miniBoss, bossPos, transform.rotation);					
			enemyBoss.tag = "enemySpawn";
			currentWave += 1;
			
			Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
			Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);

			Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
			Transform enemy2 = (Transform) Instantiate(steambotSpawn, enemyPos2, transform.rotation);	
			goalCount = 2;
			//winGame = true;
		}
		
		//if (bossMode && co
		
		if (currentWave > maxWave + 1) {
			winGame = true;	
		}
		
		// not fighting a boss spawn normal wave!
		if (!bossMode) { 
			if (currentWave < maxWave) {
				//goalCount = currentWave + currentWave + 1 + 1;
				destroyCount = 0;
				
				
				// then spawn enemies based on appropriate wave
				// for now this is hardcoded to two sphere enemies
				//Vector3 enemyPos1 = new Vector3(10.0f, -2.0f, 0.0f); // these values will be coming from a text file
				
				// spawn patterns
				if (currentWave == 0) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);
					goalCount = 1;
				}
				if (currentWave == 1) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);

					Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy2 = (Transform) Instantiate(steambotSpawn, enemyPos2, transform.rotation);	
					goalCount = 2;
				}
				if (currentWave == 2) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);

					Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy2 = (Transform) Instantiate(steambotSpawn, enemyPos2, transform.rotation);
					
					Vector3 enemyPos3 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy3 = (Transform) Instantiate(clusterSpawn, enemyPos3, transform.rotation);
					goalCount  = 3;
				}
				
				if (currentWave == 3) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);

					Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy2 = (Transform) Instantiate(steambotSpawn, enemyPos2, transform.rotation);
					
					Vector3 enemyPos3 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy3 = (Transform) Instantiate(steambotSpawn, enemyPos3, transform.rotation);
					
					Vector3 enemyPos4 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy4 = (Transform) Instantiate(clusterSpawn, enemyPos4, transform.rotation);	
					
					Vector3 enemyPos5 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy5 = (Transform) Instantiate(clusterSpawn, enemyPos5, transform.rotation);
					goalCount = 5;
				
				}
				if (currentWave == 4) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);

					Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy2 = (Transform) Instantiate(steambotSpawn, enemyPos2, transform.rotation);
					
					Vector3 enemyPos3 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy3 = (Transform) Instantiate(steambotSpawn, enemyPos3, transform.rotation);
					
					Vector3 enemyPos4 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy4 = (Transform) Instantiate(clusterSpawn, enemyPos4, transform.rotation);	
					
					Vector3 enemyPos5 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy5 = (Transform) Instantiate(clusterSpawn, enemyPos5, transform.rotation);
					goalCount = 5;
					
					Vector3 enemyPos6 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy6 = (Transform) Instantiate(clusterSpawn, enemyPos6, transform.rotation);
					goalCount = 6;
				
				
				}				
				if (currentWave == 5) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);

					Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy2 = (Transform) Instantiate(steambotSpawn, enemyPos2, transform.rotation);
					
					Vector3 enemyPos3 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy3 = (Transform) Instantiate(steambotSpawn, enemyPos3, transform.rotation);
					
					
					Vector3 enemyPos4 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy4 = (Transform) Instantiate(clusterSpawn, enemyPos4, transform.rotation);	
					
					Vector3 enemyPos5 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy5 = (Transform) Instantiate(clusterSpawn, enemyPos5, transform.rotation);	

					Vector3 enemyPos6 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy6 = (Transform) Instantiate(steambotSpawn, enemyPos6, transform.rotation);

					Vector3 enemyPos7 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy7 = (Transform) Instantiate(clusterSpawn, enemyPos7, transform.rotation);	
					goalCount = 7;
				}				
				
				/*
				for (int z = 0; z < currentWave + 1; z++) {
					Vector3 enemyPos1 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy1 = (Transform) Instantiate(steambotSpawn, enemyPos1, transform.rotation);
					
					
					Vector3 enemyPos2 = new Vector3(Random.Range(-40.0F, 40.0F),Random.Range(-15.0F, 15.0F), 5.0f); // these values will be coming from a text file			
					Transform enemy2 = (Transform) Instantiate(clusterSpawn, enemyPos2, transform.rotation);			
				}*/
				
	
				
				currentWave += 1;
			}
		}
		
		if (currentWave >= maxWave) {
			bossMode = true;	
		}
		
		GameObject waveCounter = GameObject.Find("WaveCounter");
		tk2dTextMesh wC = waveCounter.GetComponent<tk2dTextMesh>();
		
		if(!bossMode)
			wC.text = currentWave.ToString();
		else wC.text = "MINIBOSS";
		
		
		
		AudioSource.PlayClipAtPoint(roundComplete, transform.position);

	}
	
	void spawnWave() {
		
	}
	
}
