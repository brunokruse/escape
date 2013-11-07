using UnityEngine;
using System.Collections;

public class SteamBotAI : MonoBehaviour {
	
	public Transform laser;
	public Transform laserSpawn;
	LaserGun lG;
	
	public Transform damageNumber;
	public Transform steamJet;
	public Transform explosionMesh;
	public Transform spawnOutline;
	public bool isVenting;
	public bool fire = false;
	
	EightWayFire eightWayFire;
	
	public int steamBotHP = 200;
	public bool hasExploded;
	public AudioClip hitSound;
	
	public bool isTeleported;
	
	public AudioClip deathSound;
	//SmoothFollow sF;
	
	float speedOffset = 0.0f;
	float angleOffset = 0.0f;
	
	// Use this for initialization
	void Start () {
		
		speedOffset = Random.Range(5, 30);
		
		
		
		lG = laser.GetComponent<LaserGun>();
		lG.setStats(5f, 8f, 5f, 0.5f);
		foreach (Transform child in transform)
		{
		    // do whatever you want with child transform object here
			if (child.name == "LaserGun") {
				laser = child;	
			}
		}
		lG.fire ();
		//sF = gameObject.GetComponent<SmoothFollow>();
		//sF.enabled = false;
		isTeleported = false;
		isVenting = false;
		hasExploded = false;
		
		/*
		GameObject.Instantiate(spawnOutline, new Vector3(transform.position.x, 
														 transform.position.y, 
														 transform.position.z), Quaternion.identity);		
		*/
		
	}
	
	// Update is called once per frame
	void Update () {
		tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
	//	mySprite.color += new Color(255,255,255, amt);
		
		// enable movement once teleported in
		if (mySprite.color.a >= 0.75f) {
			//sF.enabled = true;
			isTeleported = true;
		}
		if(isTeleported){
			GameObject pl;
			pl = GameObject.Find("Player1(Clone)");
			
			//Vector3 targetDir = pl.transform.position - laser.transform.position;
			//float distance = targetDir.magnitude;
			//Vector3 forward = laser.transform.forward;
			//float angle = Vector3.Angle(targetDir, forward);
			//Vector3 direction = targetDir/distance;
			//laser.transform.Rotate (new Vector3(0, 0, angle));
			//laser.transform.LookAt (pl.transform.position);
			//laser.transform.Rotate (new Vector3 (0, -90, 0));
			//laser.transform.rotation = Quaternion.Lerp(laser.transform.rotation, Quaternion.Euler(direction), Time.deltaTime * 1f);
        	//Vector3 forward = lG.transform.forward;
        	//float angle = Vector3.Angle(targetDir, forward);
			
			
			
			laser.transform.Rotate (0, 0, Time.deltaTime * speedOffset);
			
			//laser.transform.eulerAngles = new Vector3(0, 0, angle);
			if(lG.status==0){
				//laser.LookAt(pl.transform);
				lG.fire();
			}
		}
	}
	
	public void vent(){
		isVenting = true;
		//Debug.Log("VENTING");
		GameObject.Instantiate(steamJet, new Vector3(transform.position.x, transform.position.y-.09f, transform.position.z), Quaternion.identity);
	}
	
	void OnParticleCollision(GameObject collision) { 
			
			//Debug.Log(collision.name);
		
			int checkStatus;
		    
			GameObject laserGun = GameObject.Find("Player1(Clone)/LaserGun");
			LaserGun gunScript = laserGun.GetComponent<LaserGun>();
		
			checkStatus = gunScript.status;
		 
		
			if (collision.name == "LaserFire" && checkStatus == 2) {
				
				//Debug.Log("Hit with laser!");
			
				int damage = PlayerPrefs.GetInt("W1Damage") + 3;
				
				if (isVenting) 
					damage *= 3;
				
				steamBotHP -= damage;
				
				AudioSource.PlayClipAtPoint(hitSound, transform.position);
				
				tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
				dT.text = damage.ToString();
				
				
				GameObject.Instantiate(damageNumber, new Vector3(transform.position.x, 
																 transform.position.y + 8, 
																 transform.position.z), Quaternion.identity);
				if(!isVenting) fire = true;
				//Destroy(collision);
				
				if (steamBotHP <= 0) {
					
					if (!hasExploded) {
						Transform clone = (Transform)Instantiate(explosionMesh,transform.position, Quaternion.Euler(90, 0, 0));
						SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
						explodeScript.blowUp();
						hasExploded = true;
						AudioSource.PlayClipAtPoint(deathSound, transform.position);
						GameObject cam = GameObject.Find("tk2dCamera");
						CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
						cS.shakeCam();
	
					}
					
					
					
					steamBotHP = 20;
					
					// give the player credit
					GameObject level = GameObject.Find("Level");
					LevelLogic LL = level.GetComponent<LevelLogic>();				
					LL.destroyCount += 1;
					
					Destroy(gameObject);
			}
		}
		
	}
	
	void OnCollisionEnter(Collision collision) {
		
		if (collision.gameObject.tag == "playerLaser" && collision.gameObject.name != "LaserFire") {
			Debug.Log("collision!");
			
			int damage = PlayerPrefs.GetInt("W1Damage") + 1;
			
			if (isVenting) 
				damage *= 3;
			
			steamBotHP -= damage;
			
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			dT.text = damage.ToString();
			
			
			GameObject.Instantiate(damageNumber, new Vector3(collision.transform.position.x, 
															 collision.transform.position.y, 
															 collision.transform.position.z), Quaternion.identity);
			if(!isVenting) fire = true;
			Destroy(collision.gameObject);
			
			if (steamBotHP <= 0) {
				
				if (!hasExploded) {
					Transform clone = (Transform)Instantiate(explosionMesh,transform.position, Quaternion.Euler(90, 0, 0));
					SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
					explodeScript.blowUp();
					hasExploded = true;
					AudioSource.PlayClipAtPoint(deathSound, transform.position);
					GameObject cam = GameObject.Find("tk2dCamera");
					CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
					cS.shakeCam();

				}
				
				
				
				steamBotHP = 20;
				
				// give the player credit
				GameObject level = GameObject.Find("Level");
				LevelLogic LL = level.GetComponent<LevelLogic>();				
				LL.destroyCount += 1;
				
				Destroy(gameObject);	

			}
			
		}
	}

	

	
}
