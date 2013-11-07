using UnityEngine;
using System.Collections;

public class ClusterBotAI : MonoBehaviour {
	
	Transform player;
	public Transform damageNumber;
	public Transform explosionMesh;
	public Transform spawnOutline;
	
	public int clusterBotHP = 40;
	
	public AudioClip hitSound;
	
	public bool isTeleported;
	public float facing;
	SmoothFollow sF;
	
	public AudioClip deathSound;
	
	public bool hasExploded;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
		sF = gameObject.GetComponent<SmoothFollow>();
		sF.enabled = false;
		isTeleported = false;
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
		//mySprite.color += new Color(255,255,255, amt);
		
		// enable movement once teleported in
		if (mySprite.color.a >= 0.75f) {
			sF.enabled = true;
			isTeleported = true;
		}
		
	}
	
	void OnParticleCollision(GameObject collision) { 
			
			//Debug.Log(collision.name);
		
			int checkStatus;
		    
			GameObject laserGun = GameObject.Find("Player1(Clone)/LaserGun");
			LaserGun gunScript = laserGun.GetComponent<LaserGun>();
		
			checkStatus = gunScript.status;
		 
		
			if (collision.name == "LaserFire" && checkStatus == 2) {
				
			int damage = PlayerPrefs.GetInt("W1Damage") + 1;
			clusterBotHP -= damage;
			
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			dT.text = damage.ToString();
			
			
			GameObject.Instantiate(damageNumber, new Vector3(transform.position.x, 
															 transform.position.y + 8, 
															 transform.position.z), Quaternion.identity);
		
			
			
			Vector3 headingBullet = transform.position - player.transform.position;
			
			if (headingBullet.x > 0) {
				transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y, 0);
				
			} else if (headingBullet.x <= 0) {
				transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y, 0);
				
			}
	
			
			
			//float distance = headingBullet.magnitude;
			//Vector3 direction = headingBullet / distance;  // This is now the normalized direction.
			//transform.rigidbody.AddForce(direction.normalized * 100);
			
			//Destroy(collision.gameObject);
			
			// calculate knockback
			//
			
			
			
			if (clusterBotHP <= 0) {
				
				
				Vector3 heading = transform.position - player.position;
				Debug.Log ("heading: " + heading);
				if (heading.x > 0) {
					facing = 0.0f;
				} else if (heading.x <= 0) {
					facing = -180.0f;
				}
		
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
				
				if (!hasExploded) {
				Transform clone = (Transform)Instantiate(explosionMesh, transform.position, Quaternion.Euler(90, facing, 0));
				SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
				explodeScript.blowUp();
				GameObject cam = GameObject.Find("tk2dCamera");
				CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
				cS.shakeCam();
				hasExploded = true;
				}
				clusterBotHP = 20;
				
				// give the player credit
				GameObject level = GameObject.Find("Level");
				LevelLogic LL = level.GetComponent<LevelLogic>();				
				LL.destroyCount += 1;
				
				Destroy(gameObject);	

			}
			
			
			}
		
	}
	
	
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "playerLaser") {
			Debug.Log("collision!");
			
			int damage = PlayerPrefs.GetInt("W1Damage") + 1;
			clusterBotHP -= damage;
			
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			dT.text = damage.ToString();
			
			
			GameObject.Instantiate(damageNumber, new Vector3(collision.transform.position.x, 
															 collision.transform.position.y, 
															 collision.transform.position.z), Quaternion.identity);
			
			
			
			Vector3 headingBullet = transform.position - player.transform.position;
			
			if (headingBullet.x > 0) {
				transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y, 0);
				
			} else if (headingBullet.x <= 0) {
				transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y, 0);
				
			}
	
			
			
			//float distance = headingBullet.magnitude;
			//Vector3 direction = headingBullet / distance;  // This is now the normalized direction.
			//transform.rigidbody.AddForce(direction.normalized * 100);
			
			Destroy(collision.gameObject);
			
			// calculate knockback
			//
			
			
			
			if (clusterBotHP <= 0) {
				
				
				Vector3 heading = transform.position - player.position;
				Debug.Log ("heading: " + heading);
				if (heading.x > 0) {
					facing = 0.0f;
				} else if (heading.x <= 0) {
					facing = -180.0f;
				}
		
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
				
				Transform clone = (Transform)Instantiate(explosionMesh, transform.position, Quaternion.Euler(90, facing, 0));
				SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
				explodeScript.blowUp();
				GameObject cam = GameObject.Find("tk2dCamera");
				CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
				cS.shakeCam();
				
				clusterBotHP = 20;
				
				// give the player credit
				GameObject level = GameObject.Find("Level");
				LevelLogic LL = level.GetComponent<LevelLogic>();				
				LL.destroyCount += 1;
				
				Destroy(gameObject);	

			}
			
		}
		
		if (collision.gameObject.tag == "Player") {
			clusterBotHP = 0;
			if (clusterBotHP <= 0) {
				
				
				Vector3 heading = transform.position - player.position;
				//Debug.Log ("heading: " + heading);
				if (heading.x > 0) {
					facing = 0.0f;
				} else if (heading.x <= 0) {
					facing = -180.0f;
				}
				
				AudioSource.PlayClipAtPoint(deathSound, transform.position);

				Transform clone = (Transform)Instantiate(explosionMesh, transform.position, Quaternion.Euler(90, facing, 0));
				SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
				explodeScript.blowUp();
				GameObject cam = GameObject.Find("tk2dCamera");
				CameraShake cS = (CameraShake) cam.GetComponent(typeof(CameraShake));
				cS.shakeCam();
				clusterBotHP = 20;
				
				// give the player credit
				GameObject level = GameObject.Find("Level");
				LevelLogic LL = level.GetComponent<LevelLogic>();				
				LL.destroyCount += 1;
				
				Destroy(gameObject);	

			}
		}

	}
}
