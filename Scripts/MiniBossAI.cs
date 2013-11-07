using UnityEngine;
using System.Collections;

public class MiniBossAI : MonoBehaviour {
	
	public Transform damageNumber;
	public Transform explosionMesh;
	//public Transform spawnOutline;
	
	public int miniBossHP = 50;
	
	public AudioClip hitSound;
	private ParticleSystem.CollisionEvent[] collisionEvents = new ParticleSystem.CollisionEvent[16];

	public bool isTeleported;
	//SmoothFollow sF;
	
	// Use this for initialization
	void Start () {
		//sF = gameObject.GetComponent<SmoothFollow>();
		//sF.enabled = false;
		isTeleported = false;
		
		
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
		
	}
	
	void OnParticleCollision(GameObject collision) { 
		//Destroy(collision.gameObject);
		
		GameObject laser1 = GameObject.Find("LaserFire1");
		GameObject laser2 = GameObject.Find("LaserFire2");
		
		
		
		ParticleSystem particleSystem;
        particleSystem = laser1.GetComponent<ParticleSystem>();
		
		
        int safeLength = particleSystem.safeCollisionEventSize;
        if (collisionEvents.Length < safeLength)
            collisionEvents = new ParticleSystem.CollisionEvent[safeLength];
        
        int numCollisionEvents = particleSystem.GetCollisionEvents(gameObject, collisionEvents);
        int i = 0;
        while (i < numCollisionEvents) {
            if (gameObject.rigidbody) {
                Vector3 pos = collisionEvents[i].intersection;
				
				
				 //Particle[] particles = particleSystem.particles;
				 //Destroy(particles[i]);
				
				//collisionEvents[i].velocity = 0;
                //Debug.Log(pos);
				//Destroy();
				//Vector3 force = collisionEvents[i].velocity * 10;
                //gameObject.rigidbody.AddForce(force);
            }
            i++;
        }
		
		
	}
	
	void OnCollisionEnter(Collision collision) {
		
		if (collision.gameObject.tag == "playerLaser") {
			Debug.Log("collision!");
			
			int damage = PlayerPrefs.GetInt("W1Damage") + 1;
			miniBossHP -= damage;
			
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
			
			tk2dTextMesh dT = damageNumber.GetComponent<tk2dTextMesh>();
			dT.text = damage.ToString();
			
			
			GameObject.Instantiate(damageNumber, new Vector3(collision.transform.position.x, 
															 collision.transform.position.y, 
															 collision.transform.position.z), Quaternion.identity);
			Destroy(collision.gameObject);
			
			if (miniBossHP <= 0) {
				
				Transform clone = (Transform)Instantiate(explosionMesh,transform.position, Quaternion.Euler(90, 0, 0));
				SplitMeshIntoTriangles explodeScript = (SplitMeshIntoTriangles) clone.GetComponent(typeof(SplitMeshIntoTriangles));
				explodeScript.blowUp();
				miniBossHP = 500;
				
				// give the player credit
				GameObject level = GameObject.Find("Level");
				LevelLogic LL = level.GetComponent<LevelLogic>();				
				LL.destroyCount += 1;
				
				Destroy(gameObject);	

			}
			
		}
		
		
	}
}
