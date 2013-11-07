using UnityEngine;
using System.Collections;

public class OutlineScript : MonoBehaviour {

	float lifetime = 5.0f;
	public Transform sphereEnemy;
	bool isSpawned;
	
	float spawnTimer = 3.0f;
	// Use this for initialization
	void Start () {
		
		Destroy(gameObject, lifetime);
		isSpawned = false;
		//spawn();
		

	}
	
	// Update is called once per frame
	void Update () {
		if (!isSpawned) {
			spawnTimer -= Time.deltaTime;
			if (spawnTimer < 0) {
				Debug.Log("start level!");
				spawn();
			}
		}
		
		if (isSpawned)
			fadeOut (0.1f);
	}
	
	void spawn() {
		transform.position -= new Vector3(0.0f, 0.0f, 5.0f); // these values will be coming from a text file			
		
		
		Transform enemy1 = (Transform) Instantiate(sphereEnemy, transform.position, transform.rotation);
		enemy1.tag = "enemySpawn";
		isSpawned = true;
		
		
	}
	
	void fadeOut (float amt) {
		tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
		mySprite.color -= new Color(255,255,255, amt);
		
	}
}
