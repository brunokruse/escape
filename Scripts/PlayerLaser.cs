using UnityEngine;
using System.Collections;

public class PlayerLaser : MonoBehaviour {
	
	float lifetime = 5.5f;
	public float speed = 90.0f;
	public Vector3 dir;
	
	float lifetimeMod = 0.0f;
	
	float warmupTimer = 1.0f;
	bool setSpawnPoint;
	float spawnPointY;
	GameObject p1;
	
	
	// Use this for initialization
	void Start () {
		setSpawnPoint = false;
		transform.renderer.enabled = false;
		lifetimeMod = lifetime + PlayerPrefs.GetInt("W1Range") / 4;
		
		Destroy(gameObject, lifetimeMod); // destroy over time
		
		p1 = GameObject.FindGameObjectWithTag("Player");
		
		if (p1)
			dir = p1.transform.right;
	}
	
	// Update is called once per frame
	void Update () {
		
		warmupTimer -= Time.deltaTime;
		if (warmupTimer < 0) {
			
			if (!setSpawnPoint && p1) {
					spawnPointY = p1.transform.position.y;
				setSpawnPoint = true;
			}
			
			Debug.Log("start level!");
			transform.renderer.enabled = true;
			
			transform.position +=  dir * speed * Time.deltaTime;
			transform.position =  new Vector3(transform.position.x,spawnPointY, transform.position.z);
			
			transform.rotation = Quaternion.identity;
		}
				
		
	}
	
	void OnCollisionEnter(Collision collision) {	
		if (collision.gameObject.tag == "enemySteamBot") {
			Destroy(gameObject);
			//Destroy(gameObject);
			//Destroy(collision.gameObject);

		}
	}
}
