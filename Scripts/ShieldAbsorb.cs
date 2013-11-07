using UnityEngine;
using System.Collections;

public class ShieldAbsorb : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.tag == "shieldPickup") {
			
			GameObject missionControl = GameObject.Find("MissionControl");
			Debug.Log("picked up shield!");
			
			//PlayerStats pS = missionControl.GetComponent<PlayerStats>();
			GameObject player1 = GameObject.FindGameObjectWithTag("Player");
			SimpleMovement playerStats = player1.GetComponent<SimpleMovement>();
			
			//if (playerStats.currentShield < playerStats.maxShield)
				playerStats.addShield();
			//else {
					
			//}
			
			Destroy(collision.gameObject);
			
		}
	}
	
}
