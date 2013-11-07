using UnityEngine;
using System.Collections;

public class DemoFire : MonoBehaviour {

	private float NextFireAt = -1;
	private float FireRate = 0.5f;

	public Transform laser; // laser weapon
	public Transform melee;
	public Transform tripleShot;
	public Transform mortarShot;
	private float TripleFireRate = 0.6f; //0.2f;
	private float MeleeFireRate = 1.0f; //0.75f;
	private float LaserFireRate = 1.5f; //0.1f;	//float fireRateMod = 0.0f;
	float fireRateMod = 0.0f;
	// Use this for initialization
	void Start () {
	
		fireRateMod = PlayerPrefs.GetInt("W1FireRate")  / 10.0f;
		

	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= NextFireAt) {
			fireLasers();
			NextFireAt = Time.time + FireRate;
		}	
	}
	
	void fireLasers() {
		
		
		GameObject sM = GameObject.Find ("ScreenManager");
		ScreenManager script = sM.GetComponent<ScreenManager>();
		
		if (script.currentScreen == 2) {
		
			int currentWeapon = PlayerPrefs.GetInt("CurrentWeapon");
				
			if (currentWeapon == 3) { // laser
				LaserGun lG = laser.GetComponent<LaserGun>();
				//lG.setStats((float)PlayerPrefs.GetInt("W1Damage") * 3, (float)PlayerPrefs.GetInt("W1Range"), LaserFireRate - fireRateMod, 0.5f);
				lG.mute = true;	
				lG.fire();
					
				NextFireAt = Time.time + LaserFireRate - fireRateMod;
				
				//GameObject.Instantiate(laser, transform.position, Quaternion.identity);
				//NextFireAt = Time.time + LaserFireRate -  fireRateMod;
				
			}
			
			if (currentWeapon == 1) { // scatter
				GameObject.Instantiate(tripleShot, transform.position, Quaternion.identity);
				NextFireAt = Time.time + TripleFireRate -  fireRateMod;
			}
			
			if (currentWeapon == 2) { // melee
				GameObject.Instantiate(melee, transform.position, Quaternion.identity);
				NextFireAt = Time.time + MeleeFireRate - fireRateMod;
			}
			
			if (currentWeapon == 4) {
	       		 GameObject.Instantiate(mortarShot, transform.position, Quaternion.identity);
				NextFireAt = Time.time - fireRateMod;
			
			}		
		
		
		}
		
		
	}
	
}
