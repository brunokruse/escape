using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour {
	
	public ParticleSystem chargeBeam;
	public ParticleSystem fireBeam;
	public int status = 0; // 0: ready, 1: charging, 2: firing
	public float damage;
	public float fireDuration;
	public float chargeDuration;
	public float range;
	//public Color chargeColor;
	//public Color fireColor;
	public float startTime;
	
	public AudioClip chargeSound;
	public AudioClip fireSound;
	
	public bool mute;
	
	void Start () {
		
		//setStats (25, 0, 2f, .5f);
		
		chargeBeam.renderer.material.SetColor ("_TintColor", new Color(0, .5f,0, 0));
		fireBeam.renderer.material.SetColor ("_TintColor", new Color(.5f, 0,0, 0));
		chargeBeam.Simulate(10);
		fireBeam.Simulate(10);
		chargeBeam.Play();
		fireBeam.Play();
		
		//status = 0;
		//fire ();
	}
	
	void Update () {
		switch(status){
		case 0:
			break;
		case 1:
			float chargeFactor = (Time.time-startTime)/chargeDuration;
			chargeBeam.renderer.material.SetColor ("_TintColor", new Color(0, .5f,0, chargeFactor*chargeFactor*chargeFactor*chargeFactor));
			if(chargeFactor >= 1){
				status = 2;
				if(!mute)
					AudioSource.PlayClipAtPoint(fireSound, transform.position);
				chargeBeam.renderer.material.SetColor ("_TintColor", new Color(0, .5f,0, 0));
			}
			break;
		case 2:
			float fireFactor;
			if(fireDuration==0) fireFactor = 0;
			else fireFactor = (Time.time-startTime-chargeDuration)/fireDuration;
			fireBeam.renderer.material.SetColor ("_TintColor", new Color(.5f, 0,0, 1-fireFactor));
			if(fireFactor >= 1){
				status = 0;
				fireBeam.renderer.material.SetColor ("_TintColor", new Color(.5f, 0,0, 0));
			}
			break;
		default:
			break;			
		}
	}
	
	public void setStats(float _damage, float _range, float _chargeDuration, float _fireDuration){
		damage = _damage;
		fireDuration = _fireDuration;
		chargeDuration = _chargeDuration;
		range = _range;
		fireBeam.startSpeed = 8+(2*range);
		fireBeam.startLifetime = (40+(10*range))/(8+(2*range));
		fireBeam.emissionRate = 20+(5*range);
		chargeBeam.startSpeed = 80+(20*range);
		chargeBeam.startLifetime = (40+(10*range))/(80+(20*range));
		chargeBeam.emissionRate = 500+(125*range);
	}
	
	public void fire(){
		if(status == 0){
			if(!mute)
				AudioSource.PlayClipAtPoint(chargeSound, transform.position);
			chargeBeam.renderer.material.SetColor ("_TintColor", new Color(0, .5f,0, 0));
			fireBeam.renderer.material.SetColor ("_TintColor", new Color(.5f, 0,0, 0));
			status = 1;
			startTime = Time.time;
		}
	}
}
