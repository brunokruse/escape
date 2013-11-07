using UnityEngine;
using System.Collections;

public class vent : MonoBehaviour {
	float counter;
	float duration = 2f;
	float scaleAmount;
	float initialDelay = .5f;
	bool delayed = true;
	tk2dSprite mySprite;

	// Use this for initialization
	void Start () {
		scaleAmount = 0;
		mySprite = gameObject.GetComponent<tk2dSprite>();
		mySprite.color = new Color(255,255,255, 0.0f);
		//Debug.Log("STEAM");
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if(delayed){
			if(counter>=initialDelay){ 
				counter = .1f;
				delayed = false;
			}
		}
		else{
			scaleAmount += .005f*counter;
			transform.localScale = new Vector3(.5f+scaleAmount*1f, .5f+scaleAmount, 1);
			if(counter>=duration+.1f) Destroy(gameObject);
			mySprite.color = new Color(255,255,255, 1f-(counter/duration));
		}
	}
}
