using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {
	
	public float speed;
	public bool fadeInOnStart;
	public bool fadeOutOnStart;
	
	// Use this for initialization
	void Start () {
		tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
		if (fadeInOnStart)
			mySprite.color = new Color(255,255,255, 0.0f);
		
		if (fadeOutOnStart)
			mySprite.color = new Color(255,255,255, 1.0f);

		//speed = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (fadeInOnStart) {
			fadeIn(speed);
		}
		
		if (fadeOutOnStart) {
			fadeOut(speed);
		}
		
	}
	
	void fadeIn (float amt) {
		tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
		mySprite.color += new Color(255,255,255, amt);
		
	}
	
	void fadeOut (float amt) {
		tk2dSprite mySprite = gameObject.GetComponent<tk2dSprite>();
		mySprite.color -= new Color(255,255,255, amt);
		
	}
}
