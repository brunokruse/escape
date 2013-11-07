using UnityEngine;
using System.Collections;

public class DamageNumberFade : MonoBehaviour {
	
	float alpha = 1.0f;
	Color colorStart;
	Color colorEnd;
	float duration = 3.0f; 
	
	// Use this for initialization
	void Start () {
		
		transform.position -= new Vector3(0.0f, -0.1f, 0.0f);
		Destroy(gameObject, 0.25f);
		//colorStart  = bossScript. color;
		//colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
