using UnityEngine;
using System.Collections;

public class timeball : MonoBehaviour {
	float speed = 0.05f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3 (Time.deltaTime * speed, 0, 0));
	}

	void changeSpeed(float x) {
		speed = 3.66f / 30f * x;
	}

	void go(float x) {
		if (x > 0)
			transform.Translate (new Vector3 (0.14f, 0, 0));
		else transform.Translate (new Vector3 (-0.14f, 0, 0));
	}

}
