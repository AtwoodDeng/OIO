using UnityEngine;
using System.Collections;

public class testGes : MonoBehaviour {

	public GameObject logic;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	int gesc = 0, gestime = 0, gesweapon = 0;
	void OnRec( PointCloudGesture gesture ) {
		if (gesture.RecognizedTemplate.name == "ges2") {
			gesc++;
			logic.SendMessage ("gescopy", new Vector2(gesture.Position.x / 150f - 3.2f, gesture.Position.y / 150f - 1.0f));
		}
		else if (gesture.RecognizedTemplate.name == "ges3") {
			gestime ++;
			logic.SendMessage ("gestime");
		}
		else if (gesture.RecognizedTemplate.name == "ges1") {
			gesweapon ++;
			logic.SendMessage ("gesweapon");
		}
	}

}
