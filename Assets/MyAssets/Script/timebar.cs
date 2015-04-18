using UnityEngine;
using System.Collections;

public class timebar : MonoBehaviour {
	float x,y;
	int type = 0;
	public GameObject logic;
	public float speed;
	public BackAll back;
	public float minCol;
	public float maxCol;
	public float changeX = 0.1f;

	
//	public float speed;
	float sendspeed;
	public GameObject ball;

	
	// Use this for initialization
	void Start () {
		type = 0;
		x = transform.localScale.x;
		y = x;
		
		sendspeed = speed;
		ball.SendMessage ("changeSpeed", sendspeed);
	}

	void shot() {
		x -= 0.05f;
		transform.localScale = new Vector3 (x, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (type < 1) {
			x -= Time.deltaTime * (y / 30) * speed;
			transform.localScale = new Vector3 (x, 1, 1);
			if (x < 0.001) {
				type = 1;
				logic.SendMessage ("change");
				ball.SendMessage ("changeSpeed", sendspeed * 30f / speed);
			}
		}
		else if (type == 1) {
			x += Time.deltaTime * y;
			transform.localScale = new Vector3 (x, 1, 1);
			if (x > 1) {
				type = 0;
//				logic.SendMessage ("change");
				
				sendspeed *= -1f;
				ball.SendMessage ("changeSpeed", sendspeed);
			}

		}
		if ( Game.state == Game.BWState.Black )
		{
			if ( x < 1f && x > 0 )
			back.SetColor( x * (maxCol-minCol) + minCol );
		}
		if ( Game.state == Game.BWState.White )
		{
			if ( x < 1f && x > 0 )
			back.SetColor( - x * (maxCol-minCol) + maxCol );
		}
	}
}
