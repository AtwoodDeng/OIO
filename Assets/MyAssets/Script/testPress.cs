using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class testPress : MonoBehaviour {
	public bool enable = false;
	float x, y, z;
	bool ifPress = false;
	public GameObject fireballdown;
	public GameObject logic;
	public GameObject ta;
	public string textname;
	GameObject down;
	bool newdown = false;
	int num = 5;
	public Game.BWState fixState;
	public tk2dTextMesh text ;

	Vector3 initPos;

	public GameObject exposionPrefab;
	public ParticleSystem touchEffect;

//	void OnGUI()
//	{
//		if (textname == "py1") {
//						GUI.color = Color.white;
//						GUI.skin.label.fontSize = 20;
//						GUI.Label (new Rect (30, 200, 200, 40), num.ToString ());
//				} else {
//						GUI.color = Color.white;
//						GUI.skin.label.fontSize = 20;
//						GUI.Label (new Rect (590, 200, 200, 40), num.ToString ());
//				}
//	}
	
	// Use this for initialization
	public tk2dSpriteDefinition def;
	public float pixelWidth;
	public float pixelHeight;
	public float spriteWidth;
	public float spriteHeight;
	public float spriteScale = 0.01f;
	void Awake () {
			x = transform.localPosition.x;
			y = transform.localPosition.y;
			z = transform.localPosition.z;

		initPos = transform.position;

		 def = GetComponent<tk2dSprite>().CurrentSprite;
		float w = def.untrimmedBoundsData[1].x;
		float h = def.untrimmedBoundsData[1].y;
		
		// Calculate dimensions in pixel units
		 pixelWidth = w / def.texelSize.x;
		 pixelHeight = h / def.texelSize.y;
		spriteWidth = pixelWidth * GetComponent<tk2dSprite>().scale.x * spriteScale;
		spriteHeight = pixelHeight * GetComponent<tk2dSprite>().scale.y * spriteScale;


		Debug.Log("s width" + spriteWidth );

	}

	public float textTime = 1f;
	void addWeapon () {
			num ++;
		text.text = num.ToString();
			
		Color col = text.color;
		col.a = 0;
		text.color = col;
		col.a = 1;
		HOTween.To( text 
		           , textTime
		           , new TweenParms()
		           .Prop( "color" , col , false )
		           .Loops( 2 , LoopType.YoyoInverse )
		           );

	}

	void setEnable() {
		enable = true;
	}
	
	void setunEnable() {
		enable = false;
	}
	// Update is called once per frame
	void Update () {


//		if (Input.GetMouseButtonDown (0) ){
//			Ray ray = new Ray( Game.mainCamera.transform.position , Game.mainCamera.transform.position - Game.mainCamera.transform.position );
//				RaycastHit raycastHit = new RaycastHit();
//				if ( Physics.Raycast( ray , out raycastHit )  && raycastHit.collider.gameObject == gameObject )
//				ifPress = true;
//
//		}

//		if (Input.GetMouseButtonDown (0) && !ifPress) {
//			if (Vector3.Distance(new Vector3(Input.mousePosition.x / 150f - 2.2f ,Input.mousePosition.y / 150f - 0.26f, z),
//			                     new Vector3(x, y, z)) > 1f) Debug.Log (new Vector3(Input.mousePosition.x / 150f - 2.2f ,Input.mousePosition.y / 150f - 0.26f, z));
//			else {
//				if ((!enable) || num < 0.1f)
//					return;
//				ifPress = true;
//			}
//		}
//		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && enable && num > 0) { 
//			// && enable && num > 0
//			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;  
//			//transform.Translate(touchDeltaPosition.x / 100f, touchDeltaPosition.y / 100f, 0);
//			if (!ifPress) {
//				ifPress = true;
//				if (!newdown) {
//					GameObject newo = Instantiate (fireballdown, new Vector3 (x, y, z), Quaternion.identity) as GameObject;
//					down = newo;
//					newdown = true;
//				}
//			}
//			else { 
//				transform.Translate(touchDeltaPosition.x / 80f, touchDeltaPosition.y / 80f, 0);
//			}
//		}  
//
//		int i = 0;
//		while (i < Input.touchCount) {  
//			if (Input.GetTouch(i).phase == TouchPhase.Ended && ifPress)  {
//				ifPress = false;
//				logic.SendMessage("destoryLife", new Vector2(transform.position.x, transform.position.y));
//				transform.position = new Vector3(x,y,z);
//				DestroyObject(down, 0.1f);
//				newdown = false;
//				//Debug.Log (gesture.Position);
//				num --;
//			}
//			
//			++i;  
//		}  


	}



	public float dragScale = 0.007f;
	public float resetTime = 0.33f;
	public EaseType resetEase;

	public GameObject creaCursor;
	public GameObject UICursor;
	public float touchScaleX;
	public float touchScaleY;
	public float oriJudgeScale = 0.5f;
	void OnDrag( DragGesture gesture )
	{
		if ( Game.state != fixState )
			return;
		ContinuousGesturePhase phase = gesture.Phase;
		Debug.Log( "phase" + phase.ToString() );
//
//		Vector3 creaPos = gesture.Position;
//
//		creaPos.x -= Screen.width / 2;
//		creaPos.y -= Screen.height / 2 ;
//		creaPos.x *= Camera.main.fieldOfView / ( Screen.height / 2 ) * touchScaleX ;
//		creaPos.y *= Camera.main.fieldOfView / ( Screen.height / 2 ) * touchScaleY ;
//		creaPos.z = 1f;
//
//		creaCursor.transform.position = creaPos;

		if (ifPress) {
			Vector3 UIPos = Game.gesturePos2UIPos( gesture.StartPosition );
			float dis = Vector3.Distance( UIPos  , initPos );
			Debug.Log( "ifPressed " + dis.ToString());
			Debug.Log( "size " + (spriteWidth / 2) );
			//check if drag
			if ( dis < spriteWidth / 2 )
			{
				Debug.Log( "distanced " );
//				Debug.Log ( "move " + gesture.DeltaMove.ToString() );
//				transform.position += new Vector3 (gesture.DeltaMove.x * dragScale , gesture.DeltaMove.y * dragScale , 0);
				transform.position = Game.gesturePos2UIPos( gesture.Position );

			}
//				if (!newdown) {
//					//GameObject newo = Instantiate (fireballdown, new Vector3 (x, y, z), Quaternion.identity) as GameObject;
//					//down = newo;
//					//newdown = true;
//				}
		}

		if ( phase.ToString().Equals( "Started" ) )
		{
			Debug.Log( "ges" + gesture.Position + gesture.StartPosition );
			Vector3 UIPos = Game.gesturePos2UIPos( gesture.Position );
			Debug.Log( "began drag " + Vector3.Distance( UIPos , initPos ) + " < " + (spriteWidth / 2 ) );
//			Ray ray = new Ray( Game.mainCamera.transform.position , UIPos - Game.mainCamera.transform.position );
//			RaycastHit raycastHit = new RaycastHit();
//			if ( Physics.Raycast( ray , out raycastHit )  && raycastHit.collider.gameObject == gameObject )
//			{
			if ( Vector3.Distance( UIPos , initPos ) < spriteWidth / 2 && num > 0)
			{
				touchEffect.Emit( 30 );
				ifPress = true;
			}
		}

		if (phase.ToString ().Equals ("Ended") && ifPress) {
			ifPress = false;
//			DestroyObject(down, 0.1f);
//			newdown = false;
			//Debug.Log (gesture.Position);
//			logic.SendMessage("destoryLife", new Vector2(x + gesture.TotalMove.x / 150f, y + gesture.TotalMove.y / 150f));
			HOTween.To( transform
			           , resetTime
			           , "position"
			           , initPos
			           , false
			           , resetEase
			           , 0 );
			logic.SendMessage( "destoryLife" , Game.gesturePos2CreaPos( gesture.Position ) );
			num--;
			text.text = num.ToString();
			Color col = text.color;
			col.a = 0;
			text.color = col;
			col.a = 1;
			HOTween.To( text 
			           , textTime
			           , new TweenParms()
			           .Prop( "color" , col , false )
			           .Loops( 2 , LoopType.YoyoInverse )
			           );


			Vector3 gesPos = Game.gesturePos2CreaPos( gesture.Position );
			GameObject exp = Instantiate( exposionPrefab ) as GameObject;
			exp.transform.position = new Vector3( gesPos.x , gesPos.y , exp.transform.position.z );
		}

		creaCursor.transform.position = Game.gesturePos2CreaPos( gesture.Position );
		UICursor.transform.position = Game.gesturePos2UIPos( gesture.Position );
	}



}
