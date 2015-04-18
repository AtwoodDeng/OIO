using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class Game : MonoBehaviour {
	public enum BWState
	{
		Black,
		White,
		MidBlack,
		MidWhite,
	}
	static public BWState state = BWState.White;
	static public Game singlton;
	public GameObject fireball_py1;
	public GameObject fireball_py2;
	public GameObject creatureWhiteFrebrab;
	public GameObject creatureBlackFrebrab;
	public GameObject runeFrebrab;
	public GameObject runeFrebrab2;
	public GameObject runeFrebrab1;
	
	public float maxNewRuneTime;
	public float runeDeathTime;
	
	public AudioSource heal, weapon, bomb, time;
	
	float[] gesti = new float[3];
	GameObject[] runes = new GameObject[3];
	float wakeTime, randomTime;

	public GameObject timebar;
	float xx = 0,yy = 0;
	public GameObject blackField;
	public GameObject whiteField;
	public List<GameObject> whiteCreatures = new List<GameObject>();
	public List<GameObject> blackCreatures = new List<GameObject>();
	int num = 1;
	bool logic = true;// 0: god is player1, 1: god is player2

	public float createRange = 1.2f;
	void gescopy(Vector2 pos) {
		if (gesti[1] <= 0) return;
		Vector3 posRune =  runes[1].transform.position;
		if ( state == BWState.White )
		{
			for (int i = whiteCreatures.Count - 1; i >=0; --i) {
				GameObject go = whiteCreatures[i];

				if (Vector3.Distance( go.transform.position , posRune ) < createRange) {
					GameObject newCreature = Instantiate (creatureWhiteFrebrab, new Vector3(go.transform.position.x, go.transform.position.y, 1.0f), Quaternion.identity) as GameObject;
					whiteCreatures.Add (newCreature);
					newCreature.transform.parent = whiteField.transform;
					go.SendMessage("OnCreate" , bool.TrueString);
					newCreature.SendMessage( "OnCreate" , bool.FalseString );
					gesti[1] = -1f;
					runes[1].SendMessage ("OnDead");
					
					runes[1].SendMessage ("OnActive");
					heal.Play();
				}
			}
		}

		if ( state == BWState.Black )
		{
			for (int i = blackCreatures.Count - 1; i >=0; --i) {
				GameObject go = blackCreatures[i];
				if (Vector3.Distance( go.transform.position , posRune ) < createRange ) {
					GameObject newCreature = Instantiate (creatureBlackFrebrab, new Vector3(go.transform.position.x, go.transform.position.y, 1.0f), Quaternion.identity) as GameObject;
					blackCreatures.Add (newCreature);
					newCreature.transform.parent = blackField.transform;
					go.SendMessage("OnCreate" , bool.TrueString);
					newCreature.SendMessage( "OnCreate" , bool.FalseString );
					gesti[1] = -1f;
					runes[1].SendMessage ("OnDead");
					runes[1].SendMessage ("OnActive");
					heal.Play();
				}
			}
		}
	}

		void gestime() {
			if (gesti [2] > 0) {
				timebar.SendMessage ("shot");
				gesti[2] = -1f;
				runes[2].SendMessage ("OnDead");
				time.Play();
			
				runes[2].SendMessage ("OnActive");
			}
	}

	void gesweapon() {
		if (gesti[0] <= 0) return;
		if (state == BWState.White ) {
			fireball_py2.SendMessage ("addWeapon");
		} 
		if ( state == BWState.Black ) {
			fireball_py1.SendMessage ("addWeapon");
		}
		gesti[0] = -1f;
		runes[0].SendMessage ("OnActive");
		runes[0].SendMessage ("OnDead");
		weapon.Play();
	}

	public float desRange = 1.2f;

	void destoryLife(Vector2 pos )
	{
		destoryLife( new Vector3( pos.x ,pos.y , 1f ));
	}

	void destoryLife(Vector3 pos ) {
		xx = pos.x;
		yy = pos.y;
		Debug.Log (pos);
		Debug.Log ("=================");
		if ( state == BWState.White )
		{
			for (int i = whiteCreatures.Count - 1; i >=0; --i) {
				GameObject go = whiteCreatures[i];
				float dis = Vector3.Distance(go.transform.position , pos);
				if ( dis < desRange ) {
					whiteCreatures.Remove(go);
					go.SendMessage("OnDead");
					Debug.Log( "des " );
//					DestroyObject(go, 0.1f);
				}
			}
		}

		if ( state == BWState.Black )
		{
			for (int i = blackCreatures.Count - 1; i >=0; --i) {
				GameObject go = blackCreatures[i];
				float dis = Vector3.Distance(go.transform.position , pos);
				if ( dis < desRange ) {
					blackCreatures.Remove(go);
					go.SendMessage("OnDead");
					Debug.Log( "des " );
					//					DestroyObject(go, 0.1f);
				}
			}
		}
	}

	public float CreateRangeX = 2f;
	public float CreateRangeY = 2f;
	public int CreateNum = 50;
	// Use this for initialization
	void Start () {
		num = 1;
		fireball_py1.SendMessage("setEnable");
		fireball_py2.SendMessage("setunEnable");
//		GameObject origin = GameObject.FindGameObjectWithTag("creature");
		for (int i = -CreateNum/2; i < CreateNum/2; ++i) {
			GameObject newCreature = Instantiate (creatureWhiteFrebrab, new Vector3(UnityEngine.Random.Range( -CreateRangeX , CreateRangeX ),   
			                                                                   UnityEngine.Random.Range( -CreateRangeY , CreateRangeY ), 1.0f), Quaternion.identity) as GameObject;
			newCreature.transform.parent = whiteField.transform;
			whiteCreatures.Add (newCreature);
		}
		for (int i = -CreateNum/2; i < CreateNum/2; ++i) {
			GameObject newCreature = Instantiate (creatureBlackFrebrab, new Vector3(UnityEngine.Random.Range( -CreateRangeX , CreateRangeX ),   
			                                                                   UnityEngine.Random.Range( -CreateRangeY , CreateRangeY ), 1.0f), Quaternion.identity) as GameObject;
			newCreature.transform.parent = blackField.transform;
			blackCreatures.Add (newCreature);
		}
		singlton = this;
		blackField.SetActive( false );

		begin.color = new Color( 1f ,1f ,1f ,1f );
		HOTween.To( begin
		           , winTime/2
		           , "color"
		           , new Color( 1f , 1f , 1f , 0 )
		           , false
		           , EaseType.Linear
		           , winTime );
	}

//	void change () {
//		if (logic) {
//			logic = false;
//			fireball_py2.SendMessage("setEnable");
//			fireball_py1.SendMessage("setunEnable");
//		}
//		else {
//			logic = true;
//			fireball_py1.SendMessage("setEnable");
//			fireball_py2.SendMessage("setunEnable");
//		}
//	}

	static public Vector3 mousePos;
	static public Vector3 UImousePos;
	static public Vector3 creaMousePos;
	public float mousePosScale = 0.012f;
	static public Camera mainCamera;
	public GameObject cursor;
	public GameObject touchCursor;

	static public Vector3 touchPos;
	// Update is called once per frame
//	void Update () {
//		
//		float speed = 0.1F; 
//		
//		if ( mainCamera == null )
//			mainCamera = Camera.main;
//
//		mousePos = Input.mousePosition;
//		mousePos.x -= Screen.width / 2;
//		mousePos.y -= Screen.height / 2 ;
//		mousePos.x *= mainCamera.fieldOfView / ( Screen.height / 2 ) * mousePosScale ;
//		mousePos.y *= mainCamera.fieldOfView / ( Screen.height / 2 ) * mousePosScale ;
//		mousePos += mainCamera.transform.position;
//		mousePos.z = 0;
//		UImousePos = mousePos;
//
////		touchPos = new Vector3( Input.GetTouch(0).position.x , Input.GetTouch(0).position.y , 0) ;
////		touchPos.x -= Screen.width / 2;
////		touchPos.y -= Screen.height / 2 ;
////		touchPos.x *= mainCamera.fieldOfView / ( Screen.height / 2 ) * touchScaleX ;
////		touchPos.y *= mainCamera.fieldOfView / ( Screen.height / 2 ) * touchScaleY ;
////		touchPos.z = 1f;
////
//		creaMousePos = 2 * mousePos - mainCamera.transform.position;
//
//	}

//	Vector3 gesturePos;
//	void OnDrag( DragGesture gesture )
//	{
//		gesturePos = gesture.Position;
//
//	}

	void OnGUI()
	{
//		GUILayout.TextField( "touchScaleX " + touchScaleX.ToString() );
//		touchScaleX = GUILayout.HorizontalSlider( touchScaleX , 0 , 100f );
//		GUILayout.TextField( "scaleY " + touchScaleY.ToString() );
//		touchScaleY = GUILayout.HorizontalSlider( touchScaleY , 0 , 100f );

	}
	
	static public float touchScaleX = 0.024f;
	static public float touchScaleY = 0.024f;
	public static Vector3 gesturePos2CreaPos( Vector3 ges )
	{
		Vector3 creaPos = ges;
		
		Camera mainCamera = Camera.main;
		creaPos.x -= Screen.width / 2;
		creaPos.y -= Screen.height / 2 ;
		creaPos.x *= mainCamera.fieldOfView / ( Screen.height / 2 ) * touchScaleX ;
		creaPos.y *= mainCamera.fieldOfView / ( Screen.height / 2 ) * touchScaleY ;

		creaPos += mainCamera.transform.position;
		creaPos.z = 1f;

		return creaPos;
	}
	static public float UIPosScale = 0.012f;
	public static Vector3 gesturePos2UIPos( Vector3 ges )
	{

		Vector3 UIPos = ges;
		Camera mainCamera = Camera.main;
		UIPos.x -= Screen.width / 2;
		UIPos.y -= Screen.height / 2 ;
		UIPos.x *= mainCamera.fieldOfView / ( Screen.height / 2 ) * UIPosScale ;
		UIPos.y *= mainCamera.fieldOfView / ( Screen.height / 2 ) * UIPosScale ;
		UIPos += mainCamera.transform.position;
		UIPos.z = 0f;
		return UIPos;
	}
	public tk2dSprite coverBroad;
	public tk2dSprite coverWord;
	void change () {
		Debug.Log( "change " + state.ToString() );
		if ( state == BWState.White )
		{
			state = BWState.MidBlack;
			changeTimeTem = 0;

			coverBroad.color = new Color( 1f , 1f , 1f , 0f );
			HOTween.To( coverBroad
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			           new Color( 1f , 1f , 1f , 1f ) , false )
			           .OnComplete( setBlack )
			           );
			HOTween.To( coverBroad
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 1f , 1f , 1f , 0f ) , false )
			           .Delay( changeDuration / 2 + changeStop ) 
			           );
			coverWord.SetSprite( "word" + UnityEngine.Random.Range( 1 , 5).ToString() );
			if ( UnityEngine.Random.Range( 0 , 1f ) < 0.5f )
				coverWord.transform.eulerAngles += new Vector3( 0 , 0 , 180f );
			coverWord.color = new Color( 0f , 0 , 0 , 0f );
			HOTween.To( coverWord
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 0f , 0f , 0f , 1f ) , false )

			           );
			HOTween.To( coverWord
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 0f , 0f , 0f , 0f ) , false )
			           .Delay( changeDuration / 2 + changeStop ) 
			           );
			           
		}else if ( state == BWState.Black )
		{
			state = BWState.MidWhite;
			changeTimeTem = 0;

			coverBroad.color = new Color( 0f , 0f , 0f , 0f );
			HOTween.To( coverBroad
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 0f , 0f , 0f , 1f ) , false )
			           .OnComplete( SetWhite )
			           );
			HOTween.To( coverBroad
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 0f , 0f , 0f , 0f ) , false )
			           .Delay( changeDuration / 2 + changeStop )

			           );
			coverWord.color = new Color( 1f ,1f ,1f ,0f );
			coverWord.SetSprite( "word" + UnityEngine.Random.Range( 1 , 5).ToString() );
			if ( UnityEngine.Random.Range( 0 , 1f ) < 0.5f )
				coverWord.transform.eulerAngles += new Vector3( 0 , 0 , 180f );
			HOTween.To( coverWord
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 1f , 1f , 1f , 1f ) , false )

			           );
			HOTween.To( coverWord
			           , changeDuration /2 
			           , new TweenParms().Prop( "color" ,
			                        new Color( 1f , 1f , 1f , 0f ) , false )
			           .Delay( changeDuration / 2 + changeStop ) 
			           );
		}
	}

	public void SetWhite()
	{
		state = BWState.White;
		blackField.SetActive(false);
		whiteField.SetActive(true);
	}
	public void setBlack()
	{
		state = BWState.Black;
		blackField.SetActive(true);
		whiteField.SetActive(false);
	}

	public float changeDuration = 0.25f;
	public float changeStop = 0.5f;
	float changeTimeTem = 0f;

	public GameObject[] trails;
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 3; ++i) {
			if ((gesti[i] > 0) && (Time.time - gesti[i] > runeDeathTime)) {
				Debug.Log (gesti[i]);
				Debug.Log (i);
				runes[i].SendMessage ("OnDead");
				gesti[i] = -1.0f;
			}
		}
		if (Time.time - randomTime > wakeTime) {
			wakeTime = Time.time;
			randomTime = Random.Range (0.5f, maxNewRuneTime);
			int x = Random.Range (1,6);
			int jj = 0;
			int j = 0;
			int k = 0;
			while (jj < x) {
				jj ++;
				j = (j + 1) % 3;
				while (gesti[j] > 0) {
					j = (j + 1) % 3;
					++ k;
					if (k > 10) return;
				}
			}
			gesti[j] = Time.time;
			if (j == 0)
				runes[j] = Instantiate(runeFrebrab1, new Vector3(Random.Range (-2f, 2f), Random.Range (-2f, 2f), 1), Quaternion.identity) as GameObject;
			if (j == 1)
				runes[j] = Instantiate(runeFrebrab2, new Vector3(Random.Range (-2f, 2f), Random.Range (-2f, 2f), 1), Quaternion.identity) as GameObject;
			if (j == 2)
				runes[j] = Instantiate(runeFrebrab, new Vector3(Random.Range (-2f, 2f), Random.Range (-2f, 2f), 1), Quaternion.identity) as GameObject;
			float size = Random.Range (0.5f, 2.0f);
			runes[j].transform.localScale = new Vector3(size, size, size);
		}

		for( int i = 1 ;i < trails.Length ; ++ i )
		{
			if ( Input.touchCount > i )
			{
				trails[i].particleSystem.enableEmission = true;
				trails[i].transform.position = Input.touches[i].position;
			}
			else
			{
				trails[i].particleSystem.enableEmission = false;

			}
		}

		//win
		if ( whiteCreatures.Count < 4 )
		{
			
			HOTween.To( mainBack ,
			           winTime
			           , "color"
			           , new Color( 0f , 0f , 0f , 0.5f )
			           );
			HOTween.To( blackWin ,
			           winTime
			           , "color"
			           , new Color( 1f , 1f , 1f , 1f )
			           );
		}
		if ( blackCreatures.Count < 4 )
		{
			
			
			HOTween.To( mainBack ,
			           winTime
			           , "color"
			           , new Color( 0f , 0f , 0f , 0.5f )
			           );
			HOTween.To( whiteWin ,
			           winTime
			           , "color"
			           , new Color( 1f , 1f , 1f , 1f )
			           );
		}

	}

	public tk2dSprite begin;
	public tk2dSprite blackWin;
	public tk2dSprite whiteWin;
	public tk2dSprite mainBack;
			public float winTime = 4f;

//	int gesc = 0, gestime = 0, gesweapon = 0;
	void OnRec( PointCloudGesture gesture ) {
		if (gesture.RecognizedTemplate.name == "ges2") {
//			gesc++;
			Debug.Log( "ges2" );
			 gescopy( new Vector2(gesture.Position.x / 150f - 3.2f, gesture.Position.y / 150f - 1.0f));

		}
		else if (gesture.RecognizedTemplate.name == "ges3") {
//			gestime ++;
			gestime();
		}
		else if (gesture.RecognizedTemplate.name == "ges1") {
//			gesweapon ++;
			gesweapon();
		}
	}

	void OnDrag( DragGesture gesture )
	{
		ContinuousGesturePhase phase = gesture.Phase;
		Vector3 UIPos = Game.gesturePos2UIPos( gesture.StartPosition );
		trails[0].transform.position = UIPos;
		trails[0].particleSystem.enableEmission = true;
		if ( phase.ToString() == "Ended" )
			trails[0].particleSystem.enableEmission = false;

	}



}
