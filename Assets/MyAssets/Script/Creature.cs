using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class Creature : MonoBehaviour {

	public bool ifDecorate = false;
	public GameObject[] decPrafabs;
	public int decNumber = 6;
	public float decRadius = 1f;
	public List<GameObject> decs = new List<GameObject>();
	public int decNumberReal;

	public float scale = 1.0f;
	public GameObject eye;
	public GameObject body;
	public GameObject buttomLight;

	public GameObject[] trail;

	public float eyeRadius = 0.07f;
	public float eyeClose = 0.02f;

	public Color[] buttomColors;

	public enum LifeState
	{
		Life,
		Dead
	}
	public LifeState state = LifeState.Life;


//	public WeaponTrail trail;
	// Use this for initialization
	void Awake () {
		//set decorate
		if ( ifDecorate && ifDecorate != null )
		{
			decNumberReal = UnityEngine.Random.Range( decNumber / 2 , decNumber );
			float diffAngle = UnityEngine.Random.Range( 0 , 2 * Mathf.PI );

			for ( int i = 0 ; i < decNumberReal ; ++ i )
			{
				GameObject dec = Instantiate( decPrafabs[UnityEngine.Random.Range(0,decPrafabs.Length)] ) as GameObject;
				dec.transform.parent = transform;
				
				float ang = i  * 2 * Mathf.PI / decNumberReal;
				Vector3 pos = new Vector3( decRadius * Mathf.Cos( ang ) , decRadius * Mathf.Sin( ang ) , 0 );
				dec.transform.localPosition = pos;
				dec.transform.localEulerAngles = new Vector3( 0 , 0 , ( ang + diffAngle ) * 360f / 2 / Mathf.PI );

				decs.Add( dec );
			}
		}

		//set basic
		forwardDiffRange = UnityEngine.Random.Range( - forwardDiffRange , forwardDiffRange );

		transform.localScale *= scale;

		forwardDir = UnityEngine.Random.Range( 0 , 2f * Mathf.PI );

		//set buttom color
		Color buttomColor = buttomColors[ UnityEngine.Random.Range( 0 , buttomColors.Length ) ];
		buttomColor.r += UnityEngine.Random.Range( -0.05f , 0.05f );
		buttomColor.g += UnityEngine.Random.Range( -0.05f , 0.05f );
		buttomColor.b += UnityEngine.Random.Range( -0.05f , 0.05f );
		buttomLight.GetComponent<tk2dSprite>().color = buttomColor;

		//show
		OnInit();
	}

	public float initTime = 2f;
	void OnInit()
	{
		state = LifeState.Life;
		showSprite( body.GetComponent<tk2dSprite>() , initTime );
		showSprite( eye.GetComponent<tk2dSprite>() , initTime );
		foreach( GameObject dec in decs )
		{
			showSprite( dec.GetComponent<tk2dSprite>() , initTime );
		}

	}

	public tk2dSprite createLight;
	public ParticleSystem createBubble;
	public float createTime = 2f;
	public float CreateIntense = 0.01f;
	void OnCreate( string isOri )
	{
		if ( isOri.Equals(bool.TrueString) )
		{
			Color col = createLight.color;
			col.a = 0;
			createLight.color = col;
			col.a = 3f;
			HOTween.To( createLight
			           , createTime /2
			           , new TweenParms().Prop( "color" , col ) 
			           .Loops(2,LoopType.YoyoInverse));
			createBubble.Emit( 20 );
		}
		else 
		{
			Vector3 force = CreateIntense * new Vector3( UnityEngine.Random.Range( -1f , 1f ) , UnityEngine.Random.Range( -1f , 1f ) , 0 );
			rigidbody.AddForce( force , ForceMode.Impulse );
			Color col = createLight.color;
			col.a = 0;
			createLight.color = col;
			col.a = 3f;
			HOTween.To( createLight
			           , createTime /2 
			           , new TweenParms().Prop( "color" , col ) 
			           .Loops(2,LoopType.YoyoInverse));
			createBubble.Emit( 20 );
		}
	}

	// Update is called once per frame
	void Update () {

		MoveForward();

		MoveBody();

		MoveTrail();

	}



	[HideInInspector]public float forwardDir=0;
	[HideInInspector]public float forwardDiff=0;
	public float forwardChangePoss=0.02f;
	public float forwardDiffRange=1f;

	public float LimitRangeX = 1f;
	public float LimitRangeY = 1f;


	public float forwardIntense = 1f;

	void MoveForward()
	{
		if ( state != LifeState.Life )
			return;
		//check if in limit range
		if ( transform.localPosition.x > LimitRangeX )
		{
			forwardDir = Mathf.PI + UnityEngine.Random.Range( -0.3f , 0.3f );
		}
		if ( transform.localPosition.x < - LimitRangeX )
		{
			forwardDir = UnityEngine.Random.Range( -0.3f , 0.3f );
		}
		if ( transform.localPosition.y > LimitRangeY )
		{
			forwardDir = 3 * Mathf.PI / 2 + UnityEngine.Random.Range( -0.3f , 0.3f );
		}
		if ( transform.localPosition.y < - LimitRangeY )
		{
			forwardDir = Mathf.PI / 2 + UnityEngine.Random.Range( -0.3f , 0.3f );
		}
		//if randomly change the direction
		if ( UnityEngine.Random.Range( 0 , 1f ) < forwardChangePoss )
		{
			forwardDiff = UnityEngine.Random.Range( -forwardDiffRange , forwardDiffRange );
		}

		forwardDir += forwardDiff;

		Vector3 force = forwardIntense * new Vector3( Mathf.Cos( forwardDir ) , Mathf.Sin( forwardDir ));

		this.rigidbody.AddForce( force , ForceMode.Impulse );

		//update eye
		Vector3 eyePos = eye.transform.localPosition;
		Vector3 eyeDesPos = new Vector3( Mathf.Cos( forwardDir ) * eyeRadius * scale , Mathf.Sin( forwardDir ) * eyeRadius * scale );
		Vector3 eyeFinalPos = eyePos * ( 1f - eyeClose ) + eyeDesPos * eyeClose ;
		eye.transform.localPosition = eyeFinalPos;
	}

	[HideInInspector]public bool isBodyMoving = false;
	public float bodyMovePos = 0.03f;
	public float bodyMoveDuration = 1f;
	public float bodyMoveTime = 1.33f;
	public EaseType bodyMoveEase = EaseType.EaseOutQuint; 
	void MoveBody()
	{
		if ( state != LifeState.Life )
			return;
		if ( !isBodyMoving && UnityEngine.Random.Range( 0 , 1f ) < bodyMovePos )
		{
			HOTween.To( body.transform
			           , bodyMoveDuration / 2f 
			           , new TweenParms().Prop( "localScale" , body.transform.localScale * bodyMoveTime  )
			           .Loops(2,LoopType.YoyoInverse)
			           .Ease( bodyMoveEase )
			           .OnComplete( MoveBodyComplete ) );

			for ( int i = 0 ; i < decNumberReal ; ++ i )
			{
				HOTween.To( decs[i].transform
				           , bodyMoveDuration / 2f / decNumberReal * 2.7f
				           , new TweenParms().Prop( "localScale" , decs[i].transform.localScale * bodyMoveTime )
				           .Loops(2,LoopType.YoyoInverse)
				           .Ease( bodyMoveEase )
				           .Delay( i * bodyMoveDuration / 2f / decNumberReal ) );
			}
			isBodyMoving = true;
		}
	}


	void MoveBodyComplete()
	{
		isBodyMoving = false;
	}

	[HideInInspector]public float trailAng;
	public float trailAngDiff = 0.1f ;
	public float trailLength = 0.2f;
	void MoveTrail()
	{
		if ( state != LifeState.Life )
			return;

		trailAng += trailAngDiff;
		for ( int i = 0 ; i < trail.Length ; ++i )
		{
			Vector3 pos = new Vector3( trailLength * scale * Mathf.Sin( trailAng + i * 2 * Mathf.PI / trail.Length ) * Mathf.Sin( forwardDir )
			                          , -trailLength * scale * Mathf.Sin( trailAng + i * 2 * Mathf.PI / trail.Length ) * Mathf.Cos( forwardDir ) , 0 );
			trail[i].transform.localPosition = pos;
		}

	}

	public float DeadTime = 2f;
	void OnDead()
	{
		rigidbody.drag = 999f;
		fadeSprite( body.GetComponent<tk2dSprite>() , DeadTime);
		fadeSprite( eye.GetComponent<tk2dSprite>() , DeadTime);
		foreach( GameObject dec in decs )
		{
			fadeSprite( dec.GetComponent<tk2dSprite>() ,DeadTime );
		}
		fadeSprite( buttomLight.GetComponent<tk2dSprite>() ,DeadTime);

		state = LifeState.Dead;
	}
			
	void fadeSprite( tk2dSprite sprite , float time)
	{
		if ( sprite == null )
			return;
		Color col = sprite.color;
		col.a = 1f;
		sprite.color = col;
		col.a = 0;
		HOTween.To( sprite
		           , time
		           , "color"
		           , col );
	}

	void showSprite( tk2dSprite sprite , float time)
	{
		if ( sprite == null )
			return;
		Color col = sprite.color;
		col.a = 0f;
		sprite.color = col;
		col.a = 1f;
		HOTween.To( sprite
		           , time
		           , "color"
		           , col );
	}


}
