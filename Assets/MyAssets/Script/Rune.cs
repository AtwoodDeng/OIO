using UnityEngine;
using System.Collections;
using Holoville.HOTween;

[RequireComponent( typeof( LineRenderer ) )]
public class Rune : MonoBehaviour {
	
	LineRenderer lineRenderer;
	public PointCloudGestureTemplate GestureTemplate;
	public tk2dSprite sprite;

	public Color runeColor;
	public ParticleSystem activeEffect;

	float wakeTime;

	void Awake()
	{
//		lineRenderer = GetComponent<LineRenderer>();
//		lineRenderer.useWorldSpace = false;
		sprite = GetComponent<tk2dSprite>();
		sprite.SetSprite( GestureTemplate.name );
		sprite.color = runeColor;
		OnCreate();
		wakeTime = Time.time;
	}
	
	void Start()
	{
//		if( GestureTemplate )
//			Render( GestureTemplate );
	}
	
	public void Blink()
	{
		animation.Stop();
		animation.Play();
	}
	
	public bool Render( PointCloudGestureTemplate template )
	{
		if( template.PointCount < 2 )
			return false;
		
		lineRenderer.SetVertexCount( template.PointCount );
		
		for( int i = 0; i < template.PointCount; ++i )
			lineRenderer.SetPosition( i, template.GetPosition( i ) );
		
		return true;
	}
	
	public float createTime = 2f;
	public void OnCreate()
	{
		Color col = runeColor;
		col.a = 0;
		runeColor = col;
		col.a = 1f;
		HOTween.To( this
		           , createTime
		           , "runeColor"
		           , col );

		Vector3 scale = transform.localScale;
		transform.localScale = Vector3.zero;
		HOTween.To( transform
		           , createTime
		           , "localScale"
		           , scale
		           , false
		           , EaseType.EaseOutBack
		           , 0);


	}

	public float deadTime = 2f;
	public void OnDead()
	{
		
		activeEffect.enableEmission = false;
		Color col = runeColor;
		col.a = 0;
		HOTween.To( this.GetComponent<tk2dSprite>()
		           , deadTime
		           , "color"
		           , col );


		HOTween.To( transform
		           , deadTime
		           , "localScale"
		           , Vector3.zero
		           , false
		           , EaseType.EaseInBack
		           , 0);
	}

	public void Update()
	{
	}


	public int activeParNum = 20;

	public void OnActive()
	{
		activeEffect.startColor = runeColor;
		activeEffect.Emit( activeParNum );
	}

}
