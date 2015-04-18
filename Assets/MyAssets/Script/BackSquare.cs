using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BackSquare : MonoBehaviour {


	public GameObject[] backSquares;
	public EaseType spinEaseType = EaseType.EaseInOutExpo;
	public float spinDuration = 2f;

	// Use this for initialization
	void Awake () {
		for( int i = 0 ; i < backSquares.Length ; ++i )
		{
//			HOTween.To( backSquares[i].transform,
//			           spinDuration ,
//			           new TweenParms()
//			           .Prop( "eulerAngles" , new Vector3( 0 , 0 , 360f ) , true)
//			           .Ease( spinEaseType )
//			           .Loops(999 , LoopType.Incremental) ) ;
			
		}

	}
	
	// Update is called once per frame
	void Update () {
		for ( int i = 0 ; i < backSquares.Length ; ++ i )
		{
			backSquares[i].transform.eulerAngles += new Vector3( 0 , 0 , 360f / spinDuration * Time.deltaTime );
		}
	}

	public void SetColor( float c )
	{
		Color col = new Color( c , c , c , Global.BACK_SQUARE_APLAH );
		for ( int i = 0 ; i < backSquares.Length ; ++ i )
		{
			backSquares[i].GetComponent<tk2dSprite>().color = col ; 
		}

	}

}
