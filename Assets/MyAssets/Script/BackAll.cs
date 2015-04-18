using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class BackAll : MonoBehaviour {


	public GameObject squarePrefab;
	public int maxWidthNum = 10;
	public int maxHeightNum = 10;
	public float width = 100;
	public float height = 100;
	public float scaleTime = 0.5f;
	[HideInInspector]public Vector3 oriScale;
	[HideInInspector]public List<List<GameObject>> squares = new List<List<GameObject>>();
	public tk2dSprite broad;

	public float spinDuration = 15f;

	// Use this for initialization
	void Awake () {
		for( int i = 0 ; i < maxWidthNum ; ++i )
		{
			squares.Add( new List<GameObject>() );
			for ( int j = 0 ; j < maxHeightNum ; ++ j )
			{
				GameObject squ = Instantiate( squarePrefab ) as GameObject;
				squares[i].Add( squ );
				squares[i][j].transform.parent = transform;
				oriScale = squares[i][j].transform.localScale;
			}
		}

		for( int i = 0 ; i < maxWidthNum ; ++i )
		{
			for ( int j = 0 ; j < maxHeightNum ; ++ j )
			{
				squares[i][j].transform.localPosition = new Vector3( - width * ( ( maxWidthNum - 1 ) / 2 - i )
				                                                    , - height * ( ( maxHeightNum - 1 ) / 2 - j )
				                                                    , 0 );
				squares[i][j].transform.localScale = oriScale * scaleTime;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//spin
		transform.eulerAngles += new Vector3( 0 , 0 , 360f / spinDuration * Time.deltaTime );
	}

	public void SetColor( float a )
	{
		Color col = new Color( a , a , a , 1f );
		broad.color = col;
		col.a = Global.BACK_SQUARE_APLAH;
		for( int i = 0 ; i < maxWidthNum ; ++i )
		{
			for ( int j = 0 ; j < maxHeightNum ; ++ j )
			{
				squares[i][j].GetComponent<BackSquare>().SetColor(a);
			}
		}
	}
}
