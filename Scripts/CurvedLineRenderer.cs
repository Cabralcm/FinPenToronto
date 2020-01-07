using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(LineRenderer) )]
public class CurvedLineRenderer : MonoBehaviour 
{
	//PUBLIC
	public float lineSegmentSize = 0.15f;
	public float lineWidth = 0.1f;
	[Header("Gizmos")]
	public bool showGizmos = true;
	public float gizmoSize = 0.1f;
	public Color gizmoColor = new Color(1,0,0,0.5f);
	//PRIVATE
	private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
	private Vector3[] linePositions = new Vector3[0];
	private Vector3[] linePositionsOld = new Vector3[0];
    private List<GameObject> linePoints2 = new List<GameObject>();
    private List<int> drawingLengths2 = new List<int>();

    //public WriteHelper GetDrawingPoints();
    // Update is called once per frame
    public void Update () 
	{
		GetPoints();
		SetPointsToLine();
	}

	void GetPoints()
	{
        //find curved points in children
        //linePoints = GameObject.GetComponent<WriteHelper>().getDrawingPoints();
        //linePoints = this.GetComponentsInChildren<CurvedLinePoint>();
        linePoints2 = WriteHelper.getDrawingPoints();
        drawingLengths2 = WriteHelper.getDrawingLengths();
        int last_point = drawingLengths2.Count - 1;
        int numberOfPoints = linePoints2.Count;
        int previousNumPoints = WriteHelper.previousNumPoints;

        //add positions
        linePositions = new Vector3[linePoints.Length];
        if (drawingLengths2.Count < 1)
        {
            for (int i = 0; i < linePoints2.Count; i++)
            {
                linePositions[i] = linePoints2[i].transform.position;
            }
        }
        else
        {
            for(int i = numberOfPoints; i > previousNumPoints; i--)
            {
                linePositions[i] = linePoints2[i].transform.position;
            }
        }
    
		////add positions
		//linePositions = new Vector3[linePoints.Length];
        //for( int i = 0; i < linePoints.Length; i++ )
		//{
		//	linePositions[i] = linePoints[i].transform.position;
		//}
	}

	void SetPointsToLine()
	{
		//create old positions if they dont match
		if( linePositionsOld.Length != linePositions.Length )
		{
			linePositionsOld = new Vector3[linePositions.Length];
		}

		//check if line points have moved
		bool moved = false;
		for( int i = 0; i < linePositions.Length; i++ )
		{
			//compare
			if( linePositions[i] != linePositionsOld[i] )
			{
				moved = true;
			}
		}

		//update if moved
		if( moved == true )
		{
			LineRenderer line = this.GetComponent<LineRenderer>();

			//get smoothed values
			Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize );

			//set line settings
			line.positionCount = smoothedPoints.Length;
            //line.SetVertexCount(smoothedPoints.Length)
			line.SetPositions( smoothedPoints );
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            //line.SetWidth( lineWidth, lineWidth );
		}
	}

	void OnDrawGizmosSelected()
	{
		Update();
	}

	void OnDrawGizmos()
	{
		if( linePoints.Length == 0 )
		{
			GetPoints();
		}

		//settings for gizmos
		foreach( CurvedLinePoint linePoint in linePoints )
		{
			linePoint.showGizmo = showGizmos;
			linePoint.gizmoSize = gizmoSize;
			linePoint.gizmoColor = gizmoColor;
		}
	}
}
