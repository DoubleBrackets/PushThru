using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSmearEffect : MonoBehaviour
{
    public LineRenderer lineRen;
	public int steps;
	public int showFrames;
	public int hideFrames;
	public float showDuration;
	public float delay;

	public Vector3[] points;

	[ContextMenu("ResetPoints")]
	public void ResetPoints()
	{
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}

	[ContextMenu("Slash")]
	public void PerformSmear()
    {
		StartCoroutine(Corout_PerformSmear());
    }

	IEnumerator Corout_PerformSmear()
    {
		yield return new WaitForSeconds(delay);
		StartCoroutine(Corout_ShowSmear());
		StartCoroutine(Corout_HideSmear());
	}

	IEnumerator Corout_ShowSmear()
    {
		lineRen.positionCount = steps + 1;
		for (int c = 0; c <= showFrames; c++)
		{
			for (int x = 0; x <= steps; x++)
			{
				float t = Mathf.Lerp(0,c / (float)showFrames, x / (float)steps);
				lineRen.SetPosition(x, GetPointLocal(t));
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator Corout_HideSmear()
    {
		yield return new WaitForSeconds(showDuration);
		for(int c = 0;c <= hideFrames;c++)
        {
			for (int x = 0; x <= steps; x++)
			{
				float t = Mathf.Lerp(c/(float)hideFrames,1,x / (float)steps);
				lineRen.SetPosition(x, GetPointLocal(t));
			}
			yield return new WaitForFixedUpdate();
		}
		lineRen.positionCount = 0;
	}



    public Vector3 GetPoint(float t)
	{
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

	public Vector3 GetPointLocal(float t)
	{
		return (Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

	public Vector3 GetVelocity(float t)
	{
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
			transform.position;
	}

	public Vector3 GetDirection(float t)
    {
		return GetVelocity(t).normalized;
    }

}
