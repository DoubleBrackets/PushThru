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

	public SwordSmearEffect subSmear;

	//used to prevent smears teleporting when parent transform changes mid-effect
	private Matrix4x4 transformCache;

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
		transformCache = transform.localToWorldMatrix;
		StartCoroutine(Corout_PerformSmear());
		if(subSmear)
        {
			subSmear.PerformSmear();
        }
    }

	IEnumerator Corout_PerformSmear()
    {
		yield return new WaitForSeconds(delay);
		StartCoroutine(Corout_ShowSmear());
		StartCoroutine(Corout_HideSmear());
	}

	IEnumerator Corout_ShowSmear()
    {
		transformCache = transform.localToWorldMatrix;
		lineRen.positionCount = steps + 1;
		for (int c = 0; c <= showFrames; c++)
		{
			transformCache = transform.localToWorldMatrix;
			for (int x = 0; x <= steps; x++)
			{
				float t = Mathf.Lerp(0,c / (float)showFrames, x / (float)steps);
				lineRen.SetPosition(x, GetPointUsingCachedTransform(t));
			}
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator Corout_HideSmear()
    {
		yield return new WaitForSeconds(showDuration);
		transformCache = transform.localToWorldMatrix;
		for (int c = 0;c <= hideFrames;c++)
        {
			for (int x = 0; x <= steps; x++)
			{
				float t = Mathf.Lerp(c/(float)hideFrames,1,x / (float)steps);
				lineRen.SetPosition(x, GetPointUsingCachedTransform(t));
			}
			yield return new WaitForFixedUpdate();
		}
		lineRen.positionCount = 0;
	}



    public Vector3 GetPoint(float t)
	{
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

	public Vector3 GetPointUsingCachedTransform(float t)
    {
		return transformCache.MultiplyPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
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
