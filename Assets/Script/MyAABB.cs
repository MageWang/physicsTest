using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAABB : MonoBehaviour {
	public Vector3 max, min;
	public bool isDebug = false;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		Init();
	}
	void OnDisable(){
		DeInit();
	}
	
	private bool isInit = false;
	void Init(){
		if(isInit)return;
		if(BroadPhase.GetInstance()==null)return;
		BroadPhase.GetInstance().Add(this);
		isInit=true;
	}
	void DeInit(){
		if(BroadPhase.GetInstance()==null)return;
		BroadPhase.GetInstance().Remove(this);
	}
	public bool Collides(MyAABB other){
		var lhsMin = min + transform.position;
		var lhsMax = max + transform.position;
		var rhsMin = other.min + other.transform.position;
		var rhsMax = other.max + other.transform.position;
		return (lhsMin.x < rhsMax.x && lhsMax.x > rhsMin.x)
			&&(lhsMin.y < rhsMax.y && lhsMax.y > rhsMin.y)
			&&(lhsMin.z < rhsMax.z && lhsMax.z > rhsMin.z);
	}

	public bool Contains(Vector3 pos){
		var lhsMin = min + transform.position;
		var lhsMax = max + transform.position;
		return 
		(pos.x < lhsMax.x && pos.x > lhsMin.x)
		&&(pos.y < lhsMax.y && pos.y > lhsMin.y)
		&&(pos.z < lhsMax.z && pos.z > lhsMin.z);
	}

	public MyCollider Collider(){
		return GetComponent<MyCollider>();
	}

	public bool TestRay(Ray3 ray){
		Vector3 v;

		// TODO : transform ray to local position
		//Ray3 localRay;
		//localRay = new Ray3();
		
		return HitBoundingBox(transform.position + min, transform.position + max, ray.pos, ray.dir, out v);
	}

	public static bool HitBoundingBox(Vector3 minB, Vector3 maxB, Vector3 origin, Vector3 dir, out Vector3 coord)
		// double minB[NUMDIM], maxB[NUMDIM];		/*box */
		// double origin[NUMDIM], dir[NUMDIM];		/*ray */
		// double coord[NUMDIM];				/* hit point */
	{
		const short RIGHT = 0;
		const short LEFT = 1;
		const short MIDDLE = 2;
		const short NUMDIM = 3;

		coord = new Vector3();
		bool inside = true;
		short[] quadrant = new short[3];
		int whichPlane;
		float [] maxT= new float[3];
		float [] candidatePlane = new float[3];

		/* Find candidate planes; this loop can be avoided if
		rays cast all from the eye(assume perpsective view) */
		for (var i=0; i<NUMDIM; i++)
		{
			if(origin[i] < minB[i]) {
				quadrant[i] = LEFT;
				candidatePlane[i] = minB[i];
				inside = false;
			}else if (origin[i] > maxB[i]) {
				quadrant[i] = RIGHT;
				candidatePlane[i] = maxB[i];
				inside = false;
			}else	{
				quadrant[i] = MIDDLE;
			}
		}

		/* Ray origin inside bounding box */
		if(inside)	{
			coord = origin;
			return true;
		}


		/* Calculate T distances to candidate planes */
		for (var i = 0; i < NUMDIM; i++)
		{
			if (quadrant[i] != MIDDLE && dir[i] !=0)
				maxT[i] = (candidatePlane[i]-origin[i]) / dir[i];
			else
				maxT[i] = -1;
		}
		/* Get largest of the maxT's for final choice of intersection */
		whichPlane = 0;
		for (var i = 1; i < NUMDIM; i++)
		{
			if (maxT[whichPlane] < maxT[i])
				whichPlane = i;
		}

		/* Check final candidate actually inside box */
		if (maxT[whichPlane] < 0)
		{
				return false;
		}

		for (var i = 0; i < NUMDIM; i++)
		{
			if (whichPlane != i) {
				coord[i] = origin[i] + maxT[whichPlane] *dir[i];
				if (coord[i] < minB[i] || coord[i] > maxB[i])
					return false;
			} 
			else {
				coord[i] = candidatePlane[i];
			}
		}

		return true;				/* ray hits box */
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isInit)Init();
		if(!isDebug)return;

		Debug.DrawLine(transform.position + new Vector3(max.x, max.y, max.z), transform.position + new Vector3(min.x, max.y, max.z));
		Debug.DrawLine(transform.position + new Vector3(max.x, max.y, max.z), transform.position + new Vector3(max.x, min.y, max.z));
		Debug.DrawLine(transform.position + new Vector3(max.x, max.y, max.z), transform.position + new Vector3(max.x, max.y, min.z));

		Debug.DrawLine(transform.position + new Vector3(min.x, min.y, max.z), transform.position + new Vector3(max.x, min.y, max.z));
		Debug.DrawLine(transform.position + new Vector3(min.x, min.y, max.z), transform.position + new Vector3(min.x, max.y, max.z));
		Debug.DrawLine(transform.position + new Vector3(min.x, min.y, max.z), transform.position + new Vector3(min.x, min.y, min.z));

		Debug.DrawLine(transform.position + new Vector3(min.x, max.y, min.z), transform.position + new Vector3(max.x, max.y, min.z));
		Debug.DrawLine(transform.position + new Vector3(min.x, max.y, min.z), transform.position + new Vector3(min.x, min.y, min.z));
		Debug.DrawLine(transform.position + new Vector3(min.x, max.y, min.z), transform.position + new Vector3(min.x, max.y, max.z));

		Debug.DrawLine(transform.position + new Vector3(max.x, min.y, min.z), transform.position + new Vector3(min.x, min.y, min.z));
		Debug.DrawLine(transform.position + new Vector3(max.x, min.y, min.z), transform.position + new Vector3(max.x, max.y, min.z));
		Debug.DrawLine(transform.position + new Vector3(max.x, min.y, min.z), transform.position + new Vector3(max.x, min.y, max.z));
	}
}
