using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAABB : MonoBehaviour {
	public MyAABB a,b;
	public bool isCollide = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		isCollide = a.Collides(b);
		if(isCollide){
			DebugDraw(a,Color.red);
			DebugDraw(b,Color.blue);
		}
	}
	
	void DebugDraw(MyAABB aabb, Color color){
		int i = 0;
		Vector3 [] points = new Vector3[8];
		points[i++] = new Vector3(aabb.max.x, aabb.max.y, aabb.max.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.max.x, aabb.max.y, aabb.min.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.max.x, aabb.min.y, aabb.max.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.max.x, aabb.min.y, aabb.min.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.min.x, aabb.max.y, aabb.max.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.min.x, aabb.max.y, aabb.min.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.min.x, aabb.min.y, aabb.max.z) + aabb.transform.position;
		points[i++] = new Vector3(aabb.min.x, aabb.min.y, aabb.min.z) + aabb.transform.position;
		Debug.DrawLine(points[0], points[1], color);
		Debug.DrawLine(points[0], points[2], color);
		Debug.DrawLine(points[1], points[3], color);
		Debug.DrawLine(points[2], points[3], color);
		Debug.DrawLine(points[4], points[5], color);
		Debug.DrawLine(points[4], points[6], color);
		Debug.DrawLine(points[5], points[7], color);
		Debug.DrawLine(points[6], points[7], color);
		Debug.DrawLine(points[0], points[4], color);
		Debug.DrawLine(points[1], points[5], color);
		Debug.DrawLine(points[2], points[6], color);
		Debug.DrawLine(points[3], points[7], color);
	}
}
