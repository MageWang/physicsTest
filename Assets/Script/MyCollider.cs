using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCollider : MonoBehaviour {
	// already computed based on geometry
	public float m_mass;
	public Matrix4x4 m_localInertiaTensor;
	public Vector3 m_localCentroid;
	public MyRigidbody m_body;
	public MyRigidbody Body(){
		return m_body;
	}
	// Use this for initialization
	void Start(){
	}
	
	// Update is called once per frame
	void Update(){
	}
	
	// hit point = ray.pos + t * ray.dir
	virtual public bool TestRay(Ray3 ray, out float t, out Vector3 normal){
		t = 0;
		normal = Vector3.zero;
		return false;
	}

	virtual public void DebugDraw(Color c){
		Debug.DrawLine(transform.position+m_localCentroid, transform.position+m_localCentroid + Vector3.up, c);
		Debug.DrawLine(transform.position+m_localCentroid, transform.position+m_localCentroid + Vector3.left, c);
		Debug.DrawLine(transform.position+m_localCentroid, transform.position+m_localCentroid + Vector3.forward, c);
	}
}
