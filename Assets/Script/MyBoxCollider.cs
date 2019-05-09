using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBoxCollider : MyCollider 
{
	// already computed based on geometry
	public float height=1.0f, width=1.0f, depth=1.0f;
	
	public Vector3 []points;
	// Use this for initialization
	void Start()
	{
	}

	void Awake()
	{
		points = new Vector3[8];
		points[0] = new Vector3(-width/2,height/2,-depth/2);
		points[1] = new Vector3(-width/2,height/2,depth/2);
		points[2] = new Vector3(width/2,height/2,-depth/2);
		points[3] = new Vector3(width/2,height/2,depth/2);
		points[4] = new Vector3(-width/2,-height/2,-depth/2);
		points[5] = new Vector3(-width/2,-height/2,depth/2);
		points[6] = new Vector3(width/2,-height/2,-depth/2);
		points[7] = new Vector3(width/2,-height/2,depth/2);
		
		m_localInertiaTensor=Matrix4x4.identity;
		m_localInertiaTensor[0,0]=m_mass/12*(height*height+depth*depth);
		m_localInertiaTensor[1,1]=m_mass/12*(width*width+depth*depth);
		m_localInertiaTensor[2,2]=m_mass/12*(width*width+height*height);

		
	}
	
	// Update is called once per frame
	void Update()
	{
		m_localInertiaTensor=Matrix4x4.identity;
		m_localInertiaTensor[0,0]=m_mass/12*(height*height+depth*depth);
		m_localInertiaTensor[1,1]=m_mass/12*(width*width+depth*depth);
		m_localInertiaTensor[2,2]=m_mass/12*(width*width+height*height);
	}

	override public bool TestRay(Ray3 ray, out float t, out Vector3 normal)
	{
		Vector3 min = transform.position + new Vector3(-width/2,-height/2,-depth/2);
		Vector3 max = transform.position + new Vector3(width/2,height/2,depth/2);
		Vector3 intersection;
		bool b = MyAABB.HitBoundingBox(min,max,ray.pos,ray.dir, out intersection);
		// hit point = ray.pos + t * ray.dir
		// t = (hit point - ray.pos)/ray.dir
		t = (intersection-ray.pos).magnitude/ray.dir.magnitude;
		normal = (ray.pos-intersection).normalized;
		
		Debug.DrawLine(intersection, intersection+Vector3.up, Color.red);
		Debug.DrawLine(intersection, intersection+Vector3.left, Color.red);
		Debug.DrawLine(intersection, intersection+Vector3.forward, Color.red);

		return b;
	}

	override public void DebugDraw(Color c){
		
		Debug.DrawLine(transform.position+points[0], transform.position+points[1], c);
		Debug.DrawLine(transform.position+points[0], transform.position+points[2], c);
		Debug.DrawLine(transform.position+points[3], transform.position+points[1], c);
		Debug.DrawLine(transform.position+points[3], transform.position+points[2], c);

		Debug.DrawLine(transform.position+points[0], transform.position+points[4], c);
		Debug.DrawLine(transform.position+points[1], transform.position+points[5], c);
		Debug.DrawLine(transform.position+points[2], transform.position+points[6], c);
		Debug.DrawLine(transform.position+points[3], transform.position+points[7], c);

		Debug.DrawLine(transform.position+points[4], transform.position+points[5], c);
		Debug.DrawLine(transform.position+points[4], transform.position+points[6], c);
		Debug.DrawLine(transform.position+points[7], transform.position+points[5], c);
		Debug.DrawLine(transform.position+points[7], transform.position+points[6], c);
	}
}
