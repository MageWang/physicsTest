using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ColliderPair{
	public MyCollider a, b;
}

[System.Serializable]
public class Ray3{
	public Vector3 pos, dir;
}

[System.Serializable]
public class RayCastResult
{
  public bool hit;
  public MyCollider collider;
  public Vector3 position;
  public Vector3 normal;
  public float t;
  public Vector3 intersection;
};

public class BroadPhase : MonoBehaviour {
	public List<MyAABB> m_aabbs = new List<MyAABB>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	virtual public void Update () 
	{	
	}

	static protected BroadPhase instance;
	static public BroadPhase GetInstance()
	{
		return instance;
	}

	// adds a new AABB to the broadphase
    virtual public void Add(MyAABB aabb)
	{
		m_aabbs.Add(aabb);
	}

	virtual public void Remove(MyAABB aabb)
	{
		m_aabbs.Remove(aabb);
	}
 
    // returns a list of possibly colliding colliders
    virtual public List<ColliderPair> ComputePairs()
	{
		return null;
	}
     
    // returns a collider that collides with a point
    // returns null if no such collider exists
    virtual public MyCollider Pick(Vector3 point)
	{
		return null;
	}
 
    // returns a list of colliders whose AABBs collide 
    // with a query AABB
    virtual public void Query(MyAABB aabb, out List<MyCollider> output){
		output = null;
	}

    // result contains the first collider the ray hits
    // result contains null if no collider is hit
    virtual public RayCastResult RayCast(Ray3 ray)
	{
		return null;
	}

}
