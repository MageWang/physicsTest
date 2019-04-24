using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSquared : BroadPhase {
	List<ColliderPair> m_pairs = new List<ColliderPair>();
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		if(instance!=null){
			Debug.LogWarning(instance.name+" instance!=null");
		}
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	override public void Update () {
		
	}

	override public List<ColliderPair> ComputePairs()
	{
		m_pairs.Clear();
		
		// outer loop
		for (var i = 0; i < m_aabbs.Count; i++)
		{
			// inner loop
			var jStart = i;
			for (var j = ++jStart; j <m_aabbs.Count; ++j)
			{
				var aabbA = m_aabbs[i];
				var aabbB = m_aabbs[j];
				var colliderA = aabbA.Collider();
				var colliderB = aabbB.Collider();
				var bodyA = colliderA.Body();
				var bodyB = colliderB.Body();
				
				// skip same-body collision
				if (bodyA == bodyB)
					continue;
				
				// add collider pair
				if (aabbA.Collides(aabbB)){
					var pair = new ColliderPair{
						a = aabbA.Collider(),
						b = aabbB.Collider()
					};
					m_pairs.Add(pair);
				}
				
			} // end of inner loop
		} // end of outer loop
		
		return m_pairs;
	}

	override public MyCollider Pick(Vector3 point)
	{
		foreach (var aabb in m_aabbs){
			if (aabb.Contains(point)){
				return aabb.Collider();
			}
		}
	
		// no collider found
		return null;
	}

	override public void Query(MyAABB aabb, out List<MyCollider> output){
		output = new List<MyCollider>();
		foreach(var ab in m_aabbs){
			if(ab.Collides(aabb)){
				output.Add(ab.Collider());
			}
		}
	}
	// struct for storing ray-collider test results
	class ResultEntry
	{
		public MyCollider m_collider;
		public float t;
		public Vector3 normal;
		public bool GreaterThan(ResultEntry rhs){
			return true;
		}
	};

	override public RayCastResult RayCast(Ray3 ray)
	{
		// test AABBs for candidates
		List<MyCollider> candidateList = new List<MyCollider>(m_aabbs.Count);
		
		foreach (var aabb in m_aabbs)
		{
			if (aabb.TestRay(ray))
			{
				candidateList.Add(aabb.Collider());
			}
		}
	
		// test actual colliders
		List<ResultEntry> resultList = new List<ResultEntry>(candidateList.Capacity);
		foreach (var col in candidateList)
		{
			// hit point = ray.pos + t * ray.dir
			float t;
			Vector3 normal;
			if (col.TestRay(ray, out t, out normal))
			{
				ResultEntry entry = new ResultEntry(){
					m_collider=col,
					t=t, 
					normal=normal
				};
				resultList.Add(entry);
			}
		}
		
		resultList.Sort((a,b)=>{
			var r = a.t-b.t;
			if(r>0)return 1;
			if(r<0)return -1;
			return 0;
		});

		RayCastResult result = new RayCastResult();
		if (resultList.Count != 0)
		{
			// the first result entry is the closest one
			var entry = resultList[0];
			result.hit = true;
			result.collider = entry.m_collider;
			result.t = entry.t;
			result.normal = entry.normal;
			result.intersection = ray.pos + entry.t * ray.dir;
		}
		else
		{
			result.hit = false;
		}
		
		return result;
	}
}
