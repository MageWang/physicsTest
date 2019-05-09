using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRayCast : MonoBehaviour {
	public Transform from, to;
	public Ray3 ray;
	public RayCastResult rayCastResult;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(from==null)return;
		if(to==null)return;
		ray.pos = from.position;
		ray.dir = (to.position - from.position).normalized;
		rayCastResult = BroadPhase.GetInstance().RayCast(ray);
		if(rayCastResult.hit){
			Debug.DrawRay(ray.pos, ray.dir*1000, Color.red);
			rayCastResult.collider.DebugDraw(Color.red);
		}
		else{
			Debug.DrawRay(ray.pos, ray.dir*1000, Color.green);
		}
		
	}
}
