using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBroadPhase : MonoBehaviour {
	public List<ColliderPair> pairs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(BroadPhase.GetInstance()==null)return;
		pairs = BroadPhase.GetInstance().ComputePairs();
	}
}
