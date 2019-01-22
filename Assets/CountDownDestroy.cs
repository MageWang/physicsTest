using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownDestroy : MonoBehaviour {
	public float sec = 10.0f;
	private float start = 0;
	// Use this for initialization
	void Start () {
		start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time-start > sec){
			GameObject.Destroy(this.gameObject);
		}
	}
}
