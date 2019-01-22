using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	public GameObject bullet;
	public bool isShot = false;
	public Vector3 minForce;
	public Vector3 maxForce;
	public int count = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isShot){
			isShot = false;
			for(var i = 0; i < count; i++){
				var obj = GameObject.Instantiate(bullet);
				obj.transform.position = this.transform.position;
				var myRigidbody = obj.GetComponent<MyRigidbody>();
				var f = new Vector3();
				f.x = Random.Range(minForce.x, maxForce.x);
				f.y = Random.Range(minForce.y, maxForce.y);
				f.z = Random.Range(minForce.z, maxForce.z);
				myRigidbody.ApplyForce(f, Vector3.zero);
			}
		}
	}
}
