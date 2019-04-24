using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCannon : MonoBehaviour {
	public Cannon cannon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Click(){
		cannon.isShot = true;
	}
}
