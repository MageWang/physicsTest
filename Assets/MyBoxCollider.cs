using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBoxCollider : MyCollider 
{
	// already computed based on geometry
	public float height=1.0f, width=1.0f, depth=1.0f;
	// Use this for initialization
	void Start()
	{
	}

	void Awake(){
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
}
