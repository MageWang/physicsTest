using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidbody:MonoBehaviour{
	public float m_mass;
	public float m_inverseMass;
	public Matrix4x4 m_localInverseInertiaTensor;
	public Matrix4x4 localInertiaTensor;
	public Matrix4x4 m_globalInverseInertiaTensor;

	public Vector3 m_globalCentroid;
	public Vector3 m_localCentroid;
	
	public Vector3 m_position {
		get{
			return transform.localPosition;
		}
		set{
			transform.localPosition=value;
		}
	}

	public Matrix4x4 m_orientation;
	public Vector3 m_linearVelocity;
	public Vector3 m_angularVelocity;

	public Vector3 m_forceAccumulator;
	public Vector3 m_torqueAccumulator;

	public List<MyCollider> m_colliders;
	public List<MyCollider> m_notAddColliders;

	void UpdateGlobalCentroidFromPosition()
	{
		Vector4 v4 = new Vector4(m_localCentroid.x, m_localCentroid.y, m_localCentroid.z);
		v4=m_orientation*v4;
		m_globalCentroid = new Vector3(v4.x+m_position.x, v4.y+m_position.y, v4.z+m_position.z);
	}

	void UpdatePositionFromGlobalCentroid(){
		Vector4 v4 = new Vector4(m_localCentroid.x, m_localCentroid.y, m_localCentroid.z);
		v4=m_orientation*(-v4);
		m_position = new Vector3(v4.x+m_globalCentroid.x, v4.y+m_globalCentroid.y, v4.z+m_globalCentroid.z);
	}

	void UpdateOrientation(){
		m_orientation = Matrix4x4.Rotate(transform.localRotation);
	}

	Matrix4x4 OutProduct(Vector4 v1, Vector4 v2){
		var m = new Matrix4x4();
		for(var i = 0; i < 4; i++){
			for(var j = 0; j < 4; j++){
				m[i,j]= v1[i]*v2[j];
			}
		}
		return m;
	}

	Matrix4x4 Multiple(Matrix4x4 m, float f){
		var ret = new Matrix4x4();
		for(var i = 0; i < 4; i++){
			for(var j = 0; j < 4; j++){
				ret[i,j]= m[i,j]*f;
			}
		}
		return ret;
	}

	Matrix4x4 Substract(Matrix4x4 m1, Matrix4x4 m2){
		var ret = new Matrix4x4();
		for(var i = 0; i < 4; i++){
			for(var j = 0; j < 4; j++){
				ret[i,j]= m1[i,j]-m2[i,j];
			}
		}
		return ret;
	}

	Matrix4x4 Add(Matrix4x4 m1, Matrix4x4 m2){
		var ret = new Matrix4x4();
		for(var i = 0; i < 4; i++){
			for(var j = 0; j < 4; j++){
				ret[i,j]= m1[i,j]+m2[i,j];
			}
		}
		return ret;
	}

	void AddCollider(MyCollider collider)
	{
		// add collider to collider list
		m_colliders.Add(collider);
		
		// reset local centroid & mass
		m_localCentroid = Vector3.zero;
		m_mass = 0.0f;
		
		// compute local centroid & mass
		foreach (var col in m_colliders)
		{
			// accumulate mass
			m_mass += col.m_mass;
		
			// accumulate weighted contribution
			m_localCentroid += 
			col.m_mass * col.m_localCentroid;
		}
		
		// compute inverse mass
		m_inverseMass = 1.0f / m_mass;
		
		// compute final local centroid
		m_localCentroid *= m_inverseMass;
		
		// compute local inertia tensor
		localInertiaTensor = Matrix4x4.zero;
		foreach (var col in m_colliders)
		{
			Vector3 r = m_localCentroid - col.m_localCentroid;
			float rDotR = Vector3.Dot(r,r);
			Matrix4x4 rOutR = OutProduct(r,r);
		
			// accumulate local inertia tensor contribution, 
			// using Parallel Axis Theorem
			localInertiaTensor = 
				Add(localInertiaTensor,
					Add(col.m_localInertiaTensor, 
						Multiple(	
							Substract(
								Multiple(Matrix4x4.identity,rDotR),rOutR), col.m_mass)));
		}
		
		// compute inverse inertia tensor
		m_localInverseInertiaTensor = localInertiaTensor.inverse;

	}

	Vector3 LocalToGlobal( Vector3 p)
	{
		return Vector3.zero;
	}
	Vector3 GlobalToLocal( Vector3 p)
	{
		return Vector3.zero;
	}
	Vector3 LocalToGlobalVec( Vector3 v)
	{
		return Vector3.zero;
	}
	Vector3 GlobalToLocalVec( Vector3 v)
	{
		return Vector3.zero;
	}

	void ApplyForce(Vector3 f, Vector3 at)
	{
		m_forceAccumulator += f;
  		m_torqueAccumulator += Vector3.Cross((at - m_globalCentroid),f);
	}
	// Use this for initialization
	void Start()
	{
		foreach(var c in m_notAddColliders)
		{
			AddCollider(c);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		ApplyForce(new Vector3(0,-1,0), Vector3.zero);
		// integrate linear velocity
		this.m_linearVelocity += 
			this.m_inverseMass
			* (this.m_forceAccumulator * Time.deltaTime);
		
		// integrate angular velocity
		var v4 = this.m_globalInverseInertiaTensor * (this.m_torqueAccumulator * Time.deltaTime);
		this.m_angularVelocity += new Vector3(v4.x,v4.y,v4.z);
		
		// zero out accumulated force and torque
		this.m_forceAccumulator = Vector3.zero;
		this.m_torqueAccumulator = Vector3.zero;
		
		// integrate position
		m_globalCentroid += m_linearVelocity * Time.deltaTime;
		
		// integrate orientation
		Vector3 axis = m_angularVelocity.normalized;
		float angle = m_angularVelocity.magnitude * Time.deltaTime;
		m_orientation = RotateAnyAxis(axis, angle) * m_orientation;
		
		// update physical properties
		this.UpdateOrientation();
		this.UpdatePositionFromGlobalCentroid();
		
		//Inverse Inertia Tensor Update
		m_globalInverseInertiaTensor = 
			m_orientation 
			* localInertiaTensor 
			* m_orientation.inverse;
	}

	Matrix4x4 RotateAnyAxis(Vector3 axis, float angle){
		var dot = Vector3.Dot(axis.normalized, Vector3.left);
		var left = Vector3.left;
		if(1-Mathf.Abs(dot) < 0.0001f)
		{
			left = Vector3.forward;
		}

		var lookAt = Vector3.Cross(axis.normalized, left);
		var look = Matrix4x4.LookAt(Vector3.zero, lookAt.normalized, axis.normalized);
		var r = Matrix4x4.Rotate(Quaternion.Euler(0,angle,0));
		return look*r*look.inverse;
	}
}
