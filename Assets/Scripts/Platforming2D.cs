using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Platforming2D : MonoBehaviour {
	public float m_walkForce = 5f;
	public float m_maxSpeed = 10f;
	Rigidbody m_rigidBody;

	void Start () {
		m_rigidBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		m_rigidBody.AddForce(Input.GetAxis("Horizontal") * m_walkForce * Vector3.right);
		LimitVelocity();
	}
	void LimitVelocity(){
		if(m_rigidBody.velocity.x > m_maxSpeed || m_rigidBody.velocity.x < -m_maxSpeed){
			Vector3 updatedVelocity = m_rigidBody.velocity;
			updatedVelocity.x = Mathf.Clamp(m_rigidBody.velocity.x, -m_maxSpeed, m_maxSpeed);
			m_rigidBody.velocity = updatedVelocity;
		}
	}

}
