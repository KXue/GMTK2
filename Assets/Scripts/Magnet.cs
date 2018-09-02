using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MagnetState{POSITIVE, NEGATIVE, OFF}
[RequireComponent(typeof(Rigidbody))]
public class Magnet : MonoBehaviour {
	public float m_magnetStrength = 5;
	public float m_magnetInfluenceRadius = 20;
	public MagnetState m_state = MagnetState.OFF;
	private LayerMask m_influencedLayers;
	private Rigidbody m_rigidBody;
	void Start(){
		m_influencedLayers = LayerMask.GetMask(new string[]{"Magnet", "Metal"});
		m_rigidBody = GetComponent<Rigidbody>();
	}
	void Update () {
		if(m_state != MagnetState.OFF){
			PullObjects();
		}
	}
	void PullObjects(){
		RaycastHit[] hitObjects = Physics.SphereCastAll(transform.position, m_magnetInfluenceRadius, transform.forward, Mathf.Infinity, m_influencedLayers);
		foreach(RaycastHit hitObject in hitObjects){
			if(hitObject.transform != transform){
				ApplyForces(hitObject);
			}
		}
	}
	void ApplyForces(RaycastHit hitObject){
		Vector3 vectorFromObject = transform.position - hitObject.transform.position;
		float distanceToObject = vectorFromObject.magnitude;
		bool isMetal = hitObject.transform.gameObject.layer == LayerMask.NameToLayer("Metal") || (hitObject.transform.GetComponent<Magnet>() != null && hitObject.transform.GetComponent<Magnet>().m_state == MagnetState.OFF);
		if(isMetal){
			Vector3 pullingForce = vectorFromObject.normalized * m_magnetStrength / distanceToObject;
			hitObject.rigidbody.AddForce(pullingForce);
			m_rigidBody.AddForce(-pullingForce);
		}
		else{
			Magnet otherMagnet = hitObject.transform.GetComponent<Magnet>();
			float magnetForceFactor = otherMagnet.m_state == m_state ? -1 : 1;
			Vector3 pullingForce = magnetForceFactor * vectorFromObject.normalized * (m_magnetStrength + otherMagnet.m_magnetStrength) / distanceToObject;
			hitObject.rigidbody.AddForce(pullingForce);
		}
	}

	void OnDrawGizmos(){
		Color[] magnetColors = new Color[] {Color.red, Color.blue, Color.gray};
		Gizmos.color = magnetColors[(int)m_state];
		Gizmos.DrawWireSphere(transform.position, m_magnetInfluenceRadius);
	}
}
