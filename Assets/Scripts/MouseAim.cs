using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour {
	public float m_minReticleDistance = 1f;
	public float m_maxReticleDistance = 5f;
	public LayerMask [] m_blockingLayers;
	private bool m_canAim = false;
	private LayerMask m_combinedLayerMask;
	private const float EPSILON = 0.05f;
	private Vector3 m_minLinePosition;
	private Vector3 m_maxLinePosition;
	public bool canAim{
		get{
			return m_canAim;
		}
	}
	public Vector3 shotSpawnoint{
		get{
			return m_minLinePosition;
		}
	}
	public Vector3 reticlePoint {
		get{
			return m_maxLinePosition;
		}
	}
	public Quaternion reticleRotation{
		get{
			return Quaternion.LookRotation(m_maxLinePosition - m_minLinePosition, Vector3.up);
		}
	}
	void Start(){
		m_combinedLayerMask = 0;
		foreach(LayerMask mask in m_blockingLayers){
			m_combinedLayerMask |= mask;
		}
	}
	void Update () {
		if(Mathf.Abs(Input.GetAxis("Mouse X")) > EPSILON || Mathf.Abs(Input.GetAxis("Mouse Y")) > EPSILON){
			UpdateReticlePosition(GetMousePosition());
			CheckForCollision();
		}
	}
	Vector3 GetMousePosition(){
		Vector3 retVal = new Vector3();

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		float zPlaneFactor = mouseRay.origin.z / mouseRay.direction.z;
		retVal = mouseRay.origin - mouseRay.direction * zPlaneFactor;
		
		return retVal;
	}
	void UpdateReticlePosition(Vector3 mousePosition){
		float distanceToMousePosition = Vector3.Distance(transform.position, mousePosition);
		Vector3 directionToMousePosition = (mousePosition - transform.position).normalized;

		if(distanceToMousePosition < m_maxReticleDistance && distanceToMousePosition > m_minReticleDistance){
			m_maxLinePosition = mousePosition;
			m_minLinePosition = transform.position + directionToMousePosition * m_minReticleDistance;
		}
		else if(distanceToMousePosition > m_maxReticleDistance){
			m_maxLinePosition = transform.position + directionToMousePosition * m_maxReticleDistance;
			m_minLinePosition = transform.position + directionToMousePosition * m_minReticleDistance;
		}
		else{
			m_minLinePosition = transform.position + directionToMousePosition * m_minReticleDistance;
			m_maxLinePosition = m_minLinePosition;
		}
	}
	void CheckForCollision(){
		RaycastHit hitInfo;
		m_canAim = true;
		if(Physics.Linecast(transform.position, m_maxLinePosition, out hitInfo, m_combinedLayerMask)){
			float hitDistance = Vector3.Distance(hitInfo.point, transform.position);
			if(hitDistance < m_minReticleDistance){
				m_canAim = false;
			}
			else if(hitDistance < m_maxReticleDistance){
				m_maxLinePosition = hitInfo.point;
			}
		}
	}
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.green;
		Gizmos.DrawLine(m_minLinePosition, m_maxLinePosition);
	}
}
