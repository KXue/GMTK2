using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {
	void OnCollisionEnter(Collision other){
		if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
			Debug.Log("Player dies");
		} 
	}
}
