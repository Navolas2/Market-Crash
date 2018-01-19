using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Coloring : MonoBehaviour {

	public List<Renderer> owner_one_marking;
	public List<Renderer> owner_two_marking;

	// Use this for initialization
	void Start () {
		
	}
	
	public void ChangeMaterial(Material m){
		foreach (Renderer r in owner_one_marking) {
			r.material = m;
		}
		foreach (Renderer r in owner_two_marking) {
			r.material = m;
		}
	}

	public void Coowner (Material m){
		foreach (Renderer r in owner_two_marking) {
			r.material = m;
		}
	}
}
