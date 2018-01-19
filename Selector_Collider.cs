using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Collider : MonoBehaviour {

	public Property_Informer linked_informer;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider entered){
		board_space b_s = entered.GetComponent<board_space> ();
		if (b_s != null) {
			linked_informer.NewEnteredSpace (b_s);
		}
	}

	void OnTriggerExit(Collider exited){
		board_space b_s = exited.GetComponent<board_space> ();
		if (b_s != null) {
			linked_informer.NewExitedSpace (b_s);
		}
	}
}
