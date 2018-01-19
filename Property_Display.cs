using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Display : MonoBehaviour {

	public static Property_Display singleton;

	public UnityEngine.UI.Text Value_text;
	public UnityEngine.UI.Text Price_text;

	public UnityEngine.UI.Text Value_text_static;
	public UnityEngine.UI.Text Price_text_static;

	public UnityEngine.UI.Text property_name;

	// Use this for initialization
	void Awake(){
		if (singleton == null) {
			singleton = this;
		} else if (singleton != this) {
			Destroy (this.gameObject);
		}
	}

	void Start () {
		
		transform.position = new Vector3 ();
		Vector2 button_dimentions = new Vector2 (100, 100);
		Vector3 screen_spects = new Vector3 (((float)Screen.width / 2) - (button_dimentions.x * 1.5f), ((float)Screen.height / 2) - (button_dimentions.y));
		this.transform.localPosition = screen_spects;
	}
	
	public void SetInformation(board_space b_s){
		this.gameObject.SetActive (true);
		if (b_s.CompareTag ("Bank")) {
			property_name.text = "Bank";
			Value_text.text = "";
			Value_text_static.text = "";
			Price_text.text = "";
			Price_text_static.text = "";
		} else if (b_s.CompareTag ("Property")) {
			Property_Space p_s = (Property_Space)b_s;
			property_name.text = p_s._name;
			Value_text.text = "$" + p_s._value.ToString ();
			Value_text_static.text = "Value:";
			Price_text.text = "$" + p_s._price.ToString ();
			Price_text_static.text = "Price:";
		} else if (b_s.CompareTag ("Chance")) {
			property_name.text = "Chance";
			Value_text.text = "";
			Value_text_static.text = "";
			Price_text.text = "";
			Price_text_static.text = "";
		} else if (b_s.CompareTag ("Commission")) {
			property_name.text = "Commission";
			Value_text.text = "";
			Value_text_static.text = "";
			Price_text.text = "";
			Price_text_static.text = "";
		} else if (b_s.CompareTag ("Free")) {
			property_name.text = "Free Space";
			Value_text.text = "";
			Value_text_static.text = "";
			Price_text.text = "";
			Price_text_static.text = "";
		} else if (b_s.CompareTag ("Roll Again")) {
			property_name.text = "Roll Again";
			Value_text.text = "";
			Value_text_static.text = "";
			Price_text.text = "";
			Price_text_static.text = "";
		}
	}

	public void HideInformation(){
		this.gameObject.SetActive (false);
	}
}