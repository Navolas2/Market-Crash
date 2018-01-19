using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TextOffCall();

public class Global_text : MonoBehaviour {

	public static Global_text singleton;

	private UnityEngine.UI.Text my_text;
	int counter;
	private TextOffCall textoff;

	// Use this for initialization
	void Awake(){
		if (singleton == null) {
			singleton = this;
		} else if (singleton != this) {
			Destroy (this);
		}
	}

	void Start () {
		my_text = GetComponent<UnityEngine.UI.Text> ();
		my_text.enabled = false;
		textoff = voidCaller;
	}

	// Update is called once per frame
	void Update () {
		if (counter < 180) {
			counter++;
		} else if (counter == 180) {
			my_text.enabled = false;
			counter++;
			textoff ();
		}
	}

	public void SetText(string txt, int start){
		my_text.enabled = true;
		my_text.text = txt;
		Debug.Log (txt);
		counter = start;
		textoff = voidCaller;
	}

	public void SetText(string txt, int start, TextOffCall n_caller){
		my_text.enabled = true;
		my_text.text = txt;
		Debug.Log (txt);
		counter = start;
		textoff = n_caller;
	}

	public void ResetCounter(){
		counter = 181;
		my_text.text = "";
	}

	private void voidCaller(){
		//nothing
	}
}
