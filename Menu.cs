using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {


	public UnityEngine.UI.Text menu_text;
	public UnityEngine.UI.Image menu_background_image;
	public List<UnityEngine.UI.Button> menu_buttons;
	public UnityEngine.UI.Button Cancel_button;

	public bool auto_reposition = false;


	// Use this for initialization
	void Start () {
		if (auto_reposition) {
			Rect button_dimentions = menu_buttons [0].GetComponent<RectTransform> ().rect;
			Vector3 screen_spects = new Vector3 (((float)Screen.width / 2) * -1 + (button_dimentions.width * 1.5f), ((float)Screen.height / 2) - (button_dimentions.height * (menu_buttons.Count + 1)));

			transform.localPosition = screen_spects;
		}
	}

	public virtual void Change_Text(string text){
		menu_text.text = text;
	}

	public virtual Vector3 GetButtonPosition(int index){
		return menu_buttons[index].transform.position;
	}

	public virtual void ClickButton(int index){
		menu_buttons[index].onClick.Invoke ();
	}

	public virtual void ClickCancelButton(){
		if (Cancel_button != null) {
			Cancel_button.onClick.Invoke ();
		}
	}

	public virtual int GetButtonCount(){
		return menu_buttons.Count;
	}

	public virtual void Turn_Off(){
		menu_text.gameObject.SetActive(false);
		foreach (UnityEngine.UI.Button bt in menu_buttons) {
			bt.gameObject.SetActive(false);
		}
		if (menu_background_image != null) {
			menu_background_image.gameObject.SetActive (false);
		}
	}

	public virtual void Turn_On(){
		menu_text.gameObject.SetActive(true);
		foreach (UnityEngine.UI.Button bt in menu_buttons) {
			bt.gameObject.SetActive(true);
		}
		if (menu_background_image != null) {
			menu_background_image.gameObject.SetActive (true);
		}
	}
}
