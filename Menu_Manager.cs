using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Manager : MonoBehaviour {

	public static Menu_Manager singleton;

	public UnityEngine.UI.Image select_indicator;
	public List<Menu> active_menus;
	public static int Turn_Start = 0;
	public static int Stop_Move = 1;
	public static int Purchase = 2;
	public static int Investment = 3;
	public static int Stock_Market = 4;
	public static int Stock_Buyer = 5;
	public static int Force_Sell = 6;
	public static int Property_Upgrade = 7;
	public static int Sell_Confirm = 8;
	public static int Chance_Menu = 9;

	private int current_menu;
	private int index;
	private int input_delay;

	private bool active_menu = false;
	private bool open_menu;

	void Awake(){
		if (singleton == null) {
			singleton = this;
		} else if (singleton != this) {
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {
		HideAllMenu ();
		open_menu = false;
		input_delay = 0;
		index = 0;
		Turn_Menu_On (Turn_Start);
		current_menu = Turn_Start;
		MoveIndicator ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (input_delay == 0) {
			float vertical = Input.GetAxis ("Vertical");
			if (vertical > .5) {
				vertical = 1f;
			} else if (vertical < -.5) {
				vertical = -1f;
			} else {
				vertical = 0f;
			}
			if (vertical != 0) {
				
				if (current_menu == Menu_Manager.Stock_Market) {
					index += (int)vertical * -1;
				} else {
					index += (int)vertical;
				}
				if (index < 0) {
					index = active_menus[current_menu].GetButtonCount() - 1;
				} else if (index >= active_menus[current_menu].GetButtonCount()) {
					index = 0;
				}
				MoveIndicator ();
				input_delay = 10;
			}
			if (Input.GetButtonDown ("Submit")) {
				active_menus [current_menu].ClickButton (index); 
			}
			if (Input.GetButtonDown ("Cancel")) {
				active_menus [current_menu].ClickCancelButton ();
			}
		} else if(input_delay > 0) {
			input_delay--;
		}
	}

	private void MoveIndicator(){
		Vector3 position = active_menus [current_menu].GetButtonPosition (index);
		select_indicator.transform.position = position;
	}

	public void HideMenu(){
		Turn_Menu_Off (current_menu);
		
		select_indicator.gameObject.SetActive (false);
		input_delay = -1;
	}

	public void HideAllMenu(){
		for (int i = 0; i < active_menus.Count; i++) {
			Turn_Menu_Off (i);
		}
		select_indicator.gameObject.SetActive (false);
		input_delay = -1;
	}

	public void Turn_Menu_On(int menu_index, string text){
		active_menus [menu_index].Change_Text (text);
		Turn_Menu_On (menu_index);
	}

	public void Turn_Menu_On(){
		if (active_menu) {
			TurnMenuOn (current_menu);
		}
	}

	public void Turn_Menu_On(int menu_index){
		active_menu = true;
		TurnMenuOn (menu_index);
	}

	private void TurnMenuOn(int menu_index){
		open_menu = true;
		index = 0;
		select_indicator.gameObject.SetActive (true);
		current_menu = menu_index;
		active_menus [menu_index].Turn_On ();
		MoveIndicator ();
		input_delay = 0;
	}

	public void Turn_Menu_Off(){
		if (active_menu) {
			TurnMenuOff (current_menu);
		}
	}

	public void Turn_Menu_Off(int menu_index){
		active_menu = false;
		TurnMenuOff (menu_index);
	}

	private void TurnMenuOff(int menu_index){
		open_menu = false;
		select_indicator.gameObject.SetActive (false);
		active_menus [menu_index].Turn_Off ();
	}

	public Menu CurrentMenu(){
		return active_menus [current_menu];
	}

	public bool isOpenMenu{
		get{ return open_menu; }
	}

	public void DisableIndicator(){
		select_indicator.gameObject.SetActive (false);
	}
}
