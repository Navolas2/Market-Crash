using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Informer : MonoBehaviour {

	public static Property_Informer singleton;

	private List<List<board_space>> highlighted_space;

	private board_space Selected_Space;

	private Player_Information currPlayer;
	private int mode;

	// Use this for initialization
	void Awake(){
		if (singleton == null) {
			singleton = this;
		}if (singleton != this) {
			Destroy (this.gameObject);
		}
	}

	void Start () {
		highlighted_space = new List<List<board_space>> ();

	}
	
	// Update is called once per frame 
	void Update () {
		if (Input.GetButtonDown ("Submit")) {
			switch (mode) {
			case 0:
				break;
			case 1: //upgrade
				if (Selected_Space != null) {
					if(Selected_Space.CompareTag("Property")){
						if (currPlayer.CheckProperty (Selected_Space)) {
							Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Investment);
							Investment_Menu m = (Investment_Menu)Menu_Manager.singleton.CurrentMenu ();
							m.SetProperty ((Property_Space)Selected_Space, currPlayer);
						}
					}
				}
				break;
			case 2: //sell
				if (Selected_Space != null) {
					if(Selected_Space.CompareTag("Property")){
						if (currPlayer.CheckProperty (Selected_Space)) {
							Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Sell_Confirm);
						}
					}
				}
				break;
			}
		}
		if (Input.GetButtonDown ("Cancel")) {
			if (currPlayer != null) {
				currPlayer.My_Controller.SwitchCameraMode ();
			} else {
				Turn_Manager.singleton.GetCurrentPlayer ().SwitchCameraMode ();
			}
			Turn_Manager.singleton.ReturnTurnStatus ();
		}
	}

	public void NewEnteredSpace(board_space bs){
		bool placed = false;
		foreach (List<board_space> b_list in highlighted_space) {
			if (b_list.Count >= 0) {
				if (b_list [0] == bs) {
					b_list.Add (bs);
					placed = true;
					if (b_list.Count == 4) {
						DisplayInformation (bs);
					}
				}
			}
		}
		if (!placed) {
			List<board_space> B_list = new List<board_space> ();
			B_list.Add (bs);
			highlighted_space.Add (B_list);
		}
	}

	public void NewExitedSpace(board_space bs){
		bool placed = false;
		List<board_space> remove_list = null;
		foreach (List<board_space> b_list in highlighted_space) {
			if (b_list.Count >= 0) {
				if (b_list [0] == bs) {
					if (b_list.Count == 4) {
						RemoveInformationDisplay ();
					}
					b_list.Remove (bs);
					if (b_list.Count == 0) {
						remove_list = b_list;
					}
				}
			}
		}
		if (remove_list != null) {
			highlighted_space.Remove (remove_list);
		}
	}

	private void DisplayInformation(board_space bs){
		Selected_Space = bs;
		Property_Display.singleton.SetInformation (bs);
	}

	private void RemoveInformationDisplay(){
		Selected_Space = null;
		Property_Display.singleton.HideInformation ();
	}

	public board_space _selectedSpace{
		get{ return Selected_Space; }
	}

	public void SetCurrentPlayer(Player_Information p_i){
		currPlayer = p_i;
		mode = 0;
	}

	public void SelectUpgrade(Player_Information p_i){
		mode = 1;
		currPlayer = p_i;
	}

	public void SelectSell(Player_Information p_i){
		mode = 2;
		currPlayer = p_i;
	}
}
