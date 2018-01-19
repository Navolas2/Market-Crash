using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Manager : MonoBehaviour {

	public static Turn_Manager singleton;

	public Camera_Controller main_camera;
	public Player_Controller player_guide;
	public List<Material> player_colors;
	public UnityEngine.UI.Text move_counter;
	private List<Player_Controller> player_list;
	private List<Player_Information> commissions;
	private int Turn_Status;

	private int money_goal;

	private int player_index;
	private int turn_switch_timer;

	void Awake(){
		if (singleton == null) {
			singleton = this;
			player_index = 0;
		} else if (singleton != this) {
			Destroy (this);
		}
	}
		
	// Use this for initialization
	void Start () {
		money_goal = Scene_Switcher.singleton._goal;
		turn_switch_timer = -1;
		main_camera = FindObjectOfType<Camera> ().GetComponent<Camera_Controller>();
		commissions = new List<Player_Information> ();
		if (player_list != null) {
			if (player_list.Count != 0) {
				main_camera.SetTarget (players [player_index].gameObject);
			}
		}
	}

	public void CreatePlayers(List<string> _names, List<Color> _color){
		player_list = new List<Player_Controller> ();
		for (int i = 0; i < _names.Count; i++) {
			player_colors [i].color = _color[i];
			Player_Controller p_c = (Player_Controller)GameObject.Instantiate (player_guide);
			p_c.GetComponent<Player_Information> ().SetInformation (player_colors [i], _names[i]);
			p_c.move_counter = move_counter;
			player_list.Add (p_c);
		}
		Turn_Status = 0;
		players [player_index].set_playing = true;
		main_camera.SetTarget (players [player_index].gameObject); 
		Stat_Manager.singleton.Setup ();
	}
	// Update is called once per frame
	void Update () {
		if (turn_switch_timer > 0) {
			turn_switch_timer--;
		} else if (turn_switch_timer == 0) {
			turn_switch_timer--;
			Change_Turn ();
		}
	}

	public void ChangeTurn(){
		turn_switch_timer = 60;
	}

	private void Change_Turn(){
		if (players [player_index].My_Info.Money_on_hand() < 0 && players [player_index].My_Info._net_total() > 0) {
			Global_text.singleton.SetText ("You are below 0 and must sell property or stock", 0);
			Turn_Manager.singleton.SetTurnStatus (3);
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Force_Sell);
		} else if(players [player_index].My_Info.Money_on_hand() < 0 && players [player_index].My_Info._net_total() <= 0){
			PrepareToEndGame ();
		} else if(players[player_index].My_Info._net_total() >= money_goal){
			PrepareToEndGame ();
		}else if(Turn_Status != 3) {
			Menu_Manager.singleton.HideAllMenu ();
			players [player_index].set_playing = false;
			UpdateNetTotals ();
			player_index++;
			if (player_index >= players.Count) {
				player_index = 0;
			}
			players [player_index].set_playing = true;
			main_camera.ChangeTarget (players [player_index].gameObject);
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Turn_Start);
			Turn_Status = 0;
			Property_Informer.singleton.SetCurrentPlayer (players [player_index].My_Info);
			Property_Display.singleton.HideInformation ();
			Global_text.singleton.SetText (GetCurrentPlayer ().My_Info.my_name + " turn start!", 45);
			commissions.Remove (GetCurrentPlayer ().My_Info);
		} else {
			ReturnTurnStatus ();
		}
	}

	private void PrepareToEndGame(){
		List<string> player_names = new List<string> ();
		players.Sort (delegate(Player_Controller x, Player_Controller y) {
			return y.My_Info._net_total () - x.My_Info._net_total ();
		});
		foreach (Player_Controller p_c in players) {
			player_names.Add (p_c.My_Info.my_name);
		}
		Scene_Switcher.singleton.SetRankInformation (player_names);
		Scene_Switcher.singleton.SwitchToEndScene ();
	}

	private void UpdateNetTotals(){
		foreach (Player_Controller p_c in players) {
			p_c.My_Info.UpdateMoney ();
		}
	}

	public Player_Controller GetCurrentPlayer(){
		return players [player_index];
	}

	public int GetCurrentPlayerIndex(){
		return player_index;
	}

	public Player_Controller GetPlayer(int i){
		if (i >= 0 && i < players.Count) {
			return players [i];
		} else {
			return null;
		}
	}

	public void AddCommision(Player_Information p_i){
		commissions.Add (p_i);
	}

	public void PayCommision(int amount){
		int commision = (int)(amount * .1f);
		foreach (Player_Information p_i in commissions) {
			p_i.RecieveMoney (commision);
		}
	}

	public void Stop_Move(){
		players [player_index].StopMove ();
	}

	public void Keep_Moving(){
		players [player_index].StepBack ();
	}

	public void UnpauseMovement(){
		if (Turn_Status == 1) {
			players [player_index].UnPause ();
		} else if (Turn_Status == 0) {
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Turn_Start);
		}
	}

	public void Buy_Space(){
		players [player_index].My_Info.BuyProperty ();
		ChangeTurn ();
	}

	public void Dont_Buy_Space(){
		//end of turn procedure
		ChangeTurn();
	}

	public void Dont_Sell_Space(){
		ReturnTurnStatus ();
	}

	public void Invest(){
		players [player_index].My_Info.Invest ();
		players [player_index].ReturnCamera ();
		ChangeTurn ();
	}

	public void SellProperty(){
		Property_Informer.singleton.SelectSell(players[player_index].My_Info);
		SelectSpace();
	}

	public void SellConfirm(){
		Property_Space p_s = (Property_Space)Property_Informer.singleton._selectedSpace;
		p_s.SellProperty ();
	}

	public void SelectSpace(){
		players [player_index].FreeCamera ();
	}

	public List<Player_Controller> players{
		get{ return player_list; }
	}

	public void AdvanceTurnStatus(){
		Turn_Status++;
	}

	public void SetTurnStatus(int stat){
		Turn_Status = stat;
	}

	public int GetTurnStatus(){
		return Turn_Status;
	}

	public void ReturnTurnStatus(){
		players [player_index].ReturnCamera ();
		switch (Turn_Status) {
		case 0: //turn start
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Turn_Start);
			break;
		case 1: //moving
			GetCurrentPlayer ().UnPause ();
			break;
		case 2: //panel effect
			break;
		case 3: //owing money
			if (GetCurrentPlayer ().My_Info.Money_on_hand () < 0) {
				//Open Sell Menu
				Menu_Manager.singleton.Turn_Menu_On(Menu_Manager.Force_Sell);
			} else {
				Turn_Status = -1;
				ChangeTurn ();
			}
			break;
		case 4: //upgrading
			//open menu for upgrade option
			Debug.Log ("reopen");
			break;
		}
	}
}
