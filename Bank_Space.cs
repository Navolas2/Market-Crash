using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank_Space : board_space {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Activate_Pass_Effect (Player_Information player, int dir)
	{

		if (dir == 1) {
			int status = player.Cash_Status ();
			if (status >= 4) {
				//Give cash bonus
				player.GiveBonus ();
			} else {
				player.ClearBonus ();
			}
			Turn_Manager.singleton.GetCurrentPlayer ().PauseMovement ();
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Stock_Market);
			Stock_Market_Menu menuu = (Stock_Market_Menu)Menu_Manager.singleton.CurrentMenu ();
			menuu.SetBuying (true);
			player.My_Controller.PassedBank ();

		} else if (dir == -1) {
			player.UndoBankPass ();
		}

		//
	}

	public override void Activate_Space_Effect (Player_Information player)
	{
		player.My_Controller.AnyDirection ();
		Turn_Manager.singleton.ChangeTurn ();
	}

	public void SellStocks(){
		Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Stock_Market);
		Stock_Market_Menu menuu = (Stock_Market_Menu)Menu_Manager.singleton.CurrentMenu ();
		menuu.SetBuying (false);
	}

	public override void Activate_BackStep_Effect(Player_Information player){
		player.UndoBankPass ();
	}
}
