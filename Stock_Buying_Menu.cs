using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock_Buying_Menu : Menu {

	private District selling_location;
	private Player_Information buying_player;

	private int player_money;
	public UnityEngine.UI.Text money_before;
	public UnityEngine.UI.Text money_after;
	private int stock_price;
	public UnityEngine.UI.Text stock_after;
	public UnityEngine.UI.Text max_purchase;
	private int purchase;
	public UnityEngine.UI.Text current_purchase;
	private int limit;

	private bool buying;
	private int IncreaseDelay;
	private int Delay_Amount = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float vertical = Input.GetAxis ("Horizontal");
		if (vertical > .5) {
			vertical = 1f;
		} else if (vertical < -.5) {
			vertical = -1f;
		} else {
			vertical = 0f;
		}
		if (vertical != 0) {
			if (IncreaseDelay % Delay_Amount == 0) {
				purchase += (int)vertical;
				IncreaseDelay += (int)vertical;
			} else {
				IncreaseDelay += (int)vertical;
			}
			if (purchase < 0) {
				purchase = 0;
			}
			if (purchase > limit) {
				purchase = limit;
			}
			current_purchase.text = purchase.ToString ();
			if (buying) {
				money_after.text = (player_money - (stock_price * purchase)) + "";
				stock_after.text = selling_location.StockIncrease (purchase).ToString () + "%";
			} else {
				money_after.text = (player_money + (stock_price * purchase)) + "";
				stock_after.text = selling_location.StockDecrease (purchase).ToString () + "%";
			}
		}
	}

	public void assign_information(District d, Player_Information p, bool _buying){
		buying = _buying;
		selling_location = d;
		buying_player = p;
		player_money = buying_player.Money_on_hand ();
		stock_price = selling_location._value;
		limit = player_money / stock_price;
		if (buying) {
			if (limit > 99) {
				limit = 99;
			}
		} else {
			limit = buying_player.GetStockAmount (selling_location);
		}

		money_before.text = player_money.ToString ();
		money_after.text = player_money.ToString ();
		stock_after.text = "0.0%";
		max_purchase.text = limit.ToString();
		current_purchase.text = "0";
		purchase = 0;
	}

	public override void Turn_Off ()
	{
		//base.Turn_Off ();
		gameObject.SetActive (false);
	}

	public override void Turn_On ()
	{
		//base.Turn_On ();
		gameObject.SetActive (true);
	}

	public override void ClickButton (int index)
	{
		base.ClickButton (index);
	}

	public void BuyStock(){
		if (buying) {
			selling_location.SellStock (purchase, Turn_Manager.singleton.GetCurrentPlayer ().My_Info);
			Menu_Manager.singleton.Turn_Menu_Off (Menu_Manager.Stock_Market);
			Menu_Manager.singleton.HideMenu ();
			Turn_Manager.singleton.UnpauseMovement ();
		} else if (!buying) {
			buying_player.SellStock (selling_location, purchase);
			Menu_Manager.singleton.Turn_Menu_Off (Menu_Manager.Stock_Market);
			Menu_Manager.singleton.HideMenu ();
			Turn_Manager.singleton.ReturnTurnStatus ();
		}

	}

	public void CancelPurchase(){
		Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Stock_Market);
		Turn_Off ();
	}
}
