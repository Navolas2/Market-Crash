using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Information : MonoBehaviour {

	private int level;
	private int money;
	private int net_total;
	private int spendable_net_worth;
	private List<Property_Space> owned_property;
	private List<Stock> owned_stock;
	private List<char> cash_progress;
	public Material player_color;
	public GameObject position_indicator;
	public string my_name;
	[HideInInspector]
	public Player_Controller My_Controller;
	[HideInInspector]
	public Stat_Block My_Block;

	private Stock last_bought;
	private int last_spent;
	private int last_bonus;

	// Use this for initialization
	public void SetInformation(Material c, string name){
		my_name = name;
		player_color = c;
	}

	void Awake(){
		My_Controller = GetComponent<Player_Controller> ();
	}

	void Start () {
		level = 1;

		money = Scene_Switcher.singleton._money_start;
		owned_property = new List<Property_Space> ();
		owned_stock = new List<Stock> ();
		cash_progress = new List<char> ();
		UpdateNetTotals ();
		if (My_Block != null) {
			My_Block.SetValues (my_name, money, net_total);
		}
		Renderer[] renders = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in renders) {
			r.material = player_color;
		}
		position_indicator.GetComponent<Renderer> ().material = player_color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Pass_Character(char character){
		if (character != ' ') {
			if (cash_progress.Contains (character)) {
				if (character != '#') {
					money += 100;
					UpdateNetTotals ();
				}
			} else {
				cash_progress.Add (character);
				string o_string = "";
				foreach (char c in cash_progress) {
					o_string = o_string + c;
				}
				o_string = o_string.ToUpper ();

				My_Block.UpdateCashProgress (o_string);
			}
		}
	}

	public void Undo_Character(char character){
		if (cash_progress [cash_progress.Count - 1] == character) {
			cash_progress.RemoveAt (cash_progress.Count - 1);
			string o_string = "";
			foreach (char c in cash_progress) {
				o_string = o_string + c;
			}
			o_string = o_string.ToUpper ();
			My_Block.UpdateCashProgress (o_string);
		} else {
			money -= 100;
			UpdateNetTotals ();
		}
	}

	public int Cash_Status(){
		int prog = cash_progress.Count;
		prog += My_Block.WildCardCount;
		return prog;
	}

	public void ChargeFee(int price){
		Global_text.singleton.SetText ("Payed out $" + price, 0);
		money -= price;
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
		//Turn_Manager.singleton.PayCommision (price);
	}

	public void PayRent(int price){
		Global_text.singleton.SetText ("Payed out $" + price, 0);
		money -= price;
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
		Turn_Manager.singleton.PayCommision (price);
	}

	public void RecieveMoney(int price){
		money += price;
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
	}

	public void BuyStock(Stock s){
		Stock owned = null;
		last_bought = s;
		foreach (Stock s_1 in owned_stock) {
			if (s.CompareDistrict (s_1._District)) {
				owned = s_1;
			}
		}
		if (owned == null) {
			owned_stock.Add (s);
		} else {
			owned.BoughtStock (s.stock_quantity);
		}
		ChargeFee (s.Value ());
		last_spent = s.Value ();
	}

	public void SellStock(District d, int amount){
		Stock sell = null;
		foreach (Stock s in owned_stock) {
			if (s.CompareDistrict (d)) {
				sell = s;
			}
		}
		if (sell != null) {
			money += sell.SellStock (amount);
			if (sell.stock_quantity == 0) {
				owned_stock.Remove (sell);
			}
		}
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
	}

	public void UndoBankPass(){
		UndoStockPurchase ();
		money -= last_bonus;
	}

	public void UndoStockPurchase(){
		Stock sell = null;
		foreach (Stock s in owned_stock) {
			if (s.CompareDistrict (last_bought._District)) {
				sell = s;
			}
		}
		if (sell != null) {
			sell.SellStock (last_bought.stock_quantity);
			money += last_spent;
			if (sell.stock_quantity == 0) {
				owned_stock.Remove (sell);
			}
		}
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
	}

	public void ClearBonus(){
		last_bonus = 0;
	}

	public int GetStockAmount(District d){
		int owned = 0;
		foreach (Stock s in owned_stock) {
			if (s.CompareDistrict (d)) {
				owned = s.stock_quantity;
			}
		}
		return owned;
	}

	public void RemoveProperty(Property_Space p_i){
		owned_property.Remove (p_i);
	}

	public bool CheckProperty(board_space b_s){
		Property_Space p_i = (Property_Space)b_s;
		return owned_property.Contains (p_i);
	}

	private void UpdateNetTotals(){
		int total = money;
		foreach (Stock s in owned_stock) {
			total += s.Value ();
		}
		spendable_net_worth = total;
		foreach (Property_Space p in owned_property) {
			total += p.worth;
		}
		//add in stock values
		net_total = total;

	}

	public void Invest(){
		Property_Space p = (Property_Space)Property_Informer.singleton._selectedSpace;
		p.ApproveInvestment (this);
	}

	public void BuyProperty(){
		Property_Space p = (Property_Space)My_Controller.position;
		p.PropertyBought (this);
		owned_property.Add (p);
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
	}

	public void GiveBonus(){
		float bonus = 0;
		bonus += 300 * (level * 1.5f);
		int value = 0;
		foreach (Property_Space p in owned_property) {
			value += p.worth;
		}
		bonus += (value * .1f);
		Global_text.singleton.SetText("Awarded bonus of " + bonus, 0);
		money += (int)bonus;
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
		int count = cash_progress.Count;
		cash_progress.Clear ();
		for (int i = 0; i < (4 - count); i++) {
			My_Block.RemoveWildCard ();
		}
		My_Block.UpdateCashProgress ("");
	}

	public void  SetBlock(Stat_Block b){
		My_Block = b;
		My_Block.SetValues (my_name, money, net_total);
		My_Block.TextColor (player_color.color);
	
	}

	public int Spending_Limit(){
		return spendable_net_worth;
	}

	public int Money_on_hand(){
		return money;
	}

	public int _net_total(){
		return net_total;
	}

	public void UpdateMoney(){
		UpdateNetTotals ();
		My_Block.ChangeValues (money, net_total);
	}
}
