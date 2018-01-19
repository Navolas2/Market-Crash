using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Block : MonoBehaviour {

	public UnityEngine.UI.Text player_name;
	public UnityEngine.UI.Text money_text;
	private int money_int;
	private int money_change;
	public UnityEngine.UI.Text net_total;
	private int net_total_int;
	private int net_total_change;
	public List<UnityEngine.UI.Text> cash_progress;
	public UnityEngine.UI.Text wild_card;
	private int wild_cards = 0;
	public bool title_set = false;
	public int change_speed = 30;
	private int money_change_amount;
	private int net_change_amount;

	// Use this for initialization
	void Start () {
		if (title_set) {
			RemoveWildCard ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!title_set) {
			if (money_change != 0) {
				money_int += money_change_amount;
				money_text.text = money_int + "";
				money_change -= money_change_amount;
				if (Mathf.Abs (money_change_amount) > Mathf.Abs (money_change)) {
					money_change_amount = money_change;
				}
				if (money_change > 0) {
					money_text.color = Color.green;
				} else if (money_change < 0) {
					money_text.color = Color.red;
				} else {
					money_text.color = Color.white;
				}
			}
			if (net_total_change != 0) {
				net_total_int += net_change_amount;
				net_total.text = net_total_int + "";
				net_total_change -= net_change_amount;
				if (Mathf.Abs (net_change_amount) > Mathf.Abs (net_total_change)) {
					net_change_amount = net_total_change;
				}
				if (net_total_change > 0) {
					net_total.color = Color.green;
				} else if (net_total_change < 0) {
					net_total.color = Color.red;
				} else {
					net_total.color = Color.white;
				}
			}
		}
	}

	public void ChangeValues(int new_money, int new_net){
		money_change = new_money - money_int;
		net_total_change = new_net - net_total_int;
		money_change_amount = money_change / change_speed;
		if (money_change_amount == 0) {
			money_change_amount = money_change > 0 ? 1 : -1;
		}
		net_change_amount = net_total_change / change_speed;
		if (net_change_amount == 0) {
			net_change_amount = net_total_change > 0 ? 1 : -1;
		}
	}

	public void UpdateCashProgress(string prog){
		if (prog.Contains ("C")) {
			cash_progress [0].color = Color.green;
		} else {
			cash_progress [0].color = Color.clear;
		}
		if (prog.Contains ("A")) {
			cash_progress [1].color = Color.red;
		} else {
			cash_progress [1].color = Color.clear;
		}
		if (prog.Contains ("S")) {
			cash_progress [2].color = Color.magenta;
		} else {
			cash_progress [2].color = Color.clear;
		}
		if (prog.Contains ("H")) {
			cash_progress [3].color = Color.blue;
		} else {
			cash_progress [3].color = Color.clear;
		}
	}

	public void AddWildCard(){
		wild_cards++;
		wild_card.color = Color.black;
		wild_card.text = wild_cards.ToString() + "?";
	}

	public void RemoveWildCard(){
		wild_cards--;
		if (wild_cards < 0) {
			wild_cards = 0;
		}
		if (wild_cards == 0) {
			wild_card.color = Color.clear;
		} else {
			wild_card.color = Color.black;
			wild_card.text = wild_cards.ToString() + "?";
		}
	}

	public void SetValues(string p_name, int n_money, int n_net){
		money_int = n_money;
		net_total_int = n_net;
		player_name.text = p_name;
		money_text.text = money_int.ToString();
		net_total.text = net_total_int.ToString();
		UpdateCashProgress ("");
		RemoveWildCard ();
	}

	public void TextColor(Color player_color){
		player_name.color = player_color;
	}

	public int WildCardCount{
		get{ return wild_cards; }
	}
}
