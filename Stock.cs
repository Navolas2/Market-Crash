using UnityEngine;
using System.Collections;

public class Stock
{
	private int shares;

	private District market;

	public Stock(District m, int bought_shares){
		market = m;
		shares = bought_shares;
	}

	public void BoughtStock(int bought_shares){
		shares += bought_shares;
	}

	public int Value(){
		int val = shares * market._value;
		return val;
	}

	public int SellStock(int quantity){
		int money = quantity * market._value;
		shares -= quantity;
		market.Stock_Sold (quantity);
		return money;
	}

	public int stock_quantity{
		get{return shares;}
	}

	public bool CompareDistrict(District d){
		return d == market;
	}

	public District _District{
		get{ return market; }
	}
}

