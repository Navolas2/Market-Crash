using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class District : MonoBehaviour
{
	private int stock_value;
	public string district_name;
	public List<Property_Space> district_properties;
	//public Material mat;

	private int Stock_Change;
	private int Stocks_Sold;
	private List<Stock> stock_sold_updater;
	// Use this for initialization
	void Start ()
	{
		int price = 0;
		foreach (Property_Space p in district_properties) {
			p._district = this;
			price += p.value;
		}
		stock_value = (int)((float)price * .01f);
		stock_sold_updater = new List<Stock> ();
		//Debug.Log (stock_value);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SellStock(int amount, Player_Information player){
		Stock s = new Stock (this, amount);
		player.BuyStock (s);
		Stocks_Sold += amount;
		UpdateStockValue ();
	}

	public void Stock_Sold(int amount){
		Stocks_Sold -= amount;
		UpdateStockValue ();
	}

	public void UpdateStockValue(){
		int old_price = stock_value;
		int price = 0;
		foreach (Property_Space p in district_properties) {
			price += p.value;
		}
		stock_value = (int)((float)price * .01f);
		stock_value += (Stocks_Sold / 10);
		Stock_Change += stock_value - old_price;
	}

	public int StockIncrease(int stock_bought){
		int price = 0;
		foreach (Property_Space p in district_properties) {
			price += p.Projected_Value;
		}
		price = (int)((float)price * .01f);
		price += (Stocks_Sold + stock_bought) / 10;
		float inc =  ((float)price / (float)stock_value) - 1f;
		inc *= 100;
		//inc -= 100;
		return (int)inc;
	}

	public int StockDecrease(int stock_bought){
		int price = 0;
		foreach (Property_Space p in district_properties) {
			price += p.Projected_Value;
		}
		price = (int)((float)price * .01f);
		price += (Stocks_Sold - stock_bought) / 10;
		float inc =  ((float)price / (float)stock_value) - 1f;
		inc *= 100;
		//inc -= 100;
		return (int)inc;
	}

	public int ValueStock(int quantity){
		return quantity * _value;
	}

	public int _value{
		get{ return stock_value; }
	}

	public string _name{
		get{ return district_name; }
	}

	public void CompareOwners(){
		for(int i = 0; i < district_properties.Count; i++){
			int multi = 1;
			for (int j = 0; j < district_properties.Count; j++) {
				if (i != j) {
					if (district_properties [i]._owner == district_properties [j]._owner) {
						multi++;
					}
				}
			}
			if (district_properties [i]._owner == null) {
				multi = 1;
			}
			district_properties [i].Change_Multiplier = multi;
		}
	}
}

