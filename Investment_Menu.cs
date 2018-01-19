using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investment_Menu : Menu{

	private Property_Space updating;
	private Player_Information investor;

	public UnityEngine.UI.Text price_before_text;
	public UnityEngine.UI.Text price_after_text;
	public UnityEngine.UI.Text value_before_text;
	public UnityEngine.UI.Text value_after_text;
	public UnityEngine.UI.Text stock_text;
	public UnityEngine.UI.Text investment_text;

	private int investing;
	private int limit;

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
				investing += (int)vertical;
				IncreaseDelay += (int)vertical;
			} else {
				IncreaseDelay += (int)vertical;
			}
			if (investing < 0) {
				investing = 0;
			}
			if (investing > limit) {
				investing = limit;
			}
			investment_text.text = investing.ToString ();
			int price = 0;
			int value = 0;
			updating.ProposeInvestment (investing, out price, out value);
			price_after_text.text = price.ToString ();
			value_after_text.text = value.ToString ();
			stock_text.text = updating._district.StockIncrease (0).ToString() + "%";
		}
	}

	public void SetProperty(Property_Space p, Player_Information player){
		updating = p;
		investor = player;
		price_before_text.text = updating._price.ToString ();
		price_after_text.text = updating._price.ToString ();
		value_after_text.text = updating.value.ToString ();
		value_before_text.text = updating.value.ToString ();
		stock_text.text = updating._district.StockIncrease (0).ToString() + "%";
		investment_text.text = "0";
		investing = 0;
		limit = investor.Spending_Limit ();
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
}
