using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Space : board_space{
	public string purchase_text;
	public string buyout_text;
	public Material default_mat;
	[Space(15)]
	public Property_Coloring for_sale;
	public Property_Coloring Lvl_1;
	public Property_Coloring Lvl_2;
	public Property_Coloring Lvl_3;
	public TextMesh price_text;
	[Space(10)]
	public List<Renderer> owner_one_marking;
	public List<Renderer> owner_two_marking;
	private int status;
	private int last_status;
	private Property_Coloring current_building;
	public string property_name;
	private int price;
	public int value;
	private int initial_value;
	private int projected;
	private int multiplier;

	private List<Player_Information> owner;
	private District my_district;
	// Use this for initialization
	void Start () {
		owner = new List<Player_Information>();
		projected = 0;
		multiplier = 1;
		current_building = null;
		status = 0;
		last_status = -1;
		Build_House ();
		price_text.text = "$" + value + "";
		price = (int)(value * .25f);
		initial_value = value;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Build_House(){
		last_status = -1;
		if (current_building != null) {
			Destroy (current_building.gameObject);
		}
		GameObject building = for_sale.gameObject;
		Vector3 position = new Vector3 ();
		switch (status) {
		case 0:
			building = for_sale.gameObject;
			position = new Vector3 (this.transform.position.x, 75, this.transform.position.z - 50);
			break;
		case 1:
			building = Lvl_1.gameObject;
			position = new Vector3 (this.transform.position.x, 25, this.transform.position.z - 50);
			break;
		case 2:
			building = Lvl_2.gameObject;
			position = new Vector3 (this.transform.position.x, 25, this.transform.position.z - 50);
			break;
		case 3:
			building = Lvl_3.gameObject;
			position = new Vector3 (this.transform.position.x, 75, this.transform.position.z - 50);
			break;
		}
		GameObject b = Instantiate (building, this.transform);
		b.transform.position = position;
		current_building = b.GetComponent<Property_Coloring> ();
	}

	public override void Activate_Pass_Effect (Player_Information player, int dir)
	{
		//No effect for property
	}

	public override void Activate_Space_Effect (Player_Information player)
	{
		if (owner.Count == 0) {
			if (player.Spending_Limit () >= value) {
				Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Purchase, purchase_text + " " + value + "?");
			} else {
				Turn_Manager.singleton.ChangeTurn ();
			}
		} else {
			if (owner.Contains(player)) {
				Property_Informer.singleton.SelectUpgrade (player);
				Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Property_Upgrade, "Would you like to Upgrade a property?");
				//Investment_Menu m = (Investment_Menu)Menu_Manager.singleton.CurrentMenu ();
				//m.SetProperty (this, player);
			} else {
				player.PayRent (price);
				if (owner.Count == 1) {
					owner [0].RecieveMoney (price);
				}
				else if (owner.Count == 2) {
					owner [0].RecieveMoney (price / 2);
					owner [1].RecieveMoney (price / 2);
				}
				if (player.Spending_Limit () >= (value * multiplier) * 5) {
					Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Purchase, buyout_text + " " + ((value * multiplier) * 5) + "?");
				} else if(Turn_Manager.singleton.GetTurnStatus() != 3) {
					Turn_Manager.singleton.ChangeTurn ();
				}
			}
		}
	}

	public override void Activate_BackStep_Effect (Player_Information player)
	{
		//No effect for property
	}

	public void PropertyBought(Player_Information buyer){
		if (owner.Count == 0) {
			buyer.ChargeFee ((value * multiplier));
			owner.Add (buyer);
			price_text.text = "$" + (price * multiplier).ToString ();
			if (status == 0) {
				if (last_status != -1) {
					status = last_status;
				} else {
					status = 1;
				}
				Build_House ();
				ColorProperty (owner.Count);
			}
		} else {
			Player_Information old_owner = owner [0];
			owner.Clear ();
			buyer.ChargeFee ((value * multiplier) * 5);
			old_owner.RecieveMoney ((value * multiplier) * 2);
			owner.Add (buyer);
			ColorProperty (owner.Count);
		}
		my_district.CompareOwners ();
	}

	private void ColorProperty(int count){
		foreach (Renderer r in owner_two_marking) {
			if (count == 2) {
				r.material = owner [1].player_color;
				r.gameObject.layer = 0;
			} else if (count == 1) {
				r.material = owner [0].player_color;
				r.gameObject.layer = 0;
			} else {
				r.material = default_mat;
				r.gameObject.layer = 5;
			}
		}

		foreach (Renderer r in owner_one_marking) {
			if (count == 1) {
				r.material = owner [0].player_color;
				r.gameObject.layer = 0;
			} else {
				r.material = default_mat;
				r.gameObject.layer = 5;
			}
		}
		if (count == 1) {
			current_building.ChangeMaterial (owner [0].player_color);
		} else if (count == 2) {
			current_building.Coowner (owner [1].player_color);
		} else {
			current_building.ChangeMaterial (default_mat);
		}

	}

	public int worth{
		get{
			if (owner.Count == 1) {
				return value * multiplier;
			} else if (owner.Count == 2) {
				return (value * multiplier) / 2;
			} else {
				return 0;
			}
		}
	}

	public void ProposeInvestment(int amount, out int price_, out int value_){
		projected = amount + value;
		value_ = projected * multiplier;
		price_ = price + (int)((.1f * amount)) * multiplier;
	}

	public void ApproveInvestment(Player_Information p){
		p.ChargeFee (projected - value);
		price = price + (int)((.1f * (projected - value)));
		value = projected;

		price_text.text = "$" + (price * multiplier).ToString ();
		if (value > (status * 5) * initial_value) {
			status++;
			Build_House ();
			ColorProperty (owner.Count);
		}
	}

	public void SellProperty(){
		foreach (Player_Information own in owner) {
			own.RecieveMoney((value * multiplier) / owner.Count);
			own.RemoveProperty (this);
		}
		owner.Clear ();
		last_status = status;
		status = 0;
		multiplier = 1;
		Build_House ();
		price_text.text = "$" + value.ToString ();
		Turn_Manager.singleton.ReturnTurnStatus ();
		ColorProperty (owner.Count);
		my_district.CompareOwners ();
	}

	public District _district{
		get{ return my_district; }
		set{ my_district = value; }
	}

	public int Projected_Value{
		get{ return projected != 0 ? projected * value : value * multiplier; }
	}

	public int _price{
		get{ return price * multiplier; }
	}

	public int _value{
		get{ return value * multiplier; }
	}

	public int Change_Multiplier{
		set{ multiplier = value; 
			ChangePriceText ();
			}
	}

	private void ChangePriceText(){
		if (owner.Count != 0) {
			price_text.text = "$" + (price * multiplier).ToString ();
		} else {
			price_text.text = "$" + value.ToString ();
		}
	}

	public Player_Information _owner{
		get{
			if (owner.Count > 0) {
				return owner [0];
			} else {
				return null;
			}
		}
	}

	public string _name{
		get{ return property_name; }
	}
}
