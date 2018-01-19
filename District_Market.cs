using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District_Market : MonoBehaviour {

	public UnityEngine.UI.Button stock_buying_guide;
	private List<UnityEngine.UI.Button> buttons;
	private District this_district;
	public UnityEngine.UI.Text name_text;
	public UnityEngine.UI.Text price_text;

	private bool buying;
	// Use this for initialization
	void Start () {
		
	}

	public void UpdatePosition(float width, float y_pos, int players, District t_dist){
		this_district = t_dist;
		name_text.text = this_district._name;
		price_text.text = "$" + this_district._value.ToString();

		RectTransform r_t_2 = this.GetComponent<RectTransform> ();
		r_t_2.localPosition = new Vector3 (-24 - (43 * players), y_pos , 0);


		buttons = new List<UnityEngine.UI.Button> ();
		for (int i = 0; i < players; i++) {
			UnityEngine.UI.Button n_block = GameObject.Instantiate (stock_buying_guide, this.transform);
			UnityEngine.UI.Button.ButtonClickedEvent bce = new UnityEngine.UI.Button.ButtonClickedEvent ();
			bce.AddListener (BuyStock);
			n_block.onClick = bce;
			RectTransform r_t = n_block.GetComponent<RectTransform> ();
			r_t.localPosition = new Vector3 (140 + (87 * i) , 0 , 0);
			buttons.Add (n_block);
			UnityEngine.UI.Text tx = n_block.GetComponentInChildren<UnityEngine.UI.Text> ();
			tx.text = "0";
		}
	}

	public void UpdateButtonText(){
		price_text.text = "$" + this_district._value.ToString ();
		for (int i = 0; i < buttons.Count; i++) {
			UnityEngine.UI.Text button_text = buttons [i].GetComponentInChildren<UnityEngine.UI.Text> ();
			Player_Controller p_c = Turn_Manager.singleton.GetPlayer (i);
			if (p_c != null) {
				button_text.text = p_c.My_Info.GetStockAmount (this_district).ToString ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void BuyStock(){
		bool open = false;
		if (!buying) {
			int index = Turn_Manager.singleton.GetCurrentPlayerIndex ();
			if (buttons [index].GetComponentInChildren<UnityEngine.UI.Text> ().text != "0") {
				open = true;
			}
		} else {
			open = true;
		}
		if (open) {
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Stock_Buyer);
			Stock_Buying_Menu s_b_m = (Stock_Buying_Menu)Menu_Manager.singleton.CurrentMenu ();
			s_b_m.assign_information (this_district, Turn_Manager.singleton.GetCurrentPlayer ().My_Info, buying);
		}
	}

	public Vector3 GetButtonPosition(int p_index){
		return buttons [p_index].transform.position;
	}

	public void SetBuying(bool stat){
		buying = stat;
	}
}
