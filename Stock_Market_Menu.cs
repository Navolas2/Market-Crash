using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock_Market_Menu : Menu {

	public District_Market market_guide;
	public List<District> map_districts;
	private List<District_Market> markets;

	public UnityEngine.UI.Image background;
	public UnityEngine.UI.Image boarder;
	private int district_button_size = 45;

	private bool buying;

	// Use this for initialization
	void Start () {
		buying = true;
		int players = Turn_Manager.singleton.players.Count;
		float width = 200 + (86 * players);
		float height = map_districts.Count * district_button_size;
		RectTransform r_t = background.rectTransform;
		r_t.sizeDelta = new Vector2 (width, height);
		r_t = boarder.rectTransform;
		r_t.sizeDelta = new Vector2 (width, height);

		markets = new List<District_Market> ();
		for(int i = 0; i < map_districts.Count; i++){
			District_Market d_t = GameObject.Instantiate (market_guide, this.transform);
			d_t.UpdatePosition (width, height/2 - (district_button_size * i) - district_button_size/2, players, map_districts[i]);
			markets.Add (d_t);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void ClickButton (int index)
	{
		markets [index].BuyStock ();
	}

	public override void ClickCancelButton ()
	{
		base.ClickCancelButton ();
		Turn_Manager.singleton.UnpauseMovement ();
	}

	public override int GetButtonCount ()
	{
		return markets.Count;
	}

	public override Vector3 GetButtonPosition (int index)
	{
		return markets [index].GetButtonPosition (Turn_Manager.singleton.GetCurrentPlayerIndex ());
	}

	public override void Turn_Off ()
	{
		gameObject.SetActive (false);
	}

	public override void Turn_On ()
	{
		gameObject.SetActive (true);
		foreach (District_Market d_m in markets) {
			d_m.UpdateButtonText ();
		}
	}

	public void SetBuying(bool stat){
		buying = stat;
		foreach (District_Market d_m in markets) {
			d_m.SetBuying (buying);
		}
	}
}
