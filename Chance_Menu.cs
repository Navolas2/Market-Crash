using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance_Menu : Menu {

	private delegate void ChanceSpace();

	public UnityEngine.UI.Image chance_selector;
	public UnityEngine.UI.Image indicator;
	private int selector_x;
	private int selector_y;

	public Chance_Space change_guide;
	//private List<Chance_Space> chance_cards;
	private List<List<Dot>> dot_grid;


	public int spacing;
	public int horizontal_count;
	public int vertical_count;
	public Dot guide;
	private int timing;
	private Dot selected;

	private int menu_off_timer;


	// Use this for initialization
	void Start () {
		//set up board
		indicator.gameObject.SetActive(false);
		timing = -1;
		menu_off_timer = -1;
		//chance_cards = new List<Chance_Space> ();
		dot_grid = new List<List<Dot>> ();
		for (int i = 0; i < vertical_count; i++) {
			List<Dot> row = new List<Dot> ();
			for (int j = 0; j < horizontal_count; j++) {
				Dot dt  = (Dot)GameObject.Instantiate (guide, this.transform);
				dt.setCoordinates (j, i);
				float x_pos = -230 + (spacing * j);
				float z_pos = 180 - (spacing * i);
				RectTransform r_t_2 = dt.GetComponent<RectTransform> ();
				r_t_2.localPosition = new Vector3 (x_pos, z_pos , 0);
				row.Add (dt);
			}
			dot_grid.Add (row);
		}

		RectTransform r_t = chance_selector.GetComponent<RectTransform> ();
		r_t.localPosition = new Vector3 (-230, 180 , 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (timing == 0) {
			float horizontal = Input.GetAxis ("Horizontal");
			if (horizontal > .5) {
				horizontal = 1f;
			} else if (horizontal < -.5) {
				horizontal = -1f;
			} else {
				horizontal = 0f;
			}

			float vertical = Input.GetAxis ("Vertical");
			if (vertical > .5) {
				vertical = 1f;
			} else if (vertical < -.5) {
				vertical = -1f;
			} else {
				vertical = 0f;
			}
			vertical *= -1;

			Vector3 motion = new Vector3 (horizontal * spacing, -1 * (vertical * spacing), 0);
			if (horizontal != 0 || vertical != 0) {
				bool validmove = false;
				if (selected != null) {
					validmove = selected.ValidMove (selector_x + (int)horizontal, selector_y + (int)vertical);
				} else {
					if (selector_x + (int)horizontal >= 0 && selector_x + (int)horizontal < horizontal_count) {
						if (selector_y + (int)vertical >= 0 && selector_y + (int)vertical < vertical_count) {
							validmove = true;
						}
					}
				}
				if (validmove) {
					
					chance_selector.transform.localPosition += motion;
					timing = 10;
					selector_x += (int)horizontal;
					selector_y += (int)vertical;
				}
			}
			if (Input.GetButtonDown ("Submit")) {
				if (selected != null) {
					if (selected.ValidPosition (selector_x, selector_y)) {
						selected.connectDots (dot_grid [selector_y] [selector_x], this);
						menu_off_timer = 30;
						timing = -1;
					}
				} else {
					selected = dot_grid [selector_y] [selector_x];
					indicator.gameObject.SetActive(true);
					indicator.transform.localPosition = chance_selector.transform.localPosition;
				}
			}
		} else if (timing > 0) {
			timing--;
		} else if (menu_off_timer > 0) {
			menu_off_timer--;
		} else if (menu_off_timer == 0) {
			menu_off_timer--;
			Chance_Space c_s = Chance_Factory.singleton.GetChanceSpace ();
			if (c_s != null) {
				c_s.ActivateChance (Turn_Manager.singleton.GetCurrentPlayer ().My_Info);
			}
			Menu_Manager.singleton.Turn_Menu_Off (Menu_Manager.Chance_Menu);
		}
	}

	public void PlaceLine(Vector3 pos_1, Vector3 pos_2, UnityEngine.UI.Image line, Color l_color){
		UnityEngine.UI.Image placer = GameObject.Instantiate (line, gameObject.transform);
		placer.color = l_color;
		Vector3 position = pos_1 + ((pos_2 - pos_1) / 2);
		RectTransform r_t_2 = placer.GetComponent<RectTransform> ();
		r_t_2.localPosition = position;
	}

	public override void Turn_On ()
	{
		timing = 0;
		this.gameObject.SetActive (true);
		Menu_Manager.singleton.DisableIndicator ();
		indicator.gameObject.SetActive(false);
		selected = null;
		//base.Turn_On ();
	}

	public override void Turn_Off ()
	{
		this.gameObject.SetActive (false);
		indicator.gameObject.SetActive(false);
		timing = -1;
	}

	public override void ClickCancelButton ()
	{
		if (selected != null) {
			selected = null;
			indicator.gameObject.SetActive(false);
		}
	}

	public override void ClickButton (int index)
	{
		//nothing
	}

	public override Vector3 GetButtonPosition (int index)
	{
		return new Vector3 ();
	}
}
