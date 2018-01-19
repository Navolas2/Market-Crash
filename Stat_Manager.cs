using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Manager : MonoBehaviour {

	public static Stat_Manager singleton;

	private List<Player_Information> players;

	private UnityEngine.UI.Image background;
	private UnityEngine.UI.Image boarder;

	public Stat_Block stats_guide;
	private Stat_Block title_block;

	void Awake(){
		if (singleton == null) {
			singleton = this;
		} else if (singleton != this) {
			Destroy (this);
		}
	}

	// Use this for initialization
	public void Setup () {
		List<Player_Controller> player_controllers = Turn_Manager.singleton.players;
		players = new List<Player_Information> ();
		foreach (Player_Controller p_c in player_controllers) {
			players.Add (p_c.My_Info);
		}
		UnityEngine.UI.Image[] images = GetComponentsInChildren<UnityEngine.UI.Image> ();
		background = images [0];
		boarder = images [1];
		title_block = GameObject.Instantiate (stats_guide, this.transform);
		RectTransform r_t = title_block.GetComponent<RectTransform> ();
		title_block.title_set = true;
		Vector2 size_delt = new Vector2(285, (25 * (players.Count + 1)));
		background.rectTransform.sizeDelta = size_delt;
		boarder.rectTransform.sizeDelta = size_delt;
		float top_pos = size_delt.y / 2 - 15;
		r_t.localPosition = new Vector3 (-25, top_pos , 0);
		top_pos -= 20;
		foreach (Player_Information p_i in players) {
			Stat_Block n_block = GameObject.Instantiate (stats_guide, this.transform);
			RectTransform r_t_2 = n_block.GetComponent<RectTransform> ();
			r_t_2.localPosition = new Vector3 (-25, top_pos , 0);
			top_pos -= 20;
			p_i.SetBlock(n_block);
		}

		//Adjust Position of Box
		Vector2 button_dimentions = new Vector2(160, 20);
		Vector3 screen_spects = new Vector3 (((float)Screen.width / 2) - (button_dimentions.x * 1.1f), (((float)Screen.height / 2) - (button_dimentions.y * (players.Count))) * -1);
		this.transform.localPosition = screen_spects;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
