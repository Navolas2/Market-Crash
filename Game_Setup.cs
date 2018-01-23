using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Setup : MonoBehaviour {

	public Player_Information_Input guide;

	private List<Player_Information_Input> active_inputs;

	private int starting = 2000;
	private int goal = 10000;

	// Use this for initialization
	void Start () {
		ChangeActiveInputs (0);
	}
	
	public void ChangeActiveInputs(int amount){
		amount+= 2;
		if (active_inputs == null) {
			active_inputs = new List<Player_Information_Input> ();
		}
		while (active_inputs.Count < amount) {
			Player_Information_Input p_i2 = (Player_Information_Input)GameObject.Instantiate (guide, this.transform);
			p_i2.set_up_game = this;
			p_i2.ChangeColor (active_inputs.Count);

			Vector3 position = new Vector3 (0, 0 - (30 * active_inputs.Count), 0);
			p_i2.transform.localPosition = position;
			active_inputs.Add (p_i2);
		}
		while (active_inputs.Count > amount) {
			Player_Information_Input p_i2 = active_inputs [active_inputs.Count - 1];
			active_inputs.Remove (p_i2);
			Destroy (p_i2.gameObject);
		}
	}

	public void StartGame(){
		if (goal != starting && goal > starting) {
			List<Color> colors = new List<Color> ();
			foreach (Player_Information_Input p_i2 in active_inputs) {
				if (!colors.Contains (p_i2._color)) {
					colors.Add (p_i2._color);
				}
			}
			if (colors.Count == active_inputs.Count) {
				Scene_Switcher.singleton.SetPlayerInformation (active_inputs);
				Scene_Switcher.singleton._goal = goal;
				Scene_Switcher.singleton._money_start = starting;
				Scene_Switcher.singleton.SwitchToGameScene ();
			}
		}
	}

	public bool ColorValidityCheck(int color, Player_Information_Input changer){
		int color_count = 0;
		List<int> taken_colors = new List<int> ();
		taken_colors.Add (color);
		foreach (Player_Information_Input p_i2 in active_inputs) {
			if (p_i2.my_color_input.value == color) {
				color_count++;
			} else {
				taken_colors.Add (p_i2.my_color_input.value);
			}
		}
		return !(color_count > 1) ;
	}

	public void goalChanged(string new_goal){
		bool worked = int.TryParse (new_goal, out goal);
		if (!worked) {
			goal = 10000;
		}
	}

	public void startChanged(string new_goal){
		bool worked = int.TryParse (new_goal, out starting);
		if (!worked) {
			starting = 2000;
		}
	}

	public void InstructionScene(){
		Scene_Switcher.singleton.SwitchInstructionScreen ();
	}

	public void CreditScene(){
		Scene_Switcher.singleton.SwitchCreditScreen ();
	}
}
