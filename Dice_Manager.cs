using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Manager : MonoBehaviour {

	public static Dice_Manager singleton;

	private int die_roll_counter = 30;
	public int force_roll = -1;
	private int counter;
	private Player_Controller current_roller;
	private UnityEngine.UI.Text my_text;
	int roll;
	// Use this for initialization
	void Awake(){
		if (singleton == null) {
			singleton = this;
		} else if (singleton != this) {
			Destroy (this);
		}
	}

	void Start () {
		my_text = GetComponent<UnityEngine.UI.Text> ();
		my_text.enabled = false;
		my_text.text = "0";
		roll = 0;
		counter = 70;
	}
	
	// Update is called once per frame
	void Update () {
		if (counter < 30) {
			roll = Random.Range (1, 7);
			my_text.text = roll + "";
			counter++;
		} else if (counter == die_roll_counter) {
			counter++;
			if (force_roll == -1)
				PassRoll (roll);
			else
				PassRoll (force_roll);
		}else if (counter < 60) {
			counter++;
		} else if (counter == 60) {
			my_text.enabled = false;
			counter++;
		}
	}

	public void RollDie(){
		current_roller = Turn_Manager.singleton.GetCurrentPlayer();
		Global_text.singleton.ResetCounter ();
		counter = 0;
		my_text.enabled = true;
	}

	private void PassRoll(int value){
		Turn_Manager.singleton.AdvanceTurnStatus ();
		current_roller.SetMoveAmount (value);
	}
}
