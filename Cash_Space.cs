using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash_Space : board_space {

	public char Cash_Character;
	public GameObject Cash_c;
	public GameObject Cash_a;
	public GameObject Cash_s;
	public GameObject Cash_h;
	private GameObject current_character;

	public bool switch_character = false;

	// Use this for initialization
	void Start () {
		current_character = null;
		Place_Character ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Place_Character(){
		if (current_character != null) {
			Destroy (current_character);
		}
		GameObject build_character = null;
		switch (Cash_Character) {
		case 'c':
		case 'C':
			Cash_Character = 'c';
			build_character = Cash_c;
			break;
		case 'a':
		case 'A':
			Cash_Character = 'a';
			build_character = Cash_a;
			break;
		case 's':
		case 'S':
			Cash_Character = 's';
			build_character = Cash_s;
			break;
		case 'h':
		case 'H':
			Cash_Character = 'h';
			build_character = Cash_h;
			break;
		}
		if (build_character != null) {
			current_character = Instantiate (build_character, this.transform);
			current_character.transform.position = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
		}
	}

	private void Change_Character(){
		if (Cash_Character == 'c') {
			Cash_Character = 'a';
		} else if (Cash_Character == 'a') {
			Cash_Character = 's';
		} else if (Cash_Character == 's') {
			Cash_Character = 'h';
		} else if (Cash_Character == 'h') {
			Cash_Character = 'c';
		}
		Place_Character ();
	}

	private void Reverse_Character(){
		if (Cash_Character == 'c') {
			Cash_Character = 'h';
		} else if (Cash_Character == 'a') {
			Cash_Character = 'c';
		} else if (Cash_Character == 's') {
			Cash_Character = 'a';
		} else if (Cash_Character == 'h') {
			Cash_Character = 's';
		}
		Place_Character ();
	}

	public override void Activate_Pass_Effect (Player_Information player, int dir)
	{
		if (dir == 1) {
			player.Pass_Character (Cash_Character);
			if (switch_character) {
				Change_Character ();
			}
		} else if (dir == -1) {
			if (switch_character) {
				Reverse_Character ();
			}
			player.Undo_Character (Cash_Character);
		}
	}

	public override void Activate_Space_Effect (Player_Information player)
	{
		//Turn_Manager.singleton.ChangeTurn ();
		Menu_Manager.singleton.Turn_Menu_On(Menu_Manager.Chance_Menu);
		//throw new System.NotImplementedException ();
	}

	public override void Activate_BackStep_Effect (Player_Information player)
	{
		if (switch_character) {
			Reverse_Character ();
		}
		player.Undo_Character (Cash_Character);
	}
}
