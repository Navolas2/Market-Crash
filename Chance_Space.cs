using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance_Space {
	private string display_text; //text displayed when given this chance event


	private int move; //0 no move, 1 repeat move, 2 roll again
	private int direction; //0 direction remains, 1 free direction
	private string cash_given; //letters of cash given by chance event
	private int money_exchanged; //amount of money given or taken
	private int forced; //0 given option to accept, 1 forced to do the event
	private int warp; //0 no warp, 1 no place

	Player_Information effector;

	public Chance_Space(string tx, int m, int d, string c_g, int m_e, int f, int w){
		display_text = tx;
		move = m;
		direction = d;
		cash_given = c_g;
		money_exchanged = m_e;
		forced = f;
		warp = w;
	}

	public void ActivateChance(Player_Information p_i){
		Global_text.singleton.SetText (display_text, 0, DelayedActivation);
		effector = p_i;
	}

	public void DelayedActivation(){
		if (move == 1) {
			effector.My_Controller.RepeatMove ();
		} else if (move == 2) {
			Dice_Manager.singleton.RollDie ();
		}
		if (direction == 1) {
			effector.My_Controller.AnyDirection ();
		}
		char[]cash =  cash_given.ToCharArray ();
		foreach (char c in cash) {
			effector.Pass_Character (c);
		}
		effector.RecieveMoney (money_exchanged);

		if (move == 0) {
			Turn_Manager.singleton.ChangeTurn ();
		}
	}
}
