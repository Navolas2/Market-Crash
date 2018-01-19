using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class board_space : MonoBehaviour {

	public board_space up_space;
	public board_space up_left_space;
	public board_space left_space;
	public board_space down_left_space;
	public board_space down_space;
	public board_space down_right_space;
	public board_space right_space;
	public board_space up_right_space;
	[Space(15)]
	public GameObject player_position;

	public board_space move_space(int direction, out int last_dir){
		last_dir = 0;
		if (direction > 0 && direction != 5 && direction < 10) {
			switch (direction) {
			case 1:
				last_dir = 9;
				return down_left_space;
			case 2:
				last_dir = 8;
				return down_space;
			case 3:
				last_dir = 7;
				return down_right_space;
			case 4:
				last_dir = 6;
				return left_space;
			case 6:
				last_dir = 4;
				return right_space;
			case 7:
				last_dir = 3;
				return up_left_space;
			case 8:
				last_dir = 2;
				return up_space;
			case 9:
				last_dir = 1;
				return up_right_space;
			}
		}
		return this;
	}

	public abstract void Activate_Space_Effect(Player_Information player);

	public abstract void Activate_Pass_Effect(Player_Information player, int direction);

	public abstract void Activate_BackStep_Effect (Player_Information player);
}
