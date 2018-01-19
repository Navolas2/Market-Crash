using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommisionFreeSpace : board_space {

	public bool IsCommisionSpace;
	public bool IsRerollSpace;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Activate_Pass_Effect (Player_Information player, int direction)
	{
		//none
	}

	public override void Activate_Space_Effect (Player_Information player)
	{
		if (IsCommisionSpace) {
			Turn_Manager.singleton.AddCommision (player);
		}
		if (!IsRerollSpace) {
			Turn_Manager.singleton.ChangeTurn ();
		} else {
			Dice_Manager.singleton.RollDie ();
		}

	}

	public override void Activate_BackStep_Effect (Player_Information player)
	{
		//no effect
	}
}
