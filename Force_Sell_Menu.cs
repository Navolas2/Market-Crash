using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force_Sell_Menu : Menu {

	// Use this for initialization
	void Start () {
		menu_text.text = "You must sell stock\n or property";
	}

	// Update is called once per frame
	void Update () {

	}

	public override void Turn_On ()
	{
		base.Turn_On ();
	}

	public override void Turn_Off ()
	{
		base.Turn_Off ();
	}
}
