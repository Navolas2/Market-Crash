using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Information_Input : MonoBehaviour {

	private string my_name;
	private Color selected_color;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateNameInformation(string s){
		my_name = s;
	}

	public void SelectNewColor(int c){
		switch (c) {
		case 0:
			selected_color = Color.black;
			break;
		case 1:
			selected_color = Color.blue;
			break;
		case 2:
			selected_color = Color.cyan;
			break;
		case 3:
			selected_color = Color.green;
			break;
		case 4:
			selected_color = Color.magenta;
			break;
		case 5:
			selected_color = Color.red;
			break;
		case 6:
			selected_color = Color.yellow;
			break;
		}
	}

	public Color _color{
		get{selected_color.a = 1f;
			return selected_color; }
	}

	public string _name{
		get{ return my_name; }
	}
}
