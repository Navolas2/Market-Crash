using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {

	private Dot above;
	private int finished_up = 0;
	private Dot left;
	private int finished_left = 0;
	private Dot right;
	private int finished_right = 0;
	private Dot below;
	private int finished_below = 0;

	private int x_coord;
	private int y_coord;

	public UnityEngine.UI.Image Line_horizontal;
	public UnityEngine.UI.Image Line_vertical;

	private bool started = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCoordinates(int x, int y){
		x_coord = x;
		y_coord = y;
	}

	public void connectDots(Dot d, Chance_Menu c_m){
		d.connectDots_sub (this);
		Vector3 position = d.getCoordinates ();
		int x_loc = (int)position.x;
		int y_loc = (int)position.y;
		if (x_loc == x_coord) {
			if (y_loc > y_coord) {
				below = d;
			} else if (y_loc < y_coord) {
				above = d;
			}
			c_m.PlaceLine (transform.localPosition, d.transform.localPosition, Line_vertical, Turn_Manager.singleton.GetCurrentPlayer ().My_Info.player_color.color);
		} else if (y_loc == y_coord) {
			if (x_loc < x_coord) {
				left = d;
			} else if (x_loc > x_coord) {
				right = d;
			}
			c_m.PlaceLine (transform.localPosition, d.transform.localPosition, Line_horizontal, Turn_Manager.singleton.GetCurrentPlayer ().My_Info.player_color.color);
		}
		CheckBoxStart ();
	}

	public void connectDots_sub(Dot d){
		Vector3 position = d.getCoordinates ();
		int x_loc = (int)position.x;
		int y_loc = (int)position.y;
		if (x_loc == x_coord) {
			if (y_loc > y_coord) {
				below = d;
			} else if (y_loc < y_coord) {
				above = d;
			}
		} else if (y_loc == y_coord) {
			if (x_loc < x_coord) {
				left = d;
			} else if (x_loc > x_coord) {
				right = d;
			}
		}
	}
		

	private void CheckBoxStart(){
		started = true;
		List<int> dir = new List<int> ();
		int count = dir.Count;
		int results = 0;
		if (!dir.Contains (8) && above != null) {
			if (finished_up != 4) {
				int result = above.CheckBox (dir, 8);
				if (result > 0) {
					finished_up++;
					results += result;
				}
			} 
		}

		if (!dir.Contains (6) && right != null) {
			if (finished_right != 4) {
				int result = right.CheckBox (dir, 6);
				if (result > 0) {
					finished_right++;
					results+= result;
				}
			}
		}
	
		if (!dir.Contains (2) && below != null) {
			if(finished_below != 4){
				int result = below.CheckBox (dir, 2);
				if (result > 0) {
					finished_below++;
					results+= result;
				}
			}
		}

		if (!dir.Contains (4) && left != null) {
			if(finished_left != 4){
				int result = left.CheckBox (dir, 4);
				if (result > 0) {
					finished_left++;
					results+= result;
				}
			}
		}

		if (results > 0) {
			Debug.Log (results);
			Global_text.singleton.SetText ("Box complete: 100 bonus awarded per box", 0);
			Turn_Manager.singleton.GetCurrentPlayer ().My_Info.RecieveMoney (100 * (results / 2));
		}
		started = false;
	}

	private int CheckBox(List<int> dir, int last){
		if (started) {
			switch (last) {
			case 8:
				finished_below++;
				break;
			case 6:
				finished_left++;
				break;
			case 2:
				finished_up++;
				break;
			case 4:
				finished_right++;
				break;
			}
			return 1;
		} else {
			dir.Add (last);
			int count = dir.Count;
			int results = 0;
			if (!dir.Contains (8) && above != null && last != 2) {
				if (finished_up != 4) {
					int result = above.CheckBox (dir, 8);
					if (result > 0) {
						finished_up++;
						results+= result;
					}
				} 
			}
			if (!dir.Contains (6) && right != null && last != 4) {
				if (finished_right != 4) {
					int result = right.CheckBox (dir, 6);
					if (result > 0) {
						finished_right++;
						results+= result;
					}
				}
			}
			if (!dir.Contains (2) && below != null && last != 8) {
				if(finished_below != 4){
					int result = below.CheckBox (dir, 2);
					if (result > 0) {
						finished_below++;
						results+= result;
					}
				}
			}
			if (!dir.Contains (4) && left != null && last != 6) {
				if(finished_left != 4){
					int result = left.CheckBox (dir, 4);
					if (result > 0) {
						finished_left++;
						results+= result;
					}
				}
			}
			dir.Remove (last);
			return results;
		}
	}

	public Vector3 getCoordinates(){
		return new Vector3 (x_coord, y_coord);
	}

	public bool ValidMove(int x, int y){
		if (x == x_coord) {
			if (y - 1 == y_coord) {
				if (below == null) {
					return true;
				}
			} else if (y + 1 == y_coord) {
				if (above == null) {
					return true;
				}
			} else if (y == y_coord) {
				return true;
			}
		} else if (y == y_coord) {
			if (x + 1 == x_coord) {
				if (left == null) {
					return true;
				}
			} else if (x - 1 == x_coord) {
				if (right == null) {
					return true;
				}
			}
		}
		return false;
	}

	public bool ValidPosition(int x, int y){
		return !(x == x_coord && y == y_coord);
	}
}
