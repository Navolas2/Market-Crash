using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

	private board_space current_location;
	private float move_delay;
	private int move_amount;
	private int amount_moved;
	private int move_saved;
	private int last_space;
	private List<int> move_record;
	private bool playing;

	public UnityEngine.UI.Text move_counter;

	[HideInInspector]
	public Player_Information My_Info;


	void Awake(){
		My_Info = GetComponent<Player_Information> ();
	}
	// Use this for initialization
	void Start () {
		
		current_location = GameObject.FindGameObjectWithTag("Bank").GetComponent<Bank_Space>();
		this.transform.position = current_location.player_position.transform.position;
		move_delay = -1;
		amount_moved = 0;
		move_amount = 0;
		move_record = new List<int> ();
		last_space = 0;
		move_counter.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (playing) {
			if (Input.GetButtonDown ("CameraSwap")) {
				SwitchCameraMode ();
			}
			if (move_delay == 0 && amount_moved <= move_amount) {
				float horizontal = Input.GetAxis ("Horizontal");
				if (horizontal > .5) {
					horizontal = 1f;
				} else if (horizontal < -.5) {
					horizontal = -1f;
				} else {
					horizontal = 0f;
				}

				float vertical = Input.GetAxis ("Vertical");
				if (vertical > .5) {
					vertical = 1f;
				} else if (vertical < -.5) {
					vertical = -1f;
				} else {
					vertical = 0f;
				}
			
				int direction = 0;
				board_space next_space;
				if (horizontal == -1 && vertical == -1) {
					direction = 1;
				} else if (horizontal == 1 && vertical == -1) {
					direction = 3;
				} else if (horizontal == -1 && vertical == 1) {
					direction = 7;
				} else if (horizontal == 1 && vertical == 1) {
					direction = 9;
				} else if (horizontal == 0 && vertical == 1) {
					direction = 8;
				} else if (horizontal == 0 && vertical == -1) {
					direction = 2;
				} else if (horizontal == 1 && vertical == 0) {
					direction = 6;
				} else if (horizontal == -1 && vertical == 0) {
					direction = 4;
				}
				int last = 0;
				next_space = current_location.move_space (direction, out last);
				if (next_space != current_location && next_space != null) {
					if ((amount_moved == 0 && last_space != direction) || amount_moved > 0) {
						int dir = 0;
						bool valid_move = true;
						if (amount_moved == 0) {
							amount_moved++;
							dir = 1;
							move_record.Add (last);
						} 
						else if (move_record [move_record.Count - 1] != direction) {
							amount_moved++;
							dir = 1;
							if (amount_moved <= move_amount) {
								move_record.Add (last);
							} else {
								amount_moved--;
								dir = -1;
								valid_move = false;
							}
						} 
						else {
							if (amount_moved == move_amount) {
								Menu_Manager.singleton.Turn_Menu_Off (Menu_Manager.Stop_Move);
							}
							amount_moved--;
							dir = -1;
							move_record.RemoveAt (move_record.Count - 1);
						}
						if (valid_move) {
							this.transform.position = next_space.player_position.transform.position;
							if (direction == -1) {
								current_location.Activate_BackStep_Effect (My_Info);
							}
							current_location = next_space;
							current_location.Activate_Pass_Effect (My_Info, dir);
							Property_Display.singleton.SetInformation (current_location);
							if (move_delay == 0) {
								move_delay = 30;
							}
						}
						if (amount_moved == move_amount) {
							if (move_delay != -1) {
								Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Stop_Move);
							}
						}
						move_counter.text = (move_amount - amount_moved) + "";
					}
				}
			} else if (move_delay > 0) {
				move_delay--;
			}
		}
	}

	public void SetMoveAmount(int value){
		move_amount = value;
		amount_moved = 0;
		move_delay = 0;
		move_counter.gameObject.SetActive (true);
		move_counter.text = move_amount + "";
	}

	public void StopMove(){
		move_delay = -1;
		move_saved = move_amount;
		move_amount = 0;
		last_space = move_record [move_record.Count - 1];
		move_record.Clear ();
		move_counter.gameObject.SetActive (false);
		Turn_Manager.singleton.AdvanceTurnStatus ();
		current_location.Activate_Space_Effect (My_Info);
	}

	public void StepBack(){
		int last = 0;
		board_space next_space = current_location.move_space (move_record[move_record.Count -1], out last);
		if (next_space != current_location && next_space != null) {
			this.transform.position = next_space.player_position.transform.position;
			current_location = next_space;
			move_delay = 30;
			
			amount_moved--;
			move_record.RemoveAt (move_record.Count - 1);
		}
	}

	public void PauseMovement(){
		move_delay = -1;
	}

	public void UnPause(){
		move_delay = 0;
		if (amount_moved == move_amount) {
			Menu_Manager.singleton.Turn_Menu_On (Menu_Manager.Stop_Move);
		}
	}

	public void SwitchCameraMode(){
		Turn_Manager.singleton.main_camera.SwitchFreeCamera ();
		if (Turn_Manager.singleton.main_camera.GetCameraMode ()) {
			move_delay = -1;
			Menu_Manager.singleton.Turn_Menu_Off ();
		} else {
			if(move_amount != 0)
				move_delay = 30;
			Menu_Manager.singleton.Turn_Menu_On ();
		}
	}

	public void ReturnCamera(){
		Turn_Manager.singleton.main_camera.StaticCamera ();
		if(move_amount != 0)
			move_delay = 30;
		Menu_Manager.singleton.Turn_Menu_On ();
	}

	public void FreeCamera(){
		Turn_Manager.singleton.main_camera.FreeCameraMovement ();
		move_delay = -1;
		Menu_Manager.singleton.Turn_Menu_Off ();
	}

	public void AnyDirection(){
		last_space = 0;
	}

	public void RepeatMove(){
		SetMoveAmount (move_saved);
	}

	public bool set_playing{
		set{ playing = value; }
	}

	public board_space position {
		get{return current_location;}
	}

	public void PassedBank(){

	}
}
