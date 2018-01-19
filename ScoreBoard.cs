using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {

	public static ScoreBoard singleton;

	public List<UnityEngine.UI.Text> player_slots;
	public List<UnityEngine.UI.Text> rankings;

	// Use this for initialization
	void Awake(){
		if (singleton == null) {
			singleton = this;
		} else if (singleton != this) {
			Destroy (this);
		}
	}

	void Start () {
		
	}
	
	public void SetUpBoard(List<string> names){
		int i;
		for (i = 0; i < names.Count; i++) {
			player_slots [i].text = names [i];
			player_slots [i].gameObject.SetActive (true);
			rankings [i].gameObject.SetActive (true);
		}
		for (i = i; i < player_slots.Count; i++) {
			player_slots [i].gameObject.SetActive (false);
			rankings [i].gameObject.SetActive (false);
		}
	}

	public void TitleScreen(){
		Scene_Switcher.singleton.SwitchToTitleScene ();
	}
}
