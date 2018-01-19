using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Switcher : MonoBehaviour {

	public static Scene_Switcher singleton;

	private List<string> player_names;
	private List<Color> player_colors;
	private int starting_money;
	private int money_goal;

	void Awake(){
		if (singleton == null) {
			singleton = this;
			DontDestroyOnLoad (this);
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneChanged;
		} else if (singleton != this) {
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void SceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1){
		if (arg1.name == "base scene") {
			Turn_Manager.singleton.CreatePlayers (player_names, player_colors);
		}
		if (arg1.name == "Test scne") {

		}
		if (arg1.name == "endgame") {
			ScoreBoard.singleton.SetUpBoard (player_names);
		}
	}

	public void SetPlayerInformation(List<Player_Information_Input> info){
		player_names = new List<string> ();
		player_colors = new List<Color> ();
		foreach (Player_Information_Input info_input in info) {
			player_names.Add (info_input._name);
			player_colors.Add (info_input._color);
		}
	}

	public int _money_start{
		get{ return starting_money; }
		set{ starting_money = value; }
	}

	public int _goal{
		get { return money_goal; }
		set { money_goal = value; } 
	}

	public void SetRankInformation(List<string> names){
		player_names = names;
	}

	public void SwitchToGameScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("base scene");
	}

	public void SwitchToTitleScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("title scene");
	}

	public void SwitchToEndScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("endgame");
	}

	public void SwitchInstructionScreen (){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Instructions");
	}

	public void SwitchCreditScreen (){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Credits");
	}
}
