using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instructions : MonoBehaviour {

	public void TitleScreen(){
		Debug.Log ("click");
		Scene_Switcher.singleton.SwitchToTitleScene ();
	}
}
