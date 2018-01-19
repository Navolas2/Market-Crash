using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;


public class Chance_Factory : MonoBehaviour{

	public static Chance_Factory singleton;

	private List<Chance_Space> All_Actions; //make into list of lists

	void Awake(){
		if (singleton == null) {
			DontDestroyOnLoad (gameObject);
			singleton = this;
			LoadData ();
		} else if (singleton != this) {
			Destroy (gameObject);
		}
	}

	//FORM FOR SPELL XML
	/*
		<SPELL>
			<name></name>
			<speed></speed>
			<hit></hit>
			<cost></cost>
			<damage></damage>
			<focus></focus>
			<effect></effect> (NONE if the spell has no effect)
			<affinity></affinity>
			<type></type>
			<level></level>
		</SPELL>
				*/

	private void LoadData(){
		All_Actions = new List<Chance_Space> ();
		if (File.Exists (Application.dataPath + "/ChanceInformation.xml")) {
			XmlDocument data = new XmlDocument ();
			data.Load (Application.dataPath + "/ChanceInformation.xml");
			foreach (XmlNode attack_data in data.DocumentElement.ChildNodes) {
				bool success = true;

				int move = 0;
				int direction = 0;
				string cash_given = "";
				int forced = 0;
				int warp = 0;
				string display = "";
				int money = 0;


				success = success && int.TryParse (attack_data.SelectSingleNode ("move").InnerText, out move);
				success = success && int.TryParse (attack_data.SelectSingleNode ("direction").InnerText, out direction);
				cash_given = attack_data.SelectSingleNode ("cash_given").InnerText;
				success = success && int.TryParse (attack_data.SelectSingleNode ("forced").InnerText, out forced);
				success = success && int.TryParse (attack_data.SelectSingleNode ("warp").InnerText, out warp);
				success = success && int.TryParse (attack_data.SelectSingleNode ("money").InnerText, out money);
				display = attack_data.SelectSingleNode ("display").InnerText;



				if (success) {
					//MULTIPLE LISTS BASED ON RANK
					Chance_Space c_s = new Chance_Space(display, move, direction, cash_given, money, forced, warp);
					All_Actions.Add (c_s);
				}
				//iterate though list getting each attack action. and information for each attack
			}
		}
		else{
			File.Create (Application.dataPath + "/ChanceInformation.xml");
			print (Application.dataPath);
		}
	}

	public Chance_Space GetChanceSpace(){
		if (All_Actions.Count > 0) {
			Chance_Space c_s = All_Actions [Random.Range(0, All_Actions.Count)];
			//All_Actions.Remove (c_s);
			return c_s;
		}
		return null;
	}
}
