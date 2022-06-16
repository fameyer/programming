using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;


[System.Serializable]
// Stage with highscores
public struct stage{
	public int level;
	public int score; 
	public double time;
	public string name;
};

public static class SaveLoad {

	public static List<stage> save = new List<stage>();

	// path to file
	public static string file_path = Application.persistentDataPath + "/savedGames.gd";

	// save list to file
	public static void Save(int level, int score, double time, string name) {
		if (File.Exists (file_path)) {
			// Load given list first
			Load ();

			stage recent;
			recent.level = level;
			recent.score = score;
			recent.time = time;
			recent.name = name;

			// Look for existing entries, two for each level per name for best time and best score
			// first search for name and level, then compare both entries
			bool level_given = false;
			foreach (stage entry in save) {
				if(entry.level == level){
					if(entry.score < score){
						if(entry.time > time){
							save.Remove(entry);
						}
						save.Add(recent);
						level_given = true;
						break;
					}
					if(entry.time > time){
						if(entry.score < score){
							save.Remove(entry);
						}
						save.Add(recent);
						level_given = true;
						break;
					}
				}
			}
			if(!level_given){
				save.Add (recent);
			}

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (file_path);
			bf.Serialize (file, SaveLoad.save);
			file.Close ();
		} else {
			stage recent;
			recent.level = level;
			recent.score = score;
			recent.time = time;
			recent.name = name;
			save.Add (recent);		
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (file_path);
			bf.Serialize (file, SaveLoad.save);
			file.Close ();
		}
	}

	// load file
	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(file_path, FileMode.Open);
			SaveLoad.save = (List<stage>)bf.Deserialize(file);
			file.Close();
		}
	}
}