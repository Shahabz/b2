using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour {

    class SaveData {
        public int level;
        public int checkPoint;
        public int anxietyModifiers;
        public Dictionary<string, bool> triggers;

        public void LoadData() {
            string fileName = "NPSave.txt";
            if(File.Exists(fileName)){
                var sr = File.OpenText(fileName);
                level = sr.ReadLine();
                checkPoint = sr.ReadLine();
                anxietyModifiers = sr.ReadLine();

                string line = sr.ReadLine(); //Header string. Skip
                line = sr.ReadLine();
                while(!line.Contains("/Triggers")){
                    triggers.Add(line.Split(new string[] { " " }, System.StringSplitOptions.None));
                    line = sr.ReadLine();
                }  
            } else {
                Debug.Log("Could not Open the file: " + fileName + " for reading.");
                return;
            }
        }
    }

//    class Checkpoint {
//
//    }

    SaveData saveData;

//    public void SetTrigger(string identifier, bool value) {
//        triggers[identifier] = value;
//    }

    public bool HasTriggered( string identifier) {
        bool triggered = false;
        saveData.triggers.TryGetValue(identifier, out triggered);
        return triggered;
    }

    public void Save() {
        saveData.level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        saveData.checkPoint = 0; //TODO
        saveData.anxietyModifiers = 0; //TODO
         
        //File IO
        string fileName = "NPSave.txt";
        if (File.Exists(fileName))
        {
            File.Replace(fileName, fileName + "_Backup", fileName + "_Backup");
        }
        StreamWriter sw = File.CreateText(fileName);
        sw.WriteLine(saveData.level + "\n");
        sw.WriteLine(saveData.checkPoint + "\n");
        sw.WriteLine(saveData.anxietyModifiers + "\n");
        sw.WriteLine("Triggers\n");
        for (var enumerator = saveData.triggers.GetEnumerator(); enumerator.Current != null; enumerator.MoveNext())
        {
            if (enumerator.Current.Key == "")
            {
                Debug.LogError("Can't have empty trigger key");
                return;
            }
            sw.WriteLine(enumerator.Current.Key + " " + enumerator.Current.Value);
        }
        sw.WriteLine("/Triggers\n");

        sw.Close();
    }
}
