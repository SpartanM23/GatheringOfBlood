using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputSelected : MonoBehaviour {
    public Text exampleSelected;
    public string langage = "FR";
	// Use this for initialization
	void Start () {
        langage = "FR";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeLangage(string selectedLangage)
    {
        exampleSelected.text = selectedLangage;
        langage = selectedLangage;
        if (langage == "Qwerty")
        {
            langage = "EN";
        }
        else if (langage == "Azerty")
        {
            langage = "FR";
        }
    }

}
