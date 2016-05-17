using UnityEngine;
using System.Collections;

public class ColorBlindnessDropdownHandler : MonoBehaviour {

	public ColorBlindnessController ctrlr;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OnValueChanged(int index) {

		print ("Picked: " + index);

		if (ctrlr != null) {
			print ("setting ctrlr");
			ctrlr.setEffect (index);
		}
	}
}
