using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorBlindnessController : MonoBehaviour
{
	//---------------------------------------------------------------------------
	// User-configuration.  You may want to mess with this, to ensure the
	// keyboard shortcuts don't interfere with your game.
	//---------------------------------------------------------------------------
	public bool displayOnStart = true;
	public KeyCode[] toggleModifierKeys = new KeyCode[] { };
	public KeyCode[] toggleMainKeyCodes = new KeyCode[] {
		KeyCode.BackQuote
	};
	public KeyCode[] modeModifierKeys = new KeyCode[] {
		KeyCode.LeftControl,
		KeyCode.LeftShift
	};


	//---------------------------------------------------------------------------
	// Static configuration -- you shouldn't need to mess with this.
	//---------------------------------------------------------------------------
	private static KeyCode[][] modeMainKeyCodes = new KeyCode[][] {
		new KeyCode[] { KeyCode.Alpha1, KeyCode.Keypad1 },
		new KeyCode[] { KeyCode.Alpha2, KeyCode.Keypad2 },
		new KeyCode[] { KeyCode.Alpha3, KeyCode.Keypad3 },
		new KeyCode[] { KeyCode.Alpha4, KeyCode.Keypad4 },
		new KeyCode[] { KeyCode.Alpha5, KeyCode.Keypad5 },
		new KeyCode[] { KeyCode.Alpha6, KeyCode.Keypad6 },
		new KeyCode[] { KeyCode.Alpha7, KeyCode.Keypad7 },
		new KeyCode[] { KeyCode.Alpha8, KeyCode.Keypad8 },
		new KeyCode[] { KeyCode.Alpha9, KeyCode.Keypad9 },
	};
	private const float windowOffsetX = 5f,
		modeLeft = 10f,
		modeWidth = 130f,
		modeHeight = 21f,
		heightOffset = 20f,
		heightIncrement = 23f;



	//---------------------------------------------------------------------------
	// Internal state.  Don't mess with this.
	//---------------------------------------------------------------------------
	private ColorBlindnessEffect effect = null;
	private bool isActive = false;
	private int sWidth = 0;
	private GUIStyle window = null,
		leftButton = null;
	private Rect windowRect = new Rect (windowOffsetX, 5, 150, 235);


	//---------------------------------------------------------------------------
	// Initialization code.
	//---------------------------------------------------------------------------
	public void Awake ()
	{
		// Skip the overhead of GUILayout, as we're managing this all ourself....
		useGUILayout = false;
		if (displayOnStart)
			isActive = true;
	}


	public void Start() {

		print ("Start() in ColorBlindnessController");

		GameObject evtSys = (GameObject)Instantiate(Resources.Load ("ColorBlindnessEventSystem"));

		GameObject canvas = (GameObject)Instantiate(Resources.Load ("ColorBlindnessCanvas"));

		GameObject dropdownObj = null;

		ColorBlindnessDropdownHandler cbdhandler = null;

		Dropdown dropdown = null;

		try {
			dropdownObj = canvas.transform.Find ("ControlPanel").Find ("InnerPanel").Find ("PanelColorBlindness").Find("Dropdown").gameObject;
			cbdhandler = dropdownObj.GetComponent<ColorBlindnessDropdownHandler> ();
			dropdown = dropdownObj.GetComponent<Dropdown>();
		} catch(System.Exception e){
			print ("failed to find control panel bits");
			cbdhandler = null;
			dropdownObj = null;
			dropdown = null;
		}

		if (cbdhandler != null) {
			print ("setting dropdown handler ref to controller");
			cbdhandler.ctrlr = this;
		} else {
			print ("failed to find dropdown handler");
		}

		if (dropdown != null) {

			dropdown.ClearOptions ();


			System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string> ();

			foreach(ColorModification cm in System.Enum.GetValues(typeof(ColorModification))) {
				options.Add (cm.ToString ());
			}

			dropdown.AddOptions (options);
		}

	}

	//---------------------------------------------------------------------------
	// Input handling, and generally our real logic.
	//---------------------------------------------------------------------------
	private bool CheckModifiers (KeyCode[] modifiers)
	{
		foreach (KeyCode code in modifiers) {
			// No sense checking whether something is pressed if the modifier keys
			// aren't being held down...
			if (!Input.GetKey (code))
				return false;
		}
		return true;
	}


	public void setEffect(int mode) {

		if (effect != null && mode >= 0 && mode < 9) {

			effect.mode =  (ColorModification)mode;

		}
	}


	private void CheckForAndApplyModeSwitch ()
	{

		if (!CheckModifiers (modeModifierKeys))
			return;

		print ("CheckForAndApplyModeSwitch");

		int pressedMainKeyIdx = -1;
		for (int i = 0; i < modeMainKeyCodes.GetLength (0) && pressedMainKeyIdx == -1; i++) {
			KeyCode[] keyCodes = modeMainKeyCodes [i];
			for (int j = 0; j < keyCodes.Length && pressedMainKeyIdx == -1; j++) {
				if (Input.GetKeyDown (keyCodes [j]))
					pressedMainKeyIdx = i;
			}
		}

		if (pressedMainKeyIdx > -1) {
			print ("pressed: " + pressedMainKeyIdx);
			effect.mode = (ColorModification)pressedMainKeyIdx;
		}
	}

	public void CheckForAndApplyStateSwitch ()
	{
		if (!CheckModifiers (toggleModifierKeys))
			return;

		bool pressedToggleKey = false;
		foreach (KeyCode code in toggleMainKeyCodes) {
			if (Input.GetKeyDown (code)) {
				pressedToggleKey = true;
				break;
			}
		}

		if (pressedToggleKey)
			isActive = !isActive;
	}

	public void Update ()
	{

		if(effect == null) {
			effect = Camera.main.GetComponent<ColorBlindnessEffect>();
		
			if (effect == null) {
				enabled = false;
			}
		
		}

		//if(isActive)
		CheckForAndApplyModeSwitch ();

		//CheckForAndApplyStateSwitch();
	}



}
