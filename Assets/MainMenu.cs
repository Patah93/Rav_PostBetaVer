using UnityEngine;
using System.Collections;
using DataPlatform;

public class MainMenu : MonoBehaviour , IMenu
{
	public static void DisplayText(string text)
	{
		((DisplayWindow)GameObject.Find("DisplayWindow").GetComponent("DisplayWindow")).SetFromText(text);
	}

	void Start () 
	{
		DataPlatformPlugin.InitializePlugin(0);
		OnScreenLog.Add("Data Platform Example Starting Up\n");

		// Display welcome information.
		MainMenu.DisplayText("Data Platform Example\n--------------------------------\n\nThis demo attempts to demonstrate some of the functionality of the Native Data Platform plugin.\nThis example only works with the example manifest.\n\n");		
	}

	void Update () 	{}
	void OnGUI()    {}

	public void HandleMenu(MenuLayout layout, Menu self)
	{
		layout.Update(1);
		if (layout.AddButtonWithIndex("Send Button Event", DataPlatformPlugin.AmFullyInitialized()))
		{
			OnScreenLog.Add("> Sending Button Event\n");
			if (DataPlatformPlugin.SendEventButtonPress("A"))
			{
				OnScreenLog.Add("... Ok\n");
			}
			else
			{
				OnScreenLog.Add("... Problem\n");
			}
		}
	}
}
