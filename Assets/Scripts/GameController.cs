using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	//Variables
	public Text CountDown;
	static public bool GameRunning = false;
	static public bool counting = false;

	static public float count = 3; 	//from where to count down
	static public bool Timer = false;

	public GameObject textUI;
	public GameObject LapCounter;
	public GameObject Menu;

	void Awake()
	{
		CountDown.text = "Press 'Enter' to play";
		Menu.SetActive (false);
	}
	

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			GameRunning = false;
			Menu.SetActive(true);
		}
		if (counting == true) 
		{
            count =- Time.deltaTime;
            
            CountDown.text = count.ToString();
        }
		if (count <= 0) 
		{
			counting = false;
			count = 0;
			GameRunning = true;
			CountDown.text = "GO";
		}
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			counting = true;
		}
		if (count <= 0) 
		{
			textUI.SetActive(false);
			LapCounter.SetActive(true);
			Timer = true;
		}
		//if both timers are done end game
		if (Controls1.time <= 0 && Controls2.time <= 0 && GameRunning == true) 
		{
			GameRunning = false;
			textUI.SetActive(true);
			Timer = false;
			CountDown.text = "Game Over";
			counting = false;
			if (GameRunning == false) 
			{
				Menu.SetActive(true);
			}
		}
		//if both players finish end game
		if (Controls1.Lap == 3 && Controls2.Lap == 3 && GameRunning == true)
		{
			GameRunning = false;
			textUI.SetActive(true);
			CountDown.text = "Finish";
			counting = false;
			if (GameRunning == false) 
			{
				Menu.SetActive(true);
			}
		}
	}
	//button menu functions
	public void Continue()
	{
		Menu.SetActive (false);
		GameRunning = true;
	}
	public void Quit()
	{
		Application.Quit();
	}
	public void Restart()
	{
		Application.LoadLevel (0);
		Menu.SetActive (false);
	}
}
