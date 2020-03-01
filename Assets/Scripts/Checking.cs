using UnityEngine;
using System.Collections;

public class Checking : MonoBehaviour {

	public GameObject NextCheck;
	public bool P1 = false;
	public bool P2 = false;

	// Update is called once per frame
	void OnTriggerEnter (Collider other) 
	{
		//activate next checkpoint
		NextCheck.SetActive(true);

		//if both players pass close checkpoint
		if(P1 == true && P2 == true)
			gameObject.SetActive(false);

		if (other.tag == "player1")
			P1 = true;
		
		if (other.tag == "player2")
			P2 = true;
	}
}
