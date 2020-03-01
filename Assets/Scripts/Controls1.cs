using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controls1 : MonoBehaviour {

	//Variables
	public Text LapText;
	public int Laps = 3;
	static public int Lap = 0;

	public Text Timers;
	public GameObject TimerS;
	static public float time = 60;

	public float Speed;
	public double Speed_O_Meter;

	public float ACC;
	public int BrakePower = 0;
	public float[] Brakes;
	
	public bool Braking = false;
	public bool GearBox = false; //if true manual shift gearbox (BETA)
	public int Gear = 0;

	private float Top_Speed;
	private float Steer = 0;
	private float GroundDistance;
	private float Rotation = 3.25f;
	private int Steer_Max = 45;
	private bool IsGrounded;
	private int i = 0;

	public Rigidbody CarMass;
	public Transform RF;
	public Transform LF;

	public Transform Camera1;
	public Transform[] Targets;
	public Transform[] ViewSpots;
	static public int View = 0;
	public float speed = 5;
	
	
	void LateUpdate () 
	{
		//Camera Changer
		if (GameController.GameRunning == true) {
			//changing view
			if (Input.GetKeyDown (KeyCode.C)) {
				View ++;
			}
			
			
			float step = speed * Time.deltaTime;
			//Moving above the car
			if (View == 0) {
				Camera1.transform.position = new Vector3 (Targets [0].position.x, 22, Targets [0].position.z);
				Camera1.transform.LookAt (Targets [0]);
			}
			//3rd person
			if (View == 1) {
				Camera1.transform.position = Vector3.MoveTowards (transform.position, ViewSpots [1].position, step);
				Camera1.transform.LookAt (Targets [1]);
			}
			//Hood view
			if (View == 2) {
				Camera1.transform.position = Vector3.MoveTowards (transform.position, ViewSpots [2].position, step);
				Camera1.transform.LookAt (Targets [2]);
			}
			if (View >= 3) {
				View = 0;
			}
		}
	}
	

	void Update ()
	{
		//lapcounter
		LapText.text = "Lap"+Lap+"/"+Laps;

		if (GameController.Timer == true && GameController.GameRunning == true) 
		{
			time = time - 1 * Time.deltaTime;
            time = Mathf.RoundToInt(time);
            Timers.text = time.ToString();
		}

		//movement
		if (GameController.GameRunning == true) {
			Steer = Input.GetAxis ("HorizontalP1");
			transform.Translate (Vector3.forward * Speed * Time.deltaTime);
			Speed_O_Meter = CarMass.GetComponent<Rigidbody> ().velocity.magnitude * 3.6;

			if (Input.GetKeyDown (KeyCode.Q) && Gear <= 5)
				Gear ++;
			if (Input.GetKeyDown (KeyCode.E) && Gear >= 1)
				Gear --;


			if (GearBox == true) {
				if (Gear == 0) {
					Top_Speed = 0;
				}
				if (Gear == 1) {
					Top_Speed = 10;
				}
				if (Gear == 2) {
					Top_Speed = 20;
				}
				if (Gear == 3) {
					Top_Speed = 30;
				}
				if (Gear == 4) {
					Top_Speed = 40;
				}
				if (Gear == 5) {
					Top_Speed = 50;
				}
				if (Gear == 6) {
					Top_Speed = 60;
				}
			} else {
				Top_Speed = 60;
			}

			if (Lap == 3 && GameController.GameRunning == true)
				TimerS.SetActive(false);

			if (time <= 0 && GameController.GameRunning == true) 
				TimerS.SetActive(false);


			//Accelerating & Stopping
			if (Speed <= Top_Speed && IsGrounded == true && Braking == false) {
				Speed = Speed + ACC * Time.deltaTime;
			}
			if (IsGrounded == false && Speed >= 0.1f) {
				Speed = Speed - Top_Speed * Time.deltaTime;
			}
			if (Braking == true && Speed >= 0.1f) {
				Speed = Speed - Brakes [BrakePower] * Time.deltaTime;
			}

			//Steering
			if (Input.GetKey (KeyCode.A)) {
				transform.Rotate (0, -Rotation, 0);
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.Rotate (0, Rotation, 0);
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				transform.position = new Vector3 (transform.position.x, transform.position.y + 2, transform.position.z);
				transform.rotation = Quaternion.identity;
			}

			//Breaking
			if (Input.GetKeyDown (KeyCode.S)) {
				Braking = true;
			}
			if (Input.GetKeyUp (KeyCode.S)) {
				Braking = false;
			}

			if (Speed >= 0) {
				BrakePower = 0;
			}
			if (Speed >= 10) {
				BrakePower = 1;
			}
			if (Speed >= 20) {
				BrakePower = 2;
			}
			if (Speed >= 30) {
				BrakePower = 3;
			}
			if (Speed >= 40) {
				BrakePower = 4;
			}
			if (Speed >= 50) {
				BrakePower = 5;
			}
			if (Speed >= 60) {
				BrakePower = 6;
			}

			//Nitro
			if (Input.GetKey (KeyCode.LeftShift)) {
				ACC = ACC + 5;
				Top_Speed = 70;
			}
			if (Input.GetKeyUp (KeyCode.LeftShift)) {
				Top_Speed = 60;
				ACC = 2;
			}

			
			//Turning CarWheels Right/left
			RF.transform.localEulerAngles = new Vector3 (0, Steer_Max * Steer, 90);
			LF.transform.localEulerAngles = new Vector3 (0, Steer_Max * Steer, 90);

		} 
		else 
		{
			Speed = 0;
			Top_Speed = 0;
		}
	}
	
	void OnCollisionStay (Collision collisionInfo)
	{
		IsGrounded = true;
	}
	void OnCollisionExit (Collision collisionInfo)
	{
		IsGrounded = false;
	}
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "TunnelIN" && View == 0) 
		{
			View = 2;
			//print("IN");
		}
		if (other.tag == "TunnelOUT" && View == 2) 
		{
			View = 0;
			//print("OUT");
		}
		if (other.tag == "Finish" && Lap < 3 && i == 0 ) 
		{
			Lap ++;
			i = 1;
		}
		if (other.tag == "checkPoint") 
		{
			time += 5;
			i = 0;
		}
	}
}