using UnityEngine;
using System.Collections;

public class ADVControlls : MonoBehaviour 
{
	//Mass usage
	public Rigidbody CarMass;
	public Vector3 CenterCar;
	public float AntiRoll = 5000f;

	//WheelColliders
	public WheelCollider RFWheel;
	public WheelCollider LFWheel;
	public WheelCollider RBWheel;
	public WheelCollider LBWheel;
	
	//WheelTransforms
	public Transform TRFWheel;
	public Transform TLFWheel;
	public Transform TRBWheel;
	public Transform TLBWheel;
	
	//Steering max and speed
	public int Steer_Max = 45;
	public int Motor_Max = 40;
	public int Brake_Max = 100;
	public int Steer_Speed = 20;
	
	//Private Variables
	private float Steer = 0;
	public float Forward = 0;
	public float Back = 0;
	public bool BrakeRelease = false;
	private float Motor = 0;
	private float Brake = 0;
	public bool Reverse = false;
	public float Speed = 0;

	//Variables
	public double Speed_O_Meter;
	
	void Start()
	{
		//Needed for Later
		CarMass = GetComponent<Rigidbody> ();
		CarMass.centerOfMass = CenterCar;
	}
	
	//this update runs on the physics engine
	void FixedUpdate ()
	{
		if (GameController.GameRunning == true) 
		{	
			Speed_O_Meter = CarMass.GetComponent<Rigidbody>().velocity.magnitude * 3.6; 
			Speed = CarMass.velocity.sqrMagnitude;
			Steer = Input.GetAxis ("Horizontal");
			Forward = Mathf.Clamp (Input.GetAxis ("Vertical"), 0, 1);
			Back = -1 * Mathf.Clamp (Input.GetAxis ("Vertical"), -1, 0);

			if(Speed == 0 && Forward == 0 && Back == 0)
			{
				BrakeRelease = true;
			}

			//if brake isnt used then define if going reverse or forward
			if(Speed == 0 && BrakeRelease == true)
			{
				if(Back > 0)
				{
					Reverse = true;
				}
				if(Forward > 0)
				{
					Reverse = false;
				}
			}


			//if moving backwards
			if (Reverse == true) 
			{
				Motor = -1 * Back;
				Brake = Forward;
			}
			else
			{ 
				//go forward
				Motor = Forward;
				Brake = Back;
			}

			if(Brake > 0)
			{
				BrakeRelease = false;
			}

			if(Input.GetKeyDown(KeyCode.R))
			{
				transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
				transform.rotation = Quaternion.identity;
			}

			//Giving Torque to move Forward or Brake
			RBWheel.motorTorque = Motor_Max * Motor;
			LBWheel.motorTorque = Motor_Max * Motor;
			RBWheel.brakeTorque = Brake_Max * Brake;
			LBWheel.brakeTorque = Brake_Max * Brake;

			//Steering the wheel how much at a time and max to reach
			RFWheel.steerAngle = Steer_Max * Steer;
			LFWheel.steerAngle = Steer_Max * Steer;
			TRFWheel.transform.localEulerAngles = new Vector3 (0, Steer_Max * Steer, 90);
			TLFWheel.transform.localEulerAngles = new Vector3 (0, Steer_Max * Steer, 90);

			//Letting wheel rotate forward
			TRFWheel.Rotate (0, RFWheel.rpm * -6 * Time.deltaTime, 0);
			TLFWheel.Rotate (0, LFWheel.rpm * -6 * Time.deltaTime, 0);
			TRBWheel.Rotate (0, RBWheel.rpm * -6 * Time.deltaTime, 0);
			TLBWheel.Rotate (0, LBWheel.rpm * -6 * Time.deltaTime, 0);

			//Anti Rolling 

			WheelHit hit;
			float travelL=1.0f;
			float travelR=1.0f;

			bool groundedL = LFWheel.GetGroundHit(out hit);
			if (groundedL)
			{
				travelL = (-LFWheel.transform.InverseTransformPoint(hit.point).y - LFWheel.radius) / LFWheel.suspensionDistance;
			}
			bool groundedR = RFWheel.GetGroundHit(out hit);

			if (groundedR)
			{
				travelR = (-RFWheel.transform.InverseTransformPoint(hit.point).y - RFWheel.radius) / RFWheel.suspensionDistance;
			}
			float antiRollForce = (travelL - travelR) * AntiRoll;
			if (groundedL)
			{
				CarMass.AddForceAtPosition(LFWheel.transform.up * -antiRollForce, LFWheel.transform.position);
			}
			
			if (groundedR)
			{
				CarMass.AddForceAtPosition(RFWheel.transform.up * antiRollForce, RFWheel.transform.position);  
			}
		}
	}
}
