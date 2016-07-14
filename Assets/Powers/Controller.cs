using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	//Game Objects
	public Camera cam;
	private CharacterController cc;
	public GameObject weapon;

	//movement speeds & Variables
	public float speed = 6.0f;
	public float walkSpeed = 6.0F;
	public float runSpeed = 10.0f;
	public float crouchSpeed = 3.0f;
	public float proneSpeed = 2.0f;
	public float rotateSpeed = 3.0F;
	public float jumpPower = 8.0F;
	public float gravity = 20.0f;
	private float forwardSpeed, strafeSpeed;
	private Vector3 speedVector;
	private float cameraPitch;
	private float rotationX;


	//Sprint Variables
	public float stamina = 0.0f;
	public bool isRunning = false;
	public bool canRun = true;
	private float maxStamina = 5.0f;
	private float staminaRechargeDelay = 1.0f;
	private float staminaPenaltyTime = 2.0f;
	private float nextCanRunTime, nextStaminaRechargeTime;

	//Jump Variables
	public bool isJumping = false;
	public bool canJump = true;
	private Vector3 jumpVector;

	//Crouch Variables
	public bool isCrouched = false;
	public bool goingCrouch = false, crouchToRun = false, leavingCrouch = false;
	private float crouchHeight;
	private float crouchScale;

	//Prone Variables
	public bool goingProne = false, proneToCrouch = false, leavingProneFromProne = false, goingProneFromCrouch = false;
	public bool isProne = false;
	private float proneHeight;
	private float proneScale;

	//misc Variables
	private float ccOriginalHeight;
	public bool isStanding;
	private Vector3 lastPosition = Vector3.zero;
	public bool psyHolding = false, psyCharging = false;

	//UI Objects
	public Slider staminaBar;

	void Awake(){
//		cc = GetComponent <CharacterController> (); //Grab the attached Character Controller
//		crouchScale = cc.height * 0.25f; //Used for calculating Crouching Height for the Controller
//		proneScale = cc.height * 0.5f; //Used for calculating Prone Height for Controller
//		ccOriginalHeight = cc.height; //Used for Prone and Crouch logic
//		proneHeight = ccOriginalHeight - proneScale;
//		crouchHeight = ccOriginalHeight - crouchScale;
	}

	void Update() {
		//Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
		//Define what a Standing Height is for our character
//		if (cc.height >= ccOriginalHeight)
//			isStanding = true;
//		else
//			isStanding = false;

		//Execute Functionality
//		Move ();
//		MouseLook ();
//		Sprint ();
//		Jump ();
//		Crouch ();
//		Prone ();

//		BroadcastMessage("GetStamina", stamina);

		staminaBar.minValue = 0;
		staminaBar.maxValue = maxStamina;
		staminaBar.value = stamina;
	}

//	void Move(){
//
//		//This method determines character movement
//
//		forwardSpeed = Input.GetAxis ("Vertical") * speed;
//		strafeSpeed = Input.GetAxis ("Horizontal") * speed;
//		speedVector = new Vector3 (strafeSpeed, 0, forwardSpeed);
//		speedVector = transform.rotation * speedVector;
//
//		cc.SimpleMove(speedVector);
//	}
//
//	void MouseLook(){
//
//		//This method determines Mouse Look behavior
//
//		rotationX = rotateSpeed * Input.GetAxis ("Mouse X");
//		cameraPitch -= Input.GetAxis("Mouse Y") * rotateSpeed; // Can use rotateSpeed to up sensitivity
//		cameraPitch = Mathf.Clamp (cameraPitch, -90f, 90f); //This keeps camera from going beyond 180 degrees of rotation up and down
//
//		transform.Rotate (0, rotationX, 0); 
//		cam.transform.localRotation = Quaternion.Euler (cameraPitch, 0, 0);
//	}
//
//	void Jump(){
//
//		//This method defines jump behavior
//
//		//Check to see if character is on ground and is not prone. If conditions are met, character jumps.
//		if (Input.GetButtonDown("Jump") && cc.isGrounded && !isProne) {
//			jumpVector.y = jumpPower;
//			isJumping = true;
//		}
//
//		//set gravity so that our controller will fall back down appropriately. Necessary without a Rigidbody.
//		jumpVector.y -= gravity * Time.deltaTime;
//		cc.Move (jumpVector * Time.deltaTime);
//
//		//set isJumping to false when we are on the ground. Used for crouch and prone logic
//		if (cc.isGrounded == true)
//			isJumping = false;
//	}
//
//	void Sprint(){
//
//		//Sprint behavior
//
//		//check to make sure we aren't prone. We can't run if we are prone.
//		if (!isProne)
//			canRun = true;
//		else
//			canRun = false;
//
//		//find out if left shift is pressed and if game time is greater than the Stamina Cooldown Period
//		if (Input.GetKey (KeyCode.LeftShift) && Time.time > nextCanRunTime && canRun) {
//
//			//Shift is pressed and player can run. Now find out if player is actually moving
//			if (lastPosition.x != transform.position.x || lastPosition.z != transform.position.z) {
//
//				//Player is Moving while holding shift. Now Deduct Stamina 1 per second
//				stamina -= Time.deltaTime * 1;
//
//				//set a time delay for when stamina can start to regen. This is used later.
//				nextStaminaRechargeTime = Time.time + staminaRechargeDelay;
//			}
//
//			//shift is pressed so check to see if we have stamina
//			if (stamina > 0) {
//
//				//we have stamina so increase speed to 10
//				speed = runSpeed;
//				isRunning = true;
//			} 
//
//			//find out if we are out of stamina
//			else {
//
//				//we are out of stamina so punish player with a cooldown. Next Time we can run is game time + penalty time.
//				nextCanRunTime = Time.time + staminaPenaltyTime;
//
//				//set speed back to 6
//				speed = walkSpeed;
//				isRunning = false;
//			}
//		} 
//
//		//Find out if game time has passed the stamina recharge time.
//
//		else if (Time.time > nextStaminaRechargeTime && !psyHolding && !psyCharging) {
//
//			//start to regen stamina. Mathf.Min grabs the lowest value between stamina + 1 per second and Maximum stamina, which is set to 5.
//			stamina = Mathf.Min (stamina + Time.deltaTime * 1, maxStamina);
//		} 
//
//		//lastly, create logic for if our character is not pressing shift
//		else if (!Input.GetKey (KeyCode.LeftShift)) {
//
//			//set speed back to default.
//			speed = walkSpeed;
//			isRunning = false;
//		}
//
//		/*assign the current position of our object to the lastPosition Vector so we can use it in the above
//		 * logic for determining if player is moving */
//		lastPosition = transform.position;
//	}
//
//	void Crouch(){
//
//		//Crouch behavior
//
//		//Whether or not the game knows if we are crouched is determined by the height of the character controller
//
//		if (cc.height <= crouchHeight && cc.height < ccOriginalHeight && !isProne)
//			isCrouched = true;
//		else
//			isCrouched = false;
//
//		//The following if statements are used to determine whether or not we are crouching or standing up 
//		//based on a number of factors
//
//		if (Input.GetKeyDown (KeyCode.LeftControl) && isCrouched == false && !isProne)
//			goingCrouch = true;
//
//		else if (Input.GetKeyDown (KeyCode.LeftControl) && isCrouched == true)
//			leavingCrouch = true;
//	
//		else if (isCrouched == true && isRunning == true)
//			crouchToRun = true;
//
//		else if (Input.GetKeyDown (KeyCode.LeftControl) && isProne)
//			proneToCrouch = true;
//
//		else if (isCrouched == true && isJumping == true) {
//			//If we jump from crouch, set speed and height to normal immediately
//			speed = walkSpeed;
//			cc.height = ccOriginalHeight;
//		} 
//
//		//The following if statements are used to create transitions between states. Each block modifies 
//		//character speed and height over time until they are in their proper state (crouching, prone, standing, etc.)
//
//		if (goingCrouch) {
//			speed -= crouchSpeed * (Time.deltaTime * 1);
//			cc.height -= crouchHeight * (Time.deltaTime * 1);
//
//			if (isCrouched) {
//				goingCrouch = false;
//				cc.height = crouchHeight;
//				speed = crouchSpeed;
//			}
//		} else if (leavingCrouch) {
//			speed += walkSpeed * (Time.deltaTime * 0.5f);
//			cc.height += ccOriginalHeight * (Time.deltaTime * 0.5f);
//
//			if (isStanding) {
//				leavingCrouch = false;
//				cc.height = ccOriginalHeight;
//				speed = walkSpeed;
//			}
//		} else if (crouchToRun) {
//			speed += runSpeed * (Time.deltaTime * 0.75f);
//			cc.height += ccOriginalHeight * (Time.deltaTime * 0.75f);
//
//			if(isStanding){
//				crouchToRun = false;
//				cc.height = ccOriginalHeight;
//				speed = runSpeed;
//			}
//		}
//	}
//
//	void Prone(){
//
//		//Prone Behavior
//
//		//Whether we are prone or not is determined by character controller height.
//		if (cc.height <= proneHeight)
//			isProne = true;
//		else
//			isProne = false;
//
//		//The following logic is used to create transitions between states. Character Speed & Height are Changed
//		//over time until they are in their proper state.
//
//		if (Input.GetKeyDown (KeyCode.Z) && isProne == false)
//			goingProne = true;
//
//		else if (Input.GetKeyDown (KeyCode.Z) && isProne == true)
//			leavingProneFromProne = true;
//
//		else if (Input.GetKeyDown (KeyCode.Z) && isCrouched) 
//			goingProneFromCrouch = true;
//		
//		//The Following logic creates the transitions to and from prone into other states.
//
//		if (goingProne || goingProneFromCrouch) {
//			speed -= proneSpeed * (Time.deltaTime * 1);
//			cc.height -= proneHeight * (Time.deltaTime * 0.75f);
//
//			if (isProne) {
//				goingProne = false;
//				cc.height = proneHeight;
//				speed = proneSpeed;
//			}
//		} else if (leavingProneFromProne) {
//			speed += walkSpeed * (Time.deltaTime * 0.5f);
//			cc.height += ccOriginalHeight * (Time.deltaTime * 0.5f);
//
//			if (isStanding) {
//				leavingProneFromProne = false;
//				cc.height = ccOriginalHeight;
//				speed = walkSpeed;
//			}
//		}else if (proneToCrouch) {
//			speed += crouchSpeed * (Time.deltaTime * 0.25f);
//			cc.height += crouchHeight * (Time.deltaTime * 0.25f);
//			
//			if (isCrouched && speed >= crouchSpeed) {
//				proneToCrouch = false;
//				cc.height = crouchHeight;
//				speed = crouchSpeed;
//			} 
//		}
//
//	}

	void PsyStamina(float psyStamina){

		//if(!isHolding)
			stamina -= psyStamina;
		nextStaminaRechargeTime = Time.time + staminaRechargeDelay;
	}

	void PsyHolding(bool isHolding){
		psyHolding = isHolding;
	}

	void PsyCharging (bool isCharging){
		psyCharging = isCharging;
	}

}