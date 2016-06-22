using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playercast : MonoBehaviour { 
	public GameObject targetobject; 
	public GameObject tmptobject;
	
	public GameObject objectofinterest;
	
	public Text interacttext; 
	public Text lumber; 
	public int wood=0;
	
	private int woodvalue=2; 
	private string targetproperty;
	
	private string targetname; 
	private int targetquantity;
	
	
	private string tmpproperty; 
	private string tmpname;
	
	private int tmpquantity; 
	private bool tmpbool;
	
	void Awake(){
		
	}

	void Start(){
		interacttext = GetComponent<Text> ();
	}
	
	void inspectobject(GameObject x) {
		
		
		
		if(x.tag=="usableobject"){
			
			
			
			objectofinterest=GameObject.FindWithTag("messagecenter");
			
			
			
			interacttext = objectofinterest.GetComponent<Text> ();
			
			
			
			interacttext.text = "Press E to Interact";  
			
		}
		
		
		else if(x==GameObject.FindWithTag("engine")){
			
			
			
			objectofinterest=GameObject.FindWithTag("messagecenter");
			
			
			
			interacttext = objectofinterest.GetComponent<Text> ();
			
			
			
			interacttext.text = "Press E to Add Lumber";  
			
		}
		
	}
	
	
	void ignoreobject(){
		
		
		interacttext.text=""; 
		
	}
	
	void storeobject(){
		
		
		tmptobject=GameObject.Find("ingredients");
		
		
		lumber=tmptobject.GetComponent<Text>();
		
		
		
		wood += targetquantity; 
		
		lumber.text="Lumber: " +wood;  
		
		Destroy(targetobject);
		
		
	}
	
	void giveobject(GameObject y){
		
		
		if(y==GameObject.Find("engine")){
			
			
			
			objectofinterest=GameObject.Find("Temperature");
			
			
			
			thermometer tempup = objectofinterest.GetComponent<thermometer> ();
			
			
			
			tempup.CurrentTemp+=(wood*woodvalue);
			
			
			
			
			tmptobject=GameObject.Find("ingredients");
			
			
			
			lumber=tmptobject.GetComponent<Text>();
			
			
			
			
			
			
			
			wood -= wood; 
			
			
			lumber.text="Lumber: " +wood;  
			
		}
		
	}
	
	void Update() { 
		
		RaycastHit hit = new RaycastHit();
		
		Vector3 forward = transform.TransformDirection (Vector3.forward) * 10; 
		
		//Debug.DrawRay (transform.position, forward, Color.red);
		
		if(Physics.Raycast(transform.position, (forward) , out hit, 5)){
			
			
			if(hit.collider.gameObject.tag =="engine"){
				
				
				
				
				targetname= hit.collider.gameObject.transform.name;
				
				
				
				
				targetobject=hit.collider.gameObject;
				
				
				
				
				print("This Object is usable and it's tag is:" + targetname);
				
				
				
				
				inspectobject(targetobject);
				
				
				
				
				if(Input.GetKeyDown(KeyCode.E)){
					
					
					
					
					
					giveobject(targetobject);
					
					
					
					
					
					wood=-wood;
					
					
					
					
					
				}
				
				
				
			}
			
			
			
			else if(
				hit.collider.gameObject.tag =="usableobject" ){
				
				
				
				
				targetname= hit.collider.gameObject.transform.name;
				
				
				
				
				targetobject=hit.collider.gameObject;
				
				
				
				
				
				usableobject colliding = targetobject.GetComponent<usableobject>();
				
				
				
				
				targetproperty=colliding.objectproperty;
				
				
				
				
				targetquantity=colliding.objectquantity;
				
				
				
				
				
				inspectobject(targetobject); 
				
				
				
				
				print("This Object is usable and it's tag is:" + targetname);
				
				
				
				
				
				if(Input.GetKeyDown(KeyCode.E)){
					
					
					
					
					
					storeobject();
					
					
					
					
					
				}
				
				
				
				
				
				
				
				
			}
			
			
			
			
			else{
				
				
				
				
				
				
				
				
				
				print("This Object is notusable"); 
				
				
			}
			
			
			
			print("There object is called:" +targetname );
			
			
		}
		
		
		else{
			
			
			
			ignoreobject();}
		
		
		
	}
}