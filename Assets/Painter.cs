using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

public class Painter : MonoBehaviour {
	
    public RaycastHit hit;
    private Ray ray;
	public static  int paintNum = 4;
	public TextMesh direc;
	
	void Awake()
	{
		paintNum = 4;
		direc.text = "Enter a number between 5 and 30 for the Height/Width. They DO NOT have to be the same.".ToString();	
	}
	
	void Update () 
	{
        if(Input.GetButtonUp("Fire1"))
		{
			ray = camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0));
			
			if (Physics.Raycast(ray, out hit, 100.0f))
	        {
	            if(hit.collider.tag == "tile")
				{
					switch(paintNum)
					{
					case 0:
						hit.collider.SendMessage("GetStart");
						hit.collider.renderer.material.color = Color.green;
						Debug.DrawLine(transform.position, hit.point, Color.red);
						paintNum++;
					break;
						
					case 1:
						hit.collider.SendMessage("GetEnd");
						hit.collider.renderer.material.color = Color.red;
						Debug.DrawLine(transform.position, hit.point, Color.red);
						paintNum++;
					break;
						
					case 2:
						hit.collider.SendMessage("Flip");
						Debug.DrawLine(transform.position, hit.point, Color.red);
					break;
					}
				}
	        }
		}
		switch(Painter.paintNum)
		{
		case 0:
			direc.text = "Select the START TILE".ToString();
		break;
		
		case 1:
			direc.text = "Select the END TILE".ToString();
		break;
			
		case 2:
			direc.text = "Paint what blocks are WALKABLE and NOT WALKABLE".ToString();
		break;
		
		case 4:
			direc.text = "Enter a number between 5 and 30 for the Height/Width. They DO NOT have to be the same".ToString();
		break;

			
		}

	}
	

}