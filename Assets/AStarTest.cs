using UnityEngine;
using System.Collections;
[ExecuteInEditMode]

public class AStarTest : MonoBehaviour 
{
	public static int gridWidth = 51;
	public string widthHolder;
	public static int gridHeight = 51;
	public string heightHolder;
	public float buff = 1.3f;
	
	public GameObject cube;
	
	//Make Grid
	public static Tile[,] grid = new Tile[AStarTest.gridWidth,AStarTest.gridHeight];
	

	public TextMesh errorInfo;
	bool canBuild;
	
	public static bool seeButton;
	public TextMesh h;
	public TextMesh w;

	
	void Awake()
	{

		errorInfo.text = "";
		canBuild = true;
		seeButton = false;
	}
	
	
    void OnGUI() 
	{
       if(seeButton == false)
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3((float)Screen.width/854,(float)Screen.height/480,1f));
			
			widthHolder = GUI.TextField(new Rect(60, 10, 30, 20), widthHolder, 25);
			heightHolder = GUI.TextField(new Rect(60, 35, 30, 20), heightHolder, 25);
			
			
			if (GUI.Button(new Rect(10, 60, 80, 30), "Build Grid"))
			{
				if(string.IsNullOrEmpty(widthHolder) || string.IsNullOrEmpty(heightHolder))
				{
					errorInfo.text = "Did Not Enter A Width/Height".ToString();
					Debug.Log("No Nums");
				}
				else
				{
					CheckNums();
				}
				
			}
		}
	
            
    }
	
	void CheckNums()
	{
		gridWidth = int.Parse(widthHolder);
		gridHeight = int.Parse(heightHolder);
		
		if(canBuild)
		{
			if(gridWidth > 30 || gridHeight > 30 || gridWidth < 5 || gridHeight < 5)
			{
				errorInfo.text = "Entered An In Correct Value".ToString();
				Debug.Log("Bad");	
			}
			else
			{
				canBuild = false;
				h.renderer.enabled = false;
				w.renderer.enabled = false;
				seeButton = true;
				Painter.paintNum = 0;
				Build();	
			}
		}
		else
		{
			errorInfo.text = "You Have Already Built The Grid".ToString();	
		}
		
	}
	
	void Build()
	{
		
		for(int i = 0; i < AStarTest.gridWidth; i++)
		{
			
			for(int j = 0; j < AStarTest.gridHeight; j++)
			{
					//Makes new tile
					GameObject holder = Instantiate(cube, new Vector3(i,0,j)*buff, Quaternion.identity) as GameObject;
					Tile holder2 = holder.GetComponent("Tile") as Tile;
					holder2.Construct(new Vector2(i,j), true);
					grid[i,j] = holder2;

			}
		}
		
		
		errorInfo.text = "";
		Camera.main.transform.position = new Vector3(gridWidth/2,gridHeight+5,(gridHeight/2));

		
	}
	
	

}
