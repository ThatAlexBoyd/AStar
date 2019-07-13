using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
	public Vector2 ID;
	public int cost = 0;
	public int heuristic = 0;
	public int total = 0;
	public bool walkable;
	public GameObject MagicManager;
	public int sideNum;
	
	void Awake()
	{
		MagicManager = GameObject.FindGameObjectWithTag("Magic");	
	}
	
	
	public void Construct(Vector2 ID, bool walkable)
	{
		this.ID = ID;
		this.walkable =  walkable;

		
		//Swap color if not walkable
		if(!this.walkable)
		{
			renderer.material.color = Color.black;
		}
		
		GetSides();
			
	}
	
	bool GetWalk()
	{
		return walkable;	
	}
	
	
	void Flip()
	{
		if(AStarTest.grid[(int)ID.x,(int)ID.y].walkable ==  true)
		{
			AStarTest.grid[(int)ID.x,(int)ID.y].walkable =  false;
			AStarTest.grid[(int)ID.x,(int)ID.y].renderer.material.color = Color.black;
		}
		else
		{
			AStarTest.grid[(int)ID.x,(int)ID.y].walkable =  true;
			AStarTest.grid[(int)ID.x,(int)ID.y].renderer.material.color = Color.white;
		}
			
	}
	
	void GetStart()
	{
		MagicManager.SendMessage("SetStart", ID);
	}
	
		void GetEnd()
	{
		MagicManager.SendMessage("SetEnd", ID);
	}
	
		
	void GetSides()
	{
//		if(ID.x != 0 && ID.x != AStarTest.gridWidth - 1 && ID.y != 0 && ID.x != AStarTest.gridHeight - 1)
//		{
//			//Middle of pizza
//			return;
//		}
		
		if(ID.x == 0 && ID.y == AStarTest.gridHeight - 1)
		{
			//top left corner
			//renderer.material.color =  Color.cyan;
			sideNum = 2;
			return;
			
		}
		
		if(ID.x == AStarTest.gridWidth - 1 && ID.y == 0)
		{
			//bottom right corner
			//renderer.material.color =  Color.cyan;
			sideNum = 6;
			return;
		}
		
		if(ID.x == AStarTest.gridWidth - 1 && ID.y == AStarTest.gridHeight - 1)
		{
			//top right
			//renderer.material.color =  Color.cyan;
			sideNum = 4;
			return;
		}
		
		if(ID.x == 0 && ID.y == 0)
		{
			//buttom left or ORIGIN	
			//renderer.material.color =  Color.cyan;
			sideNum = 0;
			return;
		}
		
		if(ID.y == AStarTest.gridHeight - 1)
		{
			//top
			//renderer.material.color =  Color.grey;
			sideNum = 3;
			return;
			
		}
		
		if(ID.y == 0)
		{
			//bottom
			//renderer.material.color =  Color.yellow;
			sideNum = 7;
			return;
		}
		
		if(ID.x == 0)
		{
			//left side
			//renderer.material.color =  Color.white;
			sideNum = 1;
			return;
		}
		
		if(ID.x == AStarTest.gridWidth - 1)
		{
			//right side
			//renderer.material.color =  Color.blue;
			sideNum = 5;
			return;
		}
		
	
		//renderer.material.color =  Color.green;
		sideNum = 8;
		//I am middle of pizza
	}
	
}


	
