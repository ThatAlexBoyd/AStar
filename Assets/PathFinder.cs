using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]


public class PathFinder : MonoBehaviour
{

	Vector2 startTile;
	Vector2 endTile;
	Vector2 currentTile;
	
	int lookCount = 0;
	bool isStart = false;
	bool isEnd = false;
	bool isImpossible;
	//Lists that store checked and unchecked tiles
	List<Vector2> closedList = new List<Vector2>();
	List<Vector2> openList = new List<Vector2>();
	public TextMesh wrong;
	
	void Awake()
	{
		isImpossible = false;
		wrong.renderer.enabled = false;
	}
	
	public void Update()
	{
		if(Input.GetButtonUp("Jump"))
		{
			lookCount = 0;
			SearchPath();	
		}
	}
	
	
	void OnGUI()
	{
		if(AStarTest.seeButton)
		{
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3((float)Screen.width/854,(float)Screen.height/480,1f));
			
			if (GUI.Button(new Rect(10, 10, 100, 30), "Find"))
			{
				if(isStart && isEnd)
				{
					lookCount = 0;
					SearchPath();
				}
				else
				{
					Debug.Log("broke");	
				}
			}
			
			if (GUI.Button(new Rect(10, 45, 100, 30), "Reload"))
			{
				Application.LoadLevel(0);
			}
		}
	}
	
	
	void SetStart(Vector2 temp)
	{
		startTile = new Vector2(temp.x,temp.y);	
		isStart =  true;
	}
	
	void SetEnd(Vector2 temp)
	{
		endTile = new Vector2(temp.x,temp.y);
		isEnd = true;
	}
	
	public void SearchPath()
	{
		this.startTile = startTile;
		this.endTile =  endTile;
		
		bool canSearch = true;

		if(AStarTest.grid[(int)startTile.x,(int)startTile.y].walkable ==  false)
		{
			canSearch =  false;	
		}
		
		if(AStarTest.grid[(int)endTile.x,(int)endTile.y].walkable ==  false)
		{
			canSearch =  false;	
		}
		
		//Start A* algorithim
		if(canSearch)
		{
			//add starting tile to open list
			openList.Add(startTile);
			currentTile = new Vector2(-1,-1);
			
			//wwhile open list is not empty do searching
			while(openList.Count != 0 || lookCount != AStarTest.gridHeight*AStarTest.gridWidth)
			{
				if(isImpossible)
				{
					wrong.renderer.enabled = true;
					return;
				}
				lookCount++;
				//current node = node from open list with lowest cost
				currentTile = GetTileWithLowestTotal(openList);
				
				//if curren tile is the end tile, then STOP
				if(currentTile.x == endTile.x && currentTile.y == endTile.y)
				{
					//Gets out of while loop if found end tile
					Debug.Log("Found End Tile");
					break;	
				}
				else
				{
					//move the current tile to the closed list from the open list
					openList.Remove(currentTile);
					closedList.Add(currentTile);
					
					//get all adjacent tiles
					List<Vector2> adjacentTiles = GetAdjacentTiles(currentTile);
					
						foreach(Vector2 adjacentTile in adjacentTiles)
							{
									
							
							//adjacent tile cannot be in open list
								if(!openList.Contains(adjacentTile))
								{
									if(!closedList.Contains(adjacentTile))
									{
										//move it to the open list and calculate cost
										openList.Add(adjacentTile);
										
										Tile tile = AStarTest.grid[(int)adjacentTile.x,(int)adjacentTile.y];
										
										//calculate cost
										tile.cost = AStarTest.grid[(int)currentTile.x,(int)currentTile.y].cost + 1;
										
										//Calculate Manhattan Distance
										tile.heuristic =  ManhattenDistance(adjacentTile);
										
										//caculate the total amount
										tile.total = tile.cost + tile.heuristic;
										
										//make this tile green
										//tile.renderer.material.color = Color.green;
									}
								}
							}					
					
				}
			}
		}
		
		
		//Show the path
		ShowPath();
		
	}
	
	public void ShowPath()
	{
		bool startFound = false;
		Vector2 currentTile = endTile;
		List<Vector2> pathTiles = new List<Vector2>();
		
		while(startFound ==  false)
		{
			List<Vector2> adjacentTiles = GetAdjacentTiles(currentTile);
			
			//check to see what the newest current tile
			foreach(Vector2 adjacentTile in adjacentTiles)
			{
				//check if its is the start tile
				if(adjacentTile.x == startTile.x && adjacentTile.y == startTile.y)
				{
					startFound = true;	
				}
				
				//it has to be inside closed as well as inside the open list
				if(closedList.Contains(adjacentTile) || openList.Contains(adjacentTile))
				{
					if(AStarTest.grid[(int)adjacentTile.x,(int)adjacentTile.y].cost <= AStarTest.grid[(int)currentTile.x,(int)currentTile.y].cost && AStarTest.grid[(int)adjacentTile.x,(int)adjacentTile.y].cost > 0)
					{
						//change current tile
						currentTile =  adjacentTile;
						
						//add new adjacent tile
						pathTiles.Add(adjacentTile);
						
						//change color
						AStarTest.grid[(int)adjacentTile.x,(int)adjacentTile.y].renderer.material.color = Color.blue;
						
						break;
					}
				}
			}
		}
	}
	
	
	
	
	public int ManhattenDistance(Vector2 adjacentTile)
	{
		int manhattann = Mathf.Abs(((int)endTile.x - (int)adjacentTile.x)) + Mathf.Abs(((int)endTile.y - (int)adjacentTile.y));
		
		return manhattann;
	}
	
	
	
	
	public List<Vector2> GetAdjacentTiles(Vector2 currentTile)
	{
		List<Vector2> adjacentTiles = new List<Vector2>();
		//Vector2 adjacentTile;
		if(currentTile.x < 0 || currentTile.y < 0)
		{
			isImpossible = true;
			return adjacentTiles;
			
		}
		Tile temp = AStarTest.grid[(int)currentTile.x,(int)currentTile.y];
		///Adjacent Variables
		Vector2 adjacentTileUp = new Vector2(currentTile.x, currentTile.y + 1);
		Vector2 adjacentTileDown = new Vector2(currentTile.x, currentTile.y - 1);
		Vector2 adjacentTileLeft = new Vector2(currentTile.x - 1, currentTile.y);
		Vector2 adjacentTileRight = new Vector2(currentTile.x + 1, currentTile.y);

		switch(temp.sideNum)
		{
		case 0:
			//Tile above
			if(AStarTest.grid[(int)adjacentTileUp.x,(int)adjacentTileUp.y].walkable)
			{
				adjacentTiles.Add(adjacentTileUp);	
			}
			//Tile right
			if (AStarTest.grid[(int)adjacentTileRight.x,(int)adjacentTileRight.y].walkable)
			{
				adjacentTiles.Add(adjacentTileRight);	
			}
			break;
			
		case 1:
			//Tile above
			
			if(AStarTest.grid[(int)adjacentTileUp.x,(int)adjacentTileUp.y].walkable)
			{
				adjacentTiles.Add(adjacentTileUp);	
			}
			//Tile right
			if(AStarTest.grid[(int)adjacentTileRight.x,(int)adjacentTileRight.y].walkable)
			{
				adjacentTiles.Add(adjacentTileRight);	
			}
			//Tile below
			if(AStarTest.grid[(int)adjacentTileDown.x,(int)adjacentTileDown.y].walkable)
			{
				adjacentTiles.Add(adjacentTileDown);	
			}
			break;
		
		case 2:
			//Tile below
			if(AStarTest.grid[(int)adjacentTileDown.x,(int)adjacentTileDown.y].walkable)
			{
				adjacentTiles.Add(adjacentTileDown);	
			}
			//Tile right
			if(AStarTest.grid[(int)adjacentTileRight.x,(int)adjacentTileRight.y].walkable)
			{
				adjacentTiles.Add(adjacentTileRight);	
			}
			break;
		
		case 3:
			//Tile below
			if(AStarTest.grid[(int)adjacentTileDown.x,(int)adjacentTileDown.y].walkable)
			{
				adjacentTiles.Add(adjacentTileDown);	
			}
			//Tile right
			if(AStarTest.grid[(int)adjacentTileRight.x,(int)adjacentTileRight.y].walkable)
			{
				adjacentTiles.Add(adjacentTileRight);	
			}
			//Tile left
			if(AStarTest.grid[(int)adjacentTileLeft.x,(int)adjacentTileLeft.y].walkable)
			{
				adjacentTiles.Add(adjacentTileLeft);	
			}
			break;
			
		case 4:
			//Tile below
			if(AStarTest.grid[(int)adjacentTileDown.x,(int)adjacentTileDown.y].walkable)
			{
				adjacentTiles.Add(adjacentTileDown);	
			}
			//Tile left
			if(AStarTest.grid[(int)adjacentTileLeft.x,(int)adjacentTileLeft.y].walkable)
			{
				adjacentTiles.Add(adjacentTileLeft);	
			}
			break;
			
		case 5:
			//Tile above
			if(AStarTest.grid[(int)adjacentTileUp.x,(int)adjacentTileUp.y].walkable)
			{
				adjacentTiles.Add(adjacentTileUp);	
			}
			//Tile below
			if(AStarTest.grid[(int)adjacentTileDown.x,(int)adjacentTileDown.y].walkable)
			{
				adjacentTiles.Add(adjacentTileDown);	
			}
			//Tile left
			Vector2 adjacentTile15 = new Vector2(currentTile.x - 1, currentTile.y);
			
			if(AStarTest.grid[(int)adjacentTile15.x,(int)adjacentTile15.y].walkable)
			{
				adjacentTiles.Add(adjacentTile15);	
			}
			break;
			
		case 6:
			//Tile above
			if(AStarTest.grid[(int)adjacentTileUp.x,(int)adjacentTileUp.y].walkable)
			{
				adjacentTiles.Add(adjacentTileUp);	
			}
			//Tile left
			if(AStarTest.grid[(int)adjacentTileLeft.x,(int)adjacentTileLeft.y].walkable)
			{
				adjacentTiles.Add(adjacentTileLeft);	
			}
			break;
			
		case 7:
			//Tile above
			if(AStarTest.grid[(int)adjacentTileUp.x,(int)adjacentTileUp.y].walkable)
			{
				adjacentTiles.Add(adjacentTileUp);	
			}
			//Tile right
			if(AStarTest.grid[(int)adjacentTileRight.x,(int)adjacentTileRight.y].walkable)
			{
				adjacentTiles.Add(adjacentTileRight);	
			}
			//Tile left
			if(AStarTest.grid[(int)adjacentTileLeft.x,(int)adjacentTileLeft.y].walkable)
			{
				adjacentTiles.Add(adjacentTileLeft);	
			}
			break;
			
		case 8:
			//Tile above
			if(AStarTest.grid[(int)adjacentTileUp.x,(int)adjacentTileUp.y].walkable)
			{
				adjacentTiles.Add(adjacentTileUp);	
			}
			//Tile below
			if(AStarTest.grid[(int)adjacentTileDown.x,(int)adjacentTileDown.y].walkable)
			{
				adjacentTiles.Add(adjacentTileDown);	
			}
			//Tile left
			if(AStarTest.grid[(int)adjacentTileLeft.x,(int)adjacentTileLeft.y].walkable)
			{
				adjacentTiles.Add(adjacentTileLeft);	
			}
			//Tile right
			if(AStarTest.grid[(int)adjacentTileRight.x,(int)adjacentTileRight.y].walkable)
			{
				adjacentTiles.Add(adjacentTileRight);	
			}
			break;
		}
		
		return adjacentTiles;
		
		
	}
	
	
	
	
	
	//calculates lowest value
	public Vector2 GetTileWithLowestTotal(List<Vector2> openList)
	{
		//tmep variables
		Vector2 tileWithLowestTotal = new Vector2(-1,-1);
		int lowestTotal = int.MaxValue;
		
		//search all open tiles and get the tle with lowest cost.
		foreach(Vector2 openTile in openList)
		{
			if(AStarTest.grid[(int)openTile.x,(int)openTile.y].total <= lowestTotal)
			{
				lowestTotal = AStarTest.grid[(int)openTile.x,(int)openTile.y].total;
				tileWithLowestTotal = new Vector2((int)openTile.x,(int)openTile.y);
			}
		}
		
		
		return tileWithLowestTotal;
	}

}
