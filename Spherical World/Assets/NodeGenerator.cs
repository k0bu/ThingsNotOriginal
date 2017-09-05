using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGenerator : EditorWindow {

///<summary>
///It would be nice to automatically determine if the dedicated
///prefab is Quad or not
///</summary>

	Texture2D headerSectionTexture;
	Texture2D GroundSectionTexture;
	Texture2D GrassSectionTexture;
	Texture2D MountainSectionTexture;

	Color headerSectionColor = new Color(13f/255f,32f/255f,44f/255f,1f);

	Rect headerSection = new Rect();
	Rect GroundSection = new Rect();
	Rect GrassSection = new Rect();
	Rect MountainSection = new Rect();

//
	GameObject prefabGround;
	GameObject prefabGrass;
	GameObject prefabMountain;
	GameObject prefabObstacle;
//

	[MenuItem("Window/Node Generator")]
	static void OpenWindow(){
		NodeGenerator window = (NodeGenerator)GetWindow(typeof(NodeGenerator));
		window.minSize = new Vector2 (600,300);
		window.Show();
	}

	void OnEnable(){
		InitTextures();
	}

	void InitTextures(){
		headerSectionTexture = new Texture2D(1,1);
		headerSectionTexture.SetPixel(0,0,headerSectionColor);
		headerSectionTexture.Apply();

		GroundSectionTexture = Resources.Load<Texture2D>("icons/editor_mage_gradient");
		GrassSectionTexture = Resources.Load<Texture2D>("icons/editor_warrior_gradient");
		MountainSectionTexture = Resources.Load<Texture2D>("icons/editor_rogue_gradient");
	}


	void OnGUI(){
		DrawLayouts();
		DrawHeader();
		DrawGroundSettings();
		DrawGrassSettings();
		DrawMountainSettings();
	}

	void DrawLayouts(){
		headerSection.x = 0;
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 50f;

		GroundSection.x = 0;
		GroundSection.y = 50;
		GroundSection.width = Screen.width / 3f;
		GroundSection.height = Screen.width - 50f;

		GrassSection.x = Screen.width / 3f;
		GrassSection.y = 50;
		GrassSection.width = Screen.width / 3f;
		GrassSection.height = Screen.width - 50f;

		MountainSection.x = Screen.width/ 3f * 2f;
		MountainSection.y = 50;
		MountainSection.width = Screen.width / 3f;
		MountainSection.height = Screen.width - 50f;

		GUI.DrawTexture(headerSection,headerSectionTexture);
		GUI.DrawTexture(GroundSection,GroundSectionTexture);
		GUI.DrawTexture(GrassSection,GrassSectionTexture);
		GUI.DrawTexture(MountainSection,MountainSectionTexture);
	}

	void DrawHeader(){
		GUILayout.BeginArea(headerSection);

		GUILayout.Label("Node Generator");

		GUILayout.EndArea();
	}

	void DrawGroundSettings(){
		GUILayout.BeginArea(GroundSection);

		GUILayout.Label("Ground");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab");
		prefabGround = (GameObject)EditorGUILayout.ObjectField(prefabGround, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal();

		GUILayout.EndArea();
	}

	void DrawGrassSettings(){
		GUILayout.BeginArea(GrassSection);

		GUILayout.Label("Grass");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab");
		prefabGrass = (GameObject)EditorGUILayout.ObjectField(prefabGrass, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal();

			GUILayout.Label("Obstacle");

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Prefab");
			prefabObstacle = (GameObject)EditorGUILayout.ObjectField(prefabObstacle, typeof(GameObject), false);
			EditorGUILayout.EndHorizontal();

		if(prefabGround == null || prefabGrass == null || prefabMountain == null || prefabObstacle == null){
			EditorGUILayout.HelpBox("needs [Prefab] selected",MessageType.Warning);
		}
		else if(GUILayout.Button("Move to next", GUILayout.Height(30))){
			NodeGeneralSettings.OpenWindow(prefabGround,prefabGrass,prefabMountain,prefabObstacle);
		}

		GUILayout.EndArea();
	}

	void DrawMountainSettings(){
		GUILayout.BeginArea(MountainSection);

		GUILayout.Label("Mountain");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab");
		prefabMountain = (GameObject)EditorGUILayout.ObjectField(prefabMountain, typeof(GameObject), false);
		EditorGUILayout.EndHorizontal();

		GUILayout.EndArea();
	}
}

public class NodeGeneralSettings : EditorWindow{
//
	static GameObject prefabGround;
	static GameObject prefabGrass;
	static GameObject prefabMountain;
	static GameObject prefabObstacle;
//
	static NodeGeneralSettings window;

	public static void OpenWindow(GameObject Ground,GameObject Grass,GameObject Mountain, GameObject obstacle){
		prefabGround = Ground;
		prefabGrass = Grass;
		prefabMountain = Mountain;
		prefabObstacle = obstacle;
		window = (NodeGeneralSettings)GetWindow(typeof(NodeGeneralSettings));
		window.minSize = new Vector2(250,200);
		window.Show();
	}

	void OnGUI(){
		DrawSettings();
	}

	float probabilityGround;
	float probabilityGrass;
	float probabilityMountain;
	float outlinePercent;
	int seed;

	void DrawSettings(){
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("gridWorldSize");
		gridWorldSize.x = EditorGUILayout.IntSlider((int)gridWorldSize.x,1,100);
		gridWorldSize.y = gridWorldSize.x;
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("nodeDiameter");
		nodeDiameter = EditorGUILayout.IntSlider(nodeDiameter,1,(int)gridWorldSize.x);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("probabilityGround");
		probabilityGround = EditorGUILayout.Slider(probabilityGround,0,1 - probabilityGrass - probabilityMountain);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("probabilityGrass");
		probabilityGrass = EditorGUILayout.Slider(probabilityGrass,0,1 - probabilityGround - probabilityMountain);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("probabilityMountain");
		probabilityMountain = EditorGUILayout.Slider(probabilityMountain,0,1 - probabilityGrass - probabilityGround);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("outlinePercent");
		outlinePercent = EditorGUILayout.Slider(outlinePercent,0,1);
		EditorGUILayout.EndHorizontal();	

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("seed");
		seed = EditorGUILayout.IntField(seed);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("obstaclPercent");
		obstaclePercent = EditorGUILayout.Slider(obstaclePercent,0,1);
		EditorGUILayout.EndHorizontal();				

		if(gridWorldSize.x == 0 || nodeDiameter == 0 || probabilityGround == 0){
			EditorGUILayout.HelpBox("some aspects are not filled",MessageType.Warning);
		}
		else if(GUILayout.Button("Generate!", GUILayout.Height(30))){
			if(nodeParent != null){
				DestroyImmediate(nodeParent);
				foreach(GameObject node in eachNodes){
					DestroyImmediate(node);
				}
				eachNodes.Clear();

			}
			RandomMapGenerator();
		}
		if(eachNodes.Count != 0){
			if(GUILayout.Button("Clear!",GUILayout.Height(30))){
				DestroyImmediate(nodeParent);
				foreach(GameObject node in eachNodes){
					DestroyImmediate(node);
				}
				eachNodes.Clear();
			}
		}

	}

	Vector2 gridWorldSize;
	int nodeDiameter;
	GameObject nodeParent;
	List<GameObject> eachNodes = new List<GameObject>();
	List<Cord> allNodeCords;
	Queue<Cord> shuffledNodeCodes;
	int obstacleNumber;
	float obstaclePercent;
	Cord mapCentre;
	int gridSizeX;
	int gridSizeY;

	void RandomMapGenerator(){
		if(nodeParent == null){
			nodeParent = new GameObject();
		}

		float nodeRadius = nodeDiameter / 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		obstacleNumber = (int)(gridSizeX * gridSizeY * obstaclePercent);//
//		GameObject[,] nodeObject = new GameObject[gridSizeX,gridSizeY];
		
		allNodeCords = new List<Cord> ();
		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				allNodeCords.Add(new Cord(x,y));
			}
		}

		shuffledNodeCodes = new Queue<Cord> (FisherYatesShuffle.ShuffleArray (allNodeCords.ToArray (), seed));
		mapCentre = new Cord (gridSizeX / 2, gridSizeY / 2);

		Vector3 worldBottomLeft = new Vector3(0,0,0) - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
		
		for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
				float randomvalue = Random.value;
				if(randomvalue <= probabilityGround){
               		Vector3 node_pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                	GameObject ground = Instantiate(prefabGround, node_pos, Quaternion.Euler(Vector3.right * 90));
					ground.transform.localScale = nodeDiameter * (1 - outlinePercent) * new Vector3(1,1,1);
					ground.transform.parent = nodeParent.transform;
//					nodeObject[x,y] = ground;
					eachNodes.Add(ground);
				}
				else if(probabilityGround < randomvalue 
				&& randomvalue <= probabilityGround + probabilityGrass){
                	Vector3 node_pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                	GameObject grass = Instantiate(prefabGrass, node_pos, Quaternion.Euler(Vector3.right * 90));
					grass.transform.localScale = nodeDiameter * (1 - outlinePercent) * new Vector3(1,1,1);
					grass.transform.parent = nodeParent.transform;
//					nodeObject[x,y] = grass;
					eachNodes.Add(grass);
				}
				else if(probabilityGround + probabilityGrass <randomvalue 
				&& randomvalue <= 1){
                	Vector3 node_pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                	GameObject mountain = Instantiate(prefabMountain, node_pos, Quaternion.Euler(Vector3.right * 90));
					mountain.transform.localScale = nodeDiameter * (1 - outlinePercent) * new Vector3(1,1,1);
					mountain.transform.parent = nodeParent.transform;
//					nodeObject[x,y] = mountain;
					eachNodes.Add(mountain);
				}

            }
        }


		bool [,] obstacleMap = new bool [gridSizeX,gridSizeY];
		int currentObstacleCount = 0;

		for (int i = 0; i < obstacleNumber; i++){
			Cord randomCord = GetRandomCord();
			obstacleMap[randomCord.x,randomCord.y] = true;
			currentObstacleCount ++;		

			if (randomCord != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount)){
				Vector3 obstaclePosition = worldBottomLeft + Vector3.right * (randomCord.x * nodeDiameter + nodeRadius) + Vector3.forward * (randomCord.y * nodeDiameter + nodeRadius);
				GameObject newObstacle = Instantiate(prefabObstacle, obstaclePosition + Vector3.up * .5f, Quaternion.identity);
				newObstacle.transform.localScale = nodeDiameter * (1 - outlinePercent) * new Vector3(1,1,1);
				newObstacle.transform.parent = nodeParent.transform;
				eachNodes.Add(newObstacle);
			}
			else{
				obstacleMap[randomCord.x,randomCord.y] = false;
				currentObstacleCount --;
			}
		}

	}

	public Cord GetRandomCord(){
		Cord randomCord = shuffledNodeCodes.Dequeue();
		shuffledNodeCodes.Enqueue(randomCord);
		return randomCord;
	}

	bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) {
		bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
		Queue<Cord> queue = new Queue<Cord> ();
		queue.Enqueue (mapCentre);
		mapFlags [mapCentre.x, mapCentre.y] = true;

		int accessibleTileCount = 1;

		while (queue.Count > 0) {
			Cord tile = queue.Dequeue();

			for (int x = -1; x <= 1; x ++) {
				for (int y = -1; y <= 1; y ++) {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if (x == 0 || y == 0) {
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) {
							if (!mapFlags[neighbourX,neighbourY] && !obstacleMap[neighbourX,neighbourY]) {
								mapFlags[neighbourX,neighbourY] = true;
								queue.Enqueue(new Cord(neighbourX,neighbourY));
								accessibleTileCount ++;
							}
						}
					}
				}
			}
		}

	int targetAccessibleTileCount = gridSizeX * gridSizeY - currentObstacleCount;
	return targetAccessibleTileCount == accessibleTileCount;
	}

	public struct Cord {
		public int x;
		public int y;

		public Cord(int _x, int _y){
			x = _x;
			y = _y;
		}

		public static bool operator == (Cord c1, Cord c2){
			return c1.x == c2.x && c1.y == c2.y;
		}

		public static bool operator != (Cord c1, Cord c2){
			return !(c1 == c2);
		}
	}

}
