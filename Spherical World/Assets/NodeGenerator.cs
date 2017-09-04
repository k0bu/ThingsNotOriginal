using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGenerator : EditorWindow {

///<summary>
///It would be nice to automatically determine if the dedicated
///prefab is Cube or not
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

		if(prefabGround == null || prefabGrass == null || prefabMountain == null){
			EditorGUILayout.HelpBox("needs [Prefab] selected",MessageType.Warning);
		}
		else if(GUILayout.Button("Move to next", GUILayout.Height(30))){
			NodeGeneralSettings.OpenWindow(prefabGround,prefabGrass,prefabMountain);
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
//
	static NodeGeneralSettings window;

	public static void OpenWindow(GameObject Ground,GameObject Grass,GameObject Mountain){
		prefabGround = Ground;
		prefabGrass = Grass;
		prefabMountain = Mountain;
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

	void DrawSettings(){
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("gridWorldSize");
		gridWorldSize.x = EditorGUILayout.Slider(gridWorldSize.x,0,100);
		gridWorldSize.y = gridWorldSize.x;
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("nodeDiameter");
		nodeDiameter = EditorGUILayout.Slider(nodeDiameter,0,gridWorldSize.x);
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
	float nodeDiameter;
	GameObject nodeParent;
	List<GameObject> eachNodes = new List<GameObject>();

	void RandomMapGenerator(){
		if(nodeParent == null){
			nodeParent = new GameObject();
		}
		float nodeRadius = nodeDiameter / 2;
        int gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        int gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
//		GameObject[,] nodeObject = new GameObject[gridSizeX,gridSizeY];
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

	}


}
