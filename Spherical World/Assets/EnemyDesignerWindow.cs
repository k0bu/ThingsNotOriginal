using System.Collections;
using UnityEngine;
using UnityEditor;
using Types;

public class EnemyDesignerWindow : EditorWindow {

	Texture2D headerSectionTexture;
	Texture2D mageSectionTexture;
	Texture2D warriorSectionTexture;
	Texture2D rogueSectionTexture;

	Color headerSectionColor = new Color(13f/255f,32f/255f,44f/255f,1f);

	Rect headerSection = new Rect();
	Rect mageSection = new Rect();
	Rect warriorSection = new Rect();
	Rect rogueSection = new Rect();

	static MageData mageData;
	static WarriorData warriorData;
	static RogueData rogueData;

	public static MageData MageInfo {get {return mageData;}}
	public static WarriorData WarriorInfo {get {return warriorData;}}
	public static RogueData RogueInfo {get {return rogueData;}}

	[MenuItem("Window/Enemy Designer")]
	static void OpenWindow(){
		EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
		window.minSize = new Vector2 (600,300);
		window.Show();
	}

	void OnEnable(){
			InitTextures();
			InitData();
	}

	public static void InitData(){
		mageData = (MageData)ScriptableObject.CreateInstance(typeof(MageData));
		warriorData = (WarriorData)ScriptableObject.CreateInstance(typeof(WarriorData));
		rogueData = (RogueData)ScriptableObject.CreateInstance(typeof(RogueData));
	}

	///<summary>
	/// Initialize Texture2D values
	///</summary>

	void InitTextures(){
		headerSectionTexture = new Texture2D(1,1);
		headerSectionTexture.SetPixel(0,0,headerSectionColor);
		headerSectionTexture.Apply();

		mageSectionTexture = Resources.Load<Texture2D>("icons/editor_mage_gradient");
		warriorSectionTexture = Resources.Load<Texture2D>("icons/editor_warrior_gradient");
		rogueSectionTexture = Resources.Load<Texture2D>("icons/editor_rogue_gradient");
	}

	void OnGUI(){
		DrawLayouts();
		DrawHeader();
		DrawMageSettings();
		DrawRogueSettings();
		DrawWarriorSettings();
	}

	void DrawLayouts(){
		headerSection.x = 0;
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 50f;

		mageSection.x = 0;
		mageSection.y = 50;
		mageSection.width = Screen.width / 3f;
		mageSection.height = Screen.width - 50f;

		warriorSection.x = Screen.width / 3f;
		warriorSection.y = 50;
		warriorSection.width = Screen.width / 3f;
		warriorSection.height = Screen.width - 50f;

		rogueSection.x = Screen.width/ 3f * 2f;
		rogueSection.y = 50;
		rogueSection.width = Screen.width / 3f;
		rogueSection.height = Screen.width - 50f;

		GUI.DrawTexture(headerSection,headerSectionTexture);
		GUI.DrawTexture(mageSection,mageSectionTexture);
		GUI.DrawTexture(warriorSection,warriorSectionTexture);
		GUI.DrawTexture(rogueSection,rogueSectionTexture);
	}

	void DrawHeader(){
		GUILayout.BeginArea(headerSection);

		GUILayout.Label("Enemy Designer");


		GUILayout.EndArea();
	}

	void DrawMageSettings(){
		GUILayout.BeginArea(mageSection);

		GUILayout.Label("Mage");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Damage");
		mageData.dmgType = (MageDmgType)EditorGUILayout.EnumPopup(mageData.dmgType);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Weapon");
		mageData.wpnType = (MageWpnType)EditorGUILayout.EnumPopup(mageData.wpnType);
		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button("Create!",GUILayout.Height(40))){
						GeneralSettings.OpenWindow(GeneralSettings.SettingsType.MAGE);	
		}
	
		GUILayout.EndArea();
	}

	void DrawWarriorSettings(){
		GUILayout.BeginArea(warriorSection);	

		GUILayout.Label("Warrior");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Class");
		warriorData.classType = (WarriorClassType)EditorGUILayout.EnumPopup(warriorData.classType);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Weapon");
		warriorData.wpnType = (WarriorWpnType)EditorGUILayout.EnumPopup(warriorData.wpnType);
		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button("Create!",GUILayout.Height(40))){
						GeneralSettings.OpenWindow(GeneralSettings.SettingsType.WARRIOR);	
		}

		GUILayout.EndArea();
	}

	void DrawRogueSettings(){
		GUILayout.BeginArea(rogueSection);

		GUILayout.Label("Rogue");

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Strategy");
		rogueData.strategyType = (RogueStrategyType)EditorGUILayout.EnumPopup(rogueData.strategyType);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Weapon");
		rogueData.wpnType = (RogueWpnType)EditorGUILayout.EnumPopup(rogueData.wpnType);
		EditorGUILayout.EndHorizontal();

		if(GUILayout.Button("Create!",GUILayout.Height(40))){
						GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ROGUE);	
		}

		GUILayout.EndArea();
	}

}

public class GeneralSettings : EditorWindow{

	public enum SettingsType{
		MAGE,
		WARRIOR,
		ROGUE
	}

	static SettingsType dataSetting;
	static GeneralSettings window;

	public static void OpenWindow(SettingsType setting){
		dataSetting = setting;
		window = (GeneralSettings)GetWindow(typeof(GeneralSettings));
		window.minSize = new Vector2 (250,200);
		window.Show();

	}

	void OnGUI(){
		switch(dataSetting){
			case SettingsType.MAGE:
				DrawSettings((CharacterData)EnemyDesignerWindow.MageInfo);
			break;
			case SettingsType.WARRIOR:
				DrawSettings((CharacterData)EnemyDesignerWindow.WarriorInfo);
			break;
			case SettingsType.ROGUE:
				DrawSettings((CharacterData)EnemyDesignerWindow.RogueInfo);
			break;
		}

	}

	void DrawSettings(CharacterData charData){
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefab");
		charData.prefab = (GameObject)EditorGUILayout.ObjectField(charData.prefab, typeof(GameObject),false);
		EditorGUILayout.EndHorizontal();	
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Helath");
		charData.maxHealth = EditorGUILayout.FloatField(charData.maxHealth);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Max Energy");
		charData.maxEnergy = EditorGUILayout.FloatField(charData.maxEnergy);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Power");
		charData.power = EditorGUILayout.Slider(charData.power, 0 , 100);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("% Crit Chance");
		charData.critChance = EditorGUILayout.Slider(charData.critChance, 0 , charData.power);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Name");
		charData.characterName = EditorGUILayout.TextField(charData.characterName);
		EditorGUILayout.EndHorizontal();

		if(charData.prefab == null){
			EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created ",MessageType.Warning);
		}
		else if(charData.characterName == null || charData.characterName.Length < 1){
			EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created ",MessageType.Warning);
		}
		else if(GUILayout.Button("Finish and Save", GUILayout.Height(30))){
			SaveCharacterData();
			window.Close();
		}

	}

	void SaveCharacterData(){
		string prefabPath;
		string newPrefabPath = "Assets/prefabs/characters/";
		string dataPath = "Assets/resources/characterData/data/";

		switch(dataSetting){
			case SettingsType.MAGE:
				//create the .asset file
				dataPath += "mage/" + EnemyDesignerWindow.MageInfo.characterName + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.MageInfo, dataPath);

				newPrefabPath += "mage/" + EnemyDesignerWindow.MageInfo.characterName + ".prefab";
				//get prefab path
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.MageInfo.prefab);
				AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				GameObject magePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath,typeof(GameObject));
				if(magePrefab.GetComponent<Mage>() == null){
					magePrefab.AddComponent(typeof(Mage));
				}
				magePrefab.GetComponent<Mage>().mageData = EnemyDesignerWindow.MageInfo;
			break;
			case SettingsType.WARRIOR:
				//create the .asset file
				dataPath += "warrior/" + EnemyDesignerWindow.WarriorInfo.characterName + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.WarriorInfo, dataPath);

				newPrefabPath += "warrior/" + EnemyDesignerWindow.WarriorInfo.characterName + ".prefab";
				//get prefab path
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.WarriorInfo.prefab);
				AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				GameObject warriorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath,typeof(GameObject));
				if(warriorPrefab.GetComponent<Warrior>() == null){
					warriorPrefab.AddComponent(typeof(Warrior));
				}
				warriorPrefab.GetComponent<Warrior>().warriorData = EnemyDesignerWindow.WarriorInfo;
			break;
			case SettingsType.ROGUE:
				//create the .asset file
				dataPath += "rogue/" + EnemyDesignerWindow.RogueInfo.characterName + ".asset";
				AssetDatabase.CreateAsset(EnemyDesignerWindow.RogueInfo, dataPath);

				newPrefabPath += "rogue/" + EnemyDesignerWindow.RogueInfo.characterName + ".prefab";
				//get prefab path
				prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.RogueInfo.prefab);
				AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				GameObject roguePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath,typeof(GameObject));
				if(roguePrefab.GetComponent<Rogue>() == null){
					roguePrefab.AddComponent(typeof(Rogue));
				}
				roguePrefab.GetComponent<Rogue>().rogueData = EnemyDesignerWindow.RogueInfo;
			break;
		}
	}

}
