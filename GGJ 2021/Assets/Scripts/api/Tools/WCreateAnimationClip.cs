#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

using UnityEditor.Animations;
using UnityEditor.SceneManagement;

public class WCreateAnimationClip : ScriptableWizard
{
    [Space]
    [Header("Import Settings")]
    public bool _applyImportSettings = true;

    [Space]
    [Header("Animation clips and controller")]
    public bool _createAnimationClips = true;
    public string _animationOutput = "Assets\\Resources\\Animations && animators\\Players";

    public bool _createAnimationController = true;
    public string _controllerBase = "Assets\\Resources\\Animations && animators\\Players\\controllerBase.controller";

    [Space]
    [Header("Player Prefab")]
    public bool _createPlayerPrefab = true;
    public string _prefabBase = "Assets\\Resources\\Prefabs\\playersNew\\prefabBase.prefab";
    public string _prefabsOutput = "Assets\\Resources\\Prefabs\\playersNew";

    public string _IdleSprites = "Assets\\Resources\\Sprites\\playerIdles";

    [Space]
    [Header("Proyectiles")]
    public string _proyectileControllerBase = "Assets\\Resources\\Animations && animators\\Players\\proyectileBase.controller";
    public string _proyectilePrefabBase = "Assets\\Resources\\Prefabs\\playersNew\\Proyectiles\\proyectilBase.prefab";
    public string _proyectileOutput = "Assets\\Resources\\Prefabs\\playersNew\\Proyectiles";

    [Space]
    [Header("Scriptable Object")]
    public bool _createScriptableObject = true;
    public string _scriptableObjectOutput = "Assets\\Resources\\Scriptable Objects\\players";
    
    [Space]
    [Header("Audios")]
    public bool _importAudios = true;
    public string _audioInput = "Assets\\Resources\\Audio\\players";







    [MenuItem("Strato Games/Sprites to Animation Clips")]
    static void CreateWizard()
    {
        //Animations && animators
        ScriptableWizard.DisplayWizard<WCreateAnimationClip>("Sprites to Animation Clips", "Start Clip automation","Do automation for each folder");
    }

    // When automating all players
    private void OnWizardOtherButton()
    {
        string path = EditorUtility.SaveFolderPanel("Choose root folder for characters", "Assets/Resources/Sprites", "");
        if (path.Length == 0)
        {
            EditorUtility.DisplayDialog("Path Invalid!", "File path not found or invalid!", "OK");
            return;
        }

        string[] folders = Directory.GetDirectories(path);

        for (int i = 0; i < folders.Length; i++)
        {
            processCharacterFolder(folders[i], true);
        }
    }


    private void OnWizardCreate()
    {
        //insert things to do here
        string path = EditorUtility.SaveFolderPanel("Choose player folder", "Assets/Resources/Sprites", "");
        if (path.Length == 0)
        {
            EditorUtility.DisplayDialog("Path Invalid!", "File path not found or invalid!", "OK");
            return;
        }

        processCharacterFolder(path);
    }
    private void processCharacterFolder(string aPath, bool aSplitContrabarra = false)
    {
        string[] pathParts;

        if (aSplitContrabarra)
        {
            pathParts = aPath.Split('\\');
        }
        else
        {
            pathParts = aPath.Split('/');
        }

        string playerName = pathParts[pathParts.Length - 1];

        Dictionary<string, AnimationClip> dictAnimationClips = new Dictionary<string, AnimationClip>();

        Debug.Log("path: " + aPath + " folder name: " + playerName);

        string folderPath = _animationOutput + "\\" + playerName;

        Debug.Log("create folder: " + folderPath);

        // Check if folder exists.
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string[] folders = Directory.GetDirectories(aPath);

        for (int i = 0; i < folders.Length; i++)
        {
            char[] splitter = { '\\' };
            string[] folderParts = folders[i].Split(splitter);
            string animationName = folderParts[folderParts.Length - 1];

            Debug.Log("animation name: " + animationName);

            DirectoryInfo dir = new DirectoryInfo(folders[i]);

            //if (animationName == "Special")
            //{
            //    Debug.Log("found special");
            //}

            //FileInfo[] files = (FileInfo[])dir.GetFiles("*.*").Where(name => !name.Name.Contains(".meta"));
            var info = dir.GetFiles("*.*").Where(name => !name.Name.Contains(".meta"));

            List<string> tempList = new List<string>();


            //int x = -1;

            foreach (FileInfo f in info)
            {
                //x += 1;
                //Debug.Log("File: " + f.FullName);

                //Debug.Log(f.Extension);

                string[] elementPathParts = f.FullName.Split(new string[] { "\\Assets\\" }, System.StringSplitOptions.None);

                string path = "Assets\\" + elementPathParts[1];

                setSpriteImportSettings(path, animationName.Contains("Proyectil") ? false : true);

                tempList.Add(path);
                //sprArray[x] = makeImportSettings("Assets\\" + elementPathParts[1]);
            }

            string[] spritePaths = new string[tempList.Count];

            for (int x = 0; x < tempList.Count; x++)
            {
                spritePaths[x] = tempList[x];
            }


            if (_createAnimationClips)
            {
                AnimationClip clip = SpritesToAnimClip(spritePaths, 8, playerName + "_" + animationName, folderPath + "\\" + playerName + "_" + animationName);
                dictAnimationClips.Add(animationName, clip);
            }

        }

        string controllerPath;

        if (_createAnimationController)
        {
            //Create Animator controllerPath.
            controllerPath = createAnimatorController(playerName, dictAnimationClips);

            if (_createPlayerPrefab)
            {
                createPrefab(playerName, controllerPath, dictAnimationClips);
            }
        }

        
    }

    /// <summary>
    /// Imports and sets player settings.
    /// </summary>
    /// <param name="aFile"></param>
    /// <returns></returns>
    private void setSpriteImportSettings(string aFile, bool aChangePivot = true)
    {
        Debug.Log("path: " + aFile);

        if (_applyImportSettings)
        {
            TextureImporter footprintTextureImporter = (TextureImporter)AssetImporter.GetAtPath(aFile);

            footprintTextureImporter.textureType = TextureImporterType.Sprite;
            footprintTextureImporter.spritePixelsPerUnit = 1;
            if (aChangePivot)
            {
                footprintTextureImporter.spritePivot = new Vector2(0.5f, 0);
            }
            else
            {
                Debug.Log("setting middle pivot");

                footprintTextureImporter.spritePivot = new Vector2(0.5f, 0.5f);
            }

            footprintTextureImporter.filterMode = FilterMode.Point;

            TextureImporterSettings settings = new TextureImporterSettings();
            footprintTextureImporter.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;
            footprintTextureImporter.SetTextureSettings(settings);
            AssetDatabase.ImportAsset(aFile, ImportAssetOptions.ForceUpdate);


            EditorUtility.SetDirty(footprintTextureImporter);
            footprintTextureImporter.SaveAndReimport();
        }

        return;
    }

    private AnimationClip SpritesToAnimClip(string[] spritePath, int fps, string animName, string path)
    {
        Debug.Log("creating: " + animName);
        AnimationClip animClip = new AnimationClip();
        animClip.frameRate = fps;
        animClip.name = animName;

        AnimationClipSettings setting = new AnimationClipSettings { loopTime = true };
        AnimationUtility.SetAnimationClipSettings(animClip, setting);


        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[spritePath.Length];
        for (int i = 0; i < spritePath.Length; i++)
        {
            //spriteKeyFrames[i] = default(ObjectReferenceKeyframe);
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i].time = ((float)i) / fps;

            spriteKeyFrames[i].value = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath[i]);
        }

        EditorCurveBinding binding = EditorCurveBinding.PPtrCurve(string.Empty, typeof(SpriteRenderer), "m_Sprite");

        AnimationUtility.SetObjectReferenceCurve(animClip, binding, spriteKeyFrames);

        AssetDatabase.CreateAsset(animClip, path + ".anim");

        AssetDatabase.SaveAssets();

        return animClip;
    }
    private string createAnimatorController(string aPlayer, Dictionary<string, AnimationClip> aDict)
    {
        string newPath = _animationOutput + "\\" + aPlayer + "\\" + aPlayer + " Controller.controller";

        //AssetDatabase.getAsse

        AnimatorController animatorController = createAndLoadController(newPath, _controllerBase);

        for (int i = 0; i < animatorController.layers[0].stateMachine.states.Length; i++)
        {
            string name = animatorController.layers[0].stateMachine.states[i].state.name;
            if (aDict.ContainsKey(name))
            {
                Debug.Log("found animation: " + name);
                animatorController.layers[0].stateMachine.states[i].state.motion = aDict[name];
            }
            else
            {
                Debug.LogWarning("Player: " + aPlayer + " is missing animation: " + name);
            }
        }

        EditorUtility.SetDirty(animatorController);
        AssetDatabase.SaveAssets();

        return newPath;
    }

    private void createPrefab(string aPlayerName, string aControllerPath, Dictionary<string, AnimationClip> aClips)
    {
        string newPath = _prefabsOutput + "\\" + aPlayerName + ".prefab";

        // Load the contents of the Prefab Asset.
        GameObject root = createAndLoadPrefab(newPath, _prefabBase);

        // Modify Prefab contents.

        // Load controller
        Animator anim = root.GetComponentInChildren<Animator>();

        // Set the player controller
        anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(aControllerPath, typeof(AnimatorController));



        string idle = _IdleSprites + "\\" + aPlayerName + ".png";

        if (File.Exists(idle))
        {
            if (_applyImportSettings)
            {
                setSpriteImportSettings(idle, false);
            }

            SpriteRenderer sprRen = anim.gameObject.GetComponent<SpriteRenderer>();
            sprRen.sprite = (Sprite)AssetDatabase.LoadAssetAtPath(idle, typeof(Sprite));
        }
        else
        {
            Debug.LogWarning("idle sprite missing for player: " + aPlayerName);
        }

        CPlayer playerComp = root.GetComponent<CPlayer>();

        CPlayer.PlayerType playerType = CPlayer.PlayerType.Mesias;

        switch (aPlayerName)
        {
            case "Chopo":
                playerType = CPlayer.PlayerType.Mesias;
                break;
            case "Maxir":
                playerType = CPlayer.PlayerType.Maxir;
                break;
            case "Traba":
                playerType = CPlayer.PlayerType.Traba;
                break;
            case "Shark":
                playerType = CPlayer.PlayerType.Shark;
                break;
            case "Lisboa":
                playerType = CPlayer.PlayerType.Brus;
                break;
            case "Yeyo":
                playerType = CPlayer.PlayerType.Yeyo;
                break;
            case "Yerman":
                playerType = CPlayer.PlayerType.Feudal;
                break;
            case "Lycan":
                playerType = CPlayer.PlayerType.Lycan;
                break;
            case "Mariachi":
                playerType = CPlayer.PlayerType.Mariachi;
                break;
            case "Shao Tyger":
                playerType = CPlayer.PlayerType.ShaoKhan;
                break;
            case "Diegor":
                playerType = CPlayer.PlayerType.Diegor;
                break;
            case "Kills":
                playerType = CPlayer.PlayerType.Kills;
                break;
        }

        playerComp.playerType = playerType;

        // create and load proyectil
        if (aClips.ContainsKey("Proyectil"))
        {
            playerComp._bullet = (GameObject)AssetDatabase.LoadAssetAtPath(crearProyectil(aPlayerName, aClips["Proyectil"]), typeof(GameObject));
            
            if (aClips.ContainsKey("Proyectil2"))
            {
                playerComp._bullet2 = (GameObject)AssetDatabase.LoadAssetAtPath(crearProyectil(aPlayerName, aClips["Proyectil2"]), typeof(GameObject));
            }
        }

        string audioBase = _audioInput + "\\" + aPlayerName + "\\";

        // Set audios
        if (_importAudios)
        {
            //"intro", "poder", "wins"

            string audioActual = audioBase + "poder.wav";

            if (File.Exists(audioActual))
            {
                playerComp._specialSfx = (AudioClip)AssetDatabase.LoadAssetAtPath(audioActual, typeof(AudioClip));
            }
            else
            {
                Debug.LogWarning("player " + aPlayerName + " is missing poder audio");
            }
        }


        if (_createScriptableObject)
        {
            CCharacterInfo info = ScriptableObject.CreateInstance<CCharacterInfo>();

            info.name = aPlayerName;
            info.longName = aPlayerName;

            string shortName = "MAR";

            switch (aPlayerName)
            {
                case "Chopo":
                    shortName = "CHP";
                    break;
                case "Maxir":
                    shortName = "MAX";
                    break;
                case "Traba":
                    shortName = "TRA";
                    break;
                case "Shark":
                    shortName = "SHK";
                    break;
                case "Lisboa":
                    shortName = "LIS";
                    break;
                case "Yeyo":
                    shortName = "YEY";
                    break;
                case "Yerman":
                    shortName = "YER";
                    break;
                case "Lycan":
                    shortName = "LYC";
                    break;
                case "Mariachi":
                    shortName = "MAR";
                    break;
                case "Shao Tyger":
                    shortName = "STR";
                    break;
                case "Diegor":
                    shortName = "DIE";
                    break;
                case "Kills":
                    shortName = "KIL";
                    break;
            }

            info.shortName = shortName;

            if (File.Exists(idle))
            {
                // No need, previously done.
                //if (_applyImportSettings)
                //{
                //    setSpriteImportSettings(idle);
                //}

                info.idleSprite = (Sprite)AssetDatabase.LoadAssetAtPath(idle, typeof(Sprite));
            }
            // Previously warned.
            //else
            //{
            //    Debug.LogWarning("idle sprite missing for player: " + aPlayerName);
            //}

            if (_importAudios)
            {
                string audioActual = audioBase + "intro.wav";

                if (File.Exists(audioActual))
                {
                    info.intro = (AudioClip)AssetDatabase.LoadAssetAtPath(audioActual, typeof(AudioClip));
                }
                else
                {
                    Debug.LogWarning("player " + aPlayerName + " is missing intro audio");
                }

                audioActual = audioBase + "wins.wav";

                if (File.Exists(audioActual))
                {
                    info.wins = (AudioClip)AssetDatabase.LoadAssetAtPath(audioActual, typeof(AudioClip));
                }
                else
                {
                    Debug.LogWarning("player " + aPlayerName + " is missing wins audio");
                }

                audioActual = audioBase + "pj.wav";

                if (File.Exists(audioActual))
                {
                    info.playerName = (AudioClip)AssetDatabase.LoadAssetAtPath(audioActual, typeof(AudioClip));
                }
                else
                {
                    Debug.LogWarning("player " + aPlayerName + " is missing pj audio");
                }
            }

            AssetDatabase.CreateAsset(info, _scriptableObjectOutput + "\\" + aPlayerName + ".asset");
            AssetDatabase.SaveAssets();
        }

        

        
        


        EditorUtility.SetDirty(root);
        EditorSceneManager.MarkSceneDirty(root.gameObject.scene);

        //contentsRoot.AddComponent<BoxCollider>();

        //// Save contents back to Prefab Asset and unload contents.
        PrefabUtility.SaveAsPrefabAsset(root, newPath);
        PrefabUtility.UnloadPrefabContents(root);
    }

    private string crearProyectil(string aPlayerName, AnimationClip aClip)
    {
        string newPath = _proyectileOutput + "\\" + aClip.name + ".prefab";

        // Load the contents of the Prefab Asset.
        GameObject root = createAndLoadPrefab(newPath, _proyectilePrefabBase);

        // Create animator controller for the proyectile.
        string proyectileControllerPath = _animationOutput + "\\" + aPlayerName + "\\" + aClip.name + ".controller";

        AnimatorController controller = createAndLoadController(proyectileControllerPath, _proyectileControllerBase);

        for (int i = 0; i < controller.layers[0].stateMachine.states.Length; i++)
        {
            string name = controller.layers[0].stateMachine.states[i].state.name;

            if (name == "Proyectile")
            {
                controller.layers[0].stateMachine.states[i].state.motion = aClip;
            }
        }

        // Save the new controller.
        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();

        // Set the controller.
        //root.GetComponent<Animator>().runtimeAnimatorController = controller;

        Animator anim = root.GetComponentInChildren<Animator>();

        anim.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath(proyectileControllerPath, typeof(AnimatorController));

        EditorUtility.SetDirty(root);
        EditorSceneManager.MarkSceneDirty(root.gameObject.scene);

        //root.GetComponent<Animator>().controller = 

        //Resources.Load("path_to_your_controller") as RuntimeAnimatorController;

        Debug.Log(anim != null);
        Debug.Log("anim: " + anim.gameObject.transform.name);
        // Save the new prefab.
        PrefabUtility.SaveAsPrefabAsset(root, newPath);
        PrefabUtility.UnloadPrefabContents(root);

        return newPath;
    }

    private GameObject createAndLoadPrefab(string aPath, string aPrefabBase)
    {
        if (AssetDatabase.CopyAsset(aPrefabBase, aPath))
        {
            Debug.Log("Copy Success");
        }
        else
        {
            Debug.Log("Copy Fail");
        }

        return PrefabUtility.LoadPrefabContents(aPath);
    }

    private AnimatorController createAndLoadController(string aPath, string aControllerBase)
    {
        if (AssetDatabase.CopyAsset(aControllerBase, aPath))
        {
            Debug.Log("Copy Success");
        }
        else
        {
            Debug.Log("Copy Fail");
        }

        return AssetDatabase.LoadAssetAtPath<AnimatorController>(aPath);
    }

}
#endif