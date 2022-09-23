using System;
using UnityEngine;
using Vuforia;
// using UnityEngine.UI;


public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    public ImageTargetBehaviour ImageTargetTemplate;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    // downloaded
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }


    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());

    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            // Clear all known targets
        }
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Store the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;

        // Stop the scanning by disabling the behaviour
        mCloudRecoBehaviour.enabled = false;
    }


    void OnGUI()
    {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
            {
                // Reset Behaviour
                mCloudRecoBehaviour.enabled = true;
                mTargetMetadata = "";
            }
        }
    }
}

// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using Vuforia;
// using System;
// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// using TriLib;
// using TriLib.Samples;
// /// <summary>
// /// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
// /// It registers itself at the CloudRecoBehaviour and is notified of new search results as well as error messages
// /// The current state is visualized and new results are enabled using the TargetFinder API.
// /// </summary>
// public class recoEventHandler : MonoBehaviour, IObjectRecoEventHandler
// {
//     #region PRIVATE_MEMBERS
//     CloudRecoBehaviour m_CloudRecoBehaviour;
//     ObjectTracker m_ObjectTracker;
//     TargetFinder m_TargetFinder;
//     string itemName;
//     public GameObject menuContainer;
//     public GameObject menuItem;
//     public GameObject showAnimCloud;
//     private GameObject model;
//     private BlendShapeControl _blendShapeControlPrefab;
//     private Transform _blendShapesContainerTransform;
//     public GameObject sliderGO;
//     public Slider sliderComp;
//     public GameObject loading;
//     private GameObject _rootGameObject;
//     #endregion // PRIVATE_MEMBERS


//     #region PUBLIC_MEMBERS
//     /// <summary>
//     /// Can be set in the Unity inspector to reference a ImageTargetBehaviour 
//     /// that is used for augmentations of new cloud reco results.
//     /// </summary>
//     [Tooltip("Here you can set the ImageTargetBehaviour from the scene that will be used to " +
//              "augment new cloud reco search results.")]
//     public ImageTargetBehaviour m_ImageTargetBehaviour;
//     #endregion // PUBLIC_MEMBERS


//     #region MONOBEHAVIOUR_METHODS
//     /// <summary>
//     /// Register for events at the CloudRecoBehaviour
//     /// </summary>
//     void Start()
//     {
//         // Register this event handler at the CloudRecoBehaviour
//         m_CloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
//         if (m_CloudRecoBehaviour)
//         {
//             m_CloudRecoBehaviour.RegisterEventHandler(this);
//         }
//     }
//     #endregion // MONOBEHAVIOUR_METHODS


//     #region INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler
//     /// <summary>
//     /// called when TargetFinder has been initialized successfully
//     /// </summary>
//     public void OnInitialized()
//     {
//         Debug.Log("Cloud Reco initialized successfully.");

//         m_ObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
//         m_TargetFinder = m_ObjectTracker.GetTargetFinder<ImageTargetFinder>();
//     }

//     public void OnInitialized(TargetFinder targetFinder)
//     {
//         Debug.Log("Cloud Reco initialized successfully.");

//         m_ObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
//         m_TargetFinder = targetFinder;
//     }

//     // Error callback methods implemented in CloudErrorHandler
//     public void OnInitError(TargetFinder.InitState initError) { }
//     public void OnUpdateError(TargetFinder.UpdateState updateError) { }


//     /// <summary>
//     /// when we start scanning, unregister Trackable from the ImageTargetBehaviour, 
//     /// then delete all trackables
//     /// </summary>
//     public void OnStateChanged(bool scanning)
//     {
//         Debug.Log("<color=blue>OnStateChanged(): </color>" + scanning);

//         // Changing CloudRecoBehaviour.CloudRecoEnabled to false will call:
//         // 1. TargetFinder.Stop()
//         // 2. All registered ICloudRecoEventHandler.OnStateChanged() with false.

//         // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call:
//         // 1. TargetFinder.StartRecognition()
//         // 2. All registered ICloudRecoEventHandler.OnStateChanged() with true.
//     }

//     /// <summary>
//     /// Handles new search results
//     /// </summary>
//     /// <param name="targetSearchResult"></param>
//     public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
//     {
//         Debug.Log("<color=blue>OnNewSearchResult(): </color>" + targetSearchResult.TargetName);

//         TargetFinder.CloudRecoSearchResult cloudRecoResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;

//         // This code demonstrates how to reuse an ImageTargetBehaviour for new search results
//         // and modifying it according to the metadata. Depending on your application, it can
//         // make more sense to duplicate the ImageTargetBehaviour using Instantiate() or to
//         // create a new ImageTargetBehaviour for each new result. Vuforia will return a new
//         // object with the right script automatically if you use:
//         // TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)

//         // Check if the metadata isn't null
//         if (cloudRecoResult.MetaData == null)
//         {
//             Debug.Log("Target metadata not available.");
//         }
//         else
//         {
//             string[] splitString = cloudRecoResult.MetaData.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
//             itemName = splitString[0];          
//             showAnimCloud.SetActive(false);
//             Show3DModel();
//             Debug.Log("MetaData: " + cloudRecoResult.MetaData);
//             Debug.Log("TargetName: " + cloudRecoResult.TargetName);
//             Debug.Log("Pointer: " + cloudRecoResult.TargetSearchResultPtr);
//             Debug.Log("TrackingRating: " + cloudRecoResult.TrackingRating);
//             Debug.Log("UniqueTargetId: " + cloudRecoResult.UniqueTargetId);
//         }

//         // Changing CloudRecoBehaviour.CloudRecoEnabled to false will call TargetFinder.Stop()
//         // and also call all registered ICloudRecoEventHandler.OnStateChanged() with false.
//         m_CloudRecoBehaviour.CloudRecoEnabled = false;

//         // Clear any existing trackables
//         m_TargetFinder.ClearTrackables(false);

//         // Enable the new result with the same ImageTargetBehaviour:
//         m_TargetFinder.EnableTracking(cloudRecoResult, m_ImageTargetBehaviour.gameObject);

//         // Pass the TargetSearchResult to the Trackable Event Handler for processing
//         m_ImageTargetBehaviour.gameObject.SendMessage("TargetCreated", cloudRecoResult, SendMessageOptions.DontRequireReceiver);
//     }
//     #endregion // INTERFACE_IMPLEMENTATION_ICloudRecoEventHandler


//     void Show3DModel()
//     {
//         if (itemName != "")
//         {
//             if (File.Exists(Application.persistentDataPath + "/" + itemName))
//             {
//                 sliderGO.SetActive(false);
//                 LoadModel();
//             }
//             else
//             {
//                 StartCoroutine(DownloadModel());
//             }
//         }
//     }
//     private void AssetLoader_OnMetadataProcessed(AssimpMetadataType metadataType, uint metadataIndex, string metadataKey, object metadataValue)
//     {
//         Debug.Log("Found metadata of type [" + metadataType + "] at index [" + metadataIndex + "] and key [" + metadataKey + "] with value [" + metadataValue + "]");
//     }
//     void LoadModel()
//     {
//         loading.SetActive(true);
//         model = m_ImageTargetBehaviour.gameObject.transform.Find("3dmodel").gameObject;
//         var assetLoaderOptions = GetAssetLoaderOptions();
//         using (var assetLoader = new AssetLoaderAsync())
//         {
//             assetLoader.OnMetadataProcessed += AssetLoader_OnMetadataProcessed;
//             assetLoader.LoadFromFileWithTextures(Application.persistentDataPath + "/" + itemName, assetLoaderOptions, null, delegate (GameObject loadedGameObject)
//             {
//                 CheckForValidModel(assetLoader);
//                 _rootGameObject = loadedGameObject;
//                 _rootGameObject.transform.parent = model.transform;
//                 _rootGameObject.transform.localPosition = new Vector3(0, 0, 0);
//                 _rootGameObject.transform.localEulerAngles = new Vector3(0,0,0);
//                 _rootGameObject.transform.localScale = new Vector3(1, 1, 1);
//                 loading.SetActive(false);
//                 FullPostLoadSetup();
//             });
//         }
//     }
//     private void FullPostLoadSetup()
//     {
//         if (_rootGameObject != null)
//         {
//             PostLoadSetup();
//         }
//     }
//     private void PostLoadSetup()
//     {        
//         var rootAnimation = _rootGameObject.GetComponent<Animation>();
//         var skinnedMeshRenderers = _rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
//         if (skinnedMeshRenderers != null)
//         {
//             var hasBlendShapes = false;
//             foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
//             {
//                 if (!hasBlendShapes && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
//                 {
//                     hasBlendShapes = true;
//                 }
//                 for (var i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
//                 {
//                     CreateBlendShapeItem(skinnedMeshRenderer, skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i), i);
//                 }
//             }
//         }
//         if (rootAnimation != null)
//         {
//             foreach (Transform child in menuContainer.transform)
//             {
//                 GameObject.Destroy(child.gameObject);
//             }
//             showAnimCloud.SetActive(true);
//             foreach (AnimationState animationState in rootAnimation)
//             {
//                 Debug.Log(animationState.name);
//                 GameObject newItem = Instantiate(menuItem);
//                 newItem.transform.SetParent(menuContainer.transform);
//                 newItem.transform.localScale = new Vector3(1f, 1f, 1f);
//                 newItem.GetComponentInChildren<Text>().text = animationState.name;
//                 Button button = newItem.GetComponent<Button>();
//                 button.onClick.AddListener(() => { _rootGameObject.GetComponent<Animation>().Play(animationState.name); });
//             }
//         }
//     }
//     private void CreateBlendShapeItem(SkinnedMeshRenderer skinnedMeshRenderer, string name, int index)
//     {
//         var instantiated = Instantiate(_blendShapeControlPrefab, _blendShapesContainerTransform);
//         instantiated.SkinnedMeshRenderer = skinnedMeshRenderer;
//         instantiated.Text = name;
//         instantiated.BlendShapeIndex = index;
//     }
//     private void CheckForValidModel(AssetLoaderBase assetLoader)
//     {
//         if (assetLoader.MeshData == null || assetLoader.MeshData.Length == 0)
//         {
//             throw new Exception("File contains no meshes");
//         }
//     }
//     private AssetLoaderOptions GetAssetLoaderOptions()
//     {
//         var assetLoaderOptions = AssetLoaderOptions.CreateInstance();
//         assetLoaderOptions.DontLoadCameras = false;
//         assetLoaderOptions.DontLoadLights = false;
//         assetLoaderOptions.UseOriginalPositionRotationAndScale = true;
//         assetLoaderOptions.AddAssetUnloader = true;
//         assetLoaderOptions.AutoPlayAnimations = true;
//         assetLoaderOptions.AdvancedConfigs.Add(AssetAdvancedConfig.CreateConfig(AssetAdvancedPropertyClassNames.FBXImportDisableDiffuseFactor, true));
//         return assetLoaderOptions;
//     }
//     private IEnumerator DownloadModel()
//     {
//         var _www = new WWW("Enter your URL here" + itemName);
//         //_www = WWW.LoadFromCacheOrDownload(ModelURI, 5);
//         while (!_www.isDone)
//         {
//             yield return null;
//             sliderGO.SetActive(true); 
//             sliderComp.value = _www.progress;
//         }
//         sliderGO.SetActive(false);

//         var ModelLocalPath = "/" + itemName;

//         var fullPath = Application.persistentDataPath + ModelLocalPath;
//         File.WriteAllBytes(fullPath, _www.bytes);
//         StartCoroutine(LoadAfterDownload());
//     }

//     IEnumerator LoadAfterDownload()
//     {
//         yield return new WaitForSeconds(3f);
//         LoadModel();
//     }    
// }
