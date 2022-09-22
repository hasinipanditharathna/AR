
// using System;
// using UnityEngine;
// using UnityEngine.Networking; 
// using UnityEngine.UI;
// using UnityEngine.Events;
// using System.Collections;
// using System.Collections.Generic;
// using AWSConfigs;
// using Amazon.S3.Util;


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking; 
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using System.Text;
using UnityEditor;


 
class loadAsset: MonoBehaviour {
  public  void Awake() {
        // _instance = this;
         var username = "hasinipanditharathna@gmail.com";
         var password = "Hasini@1993";
         var _bucket = "ar-assetbundle";
        // BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAJTADDHY7T7GZXX5Q", "n4xV5B25mt7e6br84H2G9SXBx8eDYTQJgCxoaF49");
        //   AmazonS3Client Client = new AmazonS3Client(PublicKey, SecretKey);
        AmazonS3Client S3Client = new AmazonS3Client (username, password);

        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        string target = "benz";
      

            ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
            listObjectsRequest.BucketName = _bucket;
            listObjectsRequest.MaxKeys = 2;
            int i = 1;
            // ListObjectsResponse listObjectsResponse =  S3Client.ListObjectsAsync(listObjectsRequest, null , null);
           ListObjectsResponse listObjectsResponse = S3Client.ListObjectsAsync(listObjectsRequest,  null );
            do
            {
                
                List<S3Object> s3Objects = listObjectsResponse.S3Objects;

                foreach (S3Object s3Object in s3Objects)
                {
                    Console.WriteLine("Key " + s3Object.Key);

                    
                }
                Console.WriteLine("Request " + (++i));
                // string prefix = null;
                // ListObjectsOptions options = null;
                listObjectsRequest.Marker = listObjectsResponse.NextMarker;
                listObjectsResponse =  S3Client.ListObjectsAsync(listObjectsRequest,  null , null);
                StartCoroutine(DownloadBundleRoutine());
            }
            while (listObjectsResponse.IsTruncated);

        }           

    //     ListObjectsRequest request = new ListObjectsRequest()
    //     {
    //         // request.BucketName = _bucket
    //         BucketName = _bucket
    //     };

    //     S3Client.ListObjectsAsync(request, (responseObject) =>
    //     {
    //         if (responseObject.Exception == null) 
    //         {
    //             bool assetFound = responseObject.S3Objects.ForEach(obj => obj.key == target);

    //             if (assetFound == true) {
    //                 Debug.Log("Asset Bundle Found");
                
    //                 StartCoroutine(DownloadBundleRoutine());
    //             } else {
    //                 Debug.Log("Asset not found!");
    //             }
    //         } else {
    //             Debug.Log("Error getting list of items from S3:" + responseObject.Exception);
    //         }
    //     });    
        
    // }


    IEnumerator DownloadBundleRoutine() {
    
        string uri = "https://ar-assetbundle.s3.ap-south-1.amazonaws.com/benz";
        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return request.SendWebRequest();

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

            GameObject benz = bundle.loadAsset<GameObject>("benz");
            Instantiate(benz);
        }
    }
}
    // const string BundleFolder = "https://ar-assetbundle.s3.ap-south-1.amazonaws.com/";

    // public void GetBundleObject(UnityAction<GameObject> callback , Transform bundleParent) {
    //      StartCoroutine(GetAssetBundle( callback , bundleParent));
    // }
    // // void Start() {
    // //     StartCoroutine(GetAssetBundle());
    // // }
 
    // IEnumerator GetAssetBundle(UnityAction<GameObject> callback , Transform bundleParent) {
    //     // UnityWebRequest www = new UnityWebRequest("https://my-3d-content-image.s3.ap-south-1.amazonaws.com/plastic-cup.fbx");
    //     //    UnityWebRequest www = new UnityWebRequest("https://my-3d-content-image.s3.ap-south-1.amazonaws.com/plastic-cup.fbx?response-content-disposition=inline&X-Amz-Security-Token=IQoJb3JpZ2luX2VjEGYaCmFwLXNvdXRoLTEiRzBFAiB0w5EKlJE4t8eMMClXfPLH6eR2XEXFQD4gmTX1EdU8oAIhAJ5CQ3vtliK889gZ38xZqyQ3My64qfBNOaOnnjXNxK%2FgKu0CCO%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2FwEQARoMODgwOTkzODY1NjM0IgwCJHlV3YnA9Oosbr8qwQLDnUkDlHAySjM2FiL%2BpjZRsO9GHv1rvmEfKiQ00CkdvPR5UeTWZh62iAhIfRh1x9Oxh42D%2F9dv7hxtgO0Hd3%2F4JDm5u20EOzy5E%2BwR7ewLRQpLUHicCnWGQMIXDVJzDJqcg%2FCHxV%2BLI8jtNbR7wfeRmUxcb2Vd715j5mEFxytvhUnL%2FC2y0R5hU4g338AVLr%2FF437TytZ0PFcorxUYi3RxvI20xjUwWV4o4jLaKM2BV5%2FlrIWAeXYlFhN3RtTTitCU%2Bs%2FiOw13YC0ssH%2BY1XxDj%2BCPpuwx7ogOFlz60FjREw%2Fjy92N0lrLim1uhDDxux5xbo%2FgRCUj%2BfKiG051x2abYShNtJRsH3laEJMN0VpeCczPQ5GsXwN6WIxX65thW%2B4ok2UOWa5xS1xW7qLkY9D2Is%2FAiEqMhQgA9j7XPmTTHjwwkpjdmAY6swJ%2F0dhtORtGY9MP43wUZY1EXQuwvuZ%2FufWUBPvJhrSI7GInPLizM7kI%2FkaniNXETmj3h%2BUNWSFDmDXD2cscQeA9iAdQtuPaBrDumgu0fOLEkAckXgtvWijdS0NLzR%2B5loK9kOyWOgMyDKL%2BsMyKoZNHoN3in3FI6UInfoJ8nKO0l3%2FD4FslBZNVObyLUu1m3hpkO%2FczUaoHK88d%2FiRUlA%2FXnSs5rdpEmMsPr8lEseVSS4yb4tPTlmHNeNmG9DjqFfr3piiSdTeyoeSB65qlmqcSiPNOkxxMTRZF7gJuVkFJwtDBsDioutAqNRxc%2BPXZfgEoWDH0CRHaVFaq%2BQe%2FPcgcGtnvXY6VZHnlPsk8iWehOCxc%2BsU3WAdVpSrrnJ3E1wozuvxrBXu2uslf%2B6%2FtrAbyIMTh&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Date=20220906T155142Z&X-Amz-SignedHeaders=host&X-Amz-Expires=18000&X-Amz-Credential=ASIA42H2TR6RIFESMST5%2F20220906%2Fap-south-1%2Fs3%2Faws4_request&X-Amz-Signature=317719e9c97c33b695259894c24dbda8ac811c3cc2417a41f5c5057de723bce1");
    //     // DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
    //     //  UnityWebRequest www = new UnityWebRequest("https://my-3d-content-image.s3.ap-south-1.amazonaws.com/benz-object.obj");
       
    //     string bundleURL = BundleFolder + "benz.obj" + "-";

    // #if UNITY_ANDROID
    //     bundleURL += "Android";

    // #endif

    //     // UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("https://my-3d-content-image.s3.ap-south-1.amazonaws.com/benz-object.obj");
    //     UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        
    //     // DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
      
    //     // www.downloadHandler = handler;
    //     yield return www.SendWebRequest();
 
    //     if (www.result != UnityWebRequest.Result.Success) {
    //         Debug.Log(www.error);
    //     }
    //     else {
    //         // Extracts AssetBundle
    //         // AssetBundle bundle = handler.assetBundle;
    //         AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
    //         if(bundle != null) {
    //             // string rootAssetPath = bundle.GetAssetBundle()[0];
    //             GameObject benz = Instantiate(bundle.LoadAsset("benz-object.obj") as GameObject , bundleParent ) ;
    //             bundle.Unload(false);
    //             callback(benz);
    //         } else {
    //             Debug.Log("Not a valid asset bundle");
    //         }
    //         // GameObject benz = (GameObject)bundle.LoadAsset("benz-object.obj");
    //         // Instantiate(benz);
    //     }
    // }
// }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking; 

// public class loadAsset : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
//         string url = "https://my-3d-content-image.s3.ap-south-1.amazonaws.com/plastic-cup.fbx";        
//        UnityWebRequest www = new UnityWebRequest(url);
//         StartCoroutine(WaitForReq(www));
//         // = "s3://my-3d-content-image/plastic-cup.fbx";

// //  StartCoroutine(OutputRoutine(url));

//     }
    

//     IEnumerator WaitForReq(UnityWebRequest www )
//     {
//         yield return www;
//         AssetBundle bundle = www.assetBundle;
//         if(www.error == null){
//             GameObject plasticCup = (GameObject)bundle.loadAsset("plastic-cup.fbx");
//             Instantiate(plasticCup);
//         }
//         else {
//             // Debug.Log('error loading');
//             Debug.Log(www.error);
//         }
//     }

    //  private IEnumerator OutputRoutine (string url) {
    //      var loaded = new UnityWebRequest(url);
    //      loaded.downloadHandler = new DownloadHandlerBuffer();
    //      yield return loaded.SendWebRequest();
 
    //         //   if(www.error == null){
    //         GameObject plasticCup = (GameObject)loaded.downloadHandler.LoadAsset("plastic-cup.fbx");
    //         Instantiate(plasticCup);
    //     // }
 

   
// }

//  StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));

//  private IEnumerator OutputRoutine (string url) {
//          var loaded = new UnityWebRequest(url);
//          loaded.downloadHandler = new DownloadHandlerBuffer();
//          yield return loaded.SendWebRequest();
 
//          s = loaded.downloadHandler.text;
//  }
