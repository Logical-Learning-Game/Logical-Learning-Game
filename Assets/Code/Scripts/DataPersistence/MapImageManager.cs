using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace Unity.Game.SaveSystem
{
    public class MapImageManager 
    {
        static readonly string MapImageBaseDirectoryPath = "MapImage";

        public static IEnumerator GetMapImage(string filename,string imgpath, System.Action<Texture2D> onComplete)
        {
            if (filename is null) yield break;
            // Check if the file already exists in the persistent data path
            string filePath = Path.Combine(Application.persistentDataPath, MapImageBaseDirectoryPath, filename);
            if (File.Exists(filePath))
            {
                // If the file exists, load it from disk
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(255, 255);
                texture.LoadImage(fileData);
                onComplete?.Invoke(texture);
                yield break;
            }

            // If the file does not exist, download it from the URL
            string url = $"{imgpath}";
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error downloading image: {www.error}");
                onComplete?.Invoke(null);
                yield break;
            }

            // Save the downloaded image to the persistent data path
            byte[] imageData = www.downloadHandler.data;
            string directoryPath = Path.Combine(Application.persistentDataPath, MapImageBaseDirectoryPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllBytes(filePath, imageData);

            // Create a new texture from the downloaded image data
            Texture2D mapPreview = new Texture2D(255, 255);
            mapPreview.LoadImage(imageData);

            onComplete?.Invoke(mapPreview);
        }

        public static void DeleteAllMapImages()
        {
            string directoryPath = Path.Combine(Application.persistentDataPath, MapImageBaseDirectoryPath);
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }

}
