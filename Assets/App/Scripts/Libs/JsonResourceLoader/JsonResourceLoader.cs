using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Libs.JsonResourceLoader
{
    public static class JsonResourceLoader
    {
        public static T LoadFromResources<T>(string filePath)
        {
            var file = Resources.Load<TextAsset>(filePath);

            if (file != null)
            {
                return JsonConvert.DeserializeObject<T>(file.text);
            }

            Debug.LogWarning("File not found: " + filePath);
            return default;
        }
    }
}