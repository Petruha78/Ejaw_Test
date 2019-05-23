using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Web
{
    public class WebConnector : MonoBehaviour, IWebConnector
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }


        public void Get<T>(string url, Action<T> success = null)
        {
            StartCoroutine(GetRequest(url, success));
        }

        public void Post<T>(string url, Dictionary<string, string> form, Action<T> success = null, Action error = null)
        {
            StartCoroutine(PostRequest(url, form, success, error));
        }

        public void Put(string url, byte[] data, Action complete = null)
        {
            StartCoroutine(PutRequest(url, data, complete));
        }


        private IEnumerator GetRequest<T>(string url, Action<T> success = null)
        {
            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            try
            {
                var data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text,
                    new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore});
                
                success.Invoke(data);
                Debug.Log("Data Loaded successfully");
            }
            catch (JsonException jsonException)
            {
                success.Invoke(default);
                Debug.LogError(jsonException.Message);
            }
        }

        private IEnumerator PostRequest<T>(string url, Dictionary<string, string> form = null,
            Action<T> success = null, Action error = null)
        {
            var request = UnityWebRequest.Post(url, form);

            yield return request.SendWebRequest();

            if (request.isHttpError || request.isHttpError)
            {
                error.Invoke();

                yield break;
            }

            var data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);

            success.Invoke(data);
        }

        private IEnumerator PutRequest(string url, byte[] data, Action successCallback = null)
        {
            var request = UnityWebRequest.Put(url, data);

            request.uploadHandler = new UploadHandlerRaw(data);
            request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                yield break;
            }
            
            successCallback.Invoke();
        }
    }
}