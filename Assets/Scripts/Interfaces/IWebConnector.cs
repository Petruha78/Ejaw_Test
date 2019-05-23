using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IWebConnector
    {
        void Get<TData>(string url, Action<TData> successCallback = null);       
        
        void Post<TData>(string url, Dictionary<string, string> form,
            Action<TData> successCallback = null, Action errorCallback = null);
        
        void Put(string url, byte[] data, Action uploadComplete = null);
    }
}