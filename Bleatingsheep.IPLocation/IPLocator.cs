﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bleatingsheep.IPLocation
{
    public abstract class IPLocator : IIPLocator
    {
        protected async Task<(bool, T)> GetJsonAsync<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string sResult = await httpClient.GetStringAsync(url);
                    T result = JsonConvert.DeserializeObject<T>(sResult);
                    return (true, result);
                }
                catch (Exception)
                {
                    return (false, default(T));
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        public abstract Task<(bool, IIPLocation)> GetLocationAsync(IPAddress address);

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public virtual async Task<(bool, IIPLocation)> GetLocationAsync(string address) => await GetLocationAsync(IPAddress.Parse(address));

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public virtual async Task<(bool, IIPLocation)> GetLocationAsync(byte[] address) => await GetLocationAsync(new IPAddress(address));
    }
}