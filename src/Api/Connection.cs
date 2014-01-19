using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using ExistentialEnso.HueCSharpToolkit.Models;
using Newtonsoft.Json;

namespace ExistentialEnso.HueCSharpToolkit.Api
{
    public class Connection
    {
        private Bridge _bridge;
        private User _user;

        public Connection(string ipAddress, string username)
        {
            _bridge = new Bridge();
            _bridge.IpAddress = ipAddress;

            _user = new User();
            _user.Username = username;
        }

        public void PutLightState(Light light)
        {
            var url = "http://" + _bridge.IpAddress + "/api/" + _user.Username + "/lights/" + light.InternalId + "/state";

            string json = light.State.ToApiJson();

            Console.WriteLine(json);

            using (var client = new System.Net.WebClient())
            {
                var response = client.UploadData(url, "PUT", System.Text.Encoding.UTF8.GetBytes(json));

                Console.WriteLine(System.Text.Encoding.UTF8.GetString(response));
            }
        }

        /// <summary>
        /// Pulls a list of all available lights from the bridge. These will not have their State information populated.
        /// </summary>
        /// <returns>All lights on the bridge.</returns>
        public Dictionary<int, Light> GetAllLights()
        {
            var url = "http://" + _bridge.IpAddress + "/api/" + _user.Username + "/lights/";

            WebRequest req = WebRequest.Create(url);
            Stream obStream = req.GetResponse().GetResponseStream();

            var objReader = new StreamReader(obStream);

            string sLine = "";
            int i = 0;
            string response = "";

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                response += sLine;
            }

            response = response.Replace("name", "Name");

            var lightData = JsonConvert.DeserializeObject<Dictionary<int, Light>>(response);

            foreach (KeyValuePair<int, Light> data in lightData)
            {
                data.Value.InternalId = data.Key;
            }

            return lightData;
        }

        public Light GetLightById(int id)
        {
            var url = "http://" + _bridge.IpAddress + "/api/" + _user.Username + "/lights/" + id;

            WebRequest req = WebRequest.Create(url);
            Stream obStream = req.GetResponse().GetResponseStream();

            var objReader = new StreamReader(obStream);

            string sLine = "";
            int i = 0;
            string response = "";

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                response += sLine;
            }

            response = response.Replace("on", "On").Replace("name", "Name").Replace("type", "Type").Replace("modelid", "ModelId").Replace("swversion", "SoftwareVersion");

            Light light = JsonConvert.DeserializeObject<Light>(response);
            light.InternalId = id;

            return light;
        }
    }
}
