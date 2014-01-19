using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExistentialEnso.HueCSharpToolkit.Models
{
    /// <summary>
    /// Model class to contain data about a Hue bridge.
    /// </summary>
    public class Bridge
    {
        public string Name { get; set; }

        public int ProxyPort { get; set; }

        public string ProxyAddress { get; set; }

        public string MacAddress { get; set; }

        public string IpAddress { get; set; }

        public string NetworkMask { get; set; }

        public string GatewayIpAddress { get; set; }

        public string IsDhcp { get; set; }

        public ArrayList Whitelist { get; set; }
    }
}
