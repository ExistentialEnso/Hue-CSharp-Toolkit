using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExistentialEnso.HueCSharpToolkit.Models
{
    /// <summary>
    /// A data model class representing the current state of a light. While it can be instatiated generically for
    /// convenience, this can only really be used to do simple on/off commands. A light state should instead be
    /// preferentially instantiated as one of the three subtypes: HueSaturationLightState, ColorSpaceLightState, or
    /// TemperatureColorState.
    /// </summary>
    public class LightState
    {
        /// <summary>
        /// Simply whether or not the light is on.
        /// </summary>
        [JsonProperty(PropertyName = "on")]
        public bool On { get; set; }

        /// <summary>
        /// Brightness of the bulb when On==true
        /// Useful for dimming. Value from 0-255.
        /// </summary>
        [JsonProperty(PropertyName = "bri")]
        public int Brightness { get; set; }

        [JsonProperty(PropertyName = "alert")]
        public string Alert
        {
            get { return _alert; }
            set
            {
                value = value.ToLower();
                if (value != "none" && value != "select" && value != "lselect") return;

                _alert = value;
            }
        }

        private string _alert = "none";

        [JsonProperty(PropertyName = "effect")]
        public string Effect
        {
            get { return _effect; }
            set
            {
                value = value.ToLower();
                if (value != "none" && value != "colorloop") return;

                _effect = value;
            }
        }

        private string _effect = "none";

        [JsonProperty(PropertyName = "transitiontime")]
        public int TransitionTime { get; set; }

        /// <summary>
        /// Gets the JSON that should be sent to the API to represent this LightState. Using this will prevent
        /// extra parameters from being passed when the light is off, which, while the bridge will accept them,
        /// will pass back warnings.
        /// </summary>
        /// <returns>JSON string</returns>
        public string ToApiJson()
        {
            if (!On) return "{\"on\":false}";

            return JsonConvert.SerializeObject(this);
        }


        /// <summary>
        /// A LightState that is represented in terms of hue and saturation.
        /// </summary>
        public class HueSaturationLightState : LightState
        {
            public HueSaturationLightState()
            {
                On = true;
                Brightness = 255;
                Hue = 0;
                Saturation = 0;
                TransitionTime = 4;
            }

            /// <summary>
            /// Hue of the bulb when On==true && ColorMode=="hs"
            /// Value from 0-65535.
            /// </summary>
            [JsonProperty(PropertyName = "hue")]
            public int Hue { get; set; }

            /// <summary>
            /// Saturation of the bulb's Hue when On==true && ColorMode=="hs"
            /// </summary>
            [JsonProperty(PropertyName = "sat")]
            public int Saturation { get; set; }

        }

        /// <summary>
        /// A LightState that is represented in temperature.
        /// </summary>
        public class TemperatureLightState : LightState
        {
            public TemperatureLightState()
            {
                On = true;
                Brightness = 255;
                ColorTemperature = Temperatures.Neutral;
                TransitionTime = 4;
            }

            [JsonProperty(PropertyName = "ct")]
            public int ColorTemperature { get; set; }
        }

        /// <summary>
        /// A LightState that is represented in X/Y color space.
        /// </summary>
        public class ColorSpaceLightState : LightState
        {
            public ColorSpaceLightState()
            {
                On = true;
                Brightness = 255;
                ColorSpaceX = 0.5f;
                ColorSpaceY = 0.5f;
                TransitionTime = 4;
            }

            [JsonIgnore]
            public float ColorSpaceX { get; set; }

            [JsonIgnore]
            public float ColorSpaceY { get; set; }

            [JsonProperty(PropertyName = "ct")]
            public string ColorSpace
            {
                get { return ColorSpaceX + ".." + ColorSpaceY; }
            }
        }

        /// <summary>
        /// Helpful constant values for the TemperatureLightState's Temperature property.
        /// </summary>
        public class Temperatures
        {
            /// <summary>
            /// 2000K - Warmest temperature value supported by the API
            /// </summary>
            public const int Warmest = 500;

            /// <summary>
            /// 2700K - Equivalent to a typical warm consumer lightbulb.
            /// </summary>
            public const int Warm = 370;

            /// <summary>
            /// 3500K - Equivalent to a neutral consumer lightbulb.
            /// </summary>
            public const int Neutral = 285;

            /// <summary>
            /// 4500K - Equivalent to a typical cool consumer lightbulb
            /// </summary>
            public const int Cool = 222;

            /// <summary>
            /// 6500K - Coolest temperature value supported by the API.
            /// </summary>
            public const int Coolest = 153;
        }
    }
}
