using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArduinoSP
{
   public class DictionaryJson
    {
       public string temperDS18b20 { get; set; }
       public string pressureBMP085 { get; set; }
       public string temperBMP085 { get; set; }
       public string humidityDHT22 { get; set; }
       public string temperDHT22 { get; set; }
    }
}
