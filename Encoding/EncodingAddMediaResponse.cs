using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Transcode.Encoding
{
    [Serializable]
    [XmlRoot("response")]
    public class EncodingAddMediaResponse : EncodingResponse
    {
        [XmlElement(IsNullable = true)] 
        public string MediaID { get; set; }
    }
}
