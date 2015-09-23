using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Transcode.Encoding
{
    [Serializable]
    [XmlRoot("response")]
    public class EncodingGetStatusResponse : EncodingResponse
    {
        public EncodingGetStatusResponse()
        {
            formats = new List<EncodingFormat>();
        }
       
        [XmlElement("id")]
        public string MediaID { get; set; }

        [XmlElement(IsNullable = true)]
        public string userid { get; set; }

        [XmlElement(IsNullable = true)]
        public string sourcefile { get; set; }

        [XmlElement(IsNullable = true)]
        public string status { get; set; }

        [XmlElement(IsNullable = true)]
        public string notifyurl { get; set; }

        [XmlElement(IsNullable = true)]
        public string created { get; set; }

        [XmlElement(IsNullable = true)]
        public string started { get; set; }

        [XmlElement(IsNullable = true)]
        public string finished { get; set; }

        [XmlElement(IsNullable = true)]
        public string prevstatus { get; set; }

        [XmlElement(IsNullable = true)]
        public string downloaded { get; set; }

        [XmlElement(IsNullable = true)]
        public string uploaded { get; set; }

        [XmlElement(IsNullable = true)]
        public string time_left { get; set; }

        [XmlElement(IsNullable = true)]
        public string progress { get; set; }

        [XmlElement(IsNullable = true)]
        public string time_left_current { get; set; }

        [XmlElement(IsNullable = true)]
        public string progress_current { get; set; }

        [XmlElement("format")]
        public List<EncodingFormat> formats { get; set; }

    }
}
