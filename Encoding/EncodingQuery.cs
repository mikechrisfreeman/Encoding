using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Transcode.Encoding
{
    [XmlRoot("query")]
    public class EncodingQuery
    {
        public EncodingQuery()
        {
            formats = new List<EncodingFormat>();
        }

        public EncodingQuery(int userId, string userkey)
        {
            this.userid = userId;
            this.userkey = userkey;
        }

        [XmlElement(IsNullable = true)] 
        public int? userid { get; set; }

        [XmlElement(IsNullable = true)] 
        public string userkey { get; set; }

        [XmlElement(IsNullable = true)] 
        public string action { get; set; }

        [XmlElement(IsNullable = true)]
        public string mediaid { get; set; }

        [XmlElement(IsNullable = true)] 
        public string region { get; set; }

        [XmlElement(IsNullable = true)] 
        public string source { get; set; }

        [XmlElement(IsNullable = true)] 
        public string notify_format { get; set; }

        [XmlElement(IsNullable = true)] 
        public string notify { get; set; }

        [XmlElement(IsNullable = true)] 
        public string notify_upload { get; set; }

        [XmlElement("format")]
        public List<EncodingFormat> formats { get; set; }
    }
}
