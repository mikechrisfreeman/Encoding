using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Transcode.Encoding
{
    [Serializable]
    public class EncodingResponse
    {
        [XmlElement(IsNullable = true)]
        public string message { get; set; }
                
        //[XmlChoiceIdentifier()]
        [XmlElement("errors", IsNullable = true)]
        public EncodingErrors errors { get; set; }
    }

    [Serializable]
    [XmlType("errors")]
    public class EncodingErrors
    {
        public EncodingErrors()
        {
            errors = new List<string>();
        }

        [XmlElement("error")]
        public List<string> errors { get; set; }
    }
}
