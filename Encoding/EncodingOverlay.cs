using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Transcode.Encoding
{
    [Serializable]
    [XmlRoot("overlay")]
    public class EncodingOverlay
    {
        [XmlElement(IsNullable = true)] 
        public string overlay_source { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_left { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_right { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_top { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_bottom { get; set; }
        [XmlElement(IsNullable = true)]
        public string size { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_start { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_duration { get; set; }
    }

    [Serializable]
    [XmlRoot("text_overlay")]
    public class EncodingTextOverlay
    {
        [XmlElement(IsNullable = true)]
        public string text { get; set; }
        [XmlElement(IsNullable = true)]
        public string font_source { get; set; }
        [XmlElement(IsNullable = true)]
        public string font_rotate { get; set; }
        [XmlElement(IsNullable = true)]
        public string font_color { get; set; }
        [XmlElement(IsNullable = true)]
        public string align_center { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_x { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_y { get; set; }
        [XmlElement(IsNullable = true)]
        public string size { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_start { get; set; }
        [XmlElement(IsNullable = true)]
        public string overlay_duration { get; set; }
    }		 
}
