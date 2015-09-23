using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Transcode.Encoding
{
    [Serializable]
    [XmlRoot("format")]
    public class EncodingFormat
    {
        public EncodingFormat()
        {
            overlays = new List<EncodingOverlay>();
            text_overlays = new List<EncodingTextOverlay>();
        } 

        [XmlElement(IsNullable = true)] 
        public string noise_reduction { get; set; }

        [XmlElement(IsNullable = true)]
        public string preset_file { get; set; }

        [XmlElement(IsNullable = true)] 
        public string output { get; set; }

        [XmlElement(IsNullable = true)] 
        public string video_codec { get; set; }

        [XmlElement(IsNullable = true)] 
        public string audio_codec { get; set; }

        [XmlElement(IsNullable = true)] 
        public string bitrate { get; set; }

        [XmlElement(IsNullable = true)] 
        public string audio_bitrate { get; set; }

        [XmlElement(IsNullable = true)] 
        public string audio_sample_rate { get; set; }

        [XmlElement(IsNullable = true)] 
        public string audio_channels_number { get; set; }

        [XmlElement(IsNullable = true)] 
        public string audio_volume { get; set; }

        [XmlElement(IsNullable = true)] 
        public string framerate { get; set; }

        [XmlElement(IsNullable = true)] 
        public string framerate_upper_threshold { get; set; }

        [XmlElement(IsNullable = true)] 
        public string  size { get; set; }

        [XmlElement(IsNullable = true)] 
        public string fade_in { get; set; }

        [XmlElement(IsNullable = true)] 
        public string fade_out  { get; set; }

        [XmlElement(IsNullable = true)] 
        public string crop_left { get; set; }
        
        [XmlElement(IsNullable = true)] 
        public string crop_top { get; set; }

        [XmlElement(IsNullable = true)] 
        public string  crop_right { get; set; }

        [XmlElement(IsNullable = true)] 
        public string  crop_bottom { get; set; }

        [XmlElement(IsNullable = true)] 
        public string keep_aspect_ratio { get; set; }

        [XmlElement(IsNullable = true)] 
        public string set_aspect_ratio { get; set; }

        [XmlElement(IsNullable = true)] 
        public string add_meta { get; set; }

        [XmlElement(IsNullable=true)] 
        public string hint { get; set; }

        [XmlElement(IsNullable = true)] 
        public string rc_init_occupancy { get; set; }

        [XmlElement(IsNullable = true)] 
        public string minrate { get; set; }

        [XmlElement(IsNullable = true)] 
        public string maxrate { get; set; }

        [XmlElement(IsNullable = true)] 
        public string bufsize { get; set; }

        [XmlElement(IsNullable = true)] 
        public string keyframe { get; set; }

        [XmlElement(IsNullable = true)] 
        public string start { get; set; }

        [XmlElement(IsNullable = true)] 
        public string duration { get; set; }

        [XmlElement(IsNullable = true)] 
        public string force_keyframe { get; set; }

        [XmlElement(IsNullable = true)] 
        public string gop { get; set; }

        [XmlElement(IsNullable = true)]
        public string profile { get; set; }

        [XmlElement(IsNullable = true)]
        public string audio_sync { get; set; }

        [XmlElement(IsNullable = true)]
        public string force_interlaced { get; set; }

        [XmlElement(IsNullable = true)]
        public string destination { get; set; }

        [XmlElement(IsNullable = true)]
        public string file_extension { get; set; }

        [XmlElement("overlay")]
        public List<EncodingOverlay> overlays;

        [XmlElement("text_overlay")]
        public List<EncodingTextOverlay> text_overlays;

        public static EncodingFormat CreateEncodingFormat(string Path)
        {
            if (!File.Exists(Path))
                throw new FileNotFoundException();

            try
            {
                EncodingFormat format = null;
                XmlSerializer serializer = new XmlSerializer(typeof(EncodingFormat));

                StreamReader reader = new StreamReader(Path);
                format = (EncodingFormat)serializer.Deserialize(reader);

                return format;
            }
            catch (Exception e) { throw e; }
        }

    }
}
