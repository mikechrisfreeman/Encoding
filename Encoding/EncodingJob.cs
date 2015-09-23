using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;

namespace Transcode.Encoding
{
    public class EncodingJob : TranscodeJobBase, ITranscodeJob
    {
        public int EncoderUserID;
        public string EncoderUserKey;
        public string EncoderAPIEndPoint;
        
        private EncodingQuery query;
        private List<EncodingFormat> formats = new List<EncodingFormat>();
        private List<EncodingOverlay> overlays = new List<EncodingOverlay>();
        private List<EncodingTextOverlay> text_overlays = new List<EncodingTextOverlay>();

        private string DestinationBase;
        private string Filename;
        private string mediaID;

        public EncodingJob()
        {

        }

        public EncodingJob(int encoderUserID, string encoderUserKey, string encoderAPIEndPoint)
        {
            this.query = new EncodingQuery();
            this.query.userid = this.EncoderUserID = encoderUserID;
            this.query.userkey = this.EncoderUserKey = encoderUserKey;
            this.query.region = "eu-west-1";

            this.EncoderAPIEndPoint = encoderAPIEndPoint;
        }

        public double TranscodingProgress // Percentage transferred of all files
        {           
            get;
            set;
        }

        public TranscodeStatus Status
        {
            get
            {
                return this.status;
            }
            set { }
        }

        /// <summary>
        /// Add a file to the transcode process - this is not a file, its a link to a file
        /// HTTP        : http://[user[:password]@]hostname[:port]/[path]/[filename]
        /// FTP/SFTP    : ftp://[user[:password]@]hostname[:port]/[path]/[filename][?passive=yes]
        /// Amazon      : http://[AWS_KEY:AWS_SECRET@][bucket].s3.amazonaws.com/[filename]
        /// ASPERA      : fasp://[user[:password]@]hostname[:port]/[path]/[filename]
        /// BLOB        : http(s)://[access_key]@[account].blob.core.windows.net/[container]/path
        /// </summary>
        /// <param name="file"></param>
        public void AddFile(string file)
        {
            this.query.source = file;
        }

        /// <summary>
        /// HTTP        : http://[user[:password]@]hostname[:port]/[path]/[filename]
        /// FTP/SFTP    : ftp://[user[:password]@]hostname[:port]/[path]/[filename][?passive=yes]
        /// Amazon      : http://[AWS_KEY:AWS_SECRET@][bucket].s3.amazonaws.com/[filename]
        /// ASPERA      : fasp://[user[:password]@]hostname[:port]/[path]/[filename]
        /// BLOB        : http(s)://[access_key]@[account].blob.core.windows.net/[container]/path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="position"></param>
        public void AddFile(string file, int position)
        {
            this.query.source = file;
        }

        /// <summary>
        /// Add a list of file to the transcode process
        /// the files would be appended in the ienumerable order
        /// </summary>
        /// <param name="files"></param>
        public void AddFiles(IEnumerable<string> files)
        {
            throw new NotImplementedException();
        }

        public void addMediaID(string id)
        {
            this.mediaID = id;
        }

        /// <summary>
        /// Destination File
        /// HTTP        : http://[user[:password]@]hostname[:port]/[path]/[filename]
        /// FTP/SFTP    : ftp://[user[:password]@]hostname[:port]/[path]/[filename]
        /// Amazon      : http://[AWS_KEY:AWS_SECRET@][bucket].s3.amazonaws.com/
        /// ASPERA      : fasp://[user[:password]@]hostname[:port]/[path]/[filename]
        /// BLOB        : http(s)://[access_key]@[account].blob.core.windows.net/[container]/path
        /// </summary>
        /// <param name="path"></param>
        public void SetOutputFolder(string path)
        {
            this.DestinationBase = path;
        }

        /// <summary>
        /// Set the output filename temaplte for the transcode
        /// If nothing is set a unique id should be used
        /// </summary>
        /// <param name="path"></param>
        public void SetOutputFilenameTemplate(string filename)
        {
            this.Filename = filename;
        }

        public void StartTranscode()
        {
            //We're sending a transcode job so we set the action of the query to AddMedia
            this.query.action = "AddMedia";

            //If the destination and filename is present override from template config
            this.updateDestination();

            //We override the query formats/presets if we've added our own
            if(this.formats.Count  > 0)
                this.query.formats = this.formats;

            try
            {             
                EncodingAddMediaResponse result = sendRequestDeserialiseResponse<EncodingAddMediaResponse>();

                //We Throw the errors if 
                if (result.errors != null && result.errors.errors.Count > 0)
                    throw new TranscodeException(string.Join(",", result.errors.errors.ToArray()));

                this.mediaID = result.MediaID;
            }
            catch (Exception e)
            {
                throw new TranscodeException("Problem in StartTranscode", e);
            }         

        }

        public void StartTranscode(int priority)
        {
            StartTranscode();
        }

        public bool PauseTranscode()
        {
            throw new NotImplementedException();
        }

        public bool ResumeTranscode()
        {
            throw new NotImplementedException();
        }

        public bool ResumeTranscode(int priority)
        {
            throw new NotImplementedException();
        }

        public bool CancelTranscode()
        {
            //We're sending a transcode job so we set the action of the query to AddMedia
            this.query.action = "cancelMedia";

            //We override the query formats/presets if we've added our own
            if (this.formats.Count > 0)
                this.query.formats = this.formats;

            try
            {
                EncodingResponse result = sendRequestDeserialiseResponse<EncodingResponse>();
                if (result.errors != null && result.errors.errors.Count > 0)
                    return false;
                else
                    return true;              
            }
            catch (Exception e)
            {
                throw new TranscodeException("Problem in StartTranscode", e);
            }   
        }

        public double GetTranscodeProgress()
        {
            double progress = 0.0;

            //We need to make sure that the query is a new query and not one preveously used for transcode.
            this.query = new EncodingQuery(this.EncoderUserID, this.EncoderUserKey);

            //We're getting the status of a job so we set the action of the query
            this.query.action = "GetStatus";

            //We need to add the media ID to the query
            this.query.mediaid = this.mediaID;

            EncodingGetStatusResponse result = null;

            try
            {
                result = sendRequestDeserialiseResponse<EncodingGetStatusResponse>();
                //Throw the errors if a problem an error has occurred.
                if (result.errors != null && result.errors.errors.Count > 0)
                    throw new TranscodeException(string.Join(",", result.errors.errors.ToArray()));

                this.transcodeProgress = progress = Convert.ToDouble(result.progress);
                switch (result.status.ToLower())
                {
                    case "new": status = TranscodeStatus.Queued; break;
                    case "waiting for encoder": status = TranscodeStatus.Queued; break;
                    case "ready to process": status = TranscodeStatus.Queued; break;
                    case "downloading": status = TranscodeStatus.Queued; break;
                    case "processing": status = TranscodeStatus.Active; break;
                    case "saving": status = TranscodeStatus.Active; break;
                    case "finished": status = TranscodeStatus.Completed; break;
                    case "error": status = TranscodeStatus.Error; break;
                }
            }
            catch (Exception e)
            {
                throw new TranscodeException("Problem in GetTranscodeProgress", e);
            }

            return progress;
        }

        private T sendRequestDeserialiseResponse<T>()
        {
            T result = default(T);

            string serializedQuery = this.getXMLRequestString();
            string sRequest = "xml=" + HttpUtility.UrlEncode(serializedQuery);

            //Sending the HttpRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.EncoderAPIEndPoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = sRequest.Length;
            request.GetRequestStream().Write(System.Text.Encoding.UTF8.GetBytes(sRequest), 0, sRequest.Length);
            request.GetRequestStream().Close();
            HttpWebResponse response = null;

            //Processing the response
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string res = sr.ReadToEnd();
                XmlSerializer serial = new XmlSerializer(typeof(T));
                result = (T)serial.Deserialize(new StringReader(res));
            }

            return result;
        }

        public List<string> GetTranscodeId()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This is a path to an xml file containing EncodingFormat information, it must be accepted an xml by Encoding.com
        /// </summary>
        /// <param name="path"></param>
        public void AddPresetSetting(string path)
        {
            if (File.Exists(path))
            {
                EncodingFormat format = EncodingFormat.CreateEncodingFormat(path);
                if (format != null)
                    formats.Add(format);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }


        /// <summary>
        /// The carbon preset needs to be a valid Carbon preset file that is accessible from the link
        /// HTTP        : http://[user[:password]@]hostname[:port]/[path]/[filename]
        /// FTP/SFTP    : ftp://[user[:password]@]hostname[:port]/[path]/[filename]
        /// Amazon      : http://[AWS_KEY:AWS_SECRET@][bucket].s3.amazonaws.com/
        /// ASPERA      : fasp://[user[:password]@]hostname[:port]/[path]/[filename]
        /// BLOB        : http(s)://[access_key]@[account].blob.core.windows.net/[container]/path
        /// </summary>
        /// <param name="path"></param>
        public void AddCarbonPreset(string path)
        {
            EncodingFormat format = new EncodingFormat()
            {
                preset_file = path,
                output = "harmonic"
            };
            this.formats.Add(format);
        }



        /// <summary>
        /// Add encoding preset to this transcoding
        /// </summary>
        /// <param name="path"></param>
        public void AddPresetSetting(System.Xml.Linq.XElement xpreset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add encoding preset to this transcoding
        /// </summary>
        /// <param name="path"></param>
        public void AddPresetSetting(ITranscodePreset preset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the transcoding profile to use with the workflow/preset Id
        /// </summary>
        /// <param name="presetId"></param>
        public void SetProfileById(string presetId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the encoding profile parameters for the current setting
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public void SetProfileParams(IDictionary<string, string> param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// add a keyframe that we want to extract the poster from
        /// </summary>
        /// <param name="timecode">keyframe format must be HH:MM:ss:ff</param>
        public void AddThumbnailKeyFrame(ITNCore.MediaUtils.Timecode timecode)
        {
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// add a video thumbnail to be extracted from the video
        /// </summary>
        /// <param name="thummbnail">thumbnail setting that we wnat ot create (timecode, </param>
        public void AddThumbnailKeyFrame(PresetThumbnail thummbnail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a filter to the transcode process (cropping, video segment, ticker, subtitles, ....)
        /// </summary>
        /// <param name="filter"></param>
        public void AddFilter(ITranscodeFilter filter)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Set on which server the job is allowed to be processed
        /// </summary>
        /// <param name="serverName"></param>
        public void AddServerNameRestriction(string serverName)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Set on which farm (group of server) the job is allowed to be processed
        /// </summary>
        /// <param name="serverName"></param>
        public void AddFarmNameRestriction(string farmName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a media item representing an object in a particular system
        /// </summary>
        /// <param name="item"></param>
        public void AddMediaItem(ITNCore.MediaRepositoryBusinessObjects.MediaItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Due to the way encoding.com handle the requests - all requests need to be serialised with an xml Prefix;
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public string getXMLRequestString()
        {
            try
            {
                XmlSerializer serial = new XmlSerializer(typeof(EncodingQuery));
                MemoryStream stream = new MemoryStream();
                serial.Serialize(stream, this.query);
                stream.Position = 0;
                var sr = new StreamReader(stream);
                string result = sr.ReadToEnd();
                return result;
            }
            catch (Exception e)
            {
                throw new TranscodeException("Problem Serialising request", e);
            }
        }

        public void updateDestination()
        {
            foreach (EncodingFormat format in formats)
            {
                if (!String.IsNullOrEmpty(DestinationBase))
                {
                    format.destination = DestinationBase;
                }

                if (!String.IsNullOrEmpty(Filename))
                {
                    int index = format.destination.LastIndexOf('/');
                    if (index == format.destination.Length - 1)                    
                        format.destination = format.destination + Filename;                    
                    else                    
                        format.destination = format.destination + "/" + Filename;

                    //If a file extenstion has been specified in the preset
                    if (format.file_extension != null)
                        format.destination = format.destination + "." + format.file_extension;
                }
            }
        }

        public List<string> GetDestinations()
        {
            List<string> dests = new List<string>();
            formats.ForEach(f => dests.Add(f.destination));
            return dests;           
        }
    }
}
