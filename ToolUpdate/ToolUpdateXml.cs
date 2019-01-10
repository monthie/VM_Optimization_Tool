using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace ToolUpdate
{
    /// <summary>
    /// Contains update information
    /// </summary>
    public class ToolUpdateXml
    {
        /// <summary>
        /// The update version #
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// The location of the update binary
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// The file path of the binary
        /// for use on local computer
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// The MD5 of the update's binary
        /// </summary>
        public string MD5 { get; }

        /// <summary>
        /// The update's description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The arguments to pass to the updated application on startup
        /// </summary>
        public string LaunchArgs { get; }

        /// <summary>
        /// Creates a new ToolUpdateXml object
        /// </summary>
        public ToolUpdateXml(Version version, Uri uri, string filePath, string md5, string description, string launchArgs)
        {
            Version = version;
            Uri = uri;
            FilePath = filePath;
            MD5 = md5;
            Description = description;
            LaunchArgs = launchArgs;

        }

        /// <summary>
        /// Checks if update's version is newer than the old version
        /// </summary>
        /// <param name="version">Application's current version</param>
        /// <returns>If the update's version # is newer</returns>
        public bool IsNewerThan(Version version)
        {
            return Version > version;
        }

        /// <summary>
        /// Checks the Uri to make sure file exist
        /// </summary>
        /// <param name="location">The Uri of the update.xml</param>
        /// <returns>If the file exists</returns>
        public static bool ExistsOnServer(Uri location)
        {
            if (location.ToString().StartsWith("file"))
            {
                return System.IO.File.Exists(location.LocalPath);
            }
            else
            {
                try
                {
                    // Request the update.xml
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(location.AbsoluteUri);
                    // Read for response
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    resp.Close();

                    return resp.StatusCode == HttpStatusCode.OK;
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// Parses the update.xml into ToolUpdateXml object
        /// </summary>
        /// <param name="location">Uri of update.xml on server</param>
        /// <returns>The ToolUpdateXml object with the data, or null of any errors</returns>
        public static ToolUpdateXml[] Parse(Uri location)
        {
            List<ToolUpdateXml> result = new List<ToolUpdateXml>();
            Version version = null;
            string url = "", filePath = "", md5 = "", description = "", launchArgs = "";

            try
            {
                // Load the document
                //ServicePointManager.ServerCertificateValidationCallback = (s, ce, ch, ssl) => true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback((s, ce, ch, ssl) => true);


                XmlDocument doc = new XmlDocument();
                doc.Load(location.AbsoluteUri);

                // Gets the appId's node with the update info
                // This allows you to store all program's update nodes in one file
                // XmlNode updateNode = doc.DocumentElement.SelectSingleNode("//update[@appID='" + appID + "']");
                XmlNodeList updateNodes = doc.DocumentElement.SelectNodes("/VM_Optimization_Tool/update");
                foreach (XmlNode updateNode in updateNodes)
                {
                    // If the node doesn't exist, there is no update
                    if (updateNode == null)
                        return null;

                    // Parse data
                    version = Version.Parse(updateNode["version"].InnerText);
                    url = updateNode["url"].InnerText;
                    filePath = updateNode["filePath"].InnerText;
                    md5 = updateNode["md5"].InnerText;
                    description = updateNode["description"].InnerText;
                    launchArgs = updateNode["launchArgs"].InnerText;

                    result.Add(new ToolUpdateXml(version, new Uri(url), filePath, md5, description, launchArgs));
                }
                return result.ToArray();
            }
            catch { return null; }
        }
    }
}