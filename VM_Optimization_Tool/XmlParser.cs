using System;
using System.Collections.Generic;
using System.Xml;


namespace VM_Optimization_Tool
{

    /// <summary>
    /// Parse xml files like bwlp_Windows10.xml
    /// </summary>
    public class XmlParser
    {
        public string Type { get; }
        public string Command { get; }
        public List<string> Params { get; }


        public XmlParser(string type, string command, List<string> param)
        {
            Type = type;
            Command = command;
            Params = new List<string>(param);
        }

        public static XmlParser[] Parser(Uri location)
        {
            List<XmlParser> result = new List<XmlParser>();
            string type = "", command = "";
            List<string> param = new List<string>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(location.AbsoluteUri);
                XmlNodeList firstLevel = doc.DocumentElement.SelectNodes("/sequence/group/group");
                foreach (XmlNode secoundLevel in firstLevel)
                {
                    foreach(XmlNode thirdLevel in secoundLevel.SelectNodes("./step[@defaultSelected='true']/action"))
                    {
                        type = thirdLevel["type"].InnerText;
                        
                        if(type == "Registry")
                        {
                            command = thirdLevel["command"].InnerText;
                            if (command == "LOAD")
                            {
                                foreach (XmlNode fourthLevel in thirdLevel.SelectNodes("./params"))
                                {
                                    param.Add(fourthLevel["keyName"].InnerText);
                                    param.Add(fourthLevel["fileName"].InnerText);
                                }
                            }
                            else if (command == "UNLOAD")
                            {
                                foreach (XmlNode fourthLevel in thirdLevel.SelectNodes("./params"))
                                {
                                    param.Add(fourthLevel["keyName"].InnerText);
                                }
                            }
                            else if(command == "ADD")
                            {
                                foreach(XmlNode fourthLevel in thirdLevel.SelectNodes("./params"))
                                {
                                    if (fourthLevel.ChildNodes.Count == 1)
                                    {
                                        param.Add(fourthLevel["keyName"].InnerText);
                                    } else
                                    {
                                        param.Add(fourthLevel["keyName"].InnerText);
                                        param.Add(fourthLevel["valueName"].InnerText);
                                        param.Add(fourthLevel["type"].InnerText);
                                        param.Add(fourthLevel["data"].InnerText);
                                    }
                                }
                            }
                        } 
                        else if(type == "ShellExecute")
                        {
                            command = thirdLevel["command"].InnerText;
                        }
                        else if(type == "Service")
                        {
                            foreach (XmlNode fourthLevel in thirdLevel.SelectNodes("./params"))
                            {
                                param.Add(fourthLevel["serviceName"].InnerText);
                                param.Add(fourthLevel["startMode"].InnerText);
                            }
                        }
                        else if(type == "SchTasks")
                        {
                            foreach (XmlNode fourthLevel in thirdLevel.SelectNodes("./params"))
                            {
                                param.Add(fourthLevel["taskName"].InnerText);
                                param.Add(fourthLevel["status"].InnerText);
                            }
                        }
                        result.Add(new XmlParser(type, command, param));
                        param.Clear();
                        command = "";
                        type = "";
                    }            
                }
                return result.ToArray();
            }
            catch
            {
                return null;
            }       
        }
    }
}
