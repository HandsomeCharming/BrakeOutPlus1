using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
public class XMLUtil
{

    public static XmlDocument loadXML(string path)
    {
        string filePath = Application.dataPath + @"/Resources/XML/" + path;
        TextAsset textAsset = new TextAsset();
        string realPath = "XML/" + path;
        realPath = realPath.Substring(0, realPath.Length - 4);
        //Debug.Log (realPath);
        textAsset = (TextAsset)Resources.Load(realPath, typeof(TextAsset));
        if (textAsset == null)
            return null;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        return xmlDoc;
    }

    public static XmlDocument CreateXML(string path)
    {
        string filePath = Application.dataPath + @"/Resources/XML/" + path;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Save(filePath);
        return xmlDoc;
    }

    public static void saveXML(XmlDocument doc, string path)
    {
        string filePath = Application.dataPath + @"/Resources/XML/" + path;

        doc.Save(filePath);
    }
}
