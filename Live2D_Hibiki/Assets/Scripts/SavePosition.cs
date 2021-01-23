using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using UnityEngine;

public class SavePosition : MonoBehaviour
{
    private static float defaultPositionX = 4.2f;
    private static float defaultPositionY = -0.27f;
    private static float defaultPositionZ = -1.4f;
    private Vector3 defaultPosition;
    public AudioSource HibikiAS;
    public UISlider VolumeSLI;
    // Use this for initialization
    void Start()
    {
        defaultPosition = new Vector3(defaultPositionX, defaultPositionY, defaultPositionZ);
        string filePathD = Application.dataPath + @"/Position.xml";
        if (File.Exists(filePathD))
        {
            File.Delete(filePathD);
        }
        string filePath = Application.dataPath + @"/SetPositionAndVolume.xml";
        if (!File.Exists(filePath))
        {
            createXml();
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Position").ChildNodes;
            string valueX = nodeList[0].InnerText;
            string valueY = nodeList[1].InnerText;
            string valueZ = nodeList[2].InnerText;
            transform.position = new Vector3(Convert.ToSingle(valueX), Convert.ToSingle(valueY), Convert.ToSingle(valueZ));

            string volume = nodeList[3].InnerText;
            //Debug.Log(volume);
            VolumeSLI.value = Convert.ToSingle(volume);
        }
    }

    public void SavePositionFunc()
    {
        string filePath = Application.dataPath + @"/SetPositionAndVolume.xml";

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("Position");

        XmlElement elmNewX = xmlDoc.CreateElement("setPositionX");
        elmNewX.SetAttribute("id", "0");
        elmNewX.SetAttribute("name", "playerPositionX");
        XmlElement valueX = xmlDoc.CreateElement("valueX");
        valueX.InnerText = transform.position.x.ToString();
        elmNewX.AppendChild(valueX);
        root.AppendChild(elmNewX);

        XmlElement elmNewY = xmlDoc.CreateElement("setPositionY");
        elmNewY.SetAttribute("id", "1");
        elmNewY.SetAttribute("name", "playerPositionY");
        XmlElement valueY = xmlDoc.CreateElement("valueY");
        valueY.InnerText = transform.position.y.ToString();
        elmNewY.AppendChild(valueY);
        root.AppendChild(elmNewY);

        XmlElement elmNewZ = xmlDoc.CreateElement("setPositionZ");
        elmNewZ.SetAttribute("id", "2");
        elmNewZ.SetAttribute("name", "playerPositionZ");
        XmlElement valueZ = xmlDoc.CreateElement("valueZ");
        valueZ.InnerText = transform.position.z.ToString();
        elmNewZ.AppendChild(valueZ);
        root.AppendChild(elmNewZ);

        XmlElement elmNewVolume = xmlDoc.CreateElement("setVolume");
        elmNewVolume.SetAttribute("id", "3");
        elmNewVolume.SetAttribute("name", "VolumeValue");
        XmlElement valueVolume = xmlDoc.CreateElement("Volume");
        valueVolume.InnerText = HibikiAS.volume.ToString();
        elmNewVolume.AppendChild(valueVolume);
        root.AppendChild(elmNewVolume);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        //Debug.Log("createXml OK!");
    }

    public void ResetPositionFunc()
    {
        transform.position = defaultPosition;
        VolumeSLI.value = 1.0f;

        string filePath = Application.dataPath + @"/SetPositionAndVolume.xml";

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("Position");

        XmlElement elmNewX = xmlDoc.CreateElement("setPositionX");
        elmNewX.SetAttribute("id", "0");
        elmNewX.SetAttribute("name", "playerPositionX");
        XmlElement valueX = xmlDoc.CreateElement("valueX");
        valueX.InnerText = defaultPositionX.ToString();
        elmNewX.AppendChild(valueX);
        root.AppendChild(elmNewX);

        XmlElement elmNewY = xmlDoc.CreateElement("setPositionY");
        elmNewY.SetAttribute("id", "1");
        elmNewY.SetAttribute("name", "playerPositionY");
        XmlElement valueY = xmlDoc.CreateElement("valueY");
        valueY.InnerText = defaultPositionY.ToString();
        elmNewY.AppendChild(valueY);
        root.AppendChild(elmNewY);

        XmlElement elmNewZ = xmlDoc.CreateElement("setPositionZ");
        elmNewZ.SetAttribute("id", "2");
        elmNewZ.SetAttribute("name", "playerPositionZ");
        XmlElement valueZ = xmlDoc.CreateElement("valueZ");
        valueZ.InnerText = defaultPositionZ.ToString();
        elmNewZ.AppendChild(valueZ);
        root.AppendChild(elmNewZ);

        XmlElement elmNewVolume = xmlDoc.CreateElement("setVolume");
        elmNewVolume.SetAttribute("id", "3");
        elmNewVolume.SetAttribute("name", "VolumeValue");
        XmlElement valueVolume = xmlDoc.CreateElement("Volume");
        valueVolume.InnerText = 1.0f.ToString();
        elmNewVolume.AppendChild(valueVolume);
        root.AppendChild(elmNewVolume);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);
    }

    void createXml()
    {
        string filePath = Application.dataPath + @"/SetPositionAndVolume.xml";
        if (!File.Exists(filePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("Position");

            XmlElement elmNewX = xmlDoc.CreateElement("setPositionX");
            elmNewX.SetAttribute("id", "0");
            elmNewX.SetAttribute("name", "playerPositionX");
            XmlElement valueX = xmlDoc.CreateElement("valueX");
            valueX.InnerText = defaultPositionX.ToString();
            elmNewX.AppendChild(valueX);
            root.AppendChild(elmNewX);

            XmlElement elmNewY = xmlDoc.CreateElement("setPositionY");
            elmNewY.SetAttribute("id", "1");
            elmNewY.SetAttribute("name", "playerPositionY");
            XmlElement valueY = xmlDoc.CreateElement("valueY");
            valueY.InnerText = defaultPositionY.ToString();
            elmNewY.AppendChild(valueY);
            root.AppendChild(elmNewY);

            XmlElement elmNewZ = xmlDoc.CreateElement("setPositionZ");
            elmNewZ.SetAttribute("id", "2");
            elmNewZ.SetAttribute("name", "playerPositionZ");
            XmlElement valueZ = xmlDoc.CreateElement("valueZ");
            valueZ.InnerText = defaultPositionZ.ToString();
            elmNewZ.AppendChild(valueZ);
            root.AppendChild(elmNewZ);

            XmlElement elmNewVolume = xmlDoc.CreateElement("setVolume");
            elmNewVolume.SetAttribute("id", "3");
            elmNewVolume.SetAttribute("name", "VolumeValue");
            XmlElement valueVolume = xmlDoc.CreateElement("Volume");
            valueVolume.InnerText = 1.0f.ToString();
            elmNewVolume.AppendChild(valueVolume);
            root.AppendChild(elmNewVolume);

            xmlDoc.AppendChild(root);
            xmlDoc.Save(filePath);
        }
    }
}
