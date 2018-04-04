using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class FloorDataInEditor
{
    //public FloorType floorType;
    public float chance;

    public int minFloorCount;
    public int maxFloorCount;

    public float minFloorTurningAngle;
    public float maxFloorTurningAngle;

	public int minCoinCount;
	public int maxCoinCount;

    public ObstacleType obstacleType;
    public int minObstacleCount;
    public int maxObstacleCount;

    public float floorWidth;
    //public int minCoinCount;
    //public int maxCoinCount;

    //public int coinStartIndex;

}

[System.Serializable]
public class FloorDataBeforeScore
{
    public float activateScore;
    public float multiplier = 1.0f;
    public bool useGlideTransition;
    public float redCoinChance = 0.01f;
    public float m_Timescale = 1.0f;
    public float m_Width = 15.0f;
}

public class FloorDataConfig  {

    public static FloorDataBeforeScore[] data;

    public void Init () {
        //data = new List<FloorDataBeforeScore>();
       // data = new FloorDataBeforeScore[];
        /*string xmlPath = "Assets/Resources/XML/Difficulty.xml";
        XmlDocument doc = XMLUtil.loadXML(xmlPath);

        var serializer = new XmlSerializer(typeof(FloorDataXMLContainer));
        //serializer.Deserialize(xml)
        var stream = new FileStream(xmlPath, FileMode.Open);
        FloorDataXMLContainer container = serializer.Deserialize(stream) as FloorDataXMLContainer;
        Debug.Log(container.data.Count);
        stream.Close();
        data = container.data.ToArray();

        /*if (doc == null)
        {
            Debug.Log("No difficulty in current scene");
            return;
        }

        XmlNodeList nodeList = doc.SelectSingleNode("ArrayOfFloorDataBeforeScore").ChildNodes;
        foreach (XmlElement item in nodeList)
        {
            FloorDataBeforeScore floordata = new FloorDataBeforeScore();
            VOUtil.MakeVO<FloorDataBeforeScore>(floordata, item);
            data.Add(floordata);
        }*/
    }
}
