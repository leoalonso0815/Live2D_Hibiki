using UnityEngine;
using System.Collections;
using System.Xml;

public class SetXML : MonoBehaviour
{
	public string xmlName = "unName";
	public int atk = -1;

	void Update ()
	{
		string path = Application.dataPath + "/Resources/" + xmlName + ".xml";

		if (Input.GetKeyDown (KeyCode.K)) {

			XmlDocument xml = new XmlDocument ();
			XmlElement root = xml.CreateElement ("playerList");
			root.SetAttribute ("id", "666");
			root.SetAttribute ("value", "head");

			XmlElement aaa = xml.CreateElement ("player1");
			aaa.SetAttribute ("atk", "80");
			aaa.SetAttribute ("def", "30");
			aaa.SetAttribute ("health", "50");//覆盖属性
			root.AppendChild (aaa);

			XmlElement bbb = xml.CreateElement ("player2");
			bbb.SetAttribute ("atk", "80");
			bbb.SetAttribute ("def", "30");
			bbb.SetAttribute ("health", "50");//覆盖属性
			root.AppendChild (bbb);

			xml.AppendChild (root);

			xml.Save (path);//IO
			Debug.Log ("保存成功！");
		}

		if (Input.GetKeyDown (KeyCode.L)) {
			XmlDocument xml = new XmlDocument ();
			xml.Load (path);//IO

			int aaa;
			if (int.TryParse ("100.86", out aaa)) {
				atk = aaa;
			}

			XmlNodeList myNode = xml.SelectSingleNode ("playerList").ChildNodes;
			foreach (XmlElement item in myNode) {
				if (item.Name == "player1") {
					//atk = int.Parse (item.GetAttribute ("atk"));
				}
				if (item.GetAttribute ("atk") == "80") {
					Debug.Log (item.GetAttribute ("health"));
				}
			}
		}
	}
}
