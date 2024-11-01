using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class loadbson : MonoBehaviour
{
    static void Main(string bsonName, byte[] bsonData)
    {
        //当前内存占用
        long startMemory = GC.GetTotalMemory(true);
        float t1 = Time.realtimeSinceStartup;

        // 解析 BSON 数据
        LibBson bson = null;
        if(useDispatch)
        {
            bson = bsonDispatch.CreateBson(bsonName, bsonData);
        }
        else
        {
            bson = new LibBson(bsonName, bsonData, Bson2Json.BsonHandler);
        }
        bson.Decode();

        JObject doc = bson.CurrentDocument as JObject;
        float t2 = Time.realtimeSinceStartup;
        long endMemory = GC.GetTotalMemory(true);
        string str = doc.ToString(Newtonsoft.Json.Formatting.Indented);
        float t3 = Time.realtimeSinceStartup;

        //File.WriteAllText("D:/" + bsonName + ".csv", stringBuilder.ToString());
        //溶剂内存占用
        long end1Memory = GC.GetTotalMemory(true);
#if UNITY_EDITOR
        File.WriteAllText(string.Format("{0}/Json/{1}.json", Application.dataPath, bsonName), str);
#endif

        //在屏幕上输出 JSON 数据
        message += string.Format("{0} Parse:{1}ms, Convert:{2}ms, mem1:{3}KB, mem2:{4}KB\n", bsonName, (t2 - t1) * 1000, (t3 - t2) * 1000, (endMemory - startMemory) / 1024, (end1Memory - endMemory) / 1024);
    }

    static string message = "";
    static bool useDispatch = false;
    GUIStyle labelStype = new GUIStyle();
    static BsonDispatch bsonDispatch = null;
    void Start()
    {
        //从Assets/test.bytes中读取二进制数据
        labelStype.fontSize = 25;
        Application.targetFrameRate = 30;
        bsonDispatch = new BsonDispatch(Bson2CSharpRoute.GetHandlers());
    }

    void OnGUI()
    {
        //在屏幕上输出 JSON 数据
        GUI.Label(new Rect(0, 0, 1000, 1000), message, labelStype);

        if(GUI.Button(new Rect(100, 400, 100, 100), useDispatch ? "dispatch" : "parser"))
        {
            useDispatch = !useDispatch;
        }
        if(GUI.Button(new Rect(300, 400, 100, 100), "Click Me"))
        {
            message = "";
            TextAsset binAsset = Resources.Load("SensitiveWordConfigCategory") as TextAsset;
            Main("SensitiveWordConfigCategory", binAsset.bytes);
            /*
            binAsset = Resources.Load("RaidConfigCategory") as TextAsset;
            Main("RaidConfigCategory", binAsset.bytes);
            binAsset = Resources.Load("RobotConfigCategory") as TextAsset;
            Main("RobotConfigCategory", binAsset.bytes);
            */
        }
    }
}
