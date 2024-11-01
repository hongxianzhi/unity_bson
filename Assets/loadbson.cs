using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

public class loadbson : MonoBehaviour
{
    static void intHandler(string name, JObject jsonObject)
    {
        int value = LibBson.Int32Value;
        if(fillJson)
        {
            jsonObject[name] = value;
        }
    }
    static void longHandler(string name, JObject jsonObject)
    {
        long value = LibBson.Int64Value;
        if(fillJson)
        {
            jsonObject[name] = value;
        }
    }
    static void doubleHandler(string name, JObject jsonObject)
    {
        double value = LibBson.DoubleValue;
        if(fillJson)
        {
            jsonObject[name] = value;
        }
    }
    static void stringHandler(string name, JObject jsonObject)
    {
        string value = LibBson.StringValue;
        if(fillJson)
        {
            jsonObject[name] = value;
        }
    }
    static void boolHandler(string name, JObject jsonObject)
    {
        bool value = LibBson.BooleanValue;
        if(fillJson)
        {
            jsonObject[name] = value;
        }
    }
    static void binaryHandler(string name, JObject jsonObject)
    {
        JObject binaryObject = new JObject();
        binaryObject["BinaryLength"] = LibBson.BinaryLength;
        binaryObject["BinarySubtype"] = LibBson.BinarySubtype;
        binaryObject["BinaryStart"] = LibBson.BinaryStart;
        jsonObject[name] = binaryObject;
    }
    static void objectIdHandler(string name, JObject jsonObject)
    {
        jsonObject[name] = BitConverter.ToString(LibBson.ObjectId);
    }

    static string Main(string bsonName, byte[] bsonData)
    {
        //当前内存占用
        long startMemory = GC.GetTotalMemory(true);
        float t1 = Time.realtimeSinceStartup;
        List<JObject> stacks = new List<JObject>();
        JObject doc = new JObject();
        JObject currentJson = doc;
        stacks.Add(doc);
        // 解析 BSON 数据
        LibBson.Decode(bsonName, bsonData, delegate(string name, int type)
        {
            switch (type)
            {
                case LibBson.BSON_DOCUMENT:
                    {
                        if(LibBson.IsDocumentRoot)
                        {
                            return;
                        }

                        if (LibBson.IsBeginParse)
                        {
                            JObject obj = new JObject();
                            currentJson[name] = obj;
                            currentJson = obj;
                            stacks.Add(obj);

                            //stringBuilder.AppendFormat("OA,{0},{1}\n", name, stacks.Count);
                        }
                        else
                        {
                            currentJson["hierarchy"] = LibBson.Hierarchy;
                            stacks.RemoveAt(stacks.Count - 1);
                            currentJson = stacks[stacks.Count - 1];
                            //stringBuilder.AppendFormat("OR,{0},{1}\n", name, stacks.Count);
                        }
                    }
                    break;
                case LibBson.BSON_INT32:
                    {
                        intHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_INT64:
                    {
                        longHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_DOUBLE:
                    {
                        doubleHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_STRING:
                    {
                        stringHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_BOOLEAN:
                    {
                        boolHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_BINARY:
                    {
                        binaryHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_OBJECT_ID:
                    {
                        objectIdHandler(name, currentJson);
                    }
                    break;
                case LibBson.BSON_ARRAY:
                    {
                        if(LibBson.IsBeginParse)
                        {
                            JObject obj = new JObject();
                            currentJson[name] = obj;
                            currentJson = obj;
                            stacks.Add(obj);

                            //stringBuilder.AppendFormat("AA,{0},{1}\n", name, stacks.Count);
                        }
                        else
                        {
                            currentJson["hierarchy"] = LibBson.Hierarchy;
                            stacks.RemoveAt(stacks.Count - 1);
                            currentJson = stacks[stacks.Count - 1];
                            //stringBuilder.AppendFormat("AR,{0},{1}\n", name, stacks.Count);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Unknown type: " + type);
                    break;
            }
        });

        float t2 = Time.realtimeSinceStartup;
        long endMemory = GC.GetTotalMemory(true);
        string str = doc.ToString(Newtonsoft.Json.Formatting.Indented);
        float t3 = Time.realtimeSinceStartup;

        //File.WriteAllText("D:/" + bsonName + ".csv", stringBuilder.ToString());
        //溶剂内存占用
        long end1Memory = GC.GetTotalMemory(true);

        //在屏幕上输出 JSON 数据
        string message = string.Format("解析时间:{0}ms, 转换时间:{1}ms, 内存增长:{2}KB, 内存增长:{3}KB", (t2 - t1) * 1000, (t3 - t2) * 1000, (endMemory - startMemory) / 1024, (end1Memory - endMemory) / 1024);
        Debug.Log(message);
        return message;
    }

    string message = "";
    static bool fillJson = false;
    GUIStyle labelStype = new GUIStyle();
    void Start()
    {
        //从Assets/test.bytes中读取二进制数据
        labelStype.fontSize = 25;
    }

    void OnGUI()
    {
        //在屏幕上输出 JSON 数据
        GUI.Label(new Rect(0, 0, 1000, 1000), message, labelStype);

        if(GUI.Button(new Rect(100, 400, 100, 100), fillJson ? "Fill Json" : "Not Fill Json"))
        {
            fillJson = !fillJson;
        }
        if(GUI.Button(new Rect(300, 400, 100, 100), "Click Me"))
        {
            TextAsset binAsset = Resources.Load("RaidConfigCategory") as TextAsset;
            message = Main("RaidConfigCategory", binAsset.bytes);
        }
    }
}
