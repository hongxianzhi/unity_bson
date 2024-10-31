using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class loadbson : MonoBehaviour
{
    static void intHandler(string name, JObject jsonObject)
    {
        //jsonObject[name] = LibBson.Int32Value;
    }
    static void longHandler(string name, JObject jsonObject)
    {
        //jsonObject[name] = LibBson.Int64Value;
    }
    static void doubleHandler(string name, JObject jsonObject)
    {
        //jsonObject[name] = LibBson.DoubleValue;
    }
    static void stringHandler(string name, JObject jsonObject)
    {
        //jsonObject[name] = LibBson.StringValue;
    }
    static void boolHandler(string name, JObject jsonObject)
    {
        //jsonObject[name] = LibBson.BooleanValue;
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
        //jsonObject[name] = BitConverter.ToString(LibBson.ObjectId);
    }

    static string Main(string bsonName, byte[] bsonData)
    {
        float t1 = Time.realtimeSinceStartup;
        List<JObject> stacks = new List<JObject>();
        JObject doc = new JObject();
        JObject currentJson = doc;
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
                        }
                        else
                        {
                            currentJson["hierarchy"] = LibBson.Hierarchy;
                            stacks.RemoveAt(stacks.Count - 1);
                            currentJson = stacks[stacks.Count - 1];
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
                        }
                        else
                        {
                            currentJson["hierarchy"] = LibBson.Hierarchy;
                            stacks.RemoveAt(stacks.Count - 1);
                            currentJson = stacks[stacks.Count - 1];
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Unknown type: " + type);
                    break;
            }
        });

        float t2 = Time.realtimeSinceStartup;
        string str = doc.ToString(Newtonsoft.Json.Formatting.Indented);
        float t3 = Time.realtimeSinceStartup;

        //在屏幕上输出 JSON 数据
        string message = "解析时间：" + (t2 - t1) + "s" + " 生成 JSON 时间：" + (t3 - t2) + "s";
        Debug.Log(message);
        return message;
    }

    string message = "";
    void Start()
    {
        //从Assets/test.bytes中读取二进制数据
        TextAsset binAsset = Resources.Load("RaidConfigCategory") as TextAsset;
        message = Main("RaidConfigCategory", binAsset.bytes);
    }

    void OnGUI()
    {
        //在屏幕上输出 JSON 数据
        GUI.Label(new Rect(0, 0, 1000, 1000), message);
    }
}
