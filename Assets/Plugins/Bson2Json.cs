using System;
using Newtonsoft.Json.Linq;
public static class Bson2Json
{
    public static void BsonHandler(LibBson bson)
    {
        int type = bson.EleType;
        string name = bson.RealEleName;
        switch (type)
        {
            case LibBson.BSON_DOCUMENT:
                {
                    JObject obj = new JObject();
                    JObject currentJson = bson.GetObject(-1) as JObject;
                    if(currentJson != null)
                    {
                        currentJson[name] = obj;
                    }
                    else
                    {
                        bson.SetUserData("document", obj);
                    }
                    bson.PushObject(obj);
                }
                break;
            case LibBson.BSON_INT32:
                {
                    (bson.GetObject(-1) as JObject)[name] = bson.Int32Value;
                }
                break;
            case LibBson.BSON_INT64:
                {
                    (bson.GetObject(-1) as JObject)[name] = bson.Int64Value;
                }
                break;
            case LibBson.BSON_DOUBLE:
                {
                    (bson.GetObject(-1) as JObject)[name] = bson.DoubleValue;
                }
                break;
            case LibBson.BSON_STRING:
                {
                    (bson.GetObject(-1) as JObject)[name] = bson.StringValue;
                }
                break;
            case LibBson.BSON_BOOLEAN:
                {
                    (bson.GetObject(-1) as JObject)[name] = bson.BooleanValue;
                }
                break;
            case LibBson.BSON_BINARY:
                {
                    JObject binaryObject = new JObject();
                    binaryObject["BinaryLength"] = bson.BinaryLength;
                    binaryObject["BinarySubtype"] = bson.BinarySubtype;
                    binaryObject["BinaryStart"] = bson.BinaryStart;
                    (bson.GetObject(-1) as JObject)[name] = binaryObject;
                }
                break;
            case LibBson.BSON_OBJECT_ID:
                {
                    (bson.GetObject(-1) as JObject)[name] = BitConverter.ToString(bson.ObjectId);
                }
                break;
            case LibBson.BSON_ARRAY:
                {
                    JObject obj = new JObject();
                    JObject currentJson = bson.GetObject(-1) as JObject;
                    if(currentJson != null)
                    {
                        currentJson[name] = obj;
                    }
                    bson.PushObject(obj);
                }
                break;
            default:
                Console.WriteLine("Unknown type: " + type);
                break;
        }
    }
}