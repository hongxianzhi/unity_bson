using System;
using Newtonsoft.Json.Linq;
public static class Bson2Json
{
    public static void BsonHandler(LibBson bson, int type)
    {
        string name = bson.EleName;
        switch (type)
        {
            case LibBson.BSON_DOCUMENT:
                {
                    if (bson.IsEnter)
                    {
                        JObject obj = new JObject();
                        JObject currentJson = bson.CurrentDocument as JObject;
                        if(currentJson != null)
                        {
                            currentJson[name] = obj;
                        }
                        bson.PushDocument(obj);
                    }
                    else
                    {
                        if(!bson.IsDocumentRoot)
                        {
                            bson.PopDocument();
                        }
                    }
                }
                break;
            case LibBson.BSON_INT32:
                {
                    (bson.CurrentDocument as JObject)[name] = bson.Int32Value;
                }
                break;
            case LibBson.BSON_INT64:
                {
                    (bson.CurrentDocument as JObject)[name] = bson.Int64Value;
                }
                break;
            case LibBson.BSON_DOUBLE:
                {
                    (bson.CurrentDocument as JObject)[name] = bson.DoubleValue;
                }
                break;
            case LibBson.BSON_STRING:
                {
                    (bson.CurrentDocument as JObject)[name] = bson.StringValue;
                }
                break;
            case LibBson.BSON_BOOLEAN:
                {
                    (bson.CurrentDocument as JObject)[name] = bson.BooleanValue;
                }
                break;
            case LibBson.BSON_BINARY:
                {
                    JObject binaryObject = new JObject();
                    binaryObject["BinaryLength"] = bson.BinaryLength;
                    binaryObject["BinarySubtype"] = bson.BinarySubtype;
                    binaryObject["BinaryStart"] = bson.BinaryStart;
                    (bson.CurrentDocument as JObject)[name] = binaryObject;
                }
                break;
            case LibBson.BSON_OBJECT_ID:
                {
                    (bson.CurrentDocument as JObject)[name] = BitConverter.ToString(bson.ObjectId);
                }
                break;
            case LibBson.BSON_ARRAY:
                {
                    if(bson.IsEnter)
                    {
                        JObject obj = new JObject();
                        JObject currentJson = bson.CurrentDocument as JObject;
                        if(currentJson != null)
                        {
                            currentJson[name] = obj;
                        }
                        bson.PushDocument(obj);
                    }
                    else
                    {
                        if(!bson.IsDocumentRoot)
                        {
                            bson.PopDocument();
                        }
                    }
                }
                break;
            default:
                Console.WriteLine("Unknown type: " + type);
                break;
        }
    }
}