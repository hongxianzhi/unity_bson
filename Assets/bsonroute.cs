using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class Bson2CSharpRoute
{
    //SensitiveWordConfigCategory
    static RouteNode SensitiveWordConfigCategory = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {"SensitiveWordConfigCategory", delegate(LibBson bson)
        {
            JObject obj = new JObject();
            bson.SetUserData("document", obj);
            bson.PushObject(obj);
        }},
        {"_t", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["_t"] = bson.StringValue;
        }},
        {"list", delegate(LibBson bson)
        {
            JArray list = new JArray();
            ((JObject)bson.GetObject(-1))["list"] = list;
            bson.PushObject(list);
            bson.Router = SensitiveWordConfigCategory_list.Route;
        }},
    });

    //SensitiveWordConfigCategory.list
    static RouteNode SensitiveWordConfigCategory_list = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            JObject obj = new JObject();
            ((JArray)bson.GetObject(-1)).Add(obj);
            bson.PushObject(obj);
            bson.Router = SensitiveWordConfigCategory_list_item_.Route;
        }},
    });

    //SensitiveWordConfigCategory.list.$_ITEM_
    static RouteNode SensitiveWordConfigCategory_list_item_ = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {"_id", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["_id"] = bson.Int32Value;
        }},
        {"Mask", delegate(LibBson bson)
        {
            JArray mask = new JArray();
            ((JObject)bson.GetObject(-1))["Mask"] = mask;
            bson.PushObject(mask);
            bson.Router = SensitiveWordConfigCategory_list_item_Mask.Route;
        }},
    });

    //SensitiveWordConfigCategory.list.$_ITEM_.Mask
    static RouteNode SensitiveWordConfigCategory_list_item_Mask = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            ((JArray)bson.GetObject(-1)).Add(bson.StringValue);
        }},
    });

    static Dictionary<string, RouteNode> _RouteNodes = new Dictionary<string, RouteNode>()
    {
        {"SensitiveWordConfigCategory", SensitiveWordConfigCategory},
    };

    public static System.Action<LibBson> GetRoute(string name)
    {
        if(_RouteNodes.TryGetValue(name, out RouteNode node))
        {
            return node.Route;
        }
        return null;
    }
}