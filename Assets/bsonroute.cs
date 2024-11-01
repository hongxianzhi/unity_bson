using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class Bson2CSharpRoute
{
    static Dictionary<string, System.Action<LibBson>> handlers = new Dictionary<string, System.Action<LibBson>>()
    {
        {"SensitiveWordConfigCategory", delegate(LibBson bson)
        {
            if(bson.IsEnter)
            {
                bson.PushDocument(new JObject());
            }
        }},
        {"SensitiveWordConfigCategory._t", delegate(LibBson bson)
        {
            ((JObject)bson.CurrentDocument)["_t"] = bson.StringValue;
        }},
        {"SensitiveWordConfigCategory.list", delegate(LibBson bson)
        {
            if(bson.IsEnter)
            {
                JObject list = new JObject();
                ((JObject)bson.CurrentDocument)["list"] = list;
                bson.PushDocument(list);
            }
            else
            {
                bson.PopDocument();
            }
        }},
        {"SensitiveWordConfigCategory.list.$_CHILD_", delegate(LibBson bson)
        {
            if(bson.IsEnter)
            {
                JObject item = new JObject();
                ((JObject)bson.CurrentDocument)[bson.EleName] = item;
                bson.PushDocument(item);
            }
            else
            {
                bson.PopDocument();
            }
        }},
        {"SensitiveWordConfigCategory.list.$_CHILD_._id", delegate(LibBson bson)
        {
            ((JObject)bson.CurrentDocument)["_id"] = bson.Int32Value;
        }},
        {"SensitiveWordConfigCategory.list.$_CHILD_.Mask", delegate(LibBson bson)
        {
            if(bson.IsEnter)
            {
                JObject mask = new JObject();
                ((JObject)bson.CurrentDocument)["Mask"] = mask;
                bson.PushDocument(mask);
            }
            else
            {
                bson.PopDocument();
            }
        }},
        {"SensitiveWordConfigCategory.list.$_CHILD_.Mask.$_CHILD_", delegate(LibBson bson)
        {
            ((JObject)bson.CurrentDocument)[bson.EleName] = bson.StringValue;
        }},
    };

    public static Dictionary<string, System.Action<LibBson>> GetHandlers()
    {
        return handlers;
    }
}