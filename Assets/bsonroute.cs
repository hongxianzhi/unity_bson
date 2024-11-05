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

    //RaidConfigCategory
    static RouteNode RaidConfigCategory = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {"RaidConfigCategory", delegate(LibBson bson)
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
            bson.Router = RaidConfigCategory_list.Route;
        }},
    });
    //RaidConfigCategory.list
    static RouteNode RaidConfigCategory_list = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            JObject obj = new JObject();
            ((JArray)bson.GetObject(-1)).Add(obj);
            bson.PushObject(obj);
            bson.Router = RaidConfigCategory_list_item_.Route;
        }},
    });
    //RaidConfigCategory.list.$_ITEM_
    static RouteNode RaidConfigCategory_list_item_ = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {"_id", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["_id"] = bson.Int32Value;
        }},
        {"Chapter", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Chapter"] = bson.Int32Value;
        }},
        {"Chapterpic", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Chapterpic"] = bson.StringValue;
        }},
        {"Title", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Title"] = bson.StringValue;
        }},
        {"Nameten", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Nameten"] = bson.StringValue;
        }},
        {"Limit", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Limit"] = bson.Int32Value;
        }},
        {"Skip", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Skip"] = bson.Int32Value;
        }},
        {"Boss", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Boss"] = bson.Int32Value;
        }},
        {"Hp", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Hp"] = bson.Int32Value;
        }},
        {"Attack", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Attack"] = bson.Int32Value;
        }},
        {"Defence1", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Defence1"] = bson.Int32Value;
        }},
        {"Defence2", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Defence2"] = bson.Int32Value;
        }},
        {"Crit", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Crit"] = bson.Int32Value;
        }},
        {"Hit", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Hit"] = bson.Int32Value;
        }},
        {"Critoff", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Critoff"] = bson.Int32Value;
        }},
        {"Dog", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Dog"] = bson.Int32Value;
        }},
        {"Increase", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Increase"] = bson.Int32Value;
        }},
        {"Reduce", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Reduce"] = bson.Int32Value;
        }},
        {"Break", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Break"] = bson.Int32Value;
        }},
        {"Block", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Block"] = bson.Int32Value;
        }},
        {"Finalincrease", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Finalincrease"] = bson.Int32Value;
        }},
        {"Finalreduce", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Finalreduce"] = bson.Int32Value;
        }},
        {"BattlePic", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["BattlePic"] = bson.Int32Value;
        }},
        {"BattlePic2", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["BattlePic2"] = bson.Int32Value;
        }},
        {"Conversation", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Conversation"] = bson.Int32Value;
        }},
        {"Guard", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Guard"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Reward1", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Reward1"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Reward2", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Reward2"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Reward3", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Reward3"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Details", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Details"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Energy", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Energy"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Guardstar", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Guardstar"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Awake", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Awake"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"StarChest", delegate(LibBson bson)
        {
            JArray starChest = new JArray();
            ((JObject)bson.GetObject(-1))["StarChest"] = starChest;
            bson.PushObject(starChest);
            bson.Router = RaidConfigCategory_list_item_StarChest.Route;
        }},
    });
    //RaidConfigCategory.list.$_ITEM_.Rewardx
    static RouteNode RaidConfigCategory_list_item_Reward = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            ((JArray)bson.GetObject(-1)).Add(bson.Int32Value);
        }},
    });
    //RaidConfigCategory.list.$_ITEM_.StarChest
    static RouteNode RaidConfigCategory_list_item_StarChest = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            JArray starChest = new JArray();
            ((JArray)bson.GetObject(-1)).Add(starChest);
            bson.PushObject(starChest);
            bson.Router = RaidConfigCategory_list_item_StarChest_ITEM_.Route;
        }},
    });
    //RaidConfigCategory.list.$_ITEM_.StarChest.$_ITEM_
    static RouteNode RaidConfigCategory_list_item_StarChest_ITEM_ = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            ((JArray)bson.GetObject(-1)).Add(bson.Int32Value);
        }},
    });

    //RobotConfigCategory
    static RouteNode RobotConfigCategory = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {"RobotConfigCategory", delegate(LibBson bson)
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
            bson.Router = RobotConfigCategory_list.Route;
        }},
    });
    //RobotConfigCategory.list
    static RouteNode RobotConfigCategory_list = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {LibBson.ITEM_TAG, delegate(LibBson bson)
        {
            JObject obj = new JObject();
            ((JArray)bson.GetObject(-1)).Add(obj);
            bson.PushObject(obj);
            bson.Router = RobotConfigCategory_list_item_.Route;
        }},
    });
    //RobotConfigCategory.list.$_ITEM_
    static RouteNode RobotConfigCategory_list_item_ = new RouteNode(new Dictionary<string, System.Action<LibBson>>()
    {
        {"_id", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["_id"] = bson.Int32Value;
        }},
        {"Power", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Power"] = bson.Int32Value;
        }},
        {"Hp", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Hp"] = bson.Int32Value;
        }},
        {"Attack", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Attack"] = bson.Int32Value;
        }},
        {"Defence1", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Defence1"] = bson.Int32Value;
        }},
        {"Defence2", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Defence2"] = bson.Int32Value;
        }},
        {"Crit", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Crit"] = bson.Int32Value;
        }},
        {"Hit", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Hit"] = bson.Int32Value;
        }},
        {"Critoff", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Critoff"] = bson.Int32Value;
        }},
        {"Dog", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Dog"] = bson.Int32Value;
        }},
        {"Increase", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Increase"] = bson.Int32Value;
        }},
        {"Reduce", delegate(LibBson bson)
        {
            ((JObject)bson.GetObject(-1))["Reduce"] = bson.Int32Value;
        }},
        {"Guard", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Guard"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Details", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Details"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"Energy", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["Energy"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
        {"GuardStar", delegate(LibBson bson)
        {
            JArray reward = new JArray();
            ((JObject)bson.GetObject(-1))["GuardStar"] = reward;
            bson.PushObject(reward);
            bson.Router = RaidConfigCategory_list_item_Reward.Route;
        }},
    });

    static Dictionary<string, RouteNode> _RouteNodes = new Dictionary<string, RouteNode>()
    {
        {"RaidConfigCategory", RaidConfigCategory},
        {"RobotConfigCategory", RobotConfigCategory},
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