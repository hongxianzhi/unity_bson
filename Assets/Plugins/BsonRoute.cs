using System.Collections.Generic;

public class BsonDispatch
{
    Dictionary<string, System.Action<LibBson>> m_route = null;
    public BsonDispatch(Dictionary<string, System.Action<LibBson>> route)
    {
        m_route = route;
    }

    void BsonHandler(LibBson bson, int type)
    {
        string hierarchy = bson.Hierarchy;
        if(m_route.TryGetValue(hierarchy, out System.Action<LibBson> action) && action != null)
        {
            action(bson);
        }
        else
        {
            throw new System.Exception("BsonHandler: " + hierarchy + " not found");
        }
    }

    public LibBson CreateBson(string name, byte[] data)
    {
        return new LibBson(name, data, BsonHandler);
    }
}