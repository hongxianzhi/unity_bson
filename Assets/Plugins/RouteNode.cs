using System.Collections.Generic;

public class RouteNode
{
    Dictionary<string, System.Action<LibBson>> m_routes = null;
    public RouteNode(Dictionary<string, System.Action<LibBson>> routes)
    {
        m_routes = routes;
    }

    public void Route(LibBson bson)
    {
        if(m_routes.TryGetValue(bson.EleName, out System.Action<LibBson> action) && action != null)
        {
            action(bson);
        }
    }
}