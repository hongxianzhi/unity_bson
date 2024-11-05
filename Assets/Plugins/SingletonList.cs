using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SingletonList<T>
{
    static List<T> m_list = new List<T>();
    static List<T> Instance
    {
        get
        {
            return m_list;
        }
    }
}