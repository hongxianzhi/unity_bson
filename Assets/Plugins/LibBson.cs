using System;
using System.Text;
using System.Collections.Generic;

public class LibBson : IDisposable
{
    public const byte BSON_DOUBLE = 0x01;
    public const byte BSON_STRING = 0x02;
    public const byte BSON_DOCUMENT = 0x03;
    public const byte BSON_ARRAY = 0x04;
    public const byte BSON_BINARY = 0x05;
    public const byte BSON_UNDEFINED = 0x06;
    public const byte BSON_OBJECT_ID = 0x07;
    public const byte BSON_BOOLEAN = 0x08;
    public const byte BSON_DATETIME = 0x09;
    public const byte BSON_NULL = 0x0A;
    public const byte BSON_REGEXP = 0x0B;
    public const byte BSON_DB_POINTER = 0x0C;
    public const byte BSON_JAVASCRIPT = 0x0D;
    public const byte BSON_SYMBOL = 0x0E;
    public const byte BSON_JAVASCRIPT_SCOPE = 0x0F;
    public const byte BSON_INT32 = 0x10;
    public const byte BSON_TIMESTAMP = 0x11;
    public const byte BSON_INT64 = 0x12;
    public const byte BSON_MINIMUM = 0xFF;
    public const byte BSON_MAXIMUM = 0x7F;
    public const string ITEM_TAG = "$_ITEM_";

    struct BsonContent
    {
        public byte[] buffer;                   //BSON 数据
        public string documentName;             //文档名称
        public int documentSize;                //文档大小

        public int nodeOffset;                  //数据偏移
        public string eleName;                  //元素名称
        public string realEleName;              //真实元素名称
        public int eleType;                     //元素类型

        public int stringLength;
        public string stringValue;

        public int int32Value;
        public long int64Value;
        public bool booleanValue;
        public double doubleValue;

        public int binaryStart;
        public int binaryLength;
        public int binarySubtype;

        public byte[] objectId;
        public Action<LibBson> router;
        public List<System.Object> objects;
        public Dictionary<string, System.Object> userdata;
    }
    BsonContent _content = new BsonContent();

    public LibBson(string docName, byte[] buf)
    {
        _content.buffer = buf;
        _content.documentName = docName;
        _content.objectId = new byte[12];
        _content.objects = new List<System.Object>();
        _content.userdata = new Dictionary<string, System.Object>();
    }

    public void Dispose()
    {
        _content.buffer = null;
        _content.documentName = null;
        _content.stringValue = null;
        _content.objectId = null;
        _content.router = null;
        _content.objects.Clear();
        _content.objects = null;
        _content.userdata.Clear();
        _content.userdata = null;
    }

    static T Get<T>(List<T> list, int stack_id)
    {
        T result = default;
        int stackSize = list.Count;
        if(stack_id < 0 )
        {
            result = stackSize + stack_id < 0 ? default : list[stackSize + stack_id];
        }
        else if(stack_id > 0)
        {
            result = stack_id >= stackSize ? default : list[stack_id - 1];
        }
        return result;
    }

    public System.Object GetUserData(string key)
    {
        _content.userdata.TryGetValue(key, out System.Object value);
        return value;
    }

    public void SetUserData(string key, System.Object value)
    {
        _content.userdata[key] = value;
    }

    public int ObjectTop
    {
        get { return _content.objects.Count; }
        private set
        {
             _content.objects.RemoveRange(value, _content.objects.Count - value);
        }
    }
    public System.Object GetObject(int stack_id)
    {
        return Get(_content.objects, stack_id);
    }
    public void PushObject(System.Object obj)
    {
        if(obj == null)
        {
            throw new Exception("object is null.");
        }
        _content.objects.Add(obj);
    }

    public Action<LibBson> Router
    {
        get { return _content.router; }
        set { _content.router = value; }
    }
    public int EleType
    {
        get { return _content.eleType; }
    }
    public string EleName
    {
        get { return _content.eleName; }
    }
    public string RealEleName
    {
        get { return _content.realEleName; }
    }
    public string DocumentName
    {
        get { return _content.documentName; }
    }
    public int DocumentSize
    {
        get { return _content.documentSize; }
    }
    public int Int32Value
    {
        get { return _content.int32Value; }
    }
    public long Int64Value
    {
        get { return _content.int64Value; }
    }
    public double DoubleValue
    {
        get { return _content.doubleValue; }
    }
    public string StringValue
    {
        get { return _content.stringValue; }
    }
    public bool BooleanValue
    {
        get { return _content.booleanValue; }
    }
    public byte[] Buffer
    {
        get { return _content.buffer; }
    }
    public int BinaryStart
    {
        get { return _content.binaryStart; }
    }
    public int BinaryLength
    {
        get { return _content.binaryLength; }
    }
    public int BinarySubtype
    {
        get { return _content.binarySubtype; }
    }
    public byte[] ObjectId
    {
        get { return _content.objectId; }
    }

    void CallNotify(System.Action<LibBson> handler, string name, int type)
    {
        try
        {
            _content.eleType = type;
            _content.eleName = name;
            handler?.Invoke(this);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void Decode()
    {
        byte[] buf = _content.buffer;
        string docName = _content.documentName;
        _content.documentSize = BitConverter.ToInt32(buf, 0);
        _content.nodeOffset = 0;
        CallNotify(Router, docName, BSON_DOCUMENT);
        DoDecode(null);
    }

    void DoDecode(string typeName)
    {
        byte[] buf = _content.buffer;
        int startIndex = _content.nodeOffset;
        int bufEnd = startIndex + BitConverter.ToInt32(buf, startIndex);
        int offset = startIndex + 4;

        System.Action<LibBson> handler = Router;
        while (offset < bufEnd)
        {
            byte elemType = buf[offset];
            offset++;
            if(offset >= bufEnd)
            {
                break;
            }

            int top_object = ObjectTop;
            int nameEnd = Array.IndexOf(buf, (byte)0, offset);
            nameEnd = nameEnd == -1 ? bufEnd : nameEnd;
            _content.realEleName = Encoding.UTF8.GetString(buf, offset, nameEnd - offset);
            string eleName = string.IsNullOrEmpty(typeName) ? _content.realEleName : typeName;
            offset = nameEnd + 1;
            switch (elemType)
            {
                case BSON_DOUBLE:
                    {
                        if (offset + 8 > bufEnd)
                        {
                            throw new IndexOutOfRangeException("Buffer overrun while reading BSON_DOUBLE.");
                        }
                        _content.doubleValue = BitConverter.ToDouble(buf, offset);
                        CallNotify(handler, eleName, BSON_DOUBLE);
                        offset += 8;
                    }
                    break;
                case BSON_STRING:
                    {
                        if (offset + 4 > bufEnd)
                        {
                            throw new IndexOutOfRangeException("Buffer overrun while reading BSON_STRING length.");
                        }
                        int stringLength = BitConverter.ToInt32(buf, offset);
                        if (offset + 4 + stringLength > bufEnd)
                        {
                            throw new IndexOutOfRangeException("Buffer overrun while reading BSON_STRING value.");
                        }
                        _content.stringLength = stringLength;
                        _content.stringValue = Encoding.UTF8.GetString(buf, offset + 4, stringLength - 1);
                        CallNotify(handler, eleName, BSON_STRING);
                        offset += stringLength + 4;
                    }
                    break;
                case BSON_DOCUMENT:
                    {
                        if (offset + 4 > bufEnd)
                        {
                            throw new IndexOutOfRangeException("Buffer overrun while reading BSON_DOCUMENT size.");
                        }
                        int documentSize = BitConverter.ToInt32(buf, offset);
                        if (offset + documentSize > bufEnd)
                        {
                            throw new IndexOutOfRangeException("Buffer overrun while reading BSON_DOCUMENT content.");
                        }
                        _content.documentSize = documentSize;
                        _content.nodeOffset = offset;
                        CallNotify(handler, eleName, BSON_DOCUMENT);
                        DoDecode(null);
                        offset += documentSize;
                    }
                    break;
                case BSON_ARRAY:
                    {
                        int documentSize = BitConverter.ToInt32(buf, offset);
                        _content.documentSize = documentSize;
                        _content.nodeOffset = offset;
                        CallNotify(handler, eleName, BSON_ARRAY);
                        DoDecode(ITEM_TAG);
                        offset += documentSize;
                    }
                    break;
                case BSON_BINARY:
                    {
                        int binLength = BitConverter.ToInt32(buf, offset);
                        offset += 4;
                        byte subtype = buf[offset];
                        offset++;
                        _content.binaryStart = offset;
                        offset += binLength;
                        _content.binaryLength = binLength;
                        _content.binarySubtype = subtype;
                        CallNotify(handler, eleName, BSON_BINARY);
                    }
                    break;
                case BSON_INT32:
                    {
                        _content.int32Value = BitConverter.ToInt32(buf, offset);
                        offset += 4;
                        CallNotify(handler, eleName, BSON_INT32);
                    }
                    break;
                case BSON_INT64:
                    {
                        _content.int64Value = BitConverter.ToInt64(buf, offset);
                        offset += 8;
                        CallNotify(handler, eleName, BSON_INT64);
                    }
                    break;
                case BSON_OBJECT_ID:
                    {
                        Array.Copy(buf, offset, _content.objectId, 0, 12);
                        offset += 12;
                        CallNotify(handler, eleName, BSON_OBJECT_ID);
                    }
                    break;
                case BSON_BOOLEAN:
                    {
                        _content.booleanValue = buf[offset] != 0;
                        offset++;
                        CallNotify(handler, eleName, BSON_BOOLEAN);
                    }
                    break;
                case BSON_NULL:
                    break;
                default:
                    break;
            }
            ObjectTop = top_object;
        }
    }
}