using System;
using System.Text;

public static class LibBson
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

    struct BsonContent
    {
        public byte[] buffer;                   //BSON 数据
        public string documentName;             //文档名称
        public int documentSize;                //文档大小
        public bool documentRoot;               //是否为文档根节点

        public int nodeOffset;                  //数据偏移

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

        public Object userData;
        public string hierarchy;
        public Action<string, int> notify;
    }
    static BsonContent _content = new BsonContent();

    static LibBson()
    {
        _content.objectId = new byte[12];
    }

    public static bool IsDocumentRoot
    {
        get
        {
            return _content.documentRoot;
        }
    }
    public static bool IsBeginParse
    {
        get
        {
            return _content.documentSize > 0;
        }
    }
    public static string Hierarchy
    {
        get { return _content.hierarchy; }
    }
    public static string DocumentName
    {
        get { return _content.documentName; }
    }
    public static int DocumentSize
    {
        get { return _content.documentSize; }
    }
    public static int Int32Value
    {
        get { return _content.int32Value; }
    }
    public static long Int64Value
    {
        get { return _content.int64Value; }
    }
    public static double DoubleValue
    {
        get { return _content.doubleValue; }
    }
    public static string StringValue
    {
        get { return _content.stringValue; }
    }
    public static bool BooleanValue
    {
        get { return _content.booleanValue; }
    }
    public static byte[] Buffer
    {
        get { return _content.buffer; }
    }
    public static int BinaryStart
    {
        get { return _content.binaryStart; }
    }
    public static int BinaryLength
    {
        get { return _content.binaryLength; }
    }
    public static int BinarySubtype
    {
        get { return _content.binarySubtype; }
    }
    public static byte[] ObjectId
    {
        get { return _content.objectId; }
    }

    public static System.Object UserData
    {
        get
        {
            return _content.userData;
        }
        set
        {
            _content.userData = value;
        }
    }

    static void Notify(string name, int type)
    {
        try
        {
            _content.notify(name, type);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void Decode(string docName, byte[] buf, System.Action<string, int> handler)
    {
        _content.buffer = buf;
        _content.documentName = docName;
        _content.notify = handler;
        _content.documentSize = BitConverter.ToInt32(buf, 0);
        _content.nodeOffset = 0;
        _content.hierarchy = docName;
        _content.documentRoot = true;
        Notify(docName, BSON_DOCUMENT);
        _content.documentRoot = false;
        DoDecode(false);

        _content.documentRoot = true;
        _content.documentSize = 0;
        Notify(docName, BSON_DOCUMENT);
        _content.documentRoot = false;
    }

    static void DoDecode(bool isArray)
    {
        byte[] buf = _content.buffer;
        int startIndex = _content.nodeOffset;
        string hierarchy = _content.hierarchy;
        int bufEnd = startIndex + BitConverter.ToInt32(buf, startIndex);
        int offset = startIndex + 4;

        while (offset < bufEnd)
        {
            byte elemType = buf[offset];
            offset++;
            if(offset >= bufEnd)
            {
                break;
            }

            int nameEnd = Array.IndexOf(buf, (byte)0, offset);
            nameEnd = nameEnd == -1 ? bufEnd : nameEnd;
            string elemName = Encoding.UTF8.GetString(buf, offset, nameEnd - offset);
            offset = nameEnd + 1;

            switch (elemType)
            {
                case BSON_DOUBLE:
                    {
                        _content.doubleValue = BitConverter.ToDouble(buf, offset);
                        Notify(elemName, BSON_DOUBLE);
                        offset += 8;
                    }
                    break;
                case BSON_STRING:
                    {
                        int stringLength = BitConverter.ToInt32(buf, offset);
                        _content.stringLength = stringLength;
                        _content.stringValue = Encoding.UTF8.GetString(buf, offset + 4, stringLength - 1);
                        Notify(elemName, BSON_STRING);
                        offset += stringLength + 4;
                    }
                    break;
                case BSON_DOCUMENT:
                    {
                        int documentSize = BitConverter.ToInt32(buf, offset);
                        _content.documentSize = documentSize;
                        _content.documentName = elemName;
                        _content.nodeOffset = offset;
                        Notify(elemName, BSON_DOCUMENT);

                        if (!isArray)
                        {
                            _content.hierarchy = string.Format("{0}.{1}", hierarchy, elemName);
                        }
                        DoDecode(false);

                        offset += documentSize;
                        _content.documentSize = 0;
                        _content.hierarchy = hierarchy;
                        Notify(elemName, BSON_DOCUMENT);
                    }
                    break;
                case BSON_ARRAY:
                    {
                        int documentSize = BitConverter.ToInt32(buf, offset);
                        _content.documentSize = documentSize;
                        _content.documentName = elemName;
                        _content.nodeOffset = offset;
                        Notify(elemName, BSON_ARRAY);

                        if(!isArray)
                        {
                            _content.hierarchy = string.Format("{0}.{1}", hierarchy, elemName);
                        }
                        DoDecode(true);

                        offset += documentSize;
                        _content.documentSize = 0;
                        _content.hierarchy = hierarchy;
                        Notify(elemName, BSON_ARRAY);
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
                        Notify(elemName, BSON_BINARY);
                    }
                    break;
                case BSON_INT32:
                    {
                        _content.int32Value = BitConverter.ToInt32(buf, offset);
                        offset += 4;
                        Notify(elemName, BSON_INT32);
                    }
                    break;
                case BSON_INT64:
                    {
                        _content.int64Value = BitConverter.ToInt64(buf, offset);
                        offset += 8;
                        Notify(elemName, BSON_INT64);
                    }
                    break;
                case BSON_OBJECT_ID:
                    {
                        Array.Copy(buf, offset, _content.objectId, 0, 12);
                        offset += 12;
                        Notify(elemName, BSON_OBJECT_ID);
                    }
                    break;
                case BSON_BOOLEAN:
                    {
                        _content.booleanValue = buf[offset] != 0;
                        offset++;
                        Notify(elemName, BSON_BOOLEAN);
                    }
                    break;
                case BSON_NULL:
                    break;
                default:
                    break;
            }
        }
        _content.hierarchy = hierarchy;
    }
}