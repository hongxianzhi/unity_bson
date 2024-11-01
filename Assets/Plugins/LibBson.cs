using System;
using System.Text;
using System.Collections.Generic;

public class LibBson
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
        public string eleName;                  //元素名称

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
        public string hierarchy;
        public Action<LibBson, int> notify;
        public List<System.Object> documentStack;
        public Dictionary<string, Dictionary<string, System.Action>> documentHandlers;
    }
    BsonContent _content = new BsonContent();

    public LibBson(string docName, byte[] buf, System.Action<LibBson, int> handler)
    {
        _content.objectId = new byte[12];
        _content.documentStack = new List<System.Object>();
        _content.buffer = buf;
        _content.notify = handler;
        _content.documentName = docName;
    }

    System.Object _CurrentDocument;
    public System.Object CurrentDocument
    {
        get
        {
            return _CurrentDocument;
        }
    }

    public void PushDocument(System.Object doc)
    {
        if(doc == null)
        {
            throw new Exception("Document is null.");
        }
        _content.documentStack.Add(doc);
        _CurrentDocument = doc;
    }

    public void PopDocument()
    {
        if(_content.documentStack.Count == 0)
        {
            throw new Exception("Document stack is empty.");
        }
        _content.documentStack.RemoveAt(_content.documentStack.Count - 1);
        _CurrentDocument = _content.documentStack.Count == 0 ? null : _content.documentStack[_content.documentStack.Count - 1];
    }

    public bool IsDocumentRoot
    {
        get
        {
            return _content.documentRoot;
        }
    }
    public bool IsEnter
    {
        get
        {
            return _content.documentSize > 0;
        }
    }
    public string EleName
    {
        get { return _content.eleName; }
    }
    public string Hierarchy
    {
        get { return _content.hierarchy; }
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

    void Notify(int type)
    {
        try
        {
            _content.notify(this, type);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void Decode()
    {
        _CurrentDocument = null;
        byte[] buf = _content.buffer;
        string docName = _content.documentName;
        _content.documentStack.Clear();
        _content.documentSize = BitConverter.ToInt32(buf, 0);
        _content.nodeOffset = 0;
        _content.hierarchy = docName;
        _content.documentRoot = true;
        _content.eleName = docName;
        Notify(BSON_DOCUMENT);
        _content.documentRoot = false;
        DoDecode(null);

        _content.documentRoot = true;
        _content.documentSize = 0;
        _content.eleName = docName;
        Notify(BSON_DOCUMENT);
        _content.documentRoot = false;
    }

    void DoDecode(string typeName)
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
            string eleName = Encoding.UTF8.GetString(buf, offset, nameEnd - offset);
            _content.eleName = eleName;
            offset = nameEnd + 1;

            _content.hierarchy = string.Format("{0}.{1}", hierarchy, string.IsNullOrEmpty(typeName) ? eleName : typeName);
            switch (elemType)
            {
                case BSON_DOUBLE:
                    {
                        if (offset + 8 > bufEnd)
                        {
                            throw new IndexOutOfRangeException("Buffer overrun while reading BSON_DOUBLE.");
                        }
                        _content.doubleValue = BitConverter.ToDouble(buf, offset);
                        Notify(BSON_DOUBLE);
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
                        Notify(BSON_STRING);
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
                        Notify(BSON_DOCUMENT);
                        DoDecode(null);

                        offset += documentSize;
                        _content.documentSize = 0;
                        _content.eleName = eleName;
                        Notify(BSON_DOCUMENT);
                    }
                    break;
                case BSON_ARRAY:
                    {
                        int documentSize = BitConverter.ToInt32(buf, offset);
                        _content.documentSize = documentSize;
                        _content.nodeOffset = offset;
                        Notify(BSON_ARRAY);
                        DoDecode("$_CHILD_");

                        offset += documentSize;
                        _content.documentSize = 0;
                        _content.eleName = eleName;
                        Notify(BSON_ARRAY);
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
                        Notify(BSON_BINARY);
                    }
                    break;
                case BSON_INT32:
                    {
                        _content.int32Value = BitConverter.ToInt32(buf, offset);
                        offset += 4;
                        Notify(BSON_INT32);
                    }
                    break;
                case BSON_INT64:
                    {
                        _content.int64Value = BitConverter.ToInt64(buf, offset);
                        offset += 8;
                        Notify(BSON_INT64);
                    }
                    break;
                case BSON_OBJECT_ID:
                    {
                        Array.Copy(buf, offset, _content.objectId, 0, 12);
                        offset += 12;
                        Notify(BSON_OBJECT_ID);
                    }
                    break;
                case BSON_BOOLEAN:
                    {
                        _content.booleanValue = buf[offset] != 0;
                        offset++;
                        Notify(BSON_BOOLEAN);
                    }
                    break;
                case BSON_NULL:
                    break;
                default:
                    break;
            }
            _content.hierarchy = hierarchy;
        }
    }
}