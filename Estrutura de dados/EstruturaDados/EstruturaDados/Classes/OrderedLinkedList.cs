namespace EstruturaDados.Classes;

public class OrderedLinkedList
{
    private class Node
    {
        public float Value { get; set; }
        public Node? NodeNext { get; set; }

        public Node() { }
        public Node(float value)
        {
            Value = value;
        }
    }

    private Node _head;
    private int _size;

    public OrderedLinkedList()
    {
        _head = null;
        _size = 0;
    }

    public void Print()
    {
        Node node = _head;
        string result="";
        while (node != null)
        {
            result += node.Value +", ";
            node = node.NodeNext;
        }
        Console.WriteLine(result);
    }

    public int Size()
    {
        return _size;
    }
    
    public void Insert(float value)
    {
        Node node = new Node(value);
        Node prevNode = FindGap(value);
        if (prevNode == null)
        {
            _head = node;
            _size++;
            return;
        }
        if (Equals(_head,prevNode))
        {
            node.NodeNext = prevNode;
            _head = node;
            _size++;
            return;
        }
        if (prevNode.NodeNext != null)
        {
            node.NodeNext = prevNode.NodeNext;
        }
        prevNode.NodeNext = node;
        _size++;
    }

    public bool Contais(float value)
    {
        Node node = Find(value);
        return node != null;
    }

    public float Pop()
    {
        Node node = _head;
        _head = node.NodeNext;
        _size--;
        return node.Value;
    }

    public float PopLast()
    {
        Node node = Returnpenultimate();
        var result = node.NodeNext.Value;
        node.NodeNext = null;
        _size--;
        return result;
    }

    public void Remove(float value)
    {
        Node pre = new Node();
        Node node = Find(value, ref pre);
        if (node == null)
        {
            Console.WriteLine("Value not found");
            return ;
        }
        if (node==pre)
        {
            _head = node.NodeNext;
            _size--;
            return;
        }
        pre.NodeNext = node.NodeNext;
        _size--;
    }

    public void Clear()
    {
        _head = null;
        _size = 0;
    }

    public bool IsEmpty()
    {
        return _head == null;
    }

    private Node Find(float value)
    {
        Node node = _head;
        if (node == null) return null;
        while (node != null && node.Value<value)
        {
            if (Equals(node.Value, value))
            {
                return node;
            }
            node = node.NodeNext;
        }

        return null;
    }

    private Node FindGap(float value)
    {
        Node node = _head;
        if (node == null) return null;
        while (node != null)
        {
            if (node.NodeNext == null || node.NodeNext.Value > value)
            {
                return node;
            }
            node = node.NodeNext;
        }

        return null;
    }

    private Node Find(float value, ref Node predecessor)
    {
        Node node = _head;
        if (node == null) return null;
        Node pre = node;
        while (node != null && value <node.Value)
        {
            if (Equals(node.Value, value))
            {
                predecessor = pre;
                return node;
            }

            pre = node;
            node = node.NodeNext;
        }

        return null;
    }

    private Node Returnpenultimate()
    {
        Node node = _head;
        if (node == null) return null;
        while (node.NodeNext.NodeNext != null) node = node.NodeNext;
        return node;
    }
}