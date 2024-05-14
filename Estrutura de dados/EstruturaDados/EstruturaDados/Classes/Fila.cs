using System.Collections;

namespace EstruturaDados.Classes;

public class Fila
{
    private object?[] _itens;
    private int _size;
    private readonly int _max;

    public Fila() // Tamanho variável
    {
        _itens = Array.Empty<object>();
        _size = 0;
        _max = 0;
    }
    public Fila(int max) // Tamanho fixo
    {
        _itens = new object[max];
        _size = 0;
        _max = max;
    }

    public int Size()
    {
        return _size;
    }

    public bool Enqueue(object obj)
    {
        if (_max != 0 && _size == _max) return false;
        if (_max == 0 && _itens.Length == _size)
        {
            Resize();
        }
        
        _itens[_size] = obj;
        _size += 1;
        return true;
    }

    public object? Dequeue()
    {
        if (_size == 0) return null;
        
        var result = _itens[0];
        for (int i = 0; i < _size-1; i++)
        {
            _itens[i] = _itens[i + 1];
        }
        _size -= 1;
        return result;
    }

    public object? Peek()
    {
        if (_size == 0) return null;
        return _itens[0];
    }

    public void Clear()
    {
        if (_max == 0)
        {
            _itens = Array.Empty<object>();
        }
        _itens = new object[_max];
        _size = 0;
    }

    public object? IsEmpty()
    {
        return _size == 0;
    }
    
    public string ToString()
    {
        string result = "";

        for (int i = 0; i < _size; i++)
        {
            result = result + _itens[i] + ", ";
        }

        return result;
    }

    private void Resize()
    {
        Array.Resize(ref _itens, _size+5);
    }
}