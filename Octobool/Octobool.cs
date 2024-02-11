using System.Globalization;

namespace Bitwiser;

[Serializable]
public struct Octobool : IComparable, IConvertible, IComparable<Octobool>, IEquatable<Octobool>
{
    private byte m_values;
    
    // Holds the index, NOT mask
    private byte m_selector;
    
    public Octobool(
        bool a = false,
        bool b = false,
        bool c = false,
        bool d = false,
        bool e = false,
        bool f = false,
        bool g = false,
        bool h = false)
    {
        m_values = (byte)(
            (h ? 1 << 7 : 0) |
            (g ? 1 << 6 : 0) |
            (f ? 1 << 5 : 0) |
            (e ? 1 << 4 : 0) |
            (d ? 1 << 3 : 0) |
            (c ? 1 << 2 : 0) |
            (b ? 1 << 1 : 0) |
            (a ? 1 : 0)

        );
        m_selector = 0;
    }
    
    /// <summary>
    /// Increments the selector and returns the new value
    /// </summary>
    /// <returns>The next bool</returns>
    public bool Next()
    {
        Increment();

        return Current();
    }

    /// <summary>
    /// Decrements the selector and returns the new value
    /// </summary>
    /// <returns>The previous bool</returns>
    public bool Previous()
    {
        Decrement();

        return Current();
    }
    
    /// <summary>
    /// Increments the selector to the next bool and returns the old value
    /// </summary>
    /// <returns>The current bool</returns>
    public bool Increment()
    {
        var prev = Current();
        
        SetSelector((m_selector + 1) % 8);

        return prev;
    }
    
    /// <summary>
    /// Decrements the selector to the previous bool and returns the old value
    /// </summary>
    /// <returns>The current bool</returns>
    public bool Decrement()
    {
        var prev = Current();
        
        SetSelector((m_selector - 1 + 8) % 8);

        return prev;
    }

    /// <summary>
    /// Sets the boolean at an index to specified value
    /// </summary>
    /// <param name="index">The index to modify</param>
    /// <param name="value">The boolean value</param>
    public void SetAtIndex(int index, bool value)
    {
        if (index is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "The selector index must be between 0 and 7");
        }

        m_values &= (byte)(~(1 << index));
        m_values |= (byte)((value ? 1 : 0) << index);
    }
    
    /// <summary>
    /// Grabs the bool at the current selector
    /// </summary>
    /// <returns>The value of the boolean at the current selector</returns>
    public bool Current()
    {
        return ToBoolean();
    }
    
    /// <summary>
    /// Used to manually setting the selector at a specific index.
    /// </summary>
    /// <param name="index">The index to set the selector to</param>
    public void SetSelector(int index)
    {
        if (index is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "The selector index must be between 0 and 7");
        }
        
        m_selector = Convert.ToByte(index);
    }

    /// <summary>
    /// Gets the boolean at specified index
    /// </summary>
    /// <param name="index">The index of the bool</param>
    /// <returns>The value of the bool at the given index</returns>
    public bool At(int index)
    {
        SetSelector(index);

        return Current();
    }

    public override int GetHashCode()
    {
        return m_values;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is not Octobool)
        {
            throw new ArgumentException("Argument must be Octobool");
        }
        
        if (m_values == ((Octobool)obj).m_values)
        {
            return 0;
        }

        return m_values.CompareTo(((Octobool)obj).m_values);
    }

    //
    // IConvertible implementations
    //
    
    public TypeCode GetTypeCode()
    {
        return TypeCode.Byte;
    }

    public bool ToBoolean()
    {
        return ToBoolean(new CultureInfo("en-us"));
    }
    
    public bool ToBoolean(IFormatProvider? provider)
    {
        return ((m_values >> m_selector) & 1) != 0;
    }
    
    public bool ToBoolean(int index)
    {
        SetSelector(index);

        return ToBoolean();
    }

    public bool ToBoolean(int index, IFormatProvider? provider)
    {
        SetSelector(index);

        return ToBoolean(provider);
    }

    public byte ToByte(IFormatProvider? provider)
    {
        return m_values;
    }

    public char ToChar(IFormatProvider? provider)
    {
        return m_values.ToString().Single();
    }

    public DateTime ToDateTime(IFormatProvider? provider)
    {
        throw new InvalidCastException("Cannot convert from type Octobool to type DateTime");
    }

    public decimal ToDecimal(IFormatProvider? provider)
    {
        return Convert.ToDecimal(m_values);
    }

    public double ToDouble(IFormatProvider? provider)
    {
        return Convert.ToDouble(m_values);
    }

    public short ToInt16(IFormatProvider? provider)
    {
        return Convert.ToInt16(m_values);
    }

    public int ToInt32(IFormatProvider? provider)
    {
        return Convert.ToInt32(m_values);
    }

    public long ToInt64(IFormatProvider? provider)
    {
        return Convert.ToInt64(m_values);
    }

    public sbyte ToSByte(IFormatProvider? provider)
    {
        return Convert.ToSByte(m_values);
    }

    public float ToSingle(IFormatProvider? provider)
    {
        return Convert.ToSingle(m_values);
    }

    public override string ToString()
    {
        return ToString(new CultureInfo("en-us"));
    }

    public string ToString(IFormatProvider? provider)
    {
        return m_values.ToString(provider);
    }

    public object ToType(Type conversionType, IFormatProvider? provider)
    {
        return Convert.ChangeType(m_values, conversionType);
    }

    public ushort ToUInt16(IFormatProvider? provider)
    {
        return Convert.ToUInt16(m_values);
    }

    public uint ToUInt32(IFormatProvider? provider)
    {
        return Convert.ToUInt32(m_values);
    }

    public ulong ToUInt64(IFormatProvider? provider)
    {
        return Convert.ToUInt64(m_values);
    }

    public int CompareTo(Octobool other)
    {
        if (m_values == other.m_values)
        {
            return 0;
        }

        return m_values.CompareTo(other.m_values);
    }

    public bool Equals(Octobool other)
    {
        return m_values == other.m_values;
    }

    public override bool Equals(object? obj)
    {
        return obj is Octobool && Equals((Octobool)obj);
    }

    public static bool operator ==(Octobool left, Octobool right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Octobool left, Octobool right)
    {
        return !left.Equals(right);
    }
}