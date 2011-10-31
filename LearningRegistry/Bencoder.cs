
/*****
  * Encoding usage:
  *
  * new BDictionary()
  * {
  * {"Some Key", "Some Value"},
  * {"Another Key", 42}
  * }.ToBencodedString();
  *
  * Decoding usage:
  *
  * BencodeDecoder.Decode("d8:Some Key10:Some Value13:Another Valuei42ee");
  *
  * Feel free to use it.
  * More info about Bencoding at http://wiki.theory.org/BitTorrentSpecification#bencoding
  *
  * Originally posted at http://snipplr.com/view/37790/ by SuprDewd.
  * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bencoding
{
    /// <summary>
    /// A class used for decoding Bencoding.
    /// </summary>
    public static class BencodeDecoder
    {
        /// <summary>
        /// Decodes the string.
        /// </summary>
        /// <param name="bencodedString">The bencoded string.</param>
        /// <returns>An array of root elements.</returns>
        public static BElement[] Decode(string bencodedString)
        {
            int index = 0;

            try
            {
                if (bencodedString == null) return null;

                List<BElement> rootElements = new List<BElement>();
                while (bencodedString.Length > index)
                {
                    rootElements.Add(ReadElement(ref bencodedString, ref index));
                }

                return rootElements.ToArray();
            }
            catch (BencodingException) { throw; }
            catch (Exception e) { throw Error(e); }
        }

        private static BElement ReadElement(ref string bencodedString, ref int index)
        {
            switch (bencodedString[index])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9': return ReadString(ref bencodedString, ref index);
                case 'i': return ReadInteger(ref bencodedString, ref index);
                case 'l': return ReadList(ref bencodedString, ref index);
                case 'd': return ReadDictionary(ref bencodedString, ref index);
                default: throw Error();
            }
        }

        private static BDictionary ReadDictionary(ref string bencodedString, ref int index)
        {
            index++;
            BDictionary dict = new BDictionary();

            try
            {
                while (bencodedString[index] != 'e')
                {
                    BString K = ReadString(ref bencodedString, ref index);
                    BElement V = ReadElement(ref bencodedString, ref index);
                    dict.Add(K, V);
                }
            }
            catch (BencodingException) { throw; }
            catch (Exception e) { throw Error(e); }

            index++;
            return dict;
        }

        private static BList ReadList(ref string bencodedString, ref int index)
        {
            index++;
            BList lst = new BList();

            try
            {
                while (bencodedString[index] != 'e')
                {
                    lst.Add(ReadElement(ref bencodedString, ref index));
                }
            }
            catch (BencodingException) { throw; }
            catch (Exception e) { throw Error(e); }

            index++;
            return lst;
        }

        private static BInteger ReadInteger(ref string bencodedString, ref int index)
        {
            index++;

            int end = bencodedString.IndexOf('e', index);
            if (end == -1) throw Error();

            long integer;

            try
            {
                integer = Convert.ToInt64(bencodedString.Substring(index, end - index));
                index = end + 1;
            }
            catch (Exception e) { throw Error(e); }

            return new BInteger(integer);
        }

        private static BString ReadString(ref string bencodedString, ref int index)
        {
            int length, colon;

            try
            {
                colon = bencodedString.IndexOf(':', index);
                if (colon == -1) throw Error();
                length = Convert.ToInt32(bencodedString.Substring(index, colon - index));
            }
            catch (Exception e) { throw Error(e); }

            index = colon + 1;
            int tmpIndex = index;
            index += length;

            try
            {
                return new BString(bencodedString.Substring(tmpIndex, length));
            }
            catch (Exception e) { throw Error(e); }
        }

        private static Exception Error(Exception e)
        {
            return new BencodingException("Bencoded string invalid.", e);
        }

        private static Exception Error()
        {
            return new BencodingException("Bencoded string invalid.");
        }
    }

    /// <summary>
    /// An interface for bencoded elements.
    /// </summary>
    public interface BElement
    {
        /// <summary>
        /// Generates the bencoded equivalent of the element.
        /// </summary>
        /// <returns>The bencoded equivalent of the element.</returns>
        string ToBencodedString();

        /// <summary>
        /// Generates the bencoded equivalent of the element.
        /// </summary>
        /// <param name="u">The StringBuilder to append to.</param>
        /// <returns>The bencoded equivalent of the element.</returns>
        StringBuilder ToBencodedString(StringBuilder u);
    }

    /// <summary>
    /// A bencode integer.
    /// </summary>
    public class BInteger : BElement, IComparable<BInteger>
    {
        /// <summary>
        /// Allows you to set an integer to a BInteger.
        /// </summary>
        /// <param name="i">The integer.</param>
        /// <returns>The BInteger.</returns>
        public static implicit operator BInteger(int i)
        {
            return new BInteger(i);
        }

        /// <summary>
        /// The value of the bencoded integer.
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// The main constructor.
        /// </summary>
        /// <param name="value">The value of the bencoded integer.</param>
        public BInteger(long value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Generates the bencoded equivalent of the integer.
        /// </summary>
        /// <returns>The bencoded equivalent of the integer.</returns>
        public string ToBencodedString()
        {
            return this.ToBencodedString(new StringBuilder()).ToString();
        }

        /// <summary>
        /// Generates the bencoded equivalent of the integer.
        /// </summary>
        /// <returns>The bencoded equivalent of the integer.</returns>
        public StringBuilder ToBencodedString(StringBuilder u)
        {
            if (u == null) u = new StringBuilder('i');
            else u.Append('i');
            return u.Append(Value.ToString()).Append('e');
        }

        /// <see cref="Object.GetHashCode()"/>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Int32.Equals(object)
        /// </summary>
        public override bool Equals(object obj)
        {
            try
            {
                return this.Value.Equals(((BInteger)obj).Value);
            }
            catch { return false; }
        }

        /// <see cref="Object.ToString()"/>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <see cref="IComparable.CompareTo(object)"/>
        public int CompareTo(BInteger other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }

    /// <summary>
    /// A bencode string.
    /// </summary>
    public class BString : BElement, IComparable<BString>
    {
        /// <summary>
        /// Allows you to set a string to a BString.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The BString.</returns>
        public static implicit operator BString(string s)
        {
            return new BString(s);
        }

        /// <summary>
        /// The value of the bencoded integer.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The main constructor.
        /// </summary>
        /// <param name="value"></param>
        public BString(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Generates the bencoded equivalent of the string.
        /// </summary>
        /// <returns>The bencoded equivalent of the string.</returns>
        public string ToBencodedString()
        {
            return this.ToBencodedString(new StringBuilder()).ToString();
        }

        /// <summary>
        /// Generates the bencoded equivalent of the string.
        /// </summary>
        /// <param name="u">The StringBuilder to append to.</param>
        /// <returns>The bencoded equivalent of the string.</returns>
        public StringBuilder ToBencodedString(StringBuilder u)
        {
            if (u == null) u = new StringBuilder(this.Value.Length);
            else u.Append(this.Value.Length);
            return u.Append(':').Append(this.Value);
        }

        /// <see cref="Object.GetHashCode()"/>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// String.Equals(object)
        /// </summary>
        public override bool Equals(object obj)
        {
            try
            {
                return this.Value.Equals(((BString)obj).Value);
            }
            catch { return false; }
        }

        /// <see cref="Object.ToString()"/>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <see cref="IComparable.CompareTo(Object)"/>
        public int CompareTo(BString other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }

    /// <summary>
    /// A bencode list.
    /// </summary>
    public class BList : List<BElement>, BElement
    {
        /// <summary>
        /// Generates the bencoded equivalent of the list.
        /// </summary>
        /// <returns>The bencoded equivalent of the list.</returns>
        public string ToBencodedString()
        {
            return this.ToBencodedString(new StringBuilder()).ToString();
        }

        /// <summary>
        /// Generates the bencoded equivalent of the list.
        /// </summary>
        /// <param name="u">The StringBuilder to append to.</param>
        /// <returns>The bencoded equivalent of the list.</returns>
        public StringBuilder ToBencodedString(StringBuilder u)
        {
            if (u == null) u = new StringBuilder('l');
            else u.Append('l');

            foreach (BElement element in base.ToArray())
            {
                element.ToBencodedString(u);
            }

            return u.Append('e');
        }

        /// <summary>
        /// Adds the specified value to the list.
        /// </summary>
        /// <param name="value">The specified value.</param>
        public void Add(string value)
        {
            base.Add(new BString(value));
        }

        /// <summary>
        /// Adds the specified value to the list.
        /// </summary>
        /// <param name="value">The specified value.</param>
        public void Add(int value)
        {
            base.Add(new BInteger(value));
        }
    }

    /// <summary>
    /// A bencode dictionary.
    /// </summary>
    public class BDictionary : SortedDictionary<BString, BElement>, BElement
    {
        /// <summary>
        /// Generates the bencoded equivalent of the dictionary.
        /// </summary>
        /// <returns>The bencoded equivalent of the dictionary.</returns>
        public string ToBencodedString()
        {
            return this.ToBencodedString(new StringBuilder()).ToString();
        }

        /// <summary>
        /// Generates the bencoded equivalent of the dictionary.
        /// </summary>
        /// <param name="u">The StringBuilder to append to.</param>
        /// <returns>The bencoded equivalent of the dictionary.</returns>
        public StringBuilder ToBencodedString(StringBuilder u)
        {
            if (u == null) u = new StringBuilder('d');
            else u.Append('d');

            for (int i = 0; i < base.Count; i++)
            {
                base.Keys.ElementAt(i).ToBencodedString(u);
                base.Values.ElementAt(i).ToBencodedString(u);
            }

            return u.Append('e');
        }

        /// <summary>
        /// Adds the specified key-value pair to the dictionary.
        /// </summary>
        /// <param name="key">The specified key.</param>
        /// <param name="value">The specified value.</param>
        public void Add(string key, BElement value)
        {
            base.Add(new BString(key), value);
        }

        /// <summary>
        /// Adds the specified key-value pair to the dictionary.
        /// </summary>
        /// <param name="key">The specified key.</param>
        /// <param name="value">The specified value.</param>
        public void Add(string key, string value)
        {
            base.Add(new BString(key), new BString(value));
        }

        /// <summary>
        /// Adds the specified key-value pair to the dictionary.
        /// </summary>
        /// <param name="key">The specified key.</param>
        /// <param name="value">The specified value.</param>
        public void Add(string key, int value)
        {
            base.Add(new BString(key), new BInteger(value));
        }

        /// <summary>
        /// Gets or sets the value assosiated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value assosiated with the specified key.</returns>
        public BElement this[string key]
        {
            get
            {
                return this[new BString(key)];
            }
            set
            {
                this[new BString(key)] = value;
            }
        }
    }

    /// <summary>
    /// A bencoding exception.
    /// </summary>
    public class BencodingException : FormatException
    {
        /// <summary>
        /// Creates a new BencodingException.
        /// </summary>
        public BencodingException() { }

        /// <summary>
        /// Creates a new BencodingException.
        /// </summary>
        /// <param name="message">The message.</param>
        public BencodingException(string message) : base(message) { }

        /// <summary>
        /// Creates a new BencodingException.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public BencodingException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// A class with extension methods for use with Bencoding.
    /// </summary>
    public static class BencodingExtensions
    {
        /// <summary>
        /// Decode the current instance.
        /// </summary>
        /// <param name="s">The current instance.</param>
        /// <returns>The root elements of the decoded string.</returns>
        public static BElement[] BDecode(this string s)
        {
            return BencodeDecoder.Decode(s);
        }
    }
}
