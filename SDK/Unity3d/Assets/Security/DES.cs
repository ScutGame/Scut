using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;


public class DES
{
    private static DESCryptoServiceProvider m_desKey = new DESCryptoServiceProvider();
    private static byte[] m_iv;

    public static string StringKey
    {
        get { return Encoding.Default.GetString(m_desKey.Key); }
        set
        {
            m_desKey.BlockSize = Encoding.Default.GetByteCount(value) * 8;
            m_desKey.Key = Encoding.Default.GetBytes(value);
        }
    }

    public static byte[] ByteKey
    {
        get { return m_desKey.Key; }
        set
        {
            m_desKey.BlockSize = value.Length * 8;
            m_desKey.Key = value;
        }
    }

    public static byte[] IV
    {
        get { return m_iv; }
        set { m_iv = value; }
    }

    static DES()
    {
        m_iv = new byte[] { 0x70, 0x06, 0x09, 0x21, 0x3A, 0x8B, 0x4F, 0x1D };
        m_desKey.Key = new byte[] { 0x70, 0x06, 0x09, 0x21, 0x3A, 0x8B, 0x4F, 0x1D };
    }

    #region Encrypt
    public static string EncryptString(string input)
    {
        return Encoding.UTF8.GetString(Encrypt(m_desKey.Key, m_iv, input));
    }

    public static byte[] Encrypt(string input)
    {
        return Encrypt(m_desKey.Key, m_iv, input);
    }

    public static byte[] Encrypt(byte[] input)
    {
        return Encrypt(m_desKey.Key, m_iv, Encoding.Default.GetString(input));
    }

    public static byte[] Encrypt(string key, string input)
    {
        return Encrypt(Encoding.Default.GetBytes(key), m_iv, input);
    }

    public static byte[] Encrypt(string key, byte[] input)
    {
        return Encrypt(Encoding.Default.GetBytes(key), m_iv, Encoding.Default.GetString(input));
    }

    public static byte[] Encrypt(byte[] key, string input)
    {
        return Encrypt(key, m_iv, input);
    }

    public static byte[] Encrypt(byte[] key, byte[] input)
    {
        return Encrypt(key, m_iv, Encoding.Default.GetString(input));
    }

    public static byte[] Encrypt(byte[] key, byte[] iv, byte[] input)
    {
        return Encrypt(key, iv, Encoding.Default.GetString(input));
    }

    public static byte[] Encrypt(byte[] key, byte[] iv, string input)
    {
        if (key == null || iv == null || input == null)
        {
            return null;
        }

        // Create a memory stream.
        MemoryStream ms = new MemoryStream();

        // Create a CryptoStream using the memory stream and the 
        // CSP DES key.  
        CryptoStream encStream = new CryptoStream(ms, m_desKey.CreateEncryptor(key, iv), CryptoStreamMode.Write);

        // Create a StreamWriter to write a string
        // to the stream.
        StreamWriter sw = new StreamWriter(encStream);

        // Write the plaintext to the stream.
        sw.Write(input);

        // Close the StreamWriter and CryptoStream.
        sw.Close();
        encStream.Close();

        // Get an array of bytes that represents
        // the memory stream.
        byte[] buffer = ms.ToArray();

        // Close the memory stream.
        ms.Close();

        // Return the encrypted byte array.
        return buffer;
    }
    #endregion

    #region Decrypt
    public static string Decrypt(byte[] input)
    {
        return Decrypt(m_desKey.Key, m_iv, input);
    }
    public static string Decrypt(string key, byte[] input)
    {
        return Decrypt(Encoding.Default.GetBytes(key), m_iv, input);
    }

    public static string Decrypt(byte[] key, byte[] input)
    {
        return Decrypt(key, m_iv, input);
    }

    public static string Decrypt(byte[] key, byte[] iv, byte[] input)
    {
        if (key == null || iv == null || input == null)
        {
            return null;
        }

        // Create a memory stream to the passed buffer.
        MemoryStream ms = new MemoryStream(input);

        // Create a CryptoStream using the memory stream and the 
        // CSP DES key. 
        CryptoStream encStream = new CryptoStream(ms, m_desKey.CreateDecryptor(key, iv), CryptoStreamMode.Read);

        // Create a StreamReader for reading the stream.
        StreamReader sr = new StreamReader(encStream);

        // Read the stream as a string.
        string val = sr.ReadToEnd();

        // Close the streams.
        sr.Close();
        encStream.Close();
        ms.Close();

        return val;
    }
    #endregion
}
