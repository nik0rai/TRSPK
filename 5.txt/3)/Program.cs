using System;
using System.IO;
using System.Text;

class Test
{
    public static void PrintCountsAndBytes(string chars, Encoding enc)
    {
        // Display the name of the encoding used.
        Console.Write("Encoding: {0,-30} :", enc.ToString() + Environment.NewLine);

        // Display the exact byte count.
        int iBC = enc.GetByteCount(chars);
        Console.Write("The exact byte count: {0,-3}", iBC + Environment.NewLine);

        // Display the maximum byte count.
        int iMBC = enc.GetMaxByteCount(chars.Length);
        Console.Write("Maximum byte count: {0,-3} :", iMBC + Environment.NewLine);

        // Encode the array of chars.
        byte[] bytes = enc.GetBytes(chars);

        // Display all the encoded bytes.
        PrintHexBytes(bytes);
    }

    public static void PrintHexBytes(byte[] bytes)
    {

        if ((bytes == null) || (bytes.Length == 0))
        {
            Console.WriteLine("<none>");
        }
        else
        {
            for (int i = 0; i < bytes.Length; i++)
                Console.Write("{0:X2} ", bytes[i]);
            Console.WriteLine(Environment.NewLine);
        }
    }
    public static void Main()
    {
        string path = @"c:\temp\MyTest.txt";

        //if (!File.Exists(path))
        //{
        string createText = " UTF8: Dogs are better than cats." + Environment.NewLine + "You can't deny that." + Environment.NewLine + Environment.NewLine;
        File.WriteAllText(path, createText, Encoding.UTF8);
    //
    // }

        string createAnotherText = " ASCII: Dogs are better than cats." + Environment.NewLine + "You can't deny that." + Environment.NewLine + Environment.NewLine;
        File.AppendAllText(path, createAnotherText, Encoding.ASCII);
       
        string createOneMoreText = " Latin1: Dogs are better than cats." + Environment.NewLine + "You can't deny that." + Environment.NewLine + Environment.NewLine;
        File.AppendAllText(path, createOneMoreText, Encoding.Latin1);
        
        string createMoreText = " BigEndianUnicode: Dogs are better than cats." + Environment.NewLine + "You can't deny that." + Environment.NewLine + Environment.NewLine;
        File.AppendAllText(path, createMoreText, Encoding.BigEndianUnicode);
        
        string createMoreMoreText = " Unicode: Dogs are better than cats." + Environment.NewLine + "You can't deny that." + Environment.NewLine + Environment.NewLine;
        File.AppendAllText(path, createMoreMoreText, Encoding.Unicode);
        
        string appendText = "That's all I wanted to say. Bye!" + Environment.NewLine + Environment.NewLine;
        File.AppendAllText(path, appendText);

        string readText = File.ReadAllText(path);
        Console.WriteLine(readText);

        PrintCountsAndBytes(createText, Encoding.UTF8);
        PrintCountsAndBytes(createAnotherText, Encoding.ASCII);
        PrintCountsAndBytes(createOneMoreText, Encoding.Latin1);
        PrintCountsAndBytes(createMoreText, Encoding.BigEndianUnicode);
        PrintCountsAndBytes(createMoreMoreText, Encoding.Unicode);
    }
}