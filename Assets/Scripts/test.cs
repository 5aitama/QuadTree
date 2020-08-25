using System.IO.Compression;
using System.Collections.Generic;
using System;

#pragma warning disable 414, 3021

public interface IInterface {

}

// Hello world this is a
namespace MyApplication
{
    [Obsolete("...")]
    class Program : IInterface
    {
        public static List<int> JustDoIt(int count)
        {
            var name = "Hello world";
            Console.WriteLine($"Hello {name}!");
            return new List<int>(new int[] { 1, 2, 3 });
        }
    }
}