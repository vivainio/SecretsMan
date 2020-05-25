using System;
using System.Text;

namespace SecretsMan
{
    public static class StringExtensions
    {
        public static byte[] ToUtf8(this string text) => Encoding.UTF8.GetBytes(text);

        // woo, sane string split
        public static string[] Split(this string s, string sep, int count) =>
            s.Split(new[] {sep}, count, StringSplitOptions.None);

        public static string Decode(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

    }
}