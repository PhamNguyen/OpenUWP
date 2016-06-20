﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace OpenUWP.Extentions
{
    public static class StringExtentions
    {
        public static byte[] ToBytesBase32(this string input)
        {
            if (input == string.Empty)
                throw new ArgumentException("input");

            input = input.TrimEnd('='); //remove padding characters
            int byteCount = input.Length * 5 / 8; //this must be TRUNCATED
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int mask = 0, arrayIndex = 0;

            foreach (char c in input.ToUpper().ToCharArray())
            {
                int cValue = CharToValue(c);

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
                returnArray[arrayIndex] = curByte;

            return returnArray;
        }

        public static string ToStringBase32(this byte[] input)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException("input");

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static int CharToValue(char c)
        {
            int value = (int)c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
                return value - 65;
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
                return value - 24;
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
                return value - 97;

            throw new ArgumentException("Character is not a Base32 character.", "c");
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
                return (char)(b + 65);

            if (b < 32)
                return (char)(b + 24);

            throw new ArgumentException("Byte is not a value Base32 value.", "b");
        }

        public static string TrimAll(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                string _input = input.Trim().TrimDublicate('\r');
                string result = _input.TrimDublicate('\n');
                return result;
            }
            return String.Empty;
        }

        public static string TrimDublicate(this string input, char dublicateCharater)
        {
            string _input = input.Trim('\n').Trim('\r').Trim();

            StringBuilder resultStringBuilder = new StringBuilder();

            string[] inputData = _input.Split(dublicateCharater);

            foreach (var item in inputData)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    resultStringBuilder.Append(String.Format("{0}{1}", item, dublicateCharater));
                }
            }
            string rawResult = resultStringBuilder.ToString();
            if (!String.IsNullOrEmpty(rawResult))
            {
                string result = rawResult.Trim('\n', '\r', dublicateCharater);
                return result;
            }
            return String.Empty;
        }

        public static string Raw(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                var _rawString = input.TrimAll();
                string result = _rawString.Replace("\n", " ").Replace("\r", " ");
                return result;
            }
            return String.Empty;
        }
        public static string RawWith(this string input, string midle)
        {
            if (!String.IsNullOrEmpty(input))
            {
                var _rawString = input.TrimAll();
                string result = _rawString.Replace("\n", midle).Replace("\r", midle);
                return result;
            }
            return String.Empty;
        }

        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length || input.IndexOf(" ", length) == -1)
                return input;

            return input.Substring(0, input.IndexOf(" ", length));
        }

        public static string ToBase64String(this string input)
        {

            // Decoded the string from Base64 to binary.
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);

            // Encode the buffer back into a Base64 string.
            String base64Data = CryptographicBuffer.EncodeToBase64String(buffer);

            return base64Data;
        }

        public static string FromBase64String(this string input)
        {
            // Decoded the string from Base64 to binary.
            IBuffer buffer = CryptographicBuffer.DecodeFromBase64String(input);

            // Encode the buffer back into a Base64 string.
            String rawString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffer);

            return rawString;
        }

        public static string HtmlToText(this string content)
        {
            if (String.IsNullOrEmpty(content))
                return content;

            var doc = new HtmlDocument();

            doc.LoadHtml(content);

            if (doc.DocumentNode != null)
            {
                return doc.DocumentNode.InnerText.Trim();
            }

            return string.Empty;
        }

        #region Vùng kiểm tra xem có phải là email or phone number ko?

        private static string patternEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private static string patternPhone = @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})";
        private static string patternOnlyNumber = @"^[0-9]+$";

        /// <summary>
        /// Kiểm tra xem chuổi string này có phải là một địa chỉ email hay không?
        /// </summary>
        /// <param name="input">Chuỗi string cần kiểm tra</param>
        /// <returns>Trả về true nếu nó là địa chỉ email đúng cú pháp. Ngược lại, trả về false.</returns>
        public static bool IsEmail(this string input)
        {
            var result = false;

            if (!string.IsNullOrEmpty(input))
            {
                if (new Regex(patternEmail).IsMatch(input))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra xem chuổi string này có phải là một số điện thoại hay không?
        /// </summary>
        /// <param name="input">Chuỗi string cần kiểm tra</param>
        /// <returns>Trả về true nếu nó là số điện thoại đúng cú pháp. Ngược lại, trả về false.</returns>
        public static bool IsPhoneNumber(this string input)
        {
            var result = false;

            if (!string.IsNullOrEmpty(input))
            {
                if (new Regex(patternPhone).IsMatch(input))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra xem chuổi string này có phải là một dãy số hay không?
        /// </summary>
        /// <param name="input">Chuỗi string cần kiểm tra</param>
        /// <returns>Trả về true nếu nó là một dãy số. Ngược lại, trả về false.</returns>
        public static bool IsDigit(this string input)
        {
            var result = false;

            if (!string.IsNullOrEmpty(input))
            {
                if (new Regex(patternOnlyNumber).IsMatch(input))
                    result = true;
            }

            return result;
        }
        #endregion
    }
}
