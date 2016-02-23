using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUWP.Utils
{
    public class BaseRC4
    {
        public static byte[] Encrypt(byte[] privateKey, byte[] data)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = privateKey[i % privateKey.Length];
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }
            return cipher;
        }

        public static string Encrypt(string key, string data)
        {
            if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(data))
            {
                var _keys = Encoding.UTF8.GetBytes(key);
                var _data = Encoding.UTF8.GetBytes(data);
                var _result = Encrypt(_keys, _data);
                return Convert.ToBase64String(_result); 
            }
            return data;
        }

        public static string Decrypt(string key, string data)
        {
            var _keys = Encoding.UTF8.GetBytes(key);
            var _data = Convert.FromBase64String(data);
            var _decodeData = Decrypt(_keys, _data);
            return Encoding.UTF8.GetString(_decodeData, 0, _decodeData.Length);
        }

        public static byte[] Decrypt(byte[] privateKey, byte[] data)
        {
            return Encrypt(privateKey, data);
        }

    }
}
