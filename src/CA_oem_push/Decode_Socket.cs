using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace CA_oem_push
{
    public class Decode_Socket
    {
        public static string api_key
        {
            get { return "place correct key here"; }
        }

        public static string api_secret
        {
            get { return "place correct secret here"; }
        }

        public static Encoding my_ecoding
        {
            get { return Encoding.GetEncoding("latin1"); }
        }

        public static IPAddress oem_ip
        {
            get { return IPAddress.Parse("place ip here"); }
        }

        public static int oem_port
        {
            //place correct port here
            get { return 8000; }
        }

        public static string _crypt(string data, int[] box)
        {
            int x = 0, y = 0;
            byte[] b_data = my_ecoding.GetBytes(data);
            byte[] b_re = new byte[b_data.Length];
            for (int i = 0; i < b_data.Length; i++)
            {
                x = (x + 1) % 256;
                y = (y + box[x]) % 256;
                int b_swap = box[x];
                box[x] = box[y];
                box[y] = b_swap;

                int _o = box[(box[x] + box[y]) % 256];
                int _b_i = b_data[i] ^ _o;
                b_re[i] = (byte)_b_i;
            }
            return my_ecoding.GetString(b_re);
        }

        public static string rc4_decode(string encoding)
        {
            string key = api_secret;
            int x = 0;
            int[] box = new int[256];
            for (int i = 0; i < 256; i++)
            {
                box[i] = i;
            }
            for (int i = 0; i < 256; i++)
            {
                string c_k = key[i % key.Length].ToString();
                x = (x + box[i] + my_ecoding.GetBytes(c_k)[0]) % 256;
                int b_swap = box[x];
                box[x] = box[i];
                box[i] = b_swap;
            }

            return _crypt(encoding, box);
        }
    }
}
