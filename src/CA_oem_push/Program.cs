using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CA_oem_push
{
    class Program
    {
        static void Main(string[] args)
        {
            //call knock
            Oem_Push_Socket client_socket = new Oem_Push_Socket();
            string s_k = client_socket.knock();
            Console.Write("knock:" + s_k);

            //call rc4, this number is just an example ;)
            string s_k_num = "209879754";
            string s_k_crypt_num = Decode_Socket.rc4_decode(s_k_num);
            Console.WriteLine("decrypted num: {0}", s_k_crypt_num);
            s_k_num = Decode_Socket.rc4_decode(s_k_crypt_num);
            Console.WriteLine("org num: ", s_k_num);
        }
    }
}
