using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace CA_oem_push
{
    public class Oem_Push_Socket
    {
        public Oem_Push_Socket()
        {
        }

        private static byte[] result = new byte[1024];

        private Socket create_oem_client()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(Decode_Socket.oem_ip, Decode_Socket.oem_port));
                connect_state = true;
                return clientSocket;
            }
            catch
            {
                return null;
            }
        }

        public string encode_params()
        {
            return "";
        }

        internal byte[] GetBytesBE(ulong raw, int count)
        {
            byte[] buf = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buf[i] = (byte)((raw >> ((count - i - 1) * 8)) & 0xff);
            }

            return buf;
        }

        public byte[] send_byte(string send_str, bool include_head = true)
        {
            if (!include_head)
            {
                return Decode_Socket.my_ecoding.GetBytes(send_str);
            }
            // 大端编码
            byte[] head = GetBytesBE((ulong)send_str.Length, 4);

            byte[] buffer = new byte[6 + send_str.Length];
            Buffer.BlockCopy(Decode_Socket.my_ecoding.GetBytes("*1"), 0, buffer, 0, 2);
            Buffer.BlockCopy(head, 0, buffer, 2, 4);
            Buffer.BlockCopy(Decode_Socket.my_ecoding.GetBytes(send_str), 0, buffer, 6, send_str.Length);

            return buffer;
        }

        public string knock()
        {
            try
            {
                List<object> l_o = new List<object>();
                l_o.Add("knock");
                l_o.Add("0");
                l_o.Add(Decode_Socket.api_key);
                oem_client.Send(send_byte(Encoding_Redis.wrap(l_o)));
                int receive_len = oem_client.Receive(result);
                return Decode_Socket.my_ecoding.GetString(result, 0, receive_len);
            }
            catch (Exception ex)
            {
                connect_state = false;
                return ex.Message;
            }
        }

        private bool connect_state = false;
        private Socket _oem_client;
        private Socket oem_client
        {
            get
            {
                if (_oem_client == null)
                {
                    _oem_client = create_oem_client();
                }
                if (connect_state == false)
                {
                    _oem_client = create_oem_client();
                }
                return _oem_client;
            }
        }
    }
}
