using System;
using System.IO;

namespace Decrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Path:");

            foreach (string FileName in Directory.GetFiles(Console.ReadLine()))
            {
                File.WriteAllBytes(FileName + ".dec", Decrypt(File.ReadAllBytes(FileName)));
                Console.WriteLine("Decrypt " + FileName + " Successed");
            }
        }

        static byte[] Decrypt(byte[] Data)
        {
            byte[] Result = new byte[Data.Length];
            Data.CopyTo(Result, 0);
            if (BitConverter.ToUInt64(Result, Result.Length - 8) == 0x7366624F7A456E58) // 是加密文件
            {
                byte[] Key = BitConverter.GetBytes(BitConverter.ToInt32(Result, Result.Length - 12)); // 读取解密秘钥
                byte[] EncryptedLength = BitConverter.GetBytes(BitConverter.ToInt32(Result, Result.Length - 16)); // 读取被加密了的长度

                for (int i = 0; i < 4; i++)
                {
                    EncryptedLength[i] ^= Key[i]; // 解密长度
                }

                int Length = BitConverter.ToInt32(EncryptedLength, 0);

                for (int i = 0; i < Length; i++)
                {
                    Result[i] ^= Key[i % 4]; // 解密加密部分
                }
            }
            return Result;
        }
    }
}
