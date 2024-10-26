using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class TicTacToeClient
{
    static void Main(string[] args)
    {
        TcpClient client = new TcpClient("26.230.244.228", 27001);
        var networkStream = client.GetStream();
        var sr = new StreamReader(networkStream);
        var sw = new StreamWriter(networkStream) { AutoFlush = true };

        Console.WriteLine("Serverə qoshuldu");

        while (true)
        {
            
            string message = sr.ReadLine();
            Console.WriteLine(message);

            if (message.Contains("qalib") || message.Contains("meglub") || message.Contains("Hec-hece"))
            {
                break; 
            }

            if (message.Contains("novbeniz"))
            {
                
                Console.Write("Hereket edin (1-9): ");
                string move = Console.ReadLine();
                sw.WriteLine(move);
            }
        }

        client.Close();
    }
}
