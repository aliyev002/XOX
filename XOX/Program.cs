using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class TicTacToeServer
{
    static void Main(string[] args)
    {
        var ip = IPAddress.Parse("26.230.244.228");
        var port = 27001;
        var listener = new TcpListener(ip, port);

        try
        {
            listener.Start();
            Console.WriteLine($"Server {ip}:{port} unvaninda isleyir...");

            TcpClient player1 = listener.AcceptTcpClient(); 
            Console.WriteLine("Oyuncu 1 qoshuldu.");
            SendMessage(player1, "Siz X ile oynayirsiniz. Oyuncu gozlenilir...");

            TcpClient player2 = listener.AcceptTcpClient(); 
            Console.WriteLine("Oyuncu 2 qosuldu.");
            SendMessage(player2, "Siz O ile oynayirsiniz. Oyun baslayir!");

            bool isPlayer1Turn = true;
            char[] board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            while (true)
            {
                var currentPlayer = isPlayer1Turn ? player1 : player2;
                var opponentPlayer = isPlayer1Turn ? player2 : player1;
                SendMessage(currentPlayer, "Sizin novbeniz. Lovheni gosterin və hereket edin.");

                
                string move = ReceiveMessage(currentPlayer);
                if (int.TryParse(move, out int position) && position >= 1 && position <= 9 && board[position - 1] != 'X' && board[position - 1] != 'O')
                {
                    board[position - 1] = isPlayer1Turn ? 'X' : 'O';

                   
                    SendMessage(player1, BoardToString(board));
                    SendMessage(player2, BoardToString(board));

                    
                    if (CheckWinner(board))
                    {
                        SendMessage(currentPlayer, "Siz qalib geldiniz!");
                        SendMessage(opponentPlayer, "Siz meglub oldunuz.");
                        break;
                    }
                    else if (CheckDraw(board))
                    {
                        SendMessage(player1, "Hec-hece.");
                        SendMessage(player2, "Hec-hece.");
                        break;
                    }

                    
                    isPlayer1Turn = !isPlayer1Turn;
                }
                else
                {
                    SendMessage(currentPlayer, "Yanlıs hereket! Yeniden cehd edin.");
                }
            }

            player1.Close();
            player2.Close();
            listener.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta: {ex.Message}");
        }
    }

   
    static void SendMessage(TcpClient client, string message)
    {
        var data = Encoding.UTF8.GetBytes(message + "\n");
        client.GetStream().Write(data, 0, data.Length);
    }

    
    static string ReceiveMessage(TcpClient client)
    {
        var sr = new StreamReader(client.GetStream());
        return sr.ReadLine();
    }

    
    static string BoardToString(char[] board)
    {
        return $"{board[0]} | {board[1]} | {board[2]}\n--+---+--\n{board[3]} | {board[4]} | {board[5]}\n--+---+--\n{board[6]} | {board[7]} | {board[8]}";
    }

    
    static bool CheckWinner(char[] board)
    {
        int[,] winningCombinations = {
            { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
            { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        for (int i = 0; i < winningCombinations.GetLength(0); i++)
        {
            if (board[winningCombinations[i, 0]] == board[winningCombinations[i, 1]] &&
                board[winningCombinations[i, 1]] == board[winningCombinations[i, 2]])
            {
                return true;
            }
        }
        return false;
    }

    
    static bool CheckDraw(char[] board)
    {
        foreach (var position in board)
        {
            if (position != 'X' && position != 'O') return false;
        }
        return true;
    }
}
