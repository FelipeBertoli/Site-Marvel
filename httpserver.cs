using Systen.Net.Sockets;

class HttpServer
{
    private TcpListener Controller { get; set; }
    private int Port { get; set; }
    private int RequestAmnt { get; set; }

    public HttpServer(int por = 8080)
    {
        this.Port = Port;
        try
        {
            this.Controller = new TcpListener(IPAddress.Parse("127.0.0.1"), this.Port);
            this.Controller.Start();
            Console.WriteLine($"Servidor HTTP está rodando na porta {this.Port}.");
            Console.WriteLine($"Para acessar, digite no navegador: http://localhost: {this.Port}.");
        Task httpTaskServer = Task.Run(() => AwaitRequest());
        httpTaskServer.GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao iniciar servidor na porta {this.Port}:{e.Message}");
        }
    }

    private async Task AwaitRequest()
    {
        while (true)
        {
            Socket connection = await this.Controller.AcceptSocketAsync();
            this.RequestAmnt++;
            Task task = Task.Run(() => ProcessRequest(connection,this.RequestAmnt));
        }
    }

    private void ProcessRequest(Socket connection, int requestNum)
    {
        Console.WriteLine($"Processando request #{requestNum}...\n");
        if (connection.Connected)
        {
            byte[] requisitionBytes = new byte[1024];
            connection.Receive(requisitionBytes, requisitionBytes.Length, 0);
            string requisitionText = Encoding.UTF8.GetString(requistionBytes)
                                    .Replace((char)0, ' ').Trim(0);
            if (requisitionText.Length > 0) {
                Console.WriteLine($"\n`{requisitionText}\n");
                connection.Close();
            }
        }
        Console.WriteLine($"\nRequest {requestNum} finalizado");
    }
}


