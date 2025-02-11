using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Services.Common
{
    public class LocalHttpServer : ServiceManagerBase,IServiceManager
    {
        HttpListener listener;

        public void Start()
        {
            IsRunning = true;
            Handler = new Thread(Main);
            Handler.Start();
        }

        public void Stop() 
        {
            IsRunning = false;
            if(listener != null && listener.IsListening)
            {
                listener.Stop();
                listener.Close();
            }
            if (Handler != null && Handler.IsAlive)
            {
                Handler.Join();
            };

            listener = null;
            Handler = null;
        }

        public async void Main()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();

            while (IsRunning)
            {
                try
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    ProcessRequest(context);
                    Debug.WriteLine("Get Process Request");
                }
                catch (HttpListenerException ex)
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string responseString = "<HTML><BODY><h1>Hello from WinForm Local Server!</h1></BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }
    }
}
