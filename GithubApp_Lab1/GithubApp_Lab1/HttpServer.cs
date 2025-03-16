using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GithubApp_Lab1
{
    public class HttpServer
    {
        private readonly Form1 _form;
        private HttpListener _httpListener;

        public HttpServer(Form1 form)
        {
            _form = form;
        }

        public async Task StartServer()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost:8080/callback/");
            _httpListener.Start();

            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                var response = context.Response;

                var code = context.Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    await _form.HandleRedirect(code);
                }

                string responseString = "Successful receipt of token";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }

        public void StopServer()
        {
            _httpListener.Stop();
        }
    }
}
