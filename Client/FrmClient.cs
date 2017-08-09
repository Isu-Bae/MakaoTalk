using MakaoTalk.Models.ViewModel;
using MakaoTalk.Services.Message;
using MakaoTalk.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FrmClient : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;
        private readonly IMessageService _messageService;

        public FrmClient()
        {
            InitializeComponent();
            _messageService = new MessageService();
        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(this.textBoxMessage.Text + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            textBoxMessage.Text = string.Empty;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            clientSocket.Connect("192.168.245.56", 9999);
            stream = clientSocket.GetStream();

            message = "Connected to Chat Server";
            DisplayText(message);

            byte[] buffer = Encoding.Unicode.GetBytes(this.textBoxNickName.Text + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Thread t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            t_handler.Start();
        }

        private void GetMessage()
        {
            while (true)
            {
                stream = clientSocket.GetStream();
                int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[BUFFERSIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);

                string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                DisplayText(message);
                SaveMessage(message);
            }
        }

        private void DisplayText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(text + Environment.NewLine);
                }));
            }
            else
                richTextBox1.AppendText(text + Environment.NewLine);
        }

        private void SaveMessage(string message)
        {
            _messageService.SaveMessage(new ModelChat {
                Contents = message,
                SendDate = DateTime.Now,
                UserID = "010-7127-0202",
                UserName = "전강태"
            });
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Enter:
                    btnSendText.PerformClick();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
