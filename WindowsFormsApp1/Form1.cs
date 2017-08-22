using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string fullnet = "";
        string ipclient = "";
        string gw = "";
        string result_broad = "";
        string mask = "";
        string count_ip = "Клиентские ip адреса";
        public Form1()
        {            
            InitializeComponent();
            textBox1.Text = GetLocalIpAddress();
            ToolTip t = new ToolTip();
            t.SetToolTip(button3, "Показывает внешний ip адрес");
            ToolTip t2 = new ToolTip();
            t2.SetToolTip(button2, "Копирует блок \"для клиента\" в изображение");


        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public static string GetLocalIpAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(hostName);
            foreach (IPAddress ipAddress in ip.AddressList)
            {
                if (IPAddress.Parse(ipAddress.ToString()).AddressFamily == AddressFamily.InterNetwork)
                {

                    string IpAddress = Convert.ToString(ip.AddressList[2]);
                    return ipAddress.ToString();
                }
            }
            return "192.168.1.1";
        }
    

        private bool validateIpMaskString(String stringValue)
        {
            string[] ipMaskParts = stringValue.Split('/');
            if (ipMaskParts.Length != 2)
                return false;
            int afterSlashInt;
            if (!int.TryParse(ipMaskParts[1], out afterSlashInt) && afterSlashInt < 0 && afterSlashInt > 32)
                return false;
            string[] ipParts = ipMaskParts[0].Split('.');
            if (ipParts.Length != 4)
                return false;
            foreach (String partString in ipParts)
            {
                int partInt;
                if (!int.TryParse(partString, out partInt) && partInt < 0 && partInt > 255)
                    return false;
            }
            return true;
        }

        private UInt32 ipFromStringToInt(String stringValue)
        {
            UInt32 intValue = 0;
            foreach (String stringPart in stringValue.Split('.'))
            {
                intValue <<= 8;
                intValue |= UInt32.Parse(stringPart);
            }
            return intValue;
        }

        private String ipFromIntToString(UInt32 intValue)
        {
            return ((intValue >> 24) & 255).ToString() + "." + ((intValue >> 16) & 255).ToString() + "." + ((intValue >> 8) & 255).ToString() + "." + (intValue & 255).ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string prefix = numericUpDown1.Text;
            string putfullip = textBox1.Text + "/" + prefix;
            if (validateIpMaskString(putfullip))
            {
                UInt32 ipAddressInt = ipFromStringToInt(textBox1.Text.Split('/')[0]);
                UInt32 subnetMask = UInt32.MaxValue << (32 - int.Parse(putfullip.Split('/')[1]));
                UInt32 subnetNumber = ipAddressInt & subnetMask;
                UInt32 broadcastAddress = subnetNumber | ~subnetMask;
                UInt32 nextNet = broadcastAddress + 1;
                UInt32 ipend = broadcastAddress -1;
                UInt32 ipall = broadcastAddress - subnetNumber;
                gw = ipFromIntToString(subnetNumber + 1);
                fullnet = ipFromIntToString(subnetNumber) + "/" + prefix;
                string formatIp = ipFromIntToString(broadcastAddress - subnetNumber +1);
                string formatIp1;
                string formatip2 = ipFromIntToString(broadcastAddress - subnetNumber -2);
                string formatip3 = formatip2.Split('.')[3];
                string endipformat = ipFromIntToString(ipend);                
                string endip = endipformat.Split('.')[3];
                int ip_ip = Convert.ToInt32(ipall+1);
                int ip_ip1 = Convert.ToInt32(ipall -2);
                mask = ipFromIntToString(subnetMask);
                if (prefix == "24")

                {
                    formatIp1 = "255";
                }
                else
                {
                    formatIp1 = formatIp.Split('.')[3];
                }
                if (Convert.ToInt32(prefix) < 24)
                {
                    endip = endipformat.Split('.')[2] + "." + endipformat.Split('.')[3];
                }
                textBox2.Text = (fullnet);
                textBox4.Text = gw;
                textBox5.Text = ipFromIntToString(subnetMask);
                textBox9.Text = ipFromIntToString(broadcastAddress);
                textBox8.Text = ip_ip.ToString();
                textBox7.Text = ip_ip1.ToString();
                if (prefix == "30")
                {
                    ipclient = ipFromIntToString(subnetNumber + 2);
                    count_ip = "Клиентский ip адрес";
                }
                else
                {
                    ipclient = ipFromIntToString(subnetNumber + 2) + " - " + endip;
                    count_ip = "Клиентские ip адреса";
                }
                textBox3.Text = ipclient;
                textBox6.Text = "80.241.32.10";
                textBox10.Text = "80.241.32.18";
                textBox19.Text = (fullnet);
                textBox18.Text = ipclient;
                textBox17.Text = ipFromIntToString(subnetMask);
                textBox16.Text = gw;                
                textBox15.Text = ipFromIntToString(broadcastAddress);
                textBox14.Text = ip_ip.ToString();
                textBox13.Text = ip_ip1.ToString();
                textBox12.Text = ipFromIntToString(broadcastAddress - subnetNumber);
                textBox11.Text = ipFromIntToString(nextNet);
                result_broad = ipFromIntToString(broadcastAddress);




            }
            else
                MessageBox.Show("Введена строка не верного формата");

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

      
        private void button2_Click(object sender, EventArgs e)
        {
            Point scrPos = this.PointToScreen(groupBox1.Location);
            Size scrP = new Size(260, 285);
            Rectangle rect = new Rectangle(scrPos, scrP);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            Clipboard.SetImage(bmp);
        }


        private void button3_Click(object sender, EventArgs e)
        {

            Ping p = new Ping();
            PingReply r;

            String s = "dyndns.org"; //Replace input from your text box for IP address here

            r = p.Send(s);
            

            if (r.Status == IPStatus.Success)
            {
                string url = "http://checkip.dyndns.org";
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
               
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                string[] a = response.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];
                textBox1.Text = a4;
            }

            else
            {
                MessageBox.Show("Интернет соединение отсутствует");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String asciiString = string.Format("Клиентская сеть: {0}\n{1}: {2}\nМаска сети: {3}\nШлюз: {4}\n" +
                "DNS1: 80.241.32.10\nDNS2: 80.241.32.18", fullnet, count_ip, ipclient, mask, gw);
            Clipboard.SetText(asciiString);
        }
    }
}
