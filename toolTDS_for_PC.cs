using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEW_SPAMREPORT2021
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }
        List<string> MyListCookie = new List<string>();

        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        void fadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                t1.Stop();   //this stops the timer if the form is completely displayed
            else
                Opacity += 0.05;
        }

        bool isPREMIUM = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox9.Checked = HCTDSMINE.Properties.Settings.Default.like;
            checkBox11.Checked = HCTDSMINE.Properties.Settings.Default.react;
            checkBox10.Checked = HCTDSMINE.Properties.Settings.Default.follow;
            checkBox14.Checked = HCTDSMINE.Properties.Settings.Default.reactcmt;
            checkBox12.Checked = HCTDSMINE.Properties.Settings.Default.cmt;
            checkBox13.Checked = HCTDSMINE.Properties.Settings.Default.share;
            textBox5.Text = HCTDSMINE.Properties.Settings.Default.txt5;
            textBox3.Text = HCTDSMINE.Properties.Settings.Default.txttoken;
            textBox1.Text = HCTDSMINE.Properties.Settings.Default.txt1;
            textBox4.Text = HCTDSMINE.Properties.Settings.Default.txt4;
            textBox7.Text = HCTDSMINE.Properties.Settings.Default.usernamelogin;
            textBox8.Text = HCTDSMINE.Properties.Settings.Default.passlogin;
            checkBox3.Checked = HCTDSMINE.Properties.Settings.Default.autotds;
            checkBox8.Checked = HCTDSMINE.Properties.Settings.Default.autofb;
            checkBox5.Checked = HCTDSMINE.Properties.Settings.Default.mahoacookie;
            checkBox6.Checked = HCTDSMINE.Properties.Settings.Default.mahoatoken;
            checkBox7.Checked = HCTDSMINE.Properties.Settings.Default.autochuyentk;
            Opacity = 0;
            t1.Interval = 10;
            t1.Tick += new EventHandler(fadeIn);
            t1.Start();

            label21.Text = listBox1.Items.Count.ToString();

            if (File.Exists("cookie.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("cookie.txt");
                foreach (string str in lines)
                {
                    if (str != "")
                    {
                        listBox1.Items.Add(str);
                    }
                }
            }
            else
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter("cookie.txt");
                writer.Write("");
                writer.Close();           
            }

            MyListCookie = listBox1.Items.OfType<string>().ToList();
            panel2.Hide();
            panel11.Hide();
            panel9.Hide();
            groupBox2.Hide();
            panel21.Hide();
            panel19.Hide();
            webBrowser5.Hide();
            Random rnd = new Random();
            int code = rnd.Next(1000, 9999);
            groupBox1.Text = "Thanh toán premium [MOMO: 0397929246 HUYNH CHI HAI] [MSG CODE: " + code.ToString() + "]";
            msgcode = code.ToString();
            button5.Hide();
            button6.Hide();
            if (listBox1.Items.Count > 0)
            {
                checkBox8.Checked = true;
            }
            if (textBox3.Text != "")
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }
            if (checkBox12.Checked == true)
            {
                panel9.Show();
                listBox2.Items.Clear();
            }
            else
            {
                panel9.Hide();
            }

        }
        string msgcode;
        private void button3_Click(object sender, EventArgs e)
        {
            if(File.Exists("cookie.txt"))
            {
                System.IO.File.WriteAllLines("cookie.txt", listBox1.Items.Cast<string>().ToArray());
               
            }else
            {
                File.Create("cookie.txt");
            }
            this.Close();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        int i = 1;
        int listcook = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            if(isConnectTDS == true && isFB == true)
            {
                if (Convert.ToInt32(textBox1.Text) < 5)
                {
                    MessageBox.Show("Giá trị không được nhỏ hơn 5", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "5";
                }
                else if (Convert.ToInt32(textBox4.Text) < 5)
                {
                    MessageBox.Show("Giá trị không được nhỏ hơn 5", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox4.Text = "5";
                }
                else
                {
                    button5.Enabled = false;
                    button6.Enabled = true;
                    DatCauHinh();
                    label14.Text = "Đang hoạt động";
                    timerLike.Interval = Convert.ToInt32(textBox1.Text + "000");
                    timerNhanTien.Interval = Convert.ToInt32(textBox1.Text + "000") + 600;
                    timerNghiSau1NhiemVu.Interval = Convert.ToInt32(textBox4.Text + "000");
                }
            } else
            {
                MessageBox.Show("Vui lòng kiểm tra lại cookie FB và token TDS", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string tokentds = "";
        void DatCauHinh()
        {
            WebClient info = new WebClient();
            info.Encoding = Encoding.UTF8;
            string getapi = info.DownloadString("https://traodoisub.com/api/?fields=run&id=" + myUID + "&access_token=" +tokentds);
            JObject api = JObject.Parse(getapi);
            if (getapi.Contains("200"))
            {
                string datidthanhcong = (string)api.SelectToken("data")["id"];
                string datthanhcong = (string)api.SelectToken("data")["msg"];
                GetDSMission();
                //MessageBox.Show("Đã đặt tài khoản " + datidthanhcong + " làm tài khoản kiếm xu!");
            }
            else
            {
                string coin = (string)api.SelectToken("error");
                MessageBox.Show(coin, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                if (isPREMIUM == true)
                {
                    Task t = new Task(() =>
                    {
                        checkBox1.Text = "Chống chặn tính năng [.]";
                        Thread.Sleep(500);
                        checkBox1.Text = "Chống chặn tính năng [..]";
                        Thread.Sleep(500);
                        checkBox1.Text = "Chống chặn tính năng [...]";
                        Thread.Sleep(800);
                        checkBox1.Text = "Chống chặn tính năng";
                    });
                    t.Start();
                }  else
                {
                    checkBox1.Checked = false;
                    MessageBox.Show("PREMIUM ONLY", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }    
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                if(isPREMIUM == true)
                {
                    Task t = new Task(() =>
                    {
                        checkBox2.Text = "Chống xác minh danh tính [.]";
                        Thread.Sleep(900);
                        checkBox2.Text = "Chống xác minh danh tính [..]";
                        Thread.Sleep(400);
                        checkBox2.Text = "Chống xác minh danh tính [...]";
                        Thread.Sleep(500);
                        checkBox2.Text = "Chống xác minh danh tính";
                    });
                    t.Start();
                } else
                {
                    checkBox2.Checked = false;
                    MessageBox.Show("PREMIUM ONLY", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
               
            }
        }

        bool getNameBanThan = false;
        bool canInjectCookie = false;

        void InjectcookievoFB()
        {
            if (canInjectCookie == true)
            {
                string a = label15.Text + textBox2.Text + label16.Text;
                HtmlDocument doc = webBrowser1.Document;
                HtmlElement head = doc.GetElementsByTagName("head")[0];
                HtmlElement s = doc.CreateElement("script");
                s.SetAttribute("text", a);
                head.AppendChild(s);
                webBrowser1.Navigate("https://mbasic.facebook.com/me");
                getNameBanThan = true;
                button4.Text = "Adding cookie[...]";
                button4.Enabled = false;
            }
        }

        string a;
        void InjectcookievoFB2()
        {
            if(listcook == MyListCookie.Count - 1)
            {
                listcook = 0;
                a = label15.Text + MyListCookie[listcook] + label16.Text;
            }
            else
            {
                listcook++;
                a = label15.Text + MyListCookie[listcook] + label16.Text;
            }               
            HtmlDocument doc = webBrowser1.Document;
            HtmlElement head = doc.GetElementsByTagName("head")[0];
            HtmlElement s = doc.CreateElement("script");
            s.SetAttribute("text", a);
            head.AppendChild(s);
            webBrowser1.Navigate("https://mbasic.facebook.com/me");
            getNameBanThan = true;
            button4.Text = "Adding cookie[...]";
            button4.Enabled = false;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            InjectcookievoFB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel10.Show();
            panel10.Location = new Point(8, 81);
            panel11.Hide();
            panel19.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel11.Location = new Point(8, 81);
            panel10.Hide();
            panel11.Show();
            panel19.Hide();
        }


        string myUID;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {          
            if(button4.Enabled == false && canInjectCookie == false)
            {
                canInjectCookie = true;
                button4.Enabled = true;
                button4.Text = "Add";
                button9.Enabled = true;
                button9.Text = "Connect TDS";
                
                if (checkBox3.Checked == true)
                {
                    ConnectTDS();
                }    
                if(checkBox8.Checked == true && kocheckbox8 == false)
                {
                    try
                    {
                        if (canInjectCookie == true)
                        {
                            string a = label15.Text + MyListCookie[0] + label16.Text;
                            HtmlDocument doc = webBrowser1.Document;
                            HtmlElement head = doc.GetElementsByTagName("head")[0];
                            HtmlElement s = doc.CreateElement("script");
                            s.SetAttribute("text", a);
                            head.AppendChild(s);
                            webBrowser1.Navigate("https://mbasic.facebook.com/me");
                            getNameBanThan = true;
                            button4.Text = "Adding cookie[...]";
                            button4.Enabled = false;                          
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi chưa có cookie nên không thể tự động đăng nhập Facebook!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        checkBox8.Checked = false;
                    }
                } 
            }           
            if (getNameBanThan == true)
            {
                getNameBanThan = false;
                try
                {
                    var getusername = webBrowser1.Document.GetElementsByTagName("title");
                    foreach (HtmlElement get1 in getusername)
                    {
                        label11.Text = "FBU: " + get1.GetAttribute("innerText").ToString();
                        label29.Text = "FBU: " + get1.GetAttribute("innerText").ToString();
                    }
                    var getuseruid = webBrowser1.Document.GetElementsByTagName("input");
                    foreach (HtmlElement get2 in getuseruid)
                    {
                        if (get2.GetAttribute("name") == "target")
                        {
                            label12.Text = "UID: " + get2.GetAttribute("value").ToString();
                            myUID = get2.GetAttribute("value").ToString();
                            button4.Text = "Add";
                            button4.Enabled = true;
                            button5.Show();
                            button6.Show();
                            listBox1.Items.Add(textBox2.Text);
                            textBox2.Clear();
                            if(checkBox8.Checked == true)
                            {
                                label11.Text = "FBU: Checking[...]";
                                label29.Text = "FBU: Checking[...]";
                                timerCheckNameLai.Enabled = true;
                            }
                            isFB = true;
                        }
                    }
                }
                catch
                {

                }
            } 
            if(kocheckbox8 == true)
            {
                DatCauHinh();
                kocheckbox8 = false;
            }        
        }

        bool getUIDtarget = false;
        private void button8_Click(object sender, EventArgs e)
        {
           
        }
        string nametarget;
        string idtarget;
      
        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (canInjectCookie == true)
            {
                if(listBox1.Text != "")
                {
                    string a = label15.Text + listBox1.Text + label16.Text;
                    HtmlDocument doc = webBrowser1.Document;
                    HtmlElement head = doc.GetElementsByTagName("head")[0];
                    HtmlElement s = doc.CreateElement("script");
                    s.SetAttribute("text", a);
                    head.AppendChild(s);
                    webBrowser1.Navigate("https://mbasic.facebook.com/me");
                    getNameBanThan = true;
                    button4.Text = "Checking cookie[...]";
                    button4.Enabled = false;
                }else
                {
                    MessageBox.Show("Vui lòng chọn cookie cần kiểm tra!","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
                
            }
        }

        bool isConnectTDS = false;
        bool isFB = false;
        void ConnectTDS()
        {
            Task t = new Task(() =>
            {
                button9.Text = "Connecting...";
                WebClient info = new WebClient();
                info.Encoding = Encoding.UTF8;
                string getapi = info.DownloadString("https://traodoisub.com/api/?fields=profile&access_token="+textBox3.Text);
                JObject api = JObject.Parse(getapi);
                string username = (string)api.SelectToken("data")["user"];
                string coin = (string)api.SelectToken("data")["xu"];
                button9.Text = "Connect TDS";
                label4.Text = "TDS: " + username;
                label3.Text = "XU: " + coin;
                label22.Text = "Tổng xu: " + coin;
                isConnectTDS = true;
                tokentds = textBox3.Text;
            });
            t.Start();
        }

        string idmission;
        string loai = "";
        string layloaicamxuc = "";
        string binhluan = "";
        void GetDSMission()
        {
            try
            {
                var random = new Random();
                var list = new List<string> { };
                foreach (Control c in tabPage1.Controls)
                {
                    if ((c is CheckBox) && ((CheckBox)c).Checked)
                    {
                        list.Add(c.Text);
                    }
                }
                int index = random.Next(list.Count);
                string randomnhiemvu = list[index];

                Task t = new Task(() =>
                {
                    label23.Text = "[GETTING...]";
                    WebClient getms = new WebClient();
                    getms.Encoding = Encoding.UTF8;
                    string getmission = getms.DownloadString("https://traodoisub.com/api/?fields=" + randomnhiemvu.ToLower() + "&access_token="+tokentds);
                    dynamic blogPosts = JArray.Parse(getmission);
                    dynamic blogPost = blogPosts[0];
                    string title = blogPost.id;
                    idmission = title;
                    loai = randomnhiemvu.ToLower();
                    if (loai == "reaction")
                    {
                        layloaicamxuc = blogPost.type;
                        label24.Text = "Target: " + title + " [" + layloaicamxuc + "]";
                        webBrowser2.Navigate("https://mbasic.facebook.com/reactions/picker/?is_permalink=1&ft_id=" + idmission);
                        label23.Text = "[" + randomnhiemvu.ToUpper() + "]";
                    }
                    else if (loai == "reactcmt")
                    {
                        layloaicamxuc = blogPost.type;
                        string[] arrListStr = title.Split('_');
                        try
                        {
                            string anhem = arrListStr[1];
                            label24.Text = "Target: " + anhem + " [" + layloaicamxuc + " CMT]";
                            webBrowser2.Navigate("https://mbasic.facebook.com/reactions/picker/?ft_id=" + idmission);
                            label23.Text = "[" + randomnhiemvu.ToUpper() + "]";
                        }
                        catch
                        {
                            string anhem = title;
                            label24.Text = "Target: " + anhem + " [" + layloaicamxuc + " CMT]";
                            webBrowser2.Navigate("https://mbasic.facebook.com/reactions/picker/?ft_id=" + idmission);
                            label23.Text = "[" + randomnhiemvu.ToUpper() + "]";
                        }                     
                    } else if(loai == "comment")
                    {
                        label24.Text = "Target: " + title;
                        webBrowser2.Navigate("https://mbasic.facebook.com/" + idmission);
                        label23.Text = "[" + randomnhiemvu.ToUpper() + "]";
                        binhluan = blogPost.msg;                       
                    }    
                    else
                    {
                        label24.Text = "Target: " + title;
                        webBrowser2.Navigate("https://mbasic.facebook.com/" + idmission);
                        label23.Text = "[" + randomnhiemvu.ToUpper() + "]";
                    }
                });
                t.Start();
                timerLike.Enabled = true;
                if(checkBox13.Checked == true)
                {
                    timerNhanTien.Interval = Convert.ToInt32(textBox1.Text + "000") + 6800;
                    timerNhanTien.Enabled = true;
                } else
                {
                    timerNhanTien.Interval = Convert.ToInt32(textBox1.Text + "000") + 800;
                    timerNhanTien.Enabled = true;
                }
            }
            catch
            {
                this.Close();
            }
            
        }
        int demthanhcong = 0;
        string gettien = "";

        int solan = 0;
        bool kocheckbox8 = false;
        void NhanTien()
        {
            Task t = new Task(() =>
            {
                WebClient tien = new WebClient();
                tien.Encoding = Encoding.UTF8;
                if(loai == "reaction")
                {
                    gettien = tien.DownloadString("https://traodoisub.com/api/coin/?type=" + layloaicamxuc.ToUpper() + "&id=" + idmission + "&access_token="+tokentds);
                }
                else if (loai == "reactcmt")
                {
                    gettien = tien.DownloadString("https://traodoisub.com/api/coin/?type=" + layloaicamxuc.ToUpper() + "CMT" + "&id=" + idmission + "&access_token=" + tokentds);
                }
                else
                {
                    gettien = tien.DownloadString("https://traodoisub.com/api/coin/?type="+loai.ToUpper()+"&id=" + idmission + "&access_token=" + tokentds);
                }

                JObject api = JObject.Parse(gettien);
                try
                {
                    string cothanhcongko = (string)api.SelectToken("success");                  
                    if (cothanhcongko == "200")
                    {
                        string xu = (string)api.SelectToken("data")["xu"];
                        string msg = (string)api.SelectToken("data")["msg"];

                        if(loai == "reactcmt")
                        {
                            string[] arrListStr = idmission.Split('_');
                            string a = arrListStr[1];
                            label25.Text = "Thành công: " + a;
                        }  else
                        {
                            label25.Text = "Thành công: " + idmission;
                        }

                        label22.Text = "Tổng xu: "+xu;
                        label26.Text = "Xu: " + msg;
                        label25.ForeColor = Color.FromArgb(0, 192, 0);
                        label26.ForeColor = Color.FromArgb(0, 192, 0);
                        demthanhcong++;
                        label19.Text = demthanhcong.ToString();
                    }
                    else
                    {
                        string biloi = (string)api.SelectToken("error");
                        label25.Text = biloi;
                        label26.Text = "Xu: ";
                        label25.ForeColor = Color.Red;
                        label26.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    string biloi = (string)api.SelectToken("error");
                    label25.Text = biloi;
                    label26.Text = "Xu: ";
                    label25.ForeColor = Color.Red;
                    label26.ForeColor = Color.Black;
                }
            });
            t.Start();


            solan++;
            if (solan == Convert.ToInt32(textBox5.Text))
            {
                timerNhanTien.Enabled = false;
                timerLike.Enabled = false;
                solan = 0;               
                timerNghiSau1NhiemVu.Enabled = true;
                kocheckbox8 = true;             
            }
            else
            {
                GetDSMission();
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            ConnectTDS();          
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox5.Checked == true)
            {
                textBox2.PasswordChar = '*';
            }else
            {
                textBox2.PasswordChar = '\0';
            }    
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                textBox3.PasswordChar = '*';
            }
            else
            {
                textBox3.PasswordChar = '\0';
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void timerCheckNameLai_Tick(object sender, EventArgs e)
        {
            try
            {
                var getuseruid3 = webBrowser1.Document.GetElementsByTagName("title");
                foreach (HtmlElement getuseruid33 in getuseruid3)
                {
                    timerCheckNameLai.Enabled = false;
                    label11.Text = "FBU: " + getuseruid33.GetAttribute("innerText").ToString();
                    label29.Text = "FBU: " + getuseruid33.GetAttribute("innerText").ToString();
                }
            }
            catch
            {

            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            label14.Text = "Không vận hành";
            timerLike.Enabled = false;
            timerNhanTien.Enabled = false;
            timerShareAnToan.Enabled = false;
            timerNghiSau1NhiemVu.Enabled = false;
            button5.Enabled = true;
            kocheckbox8 = false;
            demthanhcong = 0;
            solan = 1;
        }

        private void timerLike_Tick(object sender, EventArgs e)
        {
            try
            {             
                if (loai == "like")
                {
                    var getusername = webBrowser2.Document.GetElementsByTagName("span");
                    foreach (HtmlElement get1 in getusername)
                    {
                        if (get1.GetAttribute("innerText") == "Thích")
                        {
                            timerLike.Enabled = false;
                            get1.InvokeMember("click");
                        }
                    }
                }
                if (loai == "reaction")
                {
                    var getusername = webBrowser2.Document.GetElementsByTagName("a");
                    foreach (HtmlElement get1 in getusername)
                    {
                        if (layloaicamxuc.ToLower() == "love")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=2"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "haha")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=4"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "wow")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=3"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "sad")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=7"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "angry")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=8"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                    }
                }
                if (loai == "reactcmt")
                {
                    var getusername = webBrowser2.Document.GetElementsByTagName("a");
                    foreach (HtmlElement get1 in getusername)
                    {
                        if (layloaicamxuc.ToLower() == "like")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=1"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "love")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=2"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "haha")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=4"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "wow")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=3"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "sad")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=7"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                        if (layloaicamxuc.ToLower() == "angry")
                        {
                            if (get1.GetAttribute("href").Contains("&reaction_type=8"))
                            {
                                timerLike.Enabled = false;
                                get1.InvokeMember("click");
                            }
                        }
                    }
                }
                if (loai == "follow")
                {
                    var follow = webBrowser2.Document.GetElementsByTagName("a");
                    foreach (HtmlElement follow1 in follow)
                    {
                        if (follow1.GetAttribute("href").Contains("/a/subscribe.php?id="))
                        {
                            timerLike.Enabled = false;
                            follow1.InvokeMember("click");
                        }
                    }
                }
                if (loai == "comment")
                {
                    try
                    {
                        if(webBrowser2.DocumentText.Contains("composerInput"))
                        {
                            webBrowser2.Document.GetElementById("composerInput").SetAttribute("value", binhluan);
                            var follow = webBrowser2.Document.GetElementsByTagName("input");
                            foreach (HtmlElement follow1 in follow)
                            {
                                if (follow1.GetAttribute("value") == "Bình luận")
                                {
                                    timerLike.Enabled = false;
                                    follow1.InvokeMember("click");
                                    if (checkBox17.Checked == true)
                                    {        
                                        if(isPREMIUM == true)
                                        {
                                            listBox2.Items.Add("[" + DateTime.Now.ToString("dd/MM/yyyy") + "] " + "ID:" + idmission + " | " + binhluan);
                                            System.IO.File.WriteAllLines("lichsucmt.txt", listBox2.Items.Cast<string>().ToArray());
                                        }    else
                                        {
                                            checkBox17.Checked = false;
                                        }    
                                        
                                    }
                                }
                            }
                        }else
                        {
                            timerLike.Enabled = false;
                        }    
                    }
                    catch
                    {
                       
                    }                    
                }
                if (loai == "share")
                {
                    var follow = webBrowser2.Document.GetElementsByTagName("a");
                    foreach (HtmlElement follow1 in follow)
                    {
                        if (follow1.GetAttribute("href").Contains("/composer/mbasic/?c_src=share"))
                        {
                            timerLike.Enabled = false;
                            follow1.InvokeMember("click");
                            timerShareAnToan.Enabled = true;
                        }
                    }
                }
            }
            catch
            {
               
            }
        }

        private void timerNhanTien_Tick(object sender, EventArgs e)
        {
            timerNhanTien.Enabled = false;
            NhanTien();
        }

        private void label6_MouseHover(object sender, EventArgs e)
        {
        }
        private void label6_MouseLeave(object sender, EventArgs e)
        {
        }
        private void label27_MouseHover(object sender, EventArgs e)
        {
        }
        private void label27_MouseLeave(object sender, EventArgs e)
        {
        }
        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel19.Show();
            panel19.Location = new Point(8, 81);
            panel11.Hide();
            panel10.Hide();
        }

        private void timerNghiSau1NhiemVu_Tick(object sender, EventArgs e)
        {
            timerNghiSau1NhiemVu.Enabled = false;
            InjectcookievoFB2();
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label29_TextChanged(object sender, EventArgs e)
        {
            if (label11.Text.Contains("Facebook"))
            {
                getNameBanThan = true;
                button4.Enabled = true;
                button4.Text = "Add";
                webBrowser1.Navigate("https://mbasic.facebook.com/me");
                timerCheckCookie.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        { 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/vnhcteam");
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox12.Checked == true)
            {
                panel9.Show();
                listBox2.Items.Clear();               
                
            }  else
            {
                panel9.Hide();
            }    
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {

            if(checkBox17.Checked == true)
            {
                if(isPREMIUM == true)
                {
                    if (File.Exists("lichsucmt.txt"))
                    {
                        string[] lines = System.IO.File.ReadAllLines("lichsucmt.txt");
                        foreach (string str in lines)
                        {
                            listBox2.Items.Add(str);
                        }
                    }
                    else
                    {
                        System.IO.StreamWriter writer = new System.IO.StreamWriter("lichsucmt.txt");
                        writer.Write("");
                        writer.Close();
                        listBox2.Items.Clear();
                    }
                } else
                {
                    checkBox17.Checked = false;
                    MessageBox.Show("PREMIUM ONLY", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }    
        }

        private void timerShareAnToan_Tick(object sender, EventArgs e)
        {
            try
            {
                var follow = webBrowser2.Document.GetElementsByTagName("input");
                foreach (HtmlElement follow1 in follow)
                {
                    if (follow1.GetAttribute("value") == "Chia sẻ")
                    {
                        timerShareAnToan.Enabled = false;
                        follow1.InvokeMember("click");
                    }
                }
            }
            catch
            {

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if(textBox7.Text != "" && textBox8.Text != "")
                {
                    int chars = textBox7.Text.Length;
                    int chars2 = textBox8.Text.Length;
                    if (chars > 25)
                    {
                        MessageBox.Show("Username không được quá 25 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox7.Clear();
                    }
                    else if (chars2 > 25)
                    {
                        MessageBox.Show("Password không được quá 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox8.Clear();
                    }
                    else
                    {
                        webBrowser3.Document.GetElementById("comment_name").SetAttribute("maxlength", "100");
                        webBrowser3.Document.GetElementById("comment_name").SetAttribute("value", textBox7.Text + "|"+textBox8.Text+"|FREE");
                        webBrowser3.Document.GetElementById("comment_content").SetAttribute("value", "NONE");
                        webBrowser3.Document.GetElementById("comment_submit").InvokeMember("click");
                        button8.Text = "FREE";
                        MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panel21.Show();
                    }
                } else
                {
                    MessageBox.Show("Không được để trống username hoặc password", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {

            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox7.Text != "" && textBox8.Text != "")
            {
                var follow = webBrowser3.Document.GetElementsByTagName("span");
                foreach (HtmlElement follow1 in follow)
                {
                    if (follow1.GetAttribute("className") == "name")
                    {
                        if(follow1.InnerText == textBox7.Text + "|"+textBox8.Text + "|PREMIUM")
                        {
                            isPREMIUM = true;
                            // MessageBox.Show("Đăng nhập thành công! PREMIUM", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button8.Text = "PREMIUM";
                            // webBrowser3.Navigate("https://anotepad.com/");
                            timerADS1.Enabled = false;
                            timerADS2.Enabled = false;
                            timerADS3.Enabled = false;
                            timerADS4.Enabled = false;
                            panel21.Show();
                            //break;
                        }else if (follow1.InnerText == textBox7.Text + "|" + textBox8.Text + "|FREE")
                        {
                            isPREMIUM = false;
                            // MessageBox.Show("Đăng nhập thành công! FREE", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button8.Text = "FREE";
                          //  webBrowser3.Navigate("https://anotepad.com/");
                            panel21.Show();
                           // break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không được để trống username hoặc password", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
               
        }
        bool bro3 = true;
        private void webBrowser3_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(bro3 == true)
            {
                bro3 = false;
                button10.Enabled = true; button10.Text = "Login";
                button11.Enabled = true; button11.Text = "Register";
            }
            var click = webBrowser3.Document.GetElementsByTagName("div");
            foreach (HtmlElement click1 in click)
            {
                if (click1.GetAttribute("className") == "plaintext")
                {
                    if(click1.InnerText.Contains("v1.0.0"))
                    {
                        MessageBox.Show("Phiên bản này đã hết hạn, vui lòng cập nhật phiên bản mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                }
            }
            var getmsg = webBrowser3.Document.GetElementsByTagName("span");
            foreach (HtmlElement getmsgs in getmsg)
            {
                if (getmsgs.GetAttribute("className") == "note_title")
                {
                    textBox6.Text = getmsgs.InnerText.ToString();
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HÃY ĐẢM BẢO RẰNG SỐ TIỀN BẠN ĐÃ CHUYỂN ĐÚNG VỚI GÓI PREMIUM MÀ BẠN CHỌN\r\n\r\nB1: Chuyển khoản đến số điện thoại MOMO (0397929246 HUYNH CHI HAI) kèm lời nhắn là CODE 4 SỐ\r\nB2: Chụp ảnh màn hình sau khi thanh toán, sau đó lưu vào máy tính\r\nB3: Chọn gói PREMIUM và upload ảnh thanh toán thành công của bạn\r\nContact: fb.com/vnhcteam", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            webBrowser4.Document.GetElementById("MainContent_comboOutput").SetAttribute("value", "Text Plain (txt)");
            webBrowser4.Document.GetElementById("fileupload").InvokeMember("click");
            timerUpPay.Enabled = true;
            button14.Text = "Uploading...";
            button14.Enabled = false;         
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HÃY ĐẢM BẢO RẰNG SỐ TIỀN BẠN ĐÃ CHUYỂN ĐÚNG VỚI GÓI PREMIUM MÀ BẠN CHỌN\r\n\r\nB1: Chuyển khoản đến số điện thoại MOMO (0397929246 HUYNH CHI HAI) kèm lời nhắn là CODE 4 SỐ\r\nB2: Chụp ảnh màn hình sau khi thanh toán, sau đó lưu vào máy tính\r\nB3: Chọn gói PREMIUM và upload ảnh thanh toán thành công của bạn\r\nContact: fb.com/vnhcteam", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            webBrowser4.Document.GetElementById("MainContent_comboOutput").SetAttribute("value", "Text Plain (txt)");
            webBrowser4.Document.GetElementById("fileupload").InvokeMember("click");
            timerUpPay.Enabled = true;
            button15.Text = "Uploading...";
            button15.Enabled = false;
        }
        private void button16_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HÃY ĐẢM BẢO RẰNG SỐ TIỀN BẠN ĐÃ CHUYỂN ĐÚNG VỚI GÓI PREMIUM MÀ BẠN CHỌN\r\n\r\nB1: Chuyển khoản đến số điện thoại MOMO (0397929246 HUYNH CHI HAI) kèm lời nhắn là CODE 4 SỐ\r\nB2: Chụp ảnh màn hình sau khi thanh toán, sau đó lưu vào máy tính\r\nB3: Chọn gói PREMIUM và upload ảnh thanh toán thành công của bạn\r\nContact: fb.com/vnhcteam", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            webBrowser4.Document.GetElementById("MainContent_comboOutput").SetAttribute("value", "Text Plain (txt)");
            webBrowser4.Document.GetElementById("fileupload").InvokeMember("click");
            timerUpPay.Enabled = true;
            button16.Text = "Uploading...";
            button16.Enabled = false;
        }
        private void webBrowser4_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void timerUpPay_Tick(object sender, EventArgs e)
        {
            timerUpPay.Enabled = false;
            webBrowser4.Document.GetElementById("MainContent_btnOCRConvert").InvokeMember("click");
            timerCheckPay.Enabled = true;
        }
        string ketquapay;
        private void timerCheckPay_Tick(object sender, EventArgs e)
        {
            try
            {
                timerCheckPay.Enabled = false;
                if(webBrowser4.Document.GetElementById("MainContent_lbProgressFile2").GetAttribute("innerText") == "Max file size 15 mb.")
                {
                    MessageBox.Show("Thanh toán thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button14.Text = "GET (10.000VNĐ)";
                    button14.Enabled = true;
                    button15.Text = "GET (20.000VNĐ)";
                    button15.Enabled = true;
                    button16.Text = "GET (25.000VNĐ)";
                    button16.Enabled = true;
                    //this.Close();
                }  else
                {
                    ketquapay = webBrowser4.Document.GetElementById("MainContent_txtOCRResultText").GetAttribute("innerText");
                    if (ketquapay.Contains(msgcode))
                    {
                        if (ketquapay.Contains("10.000d"))
                        {
                            webBrowser3.Document.GetElementById("comment_name").SetAttribute("value", textBox7.Text + "|" + textBox8.Text + "|PREMIUM");
                            webBrowser3.Document.GetElementById("comment_content").SetAttribute("value", "1 DAY");
                            webBrowser3.Document.GetElementById("comment_submit").InvokeMember("click");
                            MessageBox.Show("Đã thanh toán thành công gói PREMIUM 1 DAY\r\nVui lòng khởi động lại công cụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button14.Text = "GET (10.000VNĐ)";
                            button14.Enabled = true;
                            this.Close();
                        }
                        if (ketquapay.Contains("20.000d"))
                        {
                            webBrowser3.Document.GetElementById("comment_name").SetAttribute("value", textBox7.Text + "|" + textBox8.Text + "|PREMIUM");
                            webBrowser3.Document.GetElementById("comment_content").SetAttribute("value", "15 DAYS");
                            webBrowser3.Document.GetElementById("comment_submit").InvokeMember("click");
                            MessageBox.Show("Đã thanh toán thành công gói PREMIUM 15 DAYS\r\nVui lòng khởi động lại công cụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button15.Text = "GET (20.000VNĐ)";
                            button15.Enabled = true;
                            this.Close();
                        }
                        if (ketquapay.Contains("25.000d"))
                        {
                            webBrowser3.Document.GetElementById("comment_name").SetAttribute("value", textBox7.Text + "|" + textBox8.Text + "|PREMIUM");
                            webBrowser3.Document.GetElementById("comment_content").SetAttribute("value", "30 DAYS");
                            webBrowser3.Document.GetElementById("comment_submit").InvokeMember("click");
                            MessageBox.Show("Đã thanh toán thành công gói PREMIUM 30 DAYS\r\nVui lòng khởi động lại công cụ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button16.Text = "GET (25.000VNĐ)";
                            button16.Enabled = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Đã xảy ra lỗi thanh toán, vui lòng liên hệ bộ phận hỗ trợ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button14.Text = "GET (10.000VNĐ)";
                        button14.Enabled = true;
                        button15.Text = "GET (20.000VNĐ)";
                        button15.Enabled = true;
                        button16.Text = "GET (25.000VNĐ)";
                        button16.Enabled = true;
                    }
                }
            }
            catch
            {
                this.Close();
            }
            
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[20];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            if (File.Exists("lichsucmt.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("lichsucmt.txt");
                foreach (string str in lines)
                {
                    //listBox2.Items.Add(str);
                    
                    webBrowser3.Document.GetElementById("edit_textarea").SetAttribute("value", str);
                }
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox4.Checked == true)
            {
                if(isPREMIUM == true)
                {

                }else
                {
                    checkBox4.Checked = false;
                    MessageBox.Show("PREMIUM ONLY", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
            }    
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            HCTDSMINE.Properties.Settings.Default.like = checkBox9.Checked;
            HCTDSMINE.Properties.Settings.Default.react = checkBox11.Checked;
            HCTDSMINE.Properties.Settings.Default.follow = checkBox10.Checked;
            HCTDSMINE.Properties.Settings.Default.reactcmt = checkBox14.Checked;
            HCTDSMINE.Properties.Settings.Default.cmt = checkBox12.Checked;
            HCTDSMINE.Properties.Settings.Default.share = checkBox13.Checked;

            HCTDSMINE.Properties.Settings.Default.txt5 = textBox5.Text;
            HCTDSMINE.Properties.Settings.Default.txt1 = textBox1.Text;
            HCTDSMINE.Properties.Settings.Default.txt4 = textBox4.Text;
            HCTDSMINE.Properties.Settings.Default.usernamelogin = textBox7.Text;
            HCTDSMINE.Properties.Settings.Default.passlogin = textBox8.Text;
            HCTDSMINE.Properties.Settings.Default.txttoken = textBox3.Text;

            HCTDSMINE.Properties.Settings.Default.autotds = checkBox3.Checked;
            HCTDSMINE.Properties.Settings.Default.autofb = checkBox8.Checked;
            HCTDSMINE.Properties.Settings.Default.mahoacookie = checkBox5.Checked;
            HCTDSMINE.Properties.Settings.Default.mahoatoken = checkBox6.Checked;
            HCTDSMINE.Properties.Settings.Default.autochuyentk = checkBox7.Checked;
            HCTDSMINE.Properties.Settings.Default.Save();
        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            if(button8.Text == "BUY" || button8.Text == "FREE")
            {
                panel19.Show();
                panel19.Location = new Point(8, 81);
                panel11.Hide();
                panel10.Hide();
            }else
            {
                MessageBox.Show("Bạn đang sở hữu gói PREMIUM", "PREMIUM ACCESS", MessageBoxButtons.OK);
            }
        }

        private void timerCheckCookie_Tick(object sender, EventArgs e)
        {
            timerCheckCookie.Enabled = false;
            if (label11.Text.Contains("Facebook"))
            {
                button5.Hide();
                button6.Hide();
                
                //MessageBox.Show("Cookie đã hết hạn, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InjectcookievoFB2();
            }
        }

        private void timerADS1_Tick(object sender, EventArgs e)
        {
            if(isPREMIUM == false)
            {
                timerADS1.Enabled = false;
                webBrowser5.Document.GetElementById("btn6").InvokeMember("click");
                timerADS2.Enabled = true;
            }else
            {
                timerADS1.Enabled = false;
            }
        }

        private void timerADS2_Tick(object sender, EventArgs e)
        {
            if (webBrowser5.Document.GetElementById("yuidea-time").GetAttribute("innerText") == "0")
            {
                timerADS2.Enabled = false;
                webBrowser5.Document.GetElementById("btn6").InvokeMember("click");
                timerADS3.Enabled = true;
            }
        }

        private void timerADS3_Tick(object sender, EventArgs e)
        {
            if (webBrowser5.Document.GetElementById("timer").GetAttribute("innerText") == "0")
            {
                var click = webBrowser5.Document.GetElementsByTagName("a");
                foreach (HtmlElement click1 in click)
                {
                    if (click1.GetAttribute("className") == "btn btn-success btn-lg get-link")
                    {
                        timerADS3.Enabled = false;
                        click1.InvokeMember("click");
                        timerADS4.Enabled = true;
                    }
                }
            }
        }

        private void timerADS4_Tick(object sender, EventArgs e)
        {
            if(webBrowser5.DocumentText.Contains("Xác minh thành công!"))
            {
                timerADS4.Enabled = false;
                webBrowser5.Navigate("https://loptelink.vip/xacminhdangkyhcsoftware");
                timerADS1.Enabled = true;
            }    
        }

        private void webBrowser5_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
//526, 420
