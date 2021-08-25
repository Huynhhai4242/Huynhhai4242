// Bài 3 (khuyến khích xem video xong, sau đó mới thực hành copy + dán)
        
        [1]
        public ToolCayXuNhaLam()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; //Nhớ thêm dòng này vì khi thêm vào bạn mới có thể hiển thị text ra giao diện được
        }


        [2]
        void ConnectTDS() //Đây là hàm kết nối API TDS, mình cho dô "void" để gọn code hơn. Khi bạn click vô 1 button thì chỉ việc gõ "ConnectTDS();" là tự button nó hiểu nó phải thực hiện hàm này
        {
            // Task để không bị đơ khi chạy phần mềm, code đc thực hiện theo thứ tự từ đầu tới cuối
            Task t = new Task(() =>
            {
                WebClient info = new WebClient();
                info.Encoding = Encoding.UTF8;
                string getapi = info.DownloadString("https://traodoisub.com/api/?fields=profile&access_token="+ txtTokenTDS.Text); //Phần này là lấy (tải) nguyên seal API về (txtTokenTDS.Text là cái ô textBox để người dùng nhập token TDS)
                JObject api = JObject.Parse(getapi); //Phần này bắt đầu xử lý API
                string username = (string)api.SelectToken("data")["user"]; //Nó sẽ tìm mục data -> trỏ đến thằng user -> lấy thông tin thằng user (tên tài khoản TDS)
                string coin = (string)api.SelectToken("data")["xu"]; //Nó sẽ tìm mục data -> trỏ đến thằng xu -> lấy thông tin số xu
                lblTenTDS.Text = "Tên tài khoản TDS: " + username; //Đến đây dễ hiểu ời nho, tại bài 2 mình có nói ời <3
                lblSoXuTDS.Text = "Số xu TDS: " + coin;
                // Nếu có lỗi, đọc lại [1]
            });
            t.Start();
        }
        

        [3]
        private void button3_Click_1(object sender, EventArgs e)
        {
            ConnectTDS(); //Đây, các bạn thấy gọn chưa xD, khi button này đc click thì nó sẽ gọi hàm "ConnectTDS" để thực thi
        }




        [4] BỔ SUNG (Phát hiện nhập sai token TDS) (Thay thế cho [2])

        void ConnectTDS()
        {
            try
            {
                Task t = new Task(() =>
                {
                    WebClient info = new WebClient();
                    info.Encoding = Encoding.UTF8;
                    string getapi = info.DownloadString("https://traodoisub.com/api/?fields=profile&access_token=" + txtTokenTDS.Text);
                    JObject api = JObject.Parse(getapi);
                    string username = (string)api.SelectToken("data")["user"];
                    string coin = (string)api.SelectToken("data")["xu"];
                    lblTenTDS.Text = "Tên tài khoản TDS: " + username;
                    lblSoXuTDS.Text = "Số xu TDS: " + coin;
                    // Nếu có lỗi, đọc lại [1]
                });
                t.Start();
            }
            catch
            {
                MessageBox.Show("Sai token TDS, vui lòng kiểm tra lại token TDS!");
            }  
        }
