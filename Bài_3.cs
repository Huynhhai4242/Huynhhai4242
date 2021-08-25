        public ToolCayXuNhaLam()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; //Nhớ thêm dòng này
        }
        
        void ConnectTDS()
        {
            Task t = new Task(() =>
            {
                WebClient info = new WebClient();
                info.Encoding = Encoding.UTF8;
                string getapi = info.DownloadString("https://traodoisub.com/api/?fields=profile&access_token="+ "");
                JObject api = JObject.Parse(getapi);
                string username = (string)api.SelectToken("data")["user"];
                string coin = (string)api.SelectToken("data")["xu"];
            });
            t.Start();
        }
