        string Ten_Facebook; // Kiểu dữ liệu chuỗi (mục đích khi get được tên Facbeook sẽ lưu tên đó vô "Ten_Facebook")
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(Sau_Khi_Add_Cookie == true)
            {
                var hello = webBrowser1.Document.GetElementsByTagName("title"); // Kiểm tra giá trị của tất cả các thẻ <title>
                foreach (HtmlElement xinchao in hello)
                {
                    if(xinchao.GetAttribute("innerText").ToString().Contains("Facebook")) // Nếu phát hiện ra chuỗi chứa chữ "Facebook"
                    {
                        MessageBox.Show("Sai cookie, vui lòng kiểm tra lại cookie!"); // Thì tạch
                        label2.Text = "Trạng thái: Sai cookie, vui lòng kiểm tra lại!";
                    }
                    else // Không phát hiện ra chuỗi chứ chữ "Facebook" thì login thành công
                    {
                        MessageBox.Show("Cookie đúng: " + xinchao.GetAttribute("innerText").ToString());
                        Ten_Facebook = xinchao.GetAttribute("innerText").ToString(); // "Ten_Facebook" đã được lưu tên Facebook
                        label2.Text = "Tên Facebook: "+Ten_Facebook; // Label sẽ hiện ra chữ: Tên Facebook: Nguyễn Văn Tèo
                    }
                }
            }            
        }
