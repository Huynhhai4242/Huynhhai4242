        string Ten_Facebook;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(Sau_Khi_Add_Cookie == true)
            {
                var hello = webBrowser1.Document.GetElementsByTagName("title");
                foreach (HtmlElement xinchao in hello)
                {
                    if(xinchao.GetAttribute("innerText").ToString().Contains("Facebook"))
                    {
                        MessageBox.Show("Sai cookie, vui lòng kiểm tra lại cookie!");
                    }
                    else
                    {
                        MessageBox.Show("Cookie đúng: " + xinchao.GetAttribute("innerText").ToString());
                        Ten_Facebook = xinchao.GetAttribute("innerText").ToString();
                        label2.Text = "Tên Facebook: "+Ten_Facebook;
                    }
                }
            }            
        }
