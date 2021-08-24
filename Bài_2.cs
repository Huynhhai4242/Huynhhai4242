BÀI 2
---------------------------------------------------------------
Set, get, click html element từ webBrowser

// Kiểu 1 (chỉ áp dụng khi thẻ html có id. Mục đích là cho gọn)
webBrowser1.Document.GetElementById("").GetAttribute("value"); // Lấy giá trị
webBrowser1.Document.GetElementById("").SetAttribute("value", ""); // Truyền giá trị
webBrowser1.Document.GetElementById("").InvokeMember("click"); // Click vô nút, ô, blabla...
// Kiểu 2 (cân mọi loại thẻ html)
var hello = webBrowser1.Document.GetElementsByTagName("");
foreach (HtmlElement xinchao in hello)
{
     if (xinchao.GetAttribute("") == "")
     {
         xinchao.InvokeMember("click"); // Có thể thay InvokeMember bằng GetAttribute, SetAttribute, InvokeMember như kiểu 1
     }
}


---------------------------------------------------------------
//Code trong video bài 2
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
