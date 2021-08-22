CODE CHO NGƯỜI CHƯA BIẾT GÌ, CHỈ BIẾT COPY DÁN :>
---------------------------------------------------------------
[1] Set, get, click html element từ webBrowser
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
