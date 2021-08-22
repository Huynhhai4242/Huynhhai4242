CODE CHO NGƯỜI CHƯA BIẾT GÌ, CHỈ BIẾT COPY DÁN :>
---------------------------------------------------------------
[1] Đăng nhập Facebook bằng cookie
//Đăng nhập Facebook bằng cookie với đoạn Javascript mình đã chỉnh sửa lại
string doan1 = "javascript:void(function(){ function setCookie(t) { var list = t.split(\"; \"); console.log(list); for (var i = list.length - 1; i >= 0; i--) { var cname = list[i].split(\"=\")[0]; var cvalue = list[i].split(\"=\")[1]; var d = new Date(); d.setTime(d.getTime() + (7*24*60*60*1000)); var expires = \";domain=.facebook.com;expires=\"+ d.toUTCString(); document.cookie = cname + \"=\" + cvalue + \"; \" + expires; } } function hex2a(hex) { var str = ''; for (var i = 0; i < hex.length; i += 2) { var v = parseInt(hex.substr(i, 2), 16); if (v) str += String.fromCharCode(v); } return str; } var cookie ='";
string doan2 = "'; setCookie(cookie); location.href = 'https://mbasic.facebook.com/me'; })();";
string cookie = textBoxCookie.Text;
string tongket = doan1 + cookie + doan2;
HtmlDocument doc = webBrowser1.Document;
HtmlElement head = doc.GetElementsByTagName("head")[0];
HtmlElement s = doc.CreateElement("script");
s.SetAttribute("text", tongket);
head.AppendChild(s);


---------------------------------------------------------------
[2] Set, get, click html element từ webBrowser
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
