#pragma checksum "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1bd692c017b548ff5197880aa17132c6888abaa9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cart_Cart), @"mvc.1.0.view", @"/Views/Cart/Cart.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Users\bsi50128\Belajar\Razor\Views\_ViewImports.cshtml"
using Razor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Users\bsi50128\Belajar\Razor\Views\_ViewImports.cshtml"
using Razor.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1bd692c017b548ff5197880aa17132c6888abaa9", @"/Views/Cart/Cart.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc4a001eb163016710cb6ff29c4698538c16c9e9", @"/Views/_ViewImports.cshtml")]
    public class Views_Cart_Cart : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Payment", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Cart", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
  
    ViewData["Title"] = "Cart";
    ViewData["CartId"] = ViewBag.CartId;
    ViewData["User"] = ViewBag.User;
    ViewData["List"] = ViewBag.UL;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1bd692c017b548ff5197880aa17132c6888abaa94036", async() => {
                WriteLiteral("\r\n        <div class=\"text-left\" style=\"position:relative; left:50px;\">\r\n        CART\r\n        <input id=\"cart_id\" name=\"cart_id\"");
                BeginWriteAttribute("value", " value=", 347, "", 365, 1);
#nullable restore
#line 12 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 354, ViewBag.Id, 354, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" hidden> 
        <input id=""total_price"" name=""total_price"" style=""position:relative; left:800px"" placeholder=""TOTAL BIAYA BELANJA"">
        <button type=""submit"" style=""position:relative; left:530px"" class=""btn btn-success"">Bayar</button>
        </div>
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n<div style=\"width: 100%;margin: 10px auto;background-color: white;padding: 10px;overflow: hidden;\">\r\n");
#nullable restore
#line 20 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
      
    var cart = ViewBag.Cart;
    
    foreach(var c in cart)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div style=\"width: 100%;border: 2px solid;padding: 25px;margin: 5px;content: ;display: table;clear: both; \">\r\n            <div style=\"width:100%\">\r\n                <img");
            BeginWriteAttribute("src", " src=", 1004, "", 1019, 1);
#nullable restore
#line 27 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1009, c.url_img, 1009, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"width: 20%;height: 150px;margin-bottom: 5px;s\">\r\n                <a style=\"position:relative;left:50px\">");
#nullable restore
#line 28 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
                                                  Write(c.item_name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                <input type=\"text\"");
            BeginWriteAttribute("id", " id=\"", 1183, "\"", 1200, 2);
            WriteAttributeValue("", 1188, "jumlah-", 1188, 7, true);
#nullable restore
#line 29 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1195, c.id, 1195, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"Jumlah Item\"");
            BeginWriteAttribute("value", " value=\"", 1227, "\"", 1235, 0);
            EndWriteAttribute();
            WriteLiteral(" style=\"position:relative;left:70px\">\r\n                <input");
            BeginWriteAttribute("id", " id=\"", 1297, "\"", 1313, 2);
            WriteAttributeValue("", 1302, "price-", 1302, 6, true);
#nullable restore
#line 30 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1308, c.id, 1308, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("value", "value=", 1314, "", 1328, 1);
#nullable restore
#line 30 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1320, c.price, 1320, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" hidden>\r\n                <input");
            BeginWriteAttribute("id", " id=\"", 1360, "\"", 1379, 2);
            WriteAttributeValue("", 1365, "subtotal-", 1365, 9, true);
#nullable restore
#line 31 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1374, c.id, 1374, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" name=\"jumlah\"  placeholder=\"Sub Total\"style=\"position:relative;left:70px\">\r\n                <button class=\"btn btn-primary\"");
            BeginWriteAttribute("id", " id=\"", 1504, "\"", 1516, 2);
            WriteAttributeValue("", 1509, "a-", 1509, 2, true);
#nullable restore
#line 32 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1511, c.id, 1511, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" onclick=\"javascript: calculate(this)\" style=\"position:relative;left:110px\">Update</button>\r\n                <button class=\"btn btn-danger\"");
            BeginWriteAttribute("id", " id=\"", 1656, "\"", 1673, 2);
            WriteAttributeValue("", 1661, "danger-", 1661, 7, true);
#nullable restore
#line 33 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
WriteAttributeValue("", 1668, c.id, 1668, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" onclick=\"javascript: deleteCart(this)\" style=\"position:relative;left:120px\">Remove</button>\r\n            </div>\r\n            \r\n        </div>\r\n");
#nullable restore
#line 37 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
    }


#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>

<script type=""text/javascript"">
    function calculate(id)
    {
        var hasil = id.id;
        var x = hasil.substring(2,3);
        var input = 'jumlah-'+x;
        var price = 'price-'+x;
        var subtotalid = 'subtotal-'+x;
        var quantity = document.getElementById(input).value;
        var iPrice = document.getElementById(price).value;
        var subtotal = quantity*iPrice;
        var arr = document.getElementsByName('jumlah');
        var tot = 0;
        for(var i = 0 ; i<arr.length;i++)
        {
                if(parseInt(arr[i].value))
                tot += +parseInt(arr[i].value);
        } 
        document.getElementById(subtotalid).value = subtotal;
        document.getElementById('total_price').value = tot;
    }
    function deleteCart(id)
    {
        var ambil = id.id;
        var x = ambil.substring(7,8);
        var path = '/Cart/Delete?'+'id='+x+'&'+'cartid='+'");
#nullable restore
#line 67 "D:\Users\bsi50128\Belajar\Razor\Views\Cart\Cart.cshtml"
                                                     Write(ViewBag.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\';\r\n        window.location = path;\r\n    }\r\n</script>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
