using Android.Content;
using System;
using System.Threading.Tasks;
using WebViewBugNOTWorking;
using WebViewBugNOTWorking.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace WebViewBugNOTWorking.Droid
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        WebView _webView;

        public CustomWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            _webView = Control;
            _webView.VerticalScrollBarEnabled = false;
            _webView.HorizontalScrollBarEnabled = false;

            if (e.OldElement == null)
            {
                _webView.SetWebViewClient(new CustomWebViewClient(e.NewElement as CustomWebView));
            }
        }

        class CustomWebViewClient : Android.Webkit.WebViewClient
        {
            CustomWebView _xwebView = null;

            public CustomWebViewClient(CustomWebView xwebView)
            {
                _xwebView = xwebView;
            }
            public override async void OnPageFinished(WebView view, string url)
            {
                if (_xwebView != null)
                {
                    view.Settings.JavaScriptEnabled = true;
                    _xwebView.HeightRequest = 0d;
                    await Task.Delay(100);
                    string result = await _xwebView.EvaluateJavaScriptAsync(
                        @"
                        (function(){
                            if(document && document.body) {
                                return document.body.scrollHeight;
                            }
                        })()
                        ");
                    _xwebView.HeightRequest = Convert.ToDouble(result);
                }

                base.OnPageFinished(view, url);
            }
        }
    }
}