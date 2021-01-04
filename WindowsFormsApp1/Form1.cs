using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.Example;
using CefSharp.Example.Handlers;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser chrome;

        private void Form1_Load(object sender, EventArgs e)
        {
            TarayiciAc();
        }
        private void TarayiciAc ()
        {
            this.WindowState = FormWindowState.Maximized;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            //CefSettings settings = new CefSettings();
            var settings = new CefSettings()
            {
                BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                Environment.Is64BitProcess ? "x64" : "x86",
                "CefSharp.BrowserSubprocess.exe")
            };
            Cef.Initialize(settings);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            Cef.Initialize(settings);
            chrome = new ChromiumWebBrowser("https://nlbui.edmbilisim.com.tr/EFaturaUI/");
            this.panel1.Controls.Add(chrome);
            chrome.Dock = DockStyle.Fill;
            chrome.FrameLoadEnd += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                    chrome.ExecuteScriptAsync("$('input[name=loginUserName]').val(\"kullaniciadi\")");
                    chrome.ExecuteScriptAsync("$('input[name=loginPassword]').val(\"sifre\")");
                    chrome.ExecuteScriptAsync("document.getElementsByClassName('btn btn-default btn-primary pull-right')[0].click()");
                }
            };
            chrome.DownloadHandler = new DownloadHandler();
        }

    }
    private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
    {
        if (args.Name.StartsWith("CefSharp"))
        {
            string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
            string architectureSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                Environment.Is64BitProcess ? "x64" : "x86",
                assemblyName);

            return File.Exists(architectureSpecificPath)
                ? Assembly.LoadFile(architectureSpecificPath)
                : null;
        }

        return null;
    }
}

