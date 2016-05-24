using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using SharpDX.Direct3D;

namespace PointRenderer
{
    public partial class MainForm : Form
    {
        private Device device;
        private SwapChain swapChain;
        private Factory factory;
        private Texture2D backBufferTex;
        private RenderTargetView backBufView;

        public MainForm()
        {
            InitializeComponent();
            Disposed += MainForm_Disposed;
            
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //var viewportControl = this;
            SwapChainDescription swapDesc = new SwapChainDescription();
            ModeDescription modeDesc = new ModeDescription();
            modeDesc.Width = viewportControl.Width;
            modeDesc.Height = viewportControl.Height;
            modeDesc.RefreshRate = new Rational(60, 1);
            modeDesc.Format = Format.R8G8B8A8_UNorm;
            swapDesc.BufferCount = 1;
            swapDesc.ModeDescription = modeDesc;
            swapDesc.IsWindowed = true;
            swapDesc.OutputHandle = viewportControl.Handle;
            //sample desc of render target and depth buffers must match
            SampleDescription sampleDesc = new SampleDescription();
            sampleDesc.Count = 1; //multisampling levels
            sampleDesc.Quality = 0; // choose from StandardMultisampleQualityLevels if using multisampling
            swapDesc.SampleDescription = sampleDesc;
            swapDesc.SwapEffect = SwapEffect.Discard;
            swapDesc.Usage = Usage.RenderTargetOutput;

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapDesc,
                out device, out swapChain);

            backBufferTex = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            backBufView = new RenderTargetView(device, backBufferTex);
            factory = swapChain.GetParent<Factory>();
            //factory.MakeWindowAssociation(viewportControl.Handle, WindowAssociationFlags.IgnoreAll);

            //Viewport viewport = new Viewport(0, 0, viewportControl.Width, viewportControl.Height, 0.0f, 1.0f);
            //device.ImmediateContext.Rasterizer.SetViewport(viewport);
            device.ImmediateContext.OutputMerger.SetRenderTargets(backBufView);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            backBufView.Dispose();
            backBufferTex.Dispose();
            swapChain.Dispose();
            device.Dispose();
            factory.Dispose();
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            Cleanup();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            device.ImmediateContext.ClearRenderTargetView(backBufView, SharpDX.Color.CornflowerBlue);
            swapChain.Present(1, PresentFlags.None);
        }
        
    }
}
