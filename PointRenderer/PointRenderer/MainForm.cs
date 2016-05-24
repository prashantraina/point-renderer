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
        private Texture2D depthBufferTex;
        private DepthStencilView depthBufView;
        private ShaderResourceView depthShaderView;

        public MainForm()
        {
            InitializeComponent();
            Disposed += MainForm_Disposed;
            
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            //var viewportControl = this;    

            SampleDescription sampleDesc = new SampleDescription();
            sampleDesc.Count = 1; //multisampling levels
            sampleDesc.Quality = 0; // choose from StandardMultisampleQualityLevels if using multisampling

            CreateDeviceAndSwapChain(sampleDesc);

            backBufferTex = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            backBufView = new RenderTargetView(device, backBufferTex);

            CreateDepthStencilAndViews(sampleDesc);

            //factory.MakeWindowAssociation(viewportControl.Handle, WindowAssociationFlags.IgnoreAll);

            //Viewport viewport = new Viewport(0, 0, viewportControl.Width, viewportControl.Height, 0.0f, 1.0f);
            //device.ImmediateContext.Rasterizer.SetViewport(viewport);
            device.ImmediateContext.OutputMerger.SetRenderTargets(depthBufView, backBufView);
        }

        private void CreateDepthStencilAndViews(SampleDescription sampleDesc)
        {
            Texture2DDescription depthStencilDesc = new Texture2DDescription();
            depthStencilDesc.Format = Format.R32_Typeless;
            depthStencilDesc.ArraySize = 1;
            depthStencilDesc.MipLevels = 1;
            depthStencilDesc.Width = viewportControl.Width;
            depthStencilDesc.Height = viewportControl.Height;
            depthStencilDesc.SampleDescription = sampleDesc;
            depthStencilDesc.Usage = ResourceUsage.Default;
            depthStencilDesc.BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource;
            depthStencilDesc.CpuAccessFlags = CpuAccessFlags.None;
            depthStencilDesc.OptionFlags = ResourceOptionFlags.None;

            depthBufferTex = new Texture2D(device, depthStencilDesc);

            DepthStencilViewDescription depthViewDesc = new DepthStencilViewDescription();
            depthViewDesc.Format = Format.D32_Float;
            depthViewDesc.Dimension = DepthStencilViewDimension.Texture2D;
            depthBufView = new DepthStencilView(device, depthBufferTex, depthViewDesc);

            ShaderResourceViewDescription depthShaderViewDesc = new ShaderResourceViewDescription();
            depthShaderViewDesc.Dimension = ShaderResourceViewDimension.Texture2D;
            depthShaderViewDesc.Format = Format.R32_Float;
            depthShaderViewDesc.Texture2D.MipLevels = 1;
            depthShaderView = new ShaderResourceView(device, depthBufferTex, depthShaderViewDesc);
        }

        private void CreateDeviceAndSwapChain(SampleDescription sampleDesc)
        {
            SwapChainDescription swapDesc = new SwapChainDescription();
            ModeDescription modeDesc = new ModeDescription();
            modeDesc.Width = viewportControl.Width;
            modeDesc.Height = viewportControl.Height;
            modeDesc.RefreshRate = new Rational(60, 1);
            modeDesc.Format = Format.R8G8B8A8_UNorm;
            swapDesc.BufferCount = 2;
            swapDesc.ModeDescription = modeDesc;
            swapDesc.IsWindowed = true;
            swapDesc.OutputHandle = viewportControl.Handle;
            //sample desc of render target and depth buffers must match
        
            swapDesc.SampleDescription = sampleDesc;
            swapDesc.SwapEffect = SwapEffect.Discard;
            swapDesc.Usage = Usage.RenderTargetOutput;

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapDesc,
                out device, out swapChain);

            FeatureLevel level = Device.GetSupportedFeatureLevel();

            factory = swapChain.GetParent<Factory>();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            depthShaderView.Dispose();
            depthBufView.Dispose();
            depthBufferTex.Dispose();
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
            device.ImmediateContext.ClearDepthStencilView(depthBufView, DepthStencilClearFlags.Depth, 1.0f, 0);



            swapChain.Present(1, PresentFlags.None);
        }
        
    }
}
