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

            SampleDescription sampleDesc = new SampleDescription
                {
                    Count = 1, //multisampling levels
                    Quality = 0 // choose from StandardMultisampleQualityLevels if using multisampling
                };

            CreateDeviceAndSwapChain(sampleDesc);
            
            CreateDepthStencilAndViews(sampleDesc);

            //factory.MakeWindowAssociation(viewportControl.Handle, WindowAssociationFlags.IgnoreAll);

            Viewport viewport = new Viewport(0, 0, viewportControl.Width, viewportControl.Height, 0.0f, 1.0f);
            device.ImmediateContext.Rasterizer.SetViewport(viewport);
            device.ImmediateContext.OutputMerger.SetRenderTargets(depthBufView, backBufView);
        }

        private void CreateDepthStencilAndViews(SampleDescription sampleDesc)
        {
            depthBufferTex = new Texture2D(device, 
                new Texture2DDescription
                {
                    Format = Format.R32_Typeless,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = viewportControl.Width,
                    Height = viewportControl.Height,
                    SampleDescription = sampleDesc,
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                });
            
            depthBufView = new DepthStencilView(device, depthBufferTex, 
                new DepthStencilViewDescription
                {
                    Dimension = DepthStencilViewDimension.Texture2D,
                    Format = Format.D32_Float
                });

            depthShaderView = new ShaderResourceView(device, depthBufferTex,
                new ShaderResourceViewDescription
                {
                    Dimension = ShaderResourceViewDimension.Texture2D,
                    Format = Format.R32_Float,
                    Texture2D = new ShaderResourceViewDescription.Texture2DResource
                        {
                            MipLevels = 1
                        }
                });
        }

        private void CreateDeviceAndSwapChain(SampleDescription sampleDesc)
        {
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None,
                new SwapChainDescription
                {
                    BufferCount = 2,
                    ModeDescription = new ModeDescription
                        {
                            Width = viewportControl.Width,
                            Height = viewportControl.Height,
                            RefreshRate = new Rational(60, 1),
                            Format = Format.R8G8B8A8_UNorm
                        },
                    IsWindowed = true,
                    OutputHandle = viewportControl.Handle,
                    //sample desc of render target and depth buffers must match
                    SampleDescription = sampleDesc,
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                },
                out device, out swapChain);

            FeatureLevel level = Device.GetSupportedFeatureLevel();

            factory = swapChain.GetParent<Factory>();

            backBufferTex = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            backBufView = new RenderTargetView(device, backBufferTex);
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
