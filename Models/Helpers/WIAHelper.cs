using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using WIA;

namespace Helpers
{
    public class WIAHelper
    {
        private string _deviceId;
        private DeviceInfo _deviceInfo;
        private Device _device;

        #region WIA constants

        public const int WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES = 3086;
        public const int WIA_DPS_DOCUMENT_HANDLING_STATUS = 3087;
        public const int WIA_DPS_DOCUMENT_HANDLING_SELECT = 3088;

        public const string WIA_FORMAT_JPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";

        public const int FEED_READY = 0x00000001;
        public const int FLATBED_READY = 0x00000002;

        public const uint BASE_VAL_WIA_ERROR = 0x80210000;
        public const uint WIA_ERROR_PAPER_EMPTY = BASE_VAL_WIA_ERROR + 3;

        #endregion


        public async Task<List<Image>> ScanFile(int dpi = 150, double width = 8.5, double height = 8.5)
        {
            this.GetDefaultDeviceID();
            return await this.ScanPages(dpi, width: 8.5, height: 11);
        }

        private DeviceInfo FindDevice(string deviceId)
        {
            DeviceManager manager = new DeviceManager();
            foreach (DeviceInfo info in manager.DeviceInfos)
                if (info.DeviceID == deviceId)
                    return info;

            return null;
        }

        private void FindDevice()
        {
            DeviceManager manager = new DeviceManager();
            foreach (DeviceInfo info in manager.DeviceInfos)
                if (info.DeviceID == DeviceId)
                {
                    if (info != null)
                    {
                        this.deviceInfo = info;
                    }
                }
        }

        private DeviceInfo deviceInfo;

        public void GetDefaultDeviceID()
        {
            // Select a scanner
            WIA.CommonDialog wiaDiag = new WIA.CommonDialog();
            Device d = wiaDiag.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);
            if (d != null)
            {
                this.DeviceId = d.DeviceID;
                FindDevice();
                this._device = this.deviceInfo.Connect();
            }
        }

        public string DeviceId { get; set; }

        public async Task<List<Image>> ScanPages(int dpi = 150, double width = 8.5, double height = 11)
        {
            Item item = _device.Items[1];

            // configure item
            SetDeviceItemProperty(ref item, 6146, 2); // greyscale
            SetDeviceItemProperty(ref item, 6147, dpi); // 150 dpi
            SetDeviceItemProperty(ref item, 6148, dpi); // 150 dpi
            SetDeviceItemProperty(ref item, 6151, (int)(dpi * width)); // scan width
            SetDeviceItemProperty(ref item, 6152, (int)(dpi * height)); // scan height
            SetDeviceItemProperty(ref item, 4104, 8); // bit depth

            // TODO: Detect if the ADF is loaded, if not use the flatbed


            List<Image> images = await GetPagesFromScanner(ScanSource.DocumentFeeder, item);
            if (images.Count == 0)
            {
                // Try from flatbed
                DialogResult dialogResult;
                do
                {
                    List<Image> singlePage = await GetPagesFromScanner(ScanSource.Flatbed, item);
                    images.AddRange(singlePage);
                    dialogResult = MessageBox.Show("Scan another page?", "ScanToEvernote", MessageBoxButtons.YesNo);
                } while (dialogResult == DialogResult.Yes);
            }

            return images;
        }

        public List<Image> Scan()
        {
            const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";

            //CommonDialogClass wiaDiag = new CommonDialogClass();
            ICommonDialog wiaDiag = new WIA.CommonDialog();
            WIA.ImageFile wiaImage = null;
            wiaImage = wiaDiag.ShowAcquireImage(WiaDeviceType.ScannerDeviceType, WiaImageIntent.UnspecifiedIntent,
                WiaImageBias.MaximizeQuality,
                WIA_FORMAT_JPEG, false, true, false);

            List<Image> images = new List<Image>();
            if (wiaImage != null)
            {
                System.Diagnostics.Trace.WriteLine(String.Format("Image is {0} x {1} pixels",
                    (float)wiaImage.Width / 150, (float)wiaImage.Height / 150));
                Image image = ConvertToImage(wiaImage);
                images.Add(image);
            }

            return images;
        }

        private async Task<List<Image>> GetPagesFromScanner(ScanSource source, Item item)
        {
            SetDeviceProperty(ref _device, 3088, (int)source);

            List<Image> images = new List<Image>();


            try
            {
                int handlingStatus = GetDeviceProperty(ref _device, WIA_DPS_DOCUMENT_HANDLING_STATUS);
                Debug.Write(source);
                //   ICommonDialog wiaDiag = new WIA.CommonDialog();
                //var res = wiaDiag.show(this._device, WiaImageIntent.UnspecifiedIntent, WiaImageBias.MaximizeQuality, false, true, false);

                do
                {
                    ImageFile wiaImage = null;
                    try
                    {
                        // wiaImage = wiaDiag.ShowTransfer(item);
                        //  SplashScreenManager.ShowForm(this, typeof(frm), true, true, SplashFormStartPosition.Default, new Point(0, 0), ParentFormState.Locked);

                        wiaImage = await Task.Run(() => item.Transfer(WIA_FORMAT_JPEG));
                        // SplashScreenManager.CloseForm();
                    }
                    catch (COMException ex)
                    {
                        if ((uint)ex.ErrorCode == WIA_ERROR_PAPER_EMPTY)
                            break;
                    }

                    if (wiaImage != null)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("Image is {0} x {1} pixels",
                            (float)wiaImage.Width / 150, (float)wiaImage.Height / 150));
                        Image image = ConvertToImage(wiaImage);
                        images.Add(image);
                    }
                } while (source == ScanSource.DocumentFeeder);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return images;
        }

        private static Image ConvertToImage(ImageFile wiaImage)
        {
            byte[] imageBytes = (byte[])wiaImage.FileData.get_BinaryData();
            MemoryStream ms = new MemoryStream(imageBytes);
            Image image = Image.FromStream(ms);
            return image;
        }

        #region Get/set device properties

        private void SetDeviceProperty(ref Device device, int propertyID, int propertyValue)
        {
            foreach (Property p in device.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    object value = propertyValue;
                    p.set_Value(ref value);
                    break;
                }
            }
        }

        private int GetDeviceProperty(ref Device device, int propertyID)
        {
            int ret = -1;

            foreach (Property p in device.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    ret = (int)p.get_Value();
                    break;
                }
            }

            return ret;
        }

        private void SetDeviceItemProperty(ref Item item, int propertyID, int propertyValue)
        {
            try
            {
                foreach (Property p in item.Properties)
                {
                    if (p.PropertyID == propertyID)
                    {
                        object value = propertyValue;
                        p.set_Value(ref value);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private int GetDeviceItemProperty(ref Item item, int propertyID)
        {
            int ret = -1;

            foreach (Property p in item.Properties)
            {
                if (p.PropertyID == propertyID)
                {
                    ret = (int)p.get_Value();
                    break;
                }
            }

            return ret;
        }

        #endregion
    }

    enum ScanSource
    {
        DocumentFeeder = 1,
        Flatbed = 2,
    }
}