using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiGeChang
{



    class UsbOperator
    {
        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        public struct DevBroadcastVolume
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public int Mask;
            public Int16 Flags;
        }

        /// <summary>
        /// 等待 USB 存储设备接入。
        /// </summary>
        /// <returns>接入的 USB 设备盘符。</returns>
        public static char WaitForUsbDevice()
        {
            DevBroadcastVolume vol = WndProcReceiverForm.WaitForSingleWndProc(
                m =>
                    m.Msg == WM_DEVICECHANGE
                    && (int)m.WParam == DBT_DEVICEARRIVAL
                    && Marshal.ReadInt32(m.LParam, 4) == DBT_DEVTYP_VOLUME,
                m =>
                    Marshal.PtrToStructure<DevBroadcastVolume>(m.LParam)).Item2;

            BitArray bv = new BitArray(new int[] { vol.Mask });
            char letter = '\0';
            for (int i = 0; i < bv.Length; i++)
            {
                if (bv[i])
                {
                    letter = (char)('A' + i);
                }
            }

            return letter;
        }

        public static void WaitForUsbDeviceRemove()
        {
            WndProcReceiverForm.WaitForSingleWndProc(
               m =>
                   m.Msg == WM_DEVICECHANGE
                   && (int)m.WParam == DBT_DEVICEREMOVECOMPLETE
               );
        }
    }
}
