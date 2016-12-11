using System.Windows.Forms;
using System.Diagnostics;
namespace RawInput_dll
{
    public class PreMessageFilter : IMessageFilter
    {
        // true  to filter the message and stop it from being dispatched 
        // false to allow the message to continue to the next filter or control.
        public bool PreFilterMessage(ref Message m)
        {
            //return m.Msg == Win32.WM_KEYDOWN;
            //return (m.Msg == Win32.RI_MOUSE_LEFT_BUTTON_DOWN || m.Msg == Win32.RI_MOUSE_RIGHT_BUTTON_DOWN ||
            //    m.Msg == Win32.RI_MOUSE_MIDDLE_BUTTON_DOWN || m.Msg == Win32.RI_MOUSE_BUTTON_4_DOWN ||
            //    m.Msg == Win32.RI_MOUSE_BUTTON_5_DOWN);
            if (m.Msg == Win32.WM_LBUTTONDOWN || m.Msg == Win32.WM_MBUTTONDOWN || m.Msg == Win32.WM_RBUTTONDOWN || m.Msg == Win32.WM_XBUTTONDOWN)
            {
                //MessageBox.Show("Clicked! "); // Try to find out how to filter out the releases
                return true;
            }
                return false ;
        }
    }
}
