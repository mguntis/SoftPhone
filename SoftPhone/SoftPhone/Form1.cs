using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ozeki.Camera;
using Ozeki.Common;
using Ozeki.Media;
using Ozeki.Network;
using Ozeki.VoIP;


//using OzFramework2.GCode;
//using OzFramework2.Modem;
//using OzFramework2.Smpp;
//using OzFramework2.SVG;
//using FluorineFx;

namespace SoftPhone
{
    public partial class Form1 : Form
    {
        private ISoftPhone SoftPhone;
        private IPhoneLine PhoneLine;
        //private PhoneLineState PhoneLineInfo;
        private IPhoneCall PhoneCall;
        private Microphone MicrPhone = Microphone.GetDefaultDevice();
        private Speaker Speakers = Speaker.GetDefaultDevice();
        MediaConnector Conector = new MediaConnector();
        PhoneCallAudioSender MediaSender = new PhoneCallAudioSender();
        PhoneCallAudioReceiver MediaReciever = new PhoneCallAudioReceiver();
        private bool InComingCall;

        public Form1()
        {
            InitializeComponent();
        }

        private void InvokeGUIThread(Action action)
        {
            Invoke(action);
        }

        private void call_CallErrorOccured(object sender, VoIPEventArgs<CallError> e)
        {
            InvokeGUIThread(() => { lb_log.Items.Add("Error Occured"); });
        }

        private void call_CallStateChanged(object sender, VoIPEventArgs<CallState> e)
        {
            InvokeGUIThread(() => { lb_log.Items.Add("CallStateChanged: " + e.Item.ToString()); });
        }

        private void WireUpCallEvents()
        {
            
            
        }

    }
}
