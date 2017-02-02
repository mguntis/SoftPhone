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
        private RegState PhoneLineInfo;
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

        private void InitalizeSoftPhone()
        {
            SoftPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 5700, 5750);
            InvokeGUIThread(() => { lb_log.Items.Add("SoftPhoneCreated!!!"); });

            SoftPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(SoftPhone_inComingCall);

            SIPAccount sa = new SIPAccount(true, "1000", "1000", "1000", "1000", "85.254.224.146");
            InvokeGUIThread(() => { lb_log.Items.Add("SIP Acc Creates!!"); });

            NatConfiguration nc = new NatConfiguration(NatTraversalMethod.None);
            InvokeGUIThread(() => { lb_log.Items.Add("NAT Config Created!!"); });

            PhoneLine = SoftPhone.CreatePhoneLine(sa, nc);
        }
        private void InvokeGUIThread(Action action)
        {
            Invoke(action);
        }

        //private void call_CallErrorOccured(object sender, VoIPEventArgs<CallError> e)
        //{
        //    InvokeGUIThread(() => { lb_log.Items.Add("Error Occured"); });
        //}

        private void call_CallStateChanged(object sender, CallStateChangedArgs e)
        {
            InvokeGUIThread(() => { lb_log.Items.Add("CallStateChanged: " + e.ToString()); });
        }

        private void WireUpCallEvents()
        {
            PhoneCall.CallStateChanged += (call_CallStateChanged);
        }
        private void WireDownCallEvents()
        {
            PhoneCall.CallStateChanged -= (call_CallStateChanged);
        }

    }
}
