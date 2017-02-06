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

namespace SoftPhone
{
    public partial class Form1 : Form
    {
        string mymsg;
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
            InitalizeSoftPhone();
        }
        private void InitalizeSoftPhone()
        {
            try
            {
                SoftPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 5000, 10000);
                InvokeGUIThread(mymsg = "SoftPhoneCreated!");

                SoftPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(SoftPhone_inComingCall);

                var registrationRequired = true;
                var userName = "858";
                var displayName = "858";
                var authenticationId = "858";
                var registerPassword = "858";
                var domainHost = SoftPhoneFactory.GetLocalIP().ToString();
                var domainPort = 5060;
                SIPAccount sa = new SIPAccount(registrationRequired, displayName, userName, authenticationId, registerPassword, domainHost, domainPort);
                InvokeGUIThread(mymsg = "SIP Acc Created!!");

                NatConfiguration nc = new NatConfiguration();
                nc.AutoDetect = true;

                PhoneLineConfiguration config = new PhoneLineConfiguration(sa);
                config.NatConfig = nc;

                PhoneLine = SoftPhone.CreatePhoneLine(config);
                PhoneLine.RegistrationStateChanged += PhoneLine_PhoneLineInfo;
                InvokeGUIThread(mymsg = "PhoneLine Created!!");
                SoftPhone.RegisterPhoneLine(PhoneLine);

                ConnectMedia();
            }
            catch (Exception ex)
            {
                InvokeGUIThread(mymsg = ("Unknown Error: " + ex));
            }
        }

        private void StartDevices()
        {
            try
            {
                if (MicrPhone != null)
                {
                    MicrPhone.Start();
                    InvokeGUIThread(mymsg = "Microphone started!!");
                }

                if (Speakers != null)
                {
                    Speakers.Start();
                    InvokeGUIThread(mymsg = "Speakers started!!");
                }
            }
            catch (Exception ex)
            {
                InvokeGUIThread(mymsg = ("To start mic or speaker was imposible because this error: " + ex));
            }
        }

        private void StopDevices()
        {
            try
            {
                if (MicrPhone != null)
                {
                    MicrPhone.Stop();
                    InvokeGUIThread(mymsg = "Microphone started!!");
                }

                if (Speakers != null)
                {
                    Speakers.Stop();
                    InvokeGUIThread(mymsg = "Speakers started!!");
                }
            }
            catch (Exception ex)
            {
                InvokeGUIThread(mymsg = ("To start mic or speaker was imposible because this error: " + ex));
            }
        }

        private void ConnectMedia()
        {
            try
            {
                if (MicrPhone != null)
                {
                    Conector.Connect(MicrPhone, MediaSender);
                }
                if (Speakers != null)
                {
                    Conector.Connect(MediaReciever, Speakers);
                }
            }
            catch (Exception ex)
            {
                InvokeGUIThread(mymsg = ("Cant start media: " + ex));
            }
        }

        private void DisconnectMedia()
        {
            try
            {
                if (MicrPhone != null)
                {
                    Conector.Disconnect(MicrPhone, MediaSender);
                }
                if (Speakers != null)
                {
                    Conector.Disconnect(MediaReciever, Speakers);
                }
            }
            catch (Exception ex)
            {
                InvokeGUIThread(mymsg = ("Cant start media: " + ex));
            }
        }

        delegate void StringArgReturningVoidDelegate(string mymsg);
        public void InvokeGUIThread(string mymsg)
        {
            if (this.lb_log.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(InvokeGUIThread);
                this.Invoke(d, new object[] { mymsg });
            }
            else
            {
                this.lb_log.Items.Add(mymsg);
            }
            
        }

        private void SoftPhone_inComingCall(object sender, VoIPEventArgs<IPhoneCall> e)
        {
            InvokeGUIThread(mymsg = ("Incoming call from: " + e.Item.DialInfo.ToString()));

            PhoneCall = e.Item;
            WireUpCallEvents();
            InComingCall = true;
        }

        private void PhoneLine_PhoneLineInfo(object sender, RegistrationStateChangedArgs e)
        {

                if (e.State == RegState.NotRegistered || e.State == RegState.Error)
                {
                    InvokeGUIThread(mymsg = "Registration failed!!! " + e.Error);
                }
                if (e.State == RegState.RegistrationSucceeded)
                {
                    InvokeGUIThread(mymsg = "Registration succeeded - Online!");
            }
        }

        private void call_CallStateChanged(object sender, CallStateChangedArgs e)
        {
            InvokeGUIThread(mymsg = "Call state: {0}." + e.State);

            if (e.State == CallState.Answered)
            {
                StartDevices();
                MediaReciever.AttachToCall(PhoneCall);
                MediaSender.AttachToCall(PhoneCall);

                InvokeGUIThread(mymsg = "Call started" + e.State);
            }

            if (e.State.IsCallEnded() == true)
            {
                StopDevices();

                MediaReciever.Detach();
                MediaSender.Detach();

                WireDownCallEvents();

                PhoneCall = null;
                InvokeGUIThread(mymsg = "Call ended" + e.State);
            }
        }

        private void WireUpCallEvents()
        {
            PhoneCall.CallStateChanged += (call_CallStateChanged);
        }

        private void WireDownCallEvents()
        {
            PhoneCall.CallStateChanged -= (call_CallStateChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (InComingCall)
            {
                InComingCall = false;
                PhoneCall.Answer();

                InvokeGUIThread(mymsg = "Call accepted.\n");
                return;
            }
            if (PhoneCall != null)
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PhoneCall != null)
            {
                if (InComingCall && PhoneCall.CallState == CallState.Ringing)
                {
                    PhoneCall.Reject();
                    InvokeGUIThread(mymsg = "Call rejected!");
                }
                else
                {
                    PhoneCall.HangUp();
                    InComingCall = false;
                    InvokeGUIThread(mymsg = "Call hanged up");
                }

                PhoneCall = null;
            }
        }
    }
}
