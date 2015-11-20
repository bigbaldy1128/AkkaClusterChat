//-----------------------------------------------------------------------
// <copyright file="TransformationFrontend.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Akka.Actor;
using System.Windows.Forms;
using System;

namespace Samples.Cluster.Transformation
{
    public class TransformationFrontend : UntypedActor
    {
        protected List<IActorRef> Backends = new List<IActorRef>();
        protected int Jobs = 0;
        private ComboBox addressComboBox;
        private RichTextBox richTextBox;

        protected override void OnReceive(object message)
        {
            if (message is TransformationMessages.UserList)
            {
                var userList = message as TransformationMessages.UserList;
                addressComboBox.Items.Clear();
                addressComboBox.Items.AddRange(userList.Addresses.ToArray());
            }
            else if (message is Control)
            {
                if (message is ComboBox)
                {
                    addressComboBox = (ComboBox)message;
                }
                else if (message is RichTextBox)
                {
                    richTextBox = (RichTextBox)message;
                }
            }
            else if (message is TransformationMessages.SendTextMessage)
            {
                var tm = message as TransformationMessages.SendTextMessage;
                TransformationMessages.ReceiveTextMessage rtm = new TransformationMessages.ReceiveTextMessage();
                rtm.Message = tm.Message;
                if (!string.IsNullOrEmpty(addressComboBox.Text))
                {
                    Context.ActorSelection(addressComboBox.Text).Tell(rtm);
                }
            }
            else if (message is TransformationMessages.ReceiveTextMessage)
            {
                var rtm = message as TransformationMessages.ReceiveTextMessage;
                richTextBox.Invoke((MethodInvoker)(() => { richTextBox.AppendText(rtm.Message + Environment.NewLine); }));
            }
            else
            {
                Unhandled(message);
            }
        }
    }
}

