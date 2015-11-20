using Akka.Actor;
using Akka.Configuration.Hocon;
using Samples.Cluster.Transformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkkaClusterChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IActorRef frontendActor;

        private void button3_Click(object sender, EventArgs e) //send
        {
            frontendActor.Tell(new TransformationMessages.SendTextMessage() { Message = richTextBox1.Text.Trim() });
            richTextBox1.Clear();
        }

        private void button1_Click(object sender, EventArgs e) //backend
        {
            var config = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
            var system = ActorSystem.Create("ClusterSystem",config);
            system.ActorOf(Props.Create<TransformationBackend>(), "backend");
        }

        private void button2_Click(object sender, EventArgs e) //frontend
        {
            var config = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
            var system = ActorSystem.Create("ClusterSystem", config);
            frontendActor=system.ActorOf(Props.Create<TransformationFrontend>(), "frontend");
            this.Text = frontendActor.Path.ToString();
            frontendActor.Tell(this.richTextBox1);
            frontendActor.Tell(this.comboBox1);
        }
    }
}
