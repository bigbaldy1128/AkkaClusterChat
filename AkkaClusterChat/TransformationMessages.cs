//-----------------------------------------------------------------------
// <copyright file="TransformationMessages.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace Samples.Cluster.Transformation
{
    public sealed class TransformationMessages
    {
        public class UserList
        {
            public List<string> Addresses = new List<string>();
        }

        public class SendTextMessage
        {
            public string Message { get;set; }
        }

        public class ReceiveTextMessage
        {
            public string Message { get; set; }
        }
    }
}

