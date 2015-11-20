//-----------------------------------------------------------------------
// <copyright file="TransformationBackend.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2015 Typesafe Inc. <http://www.typesafe.com>
//     Copyright (C) 2013-2015 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Akka.Actor;
using Akka.Cluster;

namespace Samples.Cluster.Transformation
{
    public class TransformationBackend : UntypedActor
    {
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        protected TransformationMessages.UserList userList = new TransformationMessages.UserList();

        /// <summary>
        /// Need to subscribe to cluster changes
        /// </summary>
        protected override void PreStart()
        {
            Cluster.Subscribe(Self, new[] { typeof(ClusterEvent.MemberUp), typeof(ClusterEvent.MemberRemoved) });
        }

        /// <summary>
        /// Re-subscribe on restart
        /// </summary>
        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }

        protected override void OnReceive(object message)
        {
            if (message is ClusterEvent.MemberUp)
            {
                var memUp = (ClusterEvent.MemberUp) message;
                Register(memUp.Member);
            }
            else if (message is ClusterEvent.MemberRemoved)
            {
                var removed = (ClusterEvent.MemberRemoved)message;
                UnRegister(removed.Member);
            }
            else
            {
                Unhandled(message);
            }
        }

        protected void Register(Member member) //send user list
        {
            if (member.HasRole("frontend"))
            {
                var address = member.Address + "/user/frontend";
                userList.Addresses.Add(address);
                foreach (var addr in userList.Addresses)
                {
                    Context.ActorSelection(addr).Tell(userList, Self);
                }
            }
        }

        protected void UnRegister(Member member) //remove user and send new user list
        {
            if (member.HasRole("frontend"))
            {
                var address = member.Address + "/user/frontend";
                userList.Addresses.Remove(address);
                foreach (var addr in userList.Addresses)
                {
                    Context.ActorSelection(addr).Tell(userList, Self);
                }
            }
        }
    }
}

