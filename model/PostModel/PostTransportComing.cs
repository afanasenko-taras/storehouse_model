using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostTransportComing : FastAbstractEvent
    {
        string postTransportUid;
        (string postUid, TransportAction tAction) postAction;
        public PostTransportComing(string postTransportUid, (string postUid, TransportAction tAction) postAction) 
        {
            this.postTransportUid = postTransportUid;
            this.postAction = postAction;
        }


        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostTransport postTransport = (PostTransport)wrapper.getObject(postTransportUid);
            PostCenter postCenter = (PostCenter)wrapper.getObject(postAction.postUid);
            wrapper.WriteDebug($"Transport {postTransport.uid} is comming to {postAction.postUid} at {timeSpan} for {postAction.tAction}");
            PostWrapper pw = (PostWrapper)wrapper;
            var action = postTransport.actions.Dequeue();
            if (action.postUid!=postAction.postUid || action.tAction!=postAction.tAction)
            {
                Console.WriteLine("Alarm!!!");
                Console.ReadLine();
            }
            //Need load from all gates for transport with shedule
            if (postAction.tAction == TransportAction.Load || postAction.tAction == TransportAction.Both)
            {
                foreach (var shedule in postTransport.actions)
                {
                    if (shedule.tAction == TransportAction.Unload || shedule.tAction == TransportAction.Both)
                    {
                        if (!postCenter.gates.ContainsKey(shedule.postUid))
                            continue;
                        if (postCenter.gates[shedule.postUid].Count == 0)
                            continue;
                        if (!postTransport.messageOnBoard.ContainsKey(shedule.postUid))
                            postTransport.messageOnBoard.Add(shedule.postUid, new List<Message>());

                        wrapper.WriteDebug($"Transport {postTransport.uid} get at {postCenter.lastUpdated} from {postCenter.uid} to {shedule.postUid} messeges count {postCenter.gates[shedule.postUid].Count}");
                        postTransport.messageOnBoard[shedule.postUid].AddRange(postCenter.gates[shedule.postUid]);
                        pw.MessageLogTransport(postCenter.gates[shedule.postUid], timeSpan, postCenter.uid, postTransport.uid, "LoadOnTransport");
                        postCenter.gates[shedule.postUid] = new List<Message>();
                        
                    }
                }
            }
            if (postAction.tAction == TransportAction.Unload || postAction.tAction == TransportAction.Both)
            {
                if (postTransport.messageOnBoard.ContainsKey(postCenter.uid)) 
                    if (postTransport.messageOnBoard[postCenter.uid].Count > 0)
                    {
                        if (postCenter is PostOffice)
                        {
                            wrapper.WriteDebug($"!!!!!Message number {postTransport.messageOnBoard[postCenter.uid].Count} delivered to {postCenter.uid} at {timeSpan}");
                            pw.MessageLogTransport(postTransport.messageOnBoard[postCenter.uid], timeSpan, postCenter.uid, postTransport.uid, "UnloadFromTransport");
                            pw.MessageLogDelivered(postTransport.messageOnBoard[postCenter.uid], timeSpan, postCenter.uid, "Delivered");
                            postTransport.messageOnBoard.Remove(postCenter.uid);
                        }
                        if (postCenter is SortingCenter)
                        {
                            wrapper.WriteDebug($"Message count {postTransport.messageOnBoard[postCenter.uid].Count} delivered to {postCenter.uid} at {timeSpan}");
                            pw.MessageLogTransport(postTransport.messageOnBoard[postCenter.uid], timeSpan, postCenter.uid, postTransport.uid, "UnloadFromTransport");
                            pw.MessageLogDelivered(postTransport.messageOnBoard[postCenter.uid], timeSpan, postCenter.uid, "Delivered");
                            
                            SortingCenter sortingCenter = ((SortingCenter)postCenter);
                            foreach (var message in postTransport.messageOnBoard[postCenter.uid])
                            {
                                if (message.directionTo != postTransport.uid)
                                    sortingCenter.inLine.Enqueue((timeSpan, message));
                            }

                            postTransport.messageOnBoard.Remove(postCenter.uid);
                        }
                    }
            }
        }
    }
}
