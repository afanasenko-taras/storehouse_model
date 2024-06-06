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
            Console.WriteLine($"Transport {postTransport.uid} is comming to {postAction.postUid} at {timeSpan} for {postAction.tAction}");

            //Need load from all gates for transport with shedule
            if (postAction.tAction == TransportAction.Load || postAction.tAction == TransportAction.Both)
            {
                foreach (var shedule in postTransport.shedule)
                {
                    if (shedule.Key <= postTransport.lastUpdated.Hours)
                        continue;
                    if (shedule.Value.tAction == TransportAction.Unload || shedule.Value.tAction == TransportAction.Both)
                    {
                        if (!postCenter.gates.ContainsKey(shedule.Value.postUid))
                            continue;
                        if (postCenter.gates[shedule.Value.postUid].Count == 0)
                            continue;
                        if (!postTransport.messageOnBoard.ContainsKey(shedule.Value.postUid))
                            postTransport.messageOnBoard.Add(shedule.Value.postUid, new List<Message>());

                        Console.WriteLine($"Transport {postTransport.uid} get at {postCenter.lastUpdated} from {postCenter.uid} to {shedule.Value.postUid} messeges count {postCenter.gates[shedule.Value.postUid].Count}");
                        postTransport.messageOnBoard[shedule.Value.postUid].AddRange(postCenter.gates[shedule.Value.postUid]);
                        postCenter.gates[shedule.Value.postUid] = new List<Message>();
                        
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
                            foreach(var message in postTransport.messageOnBoard[postCenter.uid])
                            {
                                Console.WriteLine($"!!!!!Message from {message.directionFrom} delivered to {message.directionTo} at {timeSpan}");
                            }
                            postTransport.messageOnBoard.Remove(postCenter.uid);
                        }
                        if (postCenter is SortingCenter)
                        {
                            foreach (var message in postTransport.messageOnBoard[postCenter.uid])
                            {
                                ((SortingCenter)postCenter).inLine.Enqueue((timeSpan, message));
                                Console.WriteLine($"Message from {message.directionFrom} to {message.directionTo} delivered to {postCenter.uid} at {timeSpan}");
                            }
                            postTransport.messageOnBoard.Remove(postCenter.uid);
                        }
                    }
            }
        }
    }
}
