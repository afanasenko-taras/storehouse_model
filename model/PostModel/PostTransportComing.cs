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

                        wrapper.WriteDebug($"Transport {postTransport.uid} get at {postCenter.lastUpdated} from {postCenter.uid} to {shedule.Value.postUid} messeges count {postCenter.gates[shedule.Value.postUid].Count}");
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
                            wrapper.WriteDebug($"!!!!!Message number {postTransport.messageOnBoard[postCenter.uid].Count} delivered to {postCenter.uid} at {timeSpan}");
                            postTransport.messageOnBoard.Remove(postCenter.uid);
                        }
                        if (postCenter is SortingCenter)
                        {
                            SortingCenter sortingCenter = ((SortingCenter)postCenter);
                            foreach (var message in postTransport.messageOnBoard[postCenter.uid])
                            {
                                sortingCenter.inLine.Enqueue((timeSpan, message));
                            }
                            wrapper.WriteDebug($"Message count {postTransport.messageOnBoard[postCenter.uid].Count} delivered to {postCenter.uid} at {timeSpan}");
                            postTransport.messageOnBoard.Remove(postCenter.uid);
                        }
                    }
            }
        }
    }
}
