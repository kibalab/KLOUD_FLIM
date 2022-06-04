using System.Collections;
using System.Collections.Generic;

namespace KLOUD
{
    public class CoroutineRequest
    {
        IEnumerator enumerator = null;

        public void Request(IEnumerator coro)
        {
            enumerator = coro;
        }
    }
    
    public class CoroutineEventManager
    {
        public static Queue<IEnumerator> Requests = new Queue<IEnumerator>();
    }
}