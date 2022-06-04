using System.Collections;
using UnityEngine;

namespace KLOUD
{
    public class CoroutineHandler : MonoBehaviour
    {
        IEnumerator enumerator = null;

        private void Coroutine(IEnumerator coro)
        {
            enumerator = coro;
            StartCoroutine(coro);        
        }

        void Update()
        {
            if (enumerator != null)
            {
                if (enumerator.Current == null)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (CoroutineEventManager.Requests.Count != 0)
                {
                    Start_Coroutine(CoroutineEventManager.Requests.Dequeue());
                }
            }
        }

        public void Stop()
        {
            StopCoroutine(enumerator.ToString());
            Destroy(gameObject);
        }
        
        public static CoroutineHandler Start_Coroutine(IEnumerator coro)
        {
            GameObject obj = new GameObject("CoroutineHandler");
            CoroutineHandler handler = obj.AddComponent<CoroutineHandler>();
            if (handler)
            {
                handler.Coroutine(coro);
            }
            return handler;
        }
    }
}