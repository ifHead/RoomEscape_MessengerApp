using Mirror;
using UnityEngine;

namespace QuickStart
{
    public class PlayerScript : NetworkBehaviour
    {
        public string dpStr;

        [SyncVar(hook = nameof(strChange))]
        public string myStr;

        public void strChange(string oldValue, string newValue){
            dpStr = myStr;
        }

        public override void OnStartLocalPlayer()
        {
            string s = UnityEngine.Random.Range(0,101010).ToString();
            cmdSetup(s);
        }

        [Command]
        public void cmdSetup(string s)
        {
            myStr = s;
        }


        public void Update()
        {
            if (!isLocalPlayer) { return; }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);
        }
    }
}