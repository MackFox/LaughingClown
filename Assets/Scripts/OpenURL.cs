using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    // Start is called before the first frame update
    public string url = "https://www.linkedin.com/your-profile";

    public void OpenURLExternal()
    {
        Application.OpenURL(url);
    }
}
