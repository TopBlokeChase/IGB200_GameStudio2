using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDestroyOnFinish : MonoBehaviour
{
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        lifeTime = this.gameObject.GetComponent<AudioSource>().clip.length;
        Destroy(this.gameObject, lifeTime);
    }
}
