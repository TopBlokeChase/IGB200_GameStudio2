using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTrigger : MonoBehaviour
{
    [SerializeField] private bool isLeftTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GlassShatter()
    {
        if (isLeftTrigger)
        {
            transform.root.GetComponentInParent<Glass>().ShatterGlass(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            transform.root.GetComponentInParent<Glass>().ShatterGlass(false);
            this.gameObject.SetActive(false);
        }
    }
}
