using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    [SerializeField] private GameObject level2ButtonUnlocked;
    [SerializeField] private GameObject level2ButtonLocked;
    [SerializeField] private GameObject level3ButtonUnlocked;
    [SerializeField] private GameObject level3ButtonLocked;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ProgressTracker.hasPassedLevel1)
        {
            level2ButtonLocked.SetActive(false);
            level2ButtonUnlocked.SetActive(true);
        }

        if (ProgressTracker.hasPassedLevel2)
        {
            level3ButtonLocked.SetActive(false);
            level3ButtonUnlocked.SetActive(true);
        }
    }
}
