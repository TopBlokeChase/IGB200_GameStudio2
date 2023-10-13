using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteOfCourage : MonoBehaviour
{
    [SerializeField] private string noteTitle;

    [TextArea(10, 15)]
    [SerializeField] private string noteText;

    [SerializeField] private string buffName;
    [SerializeField] private int shieldAmount;

    [SerializeField] private GameObject noteSprite;
    [SerializeField] private GameObject noteInteractUI;
    [SerializeField] private GameObject noteTextUI;
    [SerializeField] private TMP_Text noteTitleText;
    [SerializeField] private TMP_Text noteBodyText;
    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private float statusEffectUIPosOffset;

    [SerializeField] private AudioSource soundLooping;
    [SerializeField] private AudioSource soundPickup;

    private GameObject player;

    private bool isReadyToRead;
    // Start is called before the first frame update
    void Start()
    {
        noteTitleText.text = noteTitle;
        noteBodyText.text = noteText;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReadyToRead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale = 0;
                noteTextUI.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            noteInteractUI.SetActive(true);
            isReadyToRead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            noteInteractUI.SetActive(false);
            isReadyToRead = false;
        }
    }

    public void TakeNote()
    {
        Vector3 UIStatusPos = new Vector3(player.transform.position.x, player.transform.position.y - statusEffectUIPosOffset, player.transform.position.z);
        player.GetComponentInChildren<PlayerCombat>().SetHasNoteOfCourage(true, shieldAmount);
        player.GetComponentInChildren<PlayerCombat>().PlayNotePickupParticle();
        GameObject note = Instantiate(statusEffectUI, UIStatusPos, Quaternion.identity);
        note.GetComponent<PlayerStatusUI>().InitiateText(buffName, shieldAmount);
        note.GetComponent<PlayerStatusUI>().InitialisePosition(player, statusEffectUIPosOffset);
        Time.timeScale = 1;

        soundLooping.Stop();

        
        soundPickup.Play();
        noteInteractUI.SetActive(false);
        noteTextUI.SetActive(false);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        noteSprite.SetActive(false);

        StartCoroutine(Delay(soundPickup.clip.length));
    }

    IEnumerator Delay(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(transform.root.gameObject);
    }
}
