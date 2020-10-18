using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NPCModel
{
    GeneralEnvironment GEM;
    Animator AnimalAnim;
    PlayerController PL;
    public NPC(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }
    private void Start()
    {
        GEM = GetComponent<GeneralEnvironment>();
        PL = PlayerController.MyPlayerControl;
    }
    public void Update()
    {

        if (GEM.isCaptured)
        {
            //if (PL = null) StartCoroutine(cariPasangan());
            PL = PlayerController.MyPlayerControl;
            Ray ray = new Ray(transform.position, PL.transform.position);
            float distance = Vector3.Distance(transform.position, PL.transform.position);
            RaycastHit hit;
            Debug.DrawLine(transform.position, PL.transform.position, Color.cyan);
            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.DrawLine(hit.point, hit.point + Vector3.up * 12, Color.blue);
                StartCoroutine(KagetNPCnya());
            }
        }
    }
    public IEnumerator cariPasangan()
    {
        while (PL = null)
        {
            PL = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            yield return null;
        }

    }
    public IEnumerator KagetNPCnya()
    {
        float move = 0f;
        while (move < maxcatchtime)
        {
            AnimalAnim = GetComponentInChildren<Animator>();
            AnimalAnim.SetBool("RUUUUNFORYOURLIFE", true);
            transform.position = transform.position + new Vector3(-0.1f * MoveSpeed * Time.deltaTime, 0, 0);
            move += Time.deltaTime;
            yield return new WaitForSeconds(0.5f);
            //AnimalAnim.SetBool("RUUUUNFORYOURLIFE", false);
        }
        if (move >= maxcatchtime)
        { 
            Destroy(gameObject); 
            
        }
    }

}
