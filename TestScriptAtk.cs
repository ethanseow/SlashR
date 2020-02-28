using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptAtk : MonoBehaviour
{
    private int playerLayer = 1 << 8;
    //float timer = 0;
    RaycastHit hit;
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3, Color.blue);
        if(Physics.Raycast(ray.origin, ray.direction, out hit, 3f, playerLayer))
        {
            float damageDone = 10f;
            if (hit.collider.tag == "Sheild")
            {
                Debug.Log("Hit shield");
                return;
            }
            hit.collider.GetComponent<Health>().Damaged(damageDone);
        }
        /*
        RaycastHit[] hitArray;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3, Color.blue);
        hitArray = Physics.RaycastAll(ray, 3);
        bool foundFirstPlayer = false;
        float damageToPlayer = 10;
        GameObject enemy = null;
        foreach (RaycastHit hitByRay in hitArray)
        {
            if (hitByRay.collider.tag == "Sheild")
            {
                damageToPlayer = 0;
            }
            if (hitByRay.collider.tag == "Player")
            {
                enemy = hitByRay.collider.gameObject;
                foundFirstPlayer = true;
            }
        }
        if (foundFirstPlayer)
        {
            enemy.GetComponent<Health>().Damaged(damageToPlayer);
            foundFirstPlayer = false;
        }
        */
    }
}
