using System.Collections;
using UnityEngine;

public class MentalStateManager : MonoBehaviour
{
    public int mentalState = 0;


    public void Goodtime()
    {
        ++mentalState;
        Debug.Log("el nuevo valor de mental es " + mentalState);

    }

    public void Badtime()
    {
        --mentalState;
        Debug.Log("el nuevo valor de mental es " + mentalState);

    }

   



    public IEnumerator DestroyAfterDelay(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }

}
