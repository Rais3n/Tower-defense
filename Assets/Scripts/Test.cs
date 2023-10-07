using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform square;
    private float rotationSpeed = 5f;

    private void Update()
    {
        //float rotationSpeed = 5.0f;
        //Vector2 dir = square.position - transform.position;
        //Quaternion targetRot = Quaternion.LookRotation(dir);
        //Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //transform.rotation = newRotation;

        Vector2 dir = square.position - transform.position;
        float targetRotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Tworzymy kwaternion tylko dla osi Z
        Quaternion targetRot = Quaternion.Euler(0, 0, targetRotationZ);

        Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        // Kombinujemy now¹ rotacjê z bie¿¹c¹ rotacj¹ poza osi¹ Z
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);

    }
}
